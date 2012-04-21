﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberBuilderFactory.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NStub.Core;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.ObjectGeneration.Builders;

    /// <summary>
    /// Provides builders for test method generation. 
    /// </summary>
    public abstract class MemberBuilderFactory : IMemberBuilderFactory
    {
        #region Fields

        private readonly Dictionary<Type, IBuildHandler> handlers = new Dictionary<Type, IBuildHandler>();
        private readonly IBuilderSerializer serializer;
        private static IMemberBuilderFactory defaultfactory;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MemberBuilderFactory"/> class.
        /// </summary>
        protected MemberBuilderFactory(IBuilderSerializer serializer)
        {
            Guard.NotNull(() => serializer, serializer);
            this.serializer = serializer;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the default <see cref="MemberBuilderFactory"/>.
        /// </summary>
        /// <value>
        /// The default <see cref="MemberBuilderFactory"/>.
        /// </value>
        /// <remarks>
        /// <para>The default <see cref="MemberBuilderFactory"/> can only set once, and that before accessing it.</para>
        /// <para>To achieve a different behavior for the functionality provided by another implementation
        /// of an <see cref="IMemberBuilderFactory"/>, derive from this class and override the abstract 
        /// <see cref="MemberBuilderFactory.Factory"/> getter.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Cannot set the default <see cref="IMemberBuilderFactory"/> twice. 
        /// Maybe you accessed it before you've written to it.</exception>
        public static IMemberBuilderFactory Default
        {
            get
            {
                return defaultfactory ?? (defaultfactory = new DefaultMemberBuilderFactory());
            }

            set
            {
                Guard.NotNull(() => value, value);
                if (defaultfactory != null)
                {
                    throw new InvalidOperationException(
                        "Cannot set the default ITestBuilderFactory twice. Maybe you accessed it before you've written to it.");
                }

                defaultfactory = value;
            }
        }

        /// <summary>
        /// Gets the name of the builders.
        /// </summary>
        public virtual IEnumerable<string> BuilderNames
        {
            get
            {
                var result = this.handlers.Values.Select(e => e.Type.FullName);
                return result;
            }
        }

        /// <summary>
        /// Gets the name of the builders.
        /// </summary>
        public virtual IEnumerable<Type> BuilderTypes
        {
            get
            {
                var result = this.handlers.Values.Select(e => e.Type);
                return result;
            }
        }

        /// <summary>
        /// Gets the factory service of this instance.
        /// </summary>
        public abstract IMemberBuilderFactory Factory { get; }

        #endregion

        /// <summary>
        /// Adds the specified handler to the factory.
        /// </summary>
        /// <param name="handler">The handler to be added.</param>
        public void AddHandler(IBuildHandler handler)
        {
            this.handlers.Add(handler.Type, handler);
        }

        /// <summary>
        /// Tries to get the builder for the specified context.
        /// </summary>
        /// <param name="context">The context of the current test object.</param>
        /// <returns>
        /// A list of member builders that can handle the request or an <c>empty</c> list if no one can be found.
        /// </returns>
        public IEnumerable<IMemberBuilder> GetBuilder(IMemberBuildContext context)
        {
            // So we have property Enabled/Disabled checks as default behavior.
            return GetBuilder(context, true);
        }

        /// <summary>
        /// Tries to get the builder for the specified context dependant on user property activation.
        /// </summary>
        /// <param name="context">The context of the current test object.</param>
        /// <param name="useUserActivation">if set to <c>true</c> use user activation in context.BuildData stored values.
        /// Means: Enable/Disable by setting the corresponding propertystore value.</param>
        /// <returns>
        /// A list of member builders that can handle the request or an <c>empty</c> list if no one can be found.
        /// </returns>
        public IEnumerable<IMemberBuilder> GetBuilder(IMemberBuildContext context, bool useUserActivation)
        {
            // Todo: maybe cache em.
            foreach (var buildHandler in this.handlers.Values)
            {
                var canHandleContext = buildHandler.CanHandle(context);
                if (!canHandleContext)
                {
                    continue;
                }

                if (useUserActivation)
                {
                    Guard.NotNull(() => context.BuildData, context.BuildData);
                    var parameter = this.GetParameters(buildHandler.Type, context.BuildData);
                    if (!parameter.Enabled)
                    {
                        continue;
                    }
                }

                var memberBuilder = buildHandler.CreateInstance(context);
                yield return memberBuilder;
            }
        }

        /// <summary>
        /// Get the parameters for the specified builder type, possibly creating it, if there
        /// is not yet one in the build data collection.
        /// </summary>
        /// <param name="builderType">Type of the builder to request a set of sample data for.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="KeyNotFoundException">The given <paramref name="builderType"/> was not present in the lookup.</exception>
        public IMemberBuildParameters GetParameters(Type builderType, IBuildDataDictionary properties)
        {
            Guard.NotNull(() => properties, properties);
            var paraType = this.handlers[builderType].ParameterDataType;
            return this.serializer.GetParameters(builderType, paraType, properties);
        }

        /// <summary>
        /// Gets the sample setup xml data for a specified <see cref="IMemberBuilder"/>.
        /// </summary>
        /// <param name="builderType">Type of the builder to request a set of sample data for.</param>
        /// <returns>
        /// A string containing the sample data for the specified <paramref name="builderType"/> as xml string.
        /// </returns>
        public string GetSampleSetupData(Type builderType)
        {
            var paraType = this.handlers[builderType].ParameterDataType;
            return serializer.GetSampleSetupData(builderType, paraType);
        }

        /// <summary>
        /// Gets the xml data representation of all registered <see cref="IMemberBuilder"/>s parameters.
        /// </summary>
        /// <param name="properties">The properties storage which stores the <see cref="IMemberBuilderParameters"/> data to serialize.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        public string SerializeAllSetupData(IBuildDataDictionary properties)
        {
            Guard.NotEmpty(() => properties, properties);
            var handlers = this.handlers.Values.AsEnumerable();
            return serializer.SerializeAllHandlers(properties, handlers);
            //return NewMethod(properties, handlers);
        }

        /// <summary>
        /// Gets the xml data representation of a single registered <see cref="IMemberBuilder"/>s parameters.
        /// </summary>
        /// <param name="builderType">Type of the builder to request a set of sample data for.</param>
        /// <param name="properties">The properties storage which stores the <see cref="IMemberBuilderParameters"/> data to serialize.</param>
        /// <returns>
        /// The serialized data of the specified <paramref name="builderType"/>.
        /// </returns>
        public string SerializeSetupData(Type builderType, BuildDataDictionary properties)
        {
            var setupPara = this.GetParameters(builderType, properties);
            return serializer.SerializeParametersForBuilderType(builderType, setupPara);
        }

        /// <summary>
        /// Set the parameters in the properties storage from a specified xml representation of the data.
        /// </summary>
        /// <param name="xml">The xml representation of the data.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="InvalidCastException"><c>InvalidCastException</c> Problem building from serialization data.</exception>
        public IEnumerable<IMemberBuildParameters> DeserializeAllSetupData(string xml, IBuildDataDictionary properties)
        {
            var handlers = this.handlers.Values.AsEnumerable();
            return serializer.DeserializeAllSetupData(xml, properties, handlers);
        }

        /// <summary>
        /// Set the parameters in the properties storage from a specified xml representation of the data.
        /// </summary>
        /// <param name="xml">The xml representation of the data.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="InvalidCastException"><c>InvalidCastException</c> Problem building from serialization data.</exception>
        public IMemberBuildParameters SetParameters(string xml, IBuildDataDictionary properties)
        {
            Guard.NotNull(() => properties, properties);
            
            var handlers = this.handlers.Values.AsEnumerable();
            IBuildHandler handler = serializer.DetermineIMemberBuilderFromXmlFragment(xml, handlers);
            return serializer.SetParameters(xml, properties, handler);
        }
    }
}