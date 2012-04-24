// --------------------------------------------------------------------------------------------------------------------
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
    using NStub.Core;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.ObjectGeneration.Builders;

    public class MultiLookup
    {
        //public IReadOnlyDictionary<string, IBuilderData> Lookup;
        public Type BuilderType;
        public IMultiBuildParameters Parameters;
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            // TODO: write your implementation of ToString() here
            return BuilderType.ToString() + " {" + Parameters.Id.ToString() + "}";
        }
        //public Guid Key;
    }

    /// <summary>
    /// Provides builders for test method generation. 
    /// </summary>
    public abstract class MemberBuilderFactory : IMemberBuilderFactory
    {
        #region Fields

        private readonly Dictionary<Type, IBuildHandler> buildHandlers = new Dictionary<Type, IBuildHandler>();
        private readonly IBuilderSerializer serializer;
        private static IMemberBuilderFactory defaultfactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberBuilderFactory"/> class.
        /// </summary>
        /// <param name="serializer">The build parameter serializer.</param>
        protected MemberBuilderFactory(IBuilderSerializer serializer)
        {
            Guard.NotNull(() => serializer, serializer);
            this.serializer = serializer;
        }

        #endregion

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
                var result = this.buildHandlers.Values.Select(e => e.Type.FullName);
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
                var result = this.buildHandlers.Values.Select(e => e.Type);
                return result;
            }
        }

        /// <summary>
        /// Gets the name of the builders.
        /// </summary>
        //public virtual IEnumerable<IReadOnlyDictionary<string, IBuilderData>> MultiParameters(IBuildDataDictionary properties)
        public virtual IEnumerable<MultiLookup> MultiParameters(IBuildDataDictionary properties)
        {
            var multis = this.buildHandlers.Where(e => e.Value.IsMultiBuilder);


            var multiparamsxx = multis
                .SelectMany((e) =>
                {
                    //var le = ((BuilderSerializer)serializer).GetMultiParameters(e.Value.Type, e.Value.ParameterDataType, properties)
                    //    .Keys.Select(x => x);
                    var lox = ((BuilderSerializer)serializer)
                        .GetMultiParameters(e.Value.Type, e.Value.ParameterDataType, properties);
                    IEnumerable<IBuilderData> lo = new List<IBuilderData>();
                    if (lox != null)
                    {
                        lo = lox.Values.Select(x => x);
                    }
                    return lo;
                }, (a, v) => new MultiLookup() { BuilderType = a.Key, Parameters = (IMultiBuildParameters)v });



            /*var multiparams = multis
                .Select((e) => 
                {
                    var lo = ((BuilderSerializer)serializer).GetMultiParameters(e.Value.Type, e.Value.ParameterDataType, properties)
                        .Values.Select(x=>x);
                    // var mlookup = new MultiLookup() { BuilderType = e.Key, Lookup = lo, Key = ((IMultiBuildParameters)e.Value).Id };
                    var mlookup = new MultiLookup() { BuilderType = e.Key, 
                        Parameters = ((IMultiBuildParameters)lo),
                        //Key = ((IMultiBuildParameters)e.Value).Id 
                    };
                    return mlookup;
                });*/
            //var result = this.buildHandlers.Values.Select(e => e.Type);
            return multiparamsxx;
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
            this.buildHandlers.Add(handler.Type, handler);
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
            var handlers = this.buildHandlers.Values.AsEnumerable();
            return this.serializer.DeserializeAllSetupData(xml, properties, handlers);
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
            return this.GetBuilder(context, true);
        }

        /// <summary>
        /// Tries to get the builder for the specified context dependent on user property activation.
        /// </summary>
        /// <param name="context">The context of the current test object.</param>
        /// <param name="useUserActivation">if set to <c>true</c> use user activation in context.BuildData stored values.
        /// Means: Enable/Disable by setting the corresponding property store value.</param>
        /// <returns>
        /// A list of member builders that can handle the request or an <c>empty</c> list if no one can be found.
        /// </returns>
        public IEnumerable<IMemberBuilder> GetBuilder(IMemberBuildContext context, bool useUserActivation)
        {
            // Todo: maybe cache em.
            foreach (var buildHandler in this.buildHandlers.Values.Where(e => !e.IsMultiBuilder))
            {
                var canHandleContext = buildHandler.CanHandle(context);
                if (!canHandleContext)
                {
                    continue;
                }

                if (useUserActivation)
                {
                    Guard.NotNull(() => context.BuildData, context.BuildData);
                    //if (buildHandler.IsMultiBuilder)
                    //{
                    //    continue;
                    //}
                    //else
                    //{
                    var parameter = this.GetParameters(buildHandler.Type, context.BuildData);
                    if (!parameter.Enabled)
                    {
                        continue;
                    }
                    //}
                }

                var memberBuilder = buildHandler.CreateInstance(context);
                if (memberBuilder != null)
                {
                    yield return memberBuilder;
                }
            }

            var multis = this.MultiParameters(context.BuildData);
            foreach (var multi in multis)
            {
                if (multi.Parameters.Enabled)
                {
                    var multiHandler = this.buildHandlers[multi.BuilderType];
                    var canHandleContext = multiHandler.CanHandle(context);
                    if (!canHandleContext)
                    {
                        continue;
                    }

                    var multiBuilder = multiHandler.CreateInstance(context);
                    ((IMultiBuilder)multiBuilder).Parameters = multi.Parameters;
                    yield return multiBuilder;
                }
                //var mbpara = GetMultiParameter(Guid.Empty, seltype, properties);
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
            Guard.NotNull(() => builderType, builderType);
            Guard.NotNull(() => properties, properties);
            var paraType = this.buildHandlers[builderType].ParameterDataType;
            return this.serializer.GetParameters(builderType, paraType, properties);
        }

        public string GetBuilderDescription(Type builderType)
        {
            Guard.NotNull(() => builderType, builderType);
            var handler = this.buildHandlers[builderType];
            return handler.Description;
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
        public IMultiBuildParameters GetMultiParameter(Guid key, Type builderType, IBuildDataDictionary properties)
        {
            Guard.NotNull(() => properties, properties);
            var paraType = this.buildHandlers[builderType].ParameterDataType;
            return ((BuilderSerializer)this.serializer).GetMultiParameter(key, builderType, paraType, properties);
        }

        /*/// <summary>
        /// Get the parameters for the specified builder type, possibly creating it, if there
        /// is not yet one in the build data collection.
        /// </summary>
        /// <param name="builderType">Type of the builder to request a set of sample data for.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="KeyNotFoundException">The given <paramref name="builderType"/> was not present in the lookup.</exception>
        public IMultiBuildParameters AddMultiBuilder(Type builderType, IBuildDataDictionary properties)
        {
            Guard.NotNull(() => properties, properties);
            var paraType = this.buildHandlers[builderType].ParameterDataType;
            // Todo: interface
            return ((BuilderSerializer)this.serializer).GetMultiParameter(Guid.NewGuid(), builderType, paraType, properties);
        }*/

        /// <summary>
        /// Gets the sample setup xml data for a specified <see cref="IMemberBuilder"/>.
        /// </summary>
        /// <param name="builderType">Type of the builder to request a set of sample data for.</param>
        /// <returns>
        /// A string containing the sample data for the specified <paramref name="builderType"/> as xml string.
        /// </returns>
        public string GetSampleSetupData(Type builderType)
        {
            var paraType = this.buildHandlers[builderType].ParameterDataType;
            return this.serializer.GetSampleSetupData(builderType, paraType);
        }

        /// <summary>
        /// Gets the xml data representation of all registered <see cref="IMemberBuilder"/>s parameters.
        /// </summary>
        /// <param name="properties">The properties storage which stores the <see cref="IMemberBuildParameters"/> data to serialize.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        public string SerializeAllSetupData(IBuildDataDictionary properties)
        {
            Guard.NotEmpty(() => properties, properties);
            var handlers = this.buildHandlers.Values.AsEnumerable();
            return this.serializer.SerializeAllHandlers(properties, handlers);

            // return NewMethod(properties, buildHandlers);
        }

        /// <summary>
        /// Gets the xml data representation of a single registered <see cref="IMemberBuilder"/>s parameters.
        /// </summary>
        /// <param name="builderType">Type of the builder to request a set of sample data for.</param>
        /// <param name="properties">The properties storage which stores the <see cref="IMemberBuildParameters"/> data to serialize.</param>
        /// <returns>
        /// The serialized data of the specified <paramref name="builderType"/>.
        /// </returns>
        public string SerializeSetupData(Type builderType, BuildDataDictionary properties)
        {
            var setupPara = this.GetParameters(builderType, properties);
            return this.serializer.SerializeParametersForBuilderType(builderType, setupPara);
        }

        public string SerializeSetupData(Guid key, Type builderType, BuildDataDictionary properties)
        {
            var setupPara = this.GetMultiParameter(key, builderType, properties);
            return this.serializer.SerializeParametersForBuilderType(builderType, setupPara);
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

            var handlers = this.buildHandlers.Values.AsEnumerable();
            IBuildHandler handler = this.serializer.DetermineIMemberBuilderFromXmlFragment(xml, handlers);
            return this.serializer.SetParameters(xml, properties, handler);
        }
    }
}