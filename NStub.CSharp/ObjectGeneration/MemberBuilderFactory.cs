// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestBuilderFactory.cs" company="EvePanix">
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
    using System.Linq;
    using System.Collections.Generic;
    using NStub.Core;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.ObjectGeneration.Builders;
    using System.Xml;
    using System.IO;

    /// <summary>
    /// Provides builders for test method generation. 
    /// </summary>
    public abstract class MemberBuilderFactory : IMemberBuilderFactory
    {
        #region Fields

        private readonly Dictionary<Type, IBuildHandler> handlers = new Dictionary<Type, IBuildHandler>();
        private static IMemberBuilderFactory defaultfactory;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default <see cref="TestBuilderFactory"/>.
        /// </summary>
        /// <value>
        /// The default <see cref="TestBuilderFactory"/>.
        /// </value>
        /// <remarks>
        /// <para>The default <see cref="TestBuilderFactory"/> can only set once, and that before accessing it.</para>
        /// <para>To achieve a different behavior for the functionality provided by another implementation
        /// of an <see cref="ITestBuilderFactory"/>, derive from this class and override the abstract 
        /// <see cref="TestBuilderFactory.Factory"/> getter.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Cannot set the default <see cref="ITestBuilderFactory"/> twice. Maybe you accessed it before you've written to it.</exception>
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
        /// Gets the factory service of this instance.
        /// </summary>
        public abstract IMemberBuilderFactory Factory { get; }

        /// <summary>
        /// Gets the name of the builders.
        /// </summary>
        public virtual IEnumerable<string> BuilderNames
        {
            get
            {
                var result = handlers.Values.Select(e => e.Type.FullName);
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
                var result = handlers.Values.Select(e => e.Type);
                return result;
            }
        }


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
            // Todo: maybe cache em.
            foreach(var buildHandler in this.handlers.Values)
            {
                var canHandleContext = buildHandler.CanHandle(context);
                if (!canHandleContext)
                {
                    continue;
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
        /// <returns>A new instance of a matching parameter data set for the specified builder.</returns>
        /// <exception cref="KeyNotFoundException">The given builderType was not present in the lookup.</exception>
        public IBuilderSetupParameters GetParameters(Type builderType, BuildDataCollection globalProperties)
        {
            IBuilderData result;
            var found = globalProperties.TryGetValue("" + builderType.FullName, out result);
            if (found)
            {
                return result as IBuilderSetupParameters;
            }

            var paraType = handlers[builderType].ParameterDataType;
            var paraInstance = Activator.CreateInstance(paraType);
            var setupPara = (IBuilderSetupParameters)paraInstance;

            globalProperties.AddDataItem("" + builderType.FullName, setupPara);
            return setupPara;
        }

        public string SerializeSetupData(Type builderType, BuildDataCollection globalProperties)
        {
            var setupPara = GetParameters(builderType, globalProperties);
            return SerializeParametersForBuilderType(builderType, setupPara);
        }

        private static string SerializeParametersForBuilderType(Type builderType, IBuilderSetupParameters setupPara)
        {
            return SerializeParametersForBuilderType(builderType, setupPara.GetType().Name, setupPara.Serialize());
        }

        private static string SerializeParametersForBuilderType(Type builderType, string setupInnerName, string setupInnerData)
        {
            //var setupParaType = setupPara.GetType();
            var xmlDoc = new XmlDocument();
            var ele = xmlDoc.CreateElement(builderType.FullName);
            var ele2 = xmlDoc.CreateElement(setupInnerName);
            xmlDoc.AppendChild(ele);
            ele.AppendChild(ele2);

            try
            {
                var innerDoc = new XmlDocument();
                innerDoc.LoadXml(setupInnerData);
                ele2.InnerXml = innerDoc[setupInnerName].InnerXml;
            }
            catch (Exception ex)
            {

                //throw;
            }
            return PrettyPrintXml(xmlDoc.OuterXml);
        }

        public string GetSampleSetupData(Type builderType)
        {
            var paraType = handlers[builderType].ParameterDataType;
            var paraInstance = Activator.CreateInstance(paraType);
            var setupPara = (IBuilderSetupParameters)paraInstance;

            return SerializeParametersForBuilderType(builderType, setupPara.GetType().Name, setupPara.SampleXml);
        }

        /// <summary>
        /// Pretty print XML data.
        /// </summary>
        /// <param name="xml">The string containing valid XML data.</param>
        /// <returns>The xml data in idented and justified form.</returns>
        private static string PrettyPrintXml(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Normalize();

            TextWriter wr = new StringWriter();
            doc.Save(wr);
            var str = wr.ToString();
            return str;
        }

        public IBuilderSetupParameters SetParameters(string xml, BuildDataCollection globalProperties)
        {
            //<NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var fc = doc.FirstChild;
            var fcName = fc.Name;

            //var asd = handlers;

            IBuildHandler result = null;
            foreach (var item in handlers.Values)
            {
                if (item.Type.FullName == fcName)
                {
                    result = item;
                }
            }

            if (result == null)
            {
                // Todo: or throw?
                return MemberBuilder.EmptyParameters;
            }

            var paraType = result.ParameterDataType;
            var paraInstance = Activator.CreateInstance(paraType);
            var setupPara = (IBuilderSetupParameters)paraInstance;
            setupPara.Deserialize(fc.InnerXml);

            globalProperties.AddDataItem("" + result.Type.FullName, setupPara);
            return setupPara;
            
            //var found = handlers.TryGetValue(fcName, out result);
            //if (found)
            {
                //    return result as IBuilderSetupParameters;
            }
            //IBuilderData result;
            //var found = globalProperties.TryGetValue("" + builderType.FullName, out result);
            //if (found)
            {
            //    return result as IBuilderSetupParameters;
            }

            /*var paraType = handlers[builderType].ParameterDataType;
            var paraInstance = Activator.CreateInstance(paraType);
            var setupPara = (IBuilderSetupParameters)paraInstance;

            globalProperties.AddDataItem("" + builderType.FullName, setupPara);
            return setupPara;*/

            //return null;
        }


    }
}