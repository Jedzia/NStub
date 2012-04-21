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
    //using System.IO;
    using System.Linq;
    using System.Reflection;
    //using System.Xml;
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
        private readonly BuilderSerializer serializer;
        private static IMemberBuilderFactory defaultfactory;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MemberBuilderFactory"/> class.
        /// </summary>
        protected MemberBuilderFactory()
        {
            this.serializer = new BuilderSerializer();
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

            return GetBuilder(context, true);
            // Todo: maybe cache em.
            /*foreach (var buildHandler in this.handlers.Values)
            {
                var canHandleContext = buildHandler.CanHandle(context);
                if (!canHandleContext)
                {
                    continue;
                }

                var memberBuilder = buildHandler.CreateInstance(context);
                yield return memberBuilder;
            }*/
        }

        /// <summary>
        /// Tries to get the builder for the specified context dependant on user property activation.
        /// </summary>
        /// <param name="context">The context of the current test object.</param>
        /// <param name="useUserActivation">if set to <c>true</c> use user activation in context.BuildData stored values.</param>
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
        public IMemberBuilderParameters GetParameters(Type builderType, IBuildDataDictionary properties)
        {
            Guard.NotNull(() => properties, properties);
            var paraType = this.handlers[builderType].ParameterDataType;
            return this.serializer.GetParameters(builderType, paraType, properties);

            /*IBuilderData result;
            var found = properties.TryGetValue(string.Empty + builderType.FullName, out result);
            if (found)
            {
                return result as IMemberBuilderParameters;
            }

            var paraType = this.handlers[builderType].ParameterDataType;
            var paraInstance = Activator.CreateInstance(paraType);
            var setupPara = (IMemberBuilderParameters)paraInstance;

            properties.AddDataItem(string.Empty + builderType.FullName, setupPara);
            return setupPara;*/
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

            /*var paraInstance = Activator.CreateInstance(paraType);
            var setupPara = (IMemberBuilderParameters)paraInstance;

            return SerializeParametersForBuilderType(builderType, setupPara.GetType().Name, setupPara.SampleXml);*/
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

        /*private string NewMethod(IBuildDataDictionary properties, IEnumerable<IBuildHandler> handlers)
        {
            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement(BuilderConstants.BuilderParametersXmlId);
            xmlDoc.AppendChild(root);
            foreach (var handler in handlers)
            {
                if (handler.Type == null || handler.Type.FullName == null)
                {
                    continue;
                }
                var setupPara = this.GetParameters(handler.Type, properties);

                var setupParaType = handler.ParameterDataType;
                var setupParaXml = setupPara.Serialize();

                var ele = xmlDoc.CreateElement(handler.Type.FullName);
                var ele2 = xmlDoc.CreateElement(setupParaType.Name);
                root.AppendChild(ele);
                ele.AppendChild(ele2);
                var innerDoc = new XmlDocument();
                var xml = setupParaXml;
                innerDoc.LoadXml(xml);
                ele2.InnerXml = innerDoc[setupParaType.Name].InnerXml;
            }

            return PrettyPrintXml(xmlDoc.OuterXml);
        }*/

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
        public IEnumerable<IMemberBuilderParameters> DeserializeAllSetupData(string xml, IBuildDataDictionary properties)
        {
            var handlers = this.handlers.Values.AsEnumerable();
            return serializer.DeserializeAllSetupData(xml, properties, handlers);

            // <NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>
            /*List<IMemberBuilderParameters> plist = new List<IMemberBuilderParameters>();
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            foreach (XmlElement item in doc[BuilderConstants.BuilderParametersXmlId])
            {
                var para = SetParameters(item.OuterXml, properties);
                plist.Add(para);
                
                // yield return para;
            }

            return plist;*/
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
        public IMemberBuilderParameters SetParameters(string xml, IBuildDataDictionary properties)
        {
            Guard.NotNull(() => properties, properties);
            
            var handlers = this.handlers.Values.AsEnumerable();
            IBuildHandler handler = serializer.DetermineIMemberBuilderFromXmlFragment(xml, handlers);
            return serializer.SetParameters(xml, properties, handler);
            
            /*if (result == null)
            {
                // Todo: or throw?
                return MemberBuilder.EmptyParameters;
            }

            var paraType = result.ParameterDataType;

            var xxxx = paraType.BaseType.GetGenericTypeDefinition();
            var serializer2 = xxxx.BaseType.GetGenericTypeDefinition();

            var paraInstance = serializer2
                .MakeGenericType(paraType)
                .GetMethod("Deserialize", new Type[] { typeof(string) })
                .Invoke(null, new object[] { firstChild.InnerXml });

            var setupPara = (IMemberBuilderParameters)paraInstance;
            try
            {
                var propertyKey = string.Empty + result.Type.FullName;

                // IBuilderData property;
                // var found = properties.TryGetValue(propertyKey, out property);
                // if (found)
                // {
                properties.AddDataItem(propertyKey, setupPara, true);
               
                // return setupPara;
                // }
                // properties.AddDataItem(propertyKey, setupPara);
            }
            catch (Exception ex)
            {
                var message = string.Format(
                    "Problem building {0} from serialization data.{1}{2}{3}",
                    result.Type.FullName,
                    Environment.NewLine,
                    firstChild.InnerXml,
                    Environment.NewLine);
                throw new InvalidCastException(message, ex);
            }

            return setupPara;*/
        }

        /*/// <summary>
        /// Pretty print XML data.
        /// </summary>
        /// <param name="xml">The string containing valid XML data.</param>
        /// <returns>The xml data in indented and justified form.</returns>
        private static string PrettyPrintXml(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Normalize();

            TextWriter wr = new StringWriter();
            doc.Save(wr);
            var str = wr.ToString();
            return str;
        }*/

        /*/// <summary>
        /// Gets the xml data representation of a single registered <see cref="IMemberBuilder"/>s parameters.
        /// </summary>
        /// <param name="builderType">Type of the builder to request data for.</param>
        /// <param name="parameters">The parameters with the data to serialize.</param>
        /// <returns>
        /// The serialized data of the specified <paramref name="builderType"/>.
        /// </returns>
        private static string SerializeParametersForBuilderType(Type builderType, IMemberBuilderParameters parameters)
        {
            return SerializeParametersForBuilderType(builderType, parameters.GetType().Name, parameters.Serialize());
        }*/

        /*/// <summary>
        /// Gets the xml data representation of a single registered <see cref="IMemberBuilder"/>s parameters.
        /// </summary>
        /// <param name="builderType">Type of the builder to request data for.</param>
        /// <param name="parameterTypeShortName">Short name of the parameter type.</param>
        /// <param name="parameterTypeXmlData">The data of the parameter type as XML representation.</param>
        /// <returns>
        /// The serialized data of the specified <paramref name="builderType"/>.
        /// </returns>
        private static string SerializeParametersForBuilderType(
            Type builderType, string parameterTypeShortName, string parameterTypeXmlData)
        {
            var xmlDoc = new XmlDocument();
            if (builderType != null && builderType.FullName != null)
            {
                var element = xmlDoc.CreateElement(builderType.FullName);
                var innerElement = xmlDoc.CreateElement(parameterTypeShortName);
                xmlDoc.AppendChild(element);
                element.AppendChild(innerElement);
                var innerDoc = new XmlDocument();
                innerDoc.LoadXml(parameterTypeXmlData);
                if (parameterTypeShortName != null)
                {
                    innerElement.InnerXml = innerDoc[parameterTypeShortName].InnerXml;
                }
            }

            return PrettyPrintXml(xmlDoc.OuterXml);
        }*/
    }
}