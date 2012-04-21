using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using NStub.Core;

namespace NStub.CSharp.ObjectGeneration.Builders
{
    public interface IBuilderSerializer
    {

        /// <summary>
        /// Set the parameters in the properties storage from a specified xml representation of the data.
        /// </summary>
        /// <param name="xml">The xml representation of the data.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <param name="handlers">The list of existing handlers to investigate.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="InvalidCastException"><c>InvalidCastException</c> Problem building from serialization data.</exception>
        IEnumerable<IMemberBuilderParameters> DeserializeAllSetupData(
            string xml,
            IBuildDataDictionary properties,
            IEnumerable<IBuildHandler> handlers);

        /// <summary>
        /// Determines the matching member builder by the first childs name from a XML fragment.
        /// </summary>
        /// <param name="xml">The xml fragment.</param>
        /// <param name="handlers">The list of existing handlers to investigate.</param>
        /// <returns>The matching handler or <c>null</c> if none is found.</returns>
        IBuildHandler DetermineIMemberBuilderFromXmlFragment(string xml, IEnumerable<IBuildHandler> handlers);


        /// <summary>
        /// Set the parameters in the properties storage from a specified xml representation of the data.
        /// </summary>
        /// <param name="xml">The xml representation of the data.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <param name="handler">The handler that holds the builder- to parameter-type relation.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="InvalidCastException"><c>InvalidCastException</c> Problem building from serialization data.</exception>
        IMemberBuilderParameters SetParameters(string xml, IBuildDataDictionary properties, IBuildHandler handler);

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
        IMemberBuilderParameters GetParameters(Type builderType, Type paraType, IBuildDataDictionary properties);


        /// <summary>
        /// Gets the xml data representation of all registered <see cref="IBuildHandler"/>s.
        /// </summary>
        /// <param name="properties">The properties storage which stores the <see cref="IMemberBuilderParameters"/> data to serialize.</param>
        /// <param name="handlers">The handlers with the links to the <see cref="IMemberBuilder"/> types.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        string SerializeAllHandlers(IBuildDataDictionary properties, IEnumerable<IBuildHandler> handlers);

        /// <summary>
        /// Gets the sample setup xml data for a specified <see cref="IMemberBuilder"/>.
        /// </summary>
        /// <param name="builderType">Type of the builder to request a set of sample data for.</param>
        /// <returns>
        /// A string containing the sample data for the specified <paramref name="builderType"/> as xml string.
        /// </returns>
        string GetSampleSetupData(Type builderType, Type paraType);

        /// <summary>
        /// Gets the xml data representation of a single registered <see cref="IMemberBuilder"/>s parameters.
        /// </summary>
        /// <param name="builderType">Type of the builder to request data for.</param>
        /// <param name="parameters">The parameters with the data to serialize.</param>
        /// <returns>
        /// The serialized data of the specified <paramref name="builderType"/>.
        /// </returns>
        string SerializeParametersForBuilderType(Type builderType, IMemberBuilderParameters parameters);
    }

    internal sealed class BuilderSerializer : IBuilderSerializer
    {

        /// <summary>
        /// Set the parameters in the properties storage from a specified xml representation of the data.
        /// </summary>
        /// <param name="xml">The xml representation of the data.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <param name="handlers">The list of existing handlers to investigate.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="InvalidCastException"><c>InvalidCastException</c> Problem building from serialization data.</exception>
        public IEnumerable<IMemberBuilderParameters> DeserializeAllSetupData(string xml, IBuildDataDictionary properties, IEnumerable<IBuildHandler> handlers)
        {
            // <NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>
            List<IMemberBuilderParameters> plist = new List<IMemberBuilderParameters>();
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            foreach (XmlElement item in doc[BuilderConstants.BuilderParametersXmlId])
            {
                IBuildHandler handler = DetermineIMemberBuilderFromXmlFragment(item.OuterXml, handlers);
                var para = SetParameters(item.OuterXml, properties, handler);

                plist.Add(para);

                // yield return para;
            }

            return plist;
        }

        /// <summary>
        /// Determines the matching member builder by the first childs name from a XML fragment.
        /// </summary>
        /// <param name="xml">The xml fragment.</param>
        /// <param name="handlers">The list of existing handlers to investigate.</param>
        /// <returns>The matching handler or <c>null</c> if none is found.</returns>
        public IBuildHandler DetermineIMemberBuilderFromXmlFragment(string xml, IEnumerable<IBuildHandler> handlers)
        {
            // <NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var firstChild = doc.FirstChild;

            // var asd = handlers;
            IBuildHandler handler = null;
            foreach (var item in handlers)
            {
                if (item.Type.FullName == firstChild.Name)
                {
                    handler = item;
                }
            }
            return handler;
        }


        /// <summary>
        /// Set the parameters in the properties storage from a specified xml representation of the data.
        /// </summary>
        /// <param name="xml">The xml representation of the data.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <param name="handler">The handler that holds the builder- to parameter-type relation.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="InvalidCastException"><c>InvalidCastException</c> Problem building from serialization data.</exception>
        public IMemberBuilderParameters SetParameters(string xml, IBuildDataDictionary properties, IBuildHandler handler)
        {
            Guard.NotNull(() => properties, properties);
            if (handler == null)
            {
                // Todo: or throw?
                return MemberBuilder.EmptyParameters;
            }

            // <NStub.CSharp.ObjectGeneration.Builders.PropertyBuilder>
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var firstChild = doc.FirstChild;


            var paraType = handler.ParameterDataType;

            var xxxx = paraType.BaseType.GetGenericTypeDefinition();
            var serializer2 = xxxx.BaseType.GetGenericTypeDefinition();

            // Todo: This strange thingy ... very good checking and logging! :)
            var paraInstance = serializer2
                .MakeGenericType(paraType)
                .GetMethod("Deserialize", new Type[] { typeof(string) })
                .Invoke(null, new object[] { firstChild.InnerXml });

            var setupPara = (IMemberBuilderParameters)paraInstance;
            try
            {
                var propertyKey = string.Empty + handler.Type.FullName;

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
                    handler.Type.FullName,
                    Environment.NewLine,
                    firstChild.InnerXml,
                    Environment.NewLine);
                throw new InvalidCastException(message, ex);
            }

            return setupPara;
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
        public IMemberBuilderParameters GetParameters(Type builderType, Type paraType, IBuildDataDictionary properties)
        {
            Guard.NotNull(() => properties, properties);
            IBuilderData result;
            var found = properties.TryGetValue(string.Empty + builderType.FullName, out result);
            if (found)
            {
                return result as IMemberBuilderParameters;
            }

            var paraInstance = Activator.CreateInstance(paraType);
            var setupPara = (IMemberBuilderParameters)paraInstance;

            properties.AddDataItem(string.Empty + builderType.FullName, setupPara);
            return setupPara;
        }


        /// <summary>
        /// Gets the xml data representation of all registered <see cref="IBuildHandler"/>s.
        /// </summary>
        /// <param name="properties">The properties storage which stores the <see cref="IMemberBuilderParameters"/> data to serialize.</param>
        /// <param name="handlers">The handlers with the links to the <see cref="IMemberBuilder"/> types.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        public string SerializeAllHandlers(IBuildDataDictionary properties, IEnumerable<IBuildHandler> handlers)
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
                var setupPara = GetParameters(handler.Type, handler.ParameterDataType, properties);

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
        }

        /// <summary>
        /// Gets the sample setup xml data for a specified <see cref="IMemberBuilder"/>.
        /// </summary>
        /// <param name="builderType">Type of the builder to request a set of sample data for.</param>
        /// <returns>
        /// A string containing the sample data for the specified <paramref name="builderType"/> as xml string.
        /// </returns>
        public string GetSampleSetupData(Type builderType, Type paraType)
        {
            var paraInstance = Activator.CreateInstance(paraType);
            var setupPara = (IMemberBuilderParameters)paraInstance;

            return SerializeParametersForBuilderType(builderType, setupPara.GetType().Name, setupPara.SampleXml);
        }

        /// <summary>
        /// Gets the xml data representation of a single registered <see cref="IMemberBuilder"/>s parameters.
        /// </summary>
        /// <param name="builderType">Type of the builder to request data for.</param>
        /// <param name="parameters">The parameters with the data to serialize.</param>
        /// <returns>
        /// The serialized data of the specified <paramref name="builderType"/>.
        /// </returns>
        public string SerializeParametersForBuilderType(Type builderType, IMemberBuilderParameters parameters)
        {
            return SerializeParametersForBuilderType(builderType, parameters.GetType().Name, parameters.Serialize());
        }

        /// <summary>
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
        }

        /// <summary>
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
        }

    }
}
