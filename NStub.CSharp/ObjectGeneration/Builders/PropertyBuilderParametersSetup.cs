using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NStub.CSharp.ObjectGeneration.Builders
{
    public interface IBuilderSetupParameters
    {
        string SampleXml { get; }
    }

    public partial class PropertyBuilderParametersSetup : IBuilderSetupParameters //: EntityBase<PropertyBuilderParametersSetup> {
    {
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string SampleXml
        {
            get
            {
                var sampleXmlData = 
@"<PropertyBuilderParametersSetup xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" " +
@"xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">" + Environment.NewLine +
@"  <MethodSuffix>OlderDepp</MethodSuffix>" + Environment.NewLine +
@"</PropertyBuilderParametersSetup>" + Environment.NewLine +
@"";
                return sampleXmlData;
            }
        }
    }
}
