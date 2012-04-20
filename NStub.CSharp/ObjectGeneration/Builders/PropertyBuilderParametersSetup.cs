using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NStub.Core;

namespace NStub.CSharp.ObjectGeneration.Builders
{
    public interface IBuilderSetupParameters : IBuilderData
    {
        /// <summary>
        /// Gets the sample XML.
        /// </summary>
        string SampleXml { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IBuilderSetupParameters"/> is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        bool Enabled { get; set; }

        void Deserialize(string xml);
        string Serialize();
    }
    
    // TestBuilderParameters
    public class EmptyBuilderParameters : IBuilderSetupParameters //: EntityBase<PropertyBuilderParametersSetup> {
    {
        #region IBuilderSetupParameters Members

        public string SampleXml
        {
            get { return "<Test>This is sample data</Test>"; }
        }

        public bool Enabled
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        #endregion

        #region IBuilderData Members

 
        public object GetData()
        {
            return this;
        }

        public bool HasDataForType(IMemberBuilder builder)
        {
            return false;
        }

        public void SetData(object data)
        {
        }

        #endregion

        #region IBuilderSetupParameters Members


        public void Deserialize(string xml)
        {
        }

        public string Serialize()
        {
            return string.Empty;
        }

        #endregion
    }
    public partial class PropertyBuilderUserParameters : IBuilderSetupParameters //: EntityBase<PropertyBuilderParametersSetup> {
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
@"  <UseDings>false</UseDings>" + Environment.NewLine +
@"  <Moep>0</Moep>" + Environment.NewLine +
@"  <Enabled>false</Enabled>" + Environment.NewLine +
@"</PropertyBuilderParametersSetup>" + Environment.NewLine +
@"";
                return new PropertyBuilderUserParameters().Serialize();
                //return sampleXmlData;
            }
        }

        #region IBuilderData Members

        public object GetData()
        {
            return this;
        }

        public bool HasDataForType(IMemberBuilder builder)
        {
            return builder is PropertyBuilder;
        }

        public void SetData(object data)
        {
            //Guard.CanBeAssigned<string>(() => data, data);
            //var xml = (string)data;
        }

        #endregion

        #region IBuilderSetupParameters Members


        public new void Deserialize(string xml)
        {
            PropertyBuilderUserParameters deserObj;
            var rdata = PropertyBuilderUserParameters.Deserialize(xml, out deserObj);
            this.Enabled = deserObj.Enabled;
            this.MethodSuffix = deserObj.MethodSuffix;
            this.UseDings = deserObj.UseDings;
        }

        #endregion
    }
}
