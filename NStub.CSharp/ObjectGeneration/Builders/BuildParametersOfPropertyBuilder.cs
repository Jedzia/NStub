// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildParametersOfPropertyBuilder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    using System.ComponentModel;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents user provided data for the <see cref="PropertyBuilder"/> class.
    /// </summary>
    [Description("Whuut a fuckin attribute!")]
    public partial class BuildParametersOfPropertyBuilder
    {
        // : EntityBase<PropertyBuilderParametersSetup> {
        #region Properties

        /// <summary>
        /// Gets the sample XML data of this instance.
        /// </summary>
        [XmlIgnore]
        public override string SampleXml
        {
            get
            {
                // var sampleXmlData =
                // @"<PropertyBuilderParametersSetup xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" " +
                // @"xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">" + Environment.NewLine +
                // @"  <MethodSuffix>OlderDepp</MethodSuffix>" + Environment.NewLine +
                // @"  <UseDings>false</UseDings>" + Environment.NewLine +
                // @"  <Moep>0</Moep>" + Environment.NewLine +
                // @"  <Enabled>false</Enabled>" + Environment.NewLine +
                // @"</PropertyBuilderParametersSetup>" + Environment.NewLine +
                // string.Empty;
                return new BuildParametersOfPropertyBuilder().Serialize();

                // return sampleXmlData;
            }
        }

        #endregion

        /*/// <summary>
        /// Deserializes the specified XML to the current instance.
        /// </summary>
        /// <param name="xml">The XML text, representing the data.</param>
        public override void Deserialize(string xml)
        {
            //base.Deserialize(xml);
            PropertyBuilderUserParameters deserObj;
            Deserialize(xml, out deserObj);
            this.Enabled = deserObj.Enabled;
            this.MethodSuffix = deserObj.MethodSuffix;
            this.UseDings = deserObj.UseDings;
        }*/

        /// <summary>
        /// Determines whether this instance holds data for the specified builder type.
        /// </summary>
        /// <param name="builder">The requesting builder.</param>
        /// <returns>
        /// <c>true</c> if this instance holds data for the specified builder type; otherwise, <c>false</c>.
        /// </returns>
        public override bool HasDataForType(IMemberBuilder builder)
        {
            return builder is PropertyBuilder;
        }

        /// <summary>
        /// Sets the data via the specified method info.
        /// </summary>
        /// <param name="data">The data to store.</param>
        public override void SetData(object data)
        {
            // Guard.CanBeAssigned<string>(() => data, data);
            // var xml = (string)data;
        }
    }
}