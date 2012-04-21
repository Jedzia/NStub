// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBuilderUserParameters.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

// <NameSpace>NStub.CSharp.ObjectGeneration.Builders</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>True</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>True</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>True</IncludeSerializeMethod><UseBaseClass>True</UseBaseClass><GenBaseClass>False</GenBaseClass><BaseClassName>EmptyBuildParametersBase</BaseClassName><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net35</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>False</GenerateXMLAttributes><EnableEncoding>False</EnableEncoding><AutomaticProperties>True</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>True</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>ASCII</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>

namespace NStub.CSharp.ObjectGeneration.Builders
{
    using System.ComponentModel;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents user provided data for the <see cref="PropertyBuilder"/> class.
    /// </summary>
    [Description("Whuut a fuckin attribute!")]
    public partial class BuildParametersOfPropertyBuilder : IMemberBuildParameters
    {
        // : EntityBase<PropertyBuilderParametersSetup> {
        #region Properties

        /// <summary>
        /// Gets the sample XML data of this instance.
        /// </summary>
        [XmlIgnore]
        public string SampleXml
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
        /// Gets the data of this instance.
        /// </summary>
        /// <returns>
        /// The stored data.
        /// </returns>
        public object GetData()
        {
            return this;
        }

        /// <summary>
        /// Determines whether this instance holds data for the specified builder type.
        /// </summary>
        /// <param name="builder">The requesting builder.</param>
        /// <returns>
        /// <c>true</c> if this instance holds data for the specified builder type; otherwise, <c>false</c>.
        /// </returns>
        public bool HasDataForType(IMemberBuilder builder)
        {
            return builder is PropertyBuilder;
        }

        /// <summary>
        /// Sets the data via the specified method info.
        /// </summary>
        /// <param name="data">The data to store.</param>
        public void SetData(object data)
        {
            // Guard.CanBeAssigned<string>(() => data, data);
            // var xml = (string)data;
        }
    }
}