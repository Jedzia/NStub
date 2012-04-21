// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyBuilderParameters.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
namespace NStub.CSharp.ObjectGeneration.Builders
{
    /// <summary>
    /// Provides an implementation of the <see cref="IMemberBuilderParameters"/> user data, that has an empty set of data.
    /// </summary>
    public class EmptyBuilderParameters : EntityBase<EmptyBuilderParameters>, IMemberBuilderParameters
    {
        // : EntityBase<PropertyBuilderParametersSetup> {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMemberBuilderParameters"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets the sample XML.
        /// </summary>
        [XmlIgnore]
        public string SampleXml
        {
            get
            {
                return "<Test>This is sample data</Test>";
            }
        }

        #endregion

        /// <summary>
        /// Deserializes the specified XML to the current instance.
        /// </summary>
        /// <param name="xml">The XML text, representing the data.</param>
        public new void Deserialize(string xml)
        {
            EmptyBuilderParameters deserObj;
            Deserialize(xml, out deserObj);
            this.Enabled = deserObj.Enabled;
        }

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
        ///   <c>true</c> if this instance holds data for the specified builder type; otherwise, <c>false</c>.
        /// </returns>
        public bool HasDataForType(IMemberBuilder builder)
        {
            return false;
        }

        /*/// <summary>
        /// Serializes this instance to a XML string.
        /// </summary>
        /// <returns>
        /// The data of the current instance as xml text.
        /// </returns>
        public string Serialize()
        {
            return "<EmptyBuilderParameters />";
        }*/

        /// <summary>
        /// Sets the data via the specified method info.
        /// </summary>
        /// <param name="data">The data to store.</param>
        public void SetData(object data)
        {
        }
    }
}