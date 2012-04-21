// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyBuildParametersBase.cs" company="EvePanix">
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
    /// Base class for implementations of the <see cref="IMemberBuildParameters"/> user data with a Enabled property, serialization
    /// and deserialization capability.
    /// </summary>
    /// <typeparam name="T">Parameter data class</typeparam>
    public class EmptyBuildParametersBase<T> : BuildParametersBase<T>, IMemberBuildParameters
        where T : IMemberBuildParameters
    {
        // : EntityBase<PropertyBuilderParametersSetup> {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMemberBuildParameters"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [Description("Determines whether this builder part is ON or OFF.")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets the sample XML.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public virtual string SampleXml
        {
            get
            {
                return "<Test>This is sample data</Test>";
            }
        }

        #endregion

        /*/// <summary>
        /// Deserializes the specified XML to the current instance.
        /// </summary>
        /// <param name="xml">The XML text, representing the data.</param>
        public new virtual void Deserialize(string xml)
        {
            T deserObj;
            Deserialize(xml, out deserObj);
            this.Enabled = deserObj.Enabled;
        }*/

        /// <summary>
        /// Gets the data of this instance.
        /// </summary>
        /// <returns>
        /// The stored data.
        /// </returns>
        public virtual object GetData()
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
        public virtual bool HasDataForType(IMemberBuilder builder)
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
        public virtual void SetData(object data)
        {
        }
    }
}