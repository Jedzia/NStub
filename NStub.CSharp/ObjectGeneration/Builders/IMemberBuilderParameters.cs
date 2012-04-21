// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberBuilderParameters.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    /// <summary>
    /// Stores information about user provided parameters of a test method for an <see cref="IMemberBuilder"/>.
    /// </summary>
    public interface IMemberBuilderParameters : IBuilderData
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IMemberBuilderParameters"/> is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets the sample XML.
        /// </summary>
        string SampleXml { get; }

        #endregion

        /// <summary>
        /// Deserializes the specified XML to the current instance.
        /// </summary>
        /// <param name="xml">The XML text, representing the data.</param>
        //void Deserialize(string xml);

        /// <summary>
        /// Serializes this instance to a XML string.
        /// </summary>
        /// <returns>The data of the current instance as xml text.</returns>
        string Serialize();
    }
}