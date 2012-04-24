// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyMultiBuildParametersBase.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    /// <summary>
    /// Base class for implementations of the <see cref="IMultiBuildParameters"/> user data with a Enabled property, a GUID
    /// identifier, serialization and deserialization capability.
    /// </summary>
    /// <typeparam name="T">Parameter data class</typeparam>
    public class EmptyMultiBuildParametersBase<T> : EmptyBuildParametersBase<T>, IMultiBuildParameters
        where T : IMultiBuildParameters
    {
        // : EntityBase<PropertyBuilderParametersSetup> {
        #region Fields

        private Guid id;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the identification of the Builder.
        /// </summary>
        [XmlAttribute, Browsable(false)]
        public Guid Id
        {
            // { get; set; }
            get
            {
                // [XmlAttribute( AttributeName="Depp", DataType="Guid")]
                if (this.id == Guid.Empty)
                {
                    this.id = Guid.NewGuid();
                }

                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        #endregion
    }
}