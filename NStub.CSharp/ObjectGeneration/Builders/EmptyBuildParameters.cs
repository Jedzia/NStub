// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyBuildParameters.cs" company="EvePanix">
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
    using System.Xml;
    using System.Xml.Serialization;
    using System.ComponentModel;

    /// <summary>
    /// Provides an implementation of the <see cref="IMemberBuildParameters"/> user data, that has an empty set of data.
    /// </summary>
    public class EmptyBuildParameters : EmptyBuildParametersBase<EmptyBuildParameters>, IMemberBuildParameters
    {
    }

    public class EmptyMultiBuildParameters : EmptyBuildParametersBase<EmptyMultiBuildParameters>, IMultiBuildParameters
    {
        private Guid id;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EmptyMultiBuildParameters"/> class.
        /// </summary>
        public EmptyMultiBuildParameters()
        {
            
        }

        /// <summary>
        /// Gets the identification of the Builder.
        /// </summary>
        //[XmlAttribute( AttributeName="Depp", DataType="Guid")]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [Browsable(false)]
        public Guid Id //{ get; set; }
        {
            get
            {
                if (id == null)
                {
                    id = Guid.NewGuid();
                }
                return this.id;
            }

            set
            {
                id = value;
            }
        }
    }

}