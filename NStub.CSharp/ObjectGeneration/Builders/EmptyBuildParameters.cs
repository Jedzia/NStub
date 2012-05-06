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
    using System.ComponentModel;
    using System.Xml.Serialization;
    using NStub.Core;

    /// <summary>
    /// Provides an implementation of the <see cref="IMemberBuildParameters"/> user data, that has an empty set of data.
    /// </summary>
    [Description("Default static parameter set.")]
    public class EmptyBuildParameters : EmptyBuildParametersBase<EmptyBuildParameters>
    {
        // Todo: rename the class name to singular .. look for the others of this kind. Beware: is used in the users parameter file.
    }

    /// <summary>
    /// Provides an implementation of the <see cref="IMemberBuildParameters"/> user data, that has a string value.
    /// </summary>
    [Description("Default static string parameter.")]
    public class StringConstantBuildParameter : EmptyBuildParametersBase<StringConstantBuildParameter>
    {
        private string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:StringConstantBuildParameter"/> class.
        /// </summary>
        public StringConstantBuildParameter(string value)
        {
            Guard.NotNull(() => value, value);
            this.value = value;
        }

        /// <summary>
        /// Gets or sets the value of this parameter.
        /// </summary>
        // [XmlAttribute, Browsable(false)]
        public string Value
        {
            get
            {
                if (this.value == null)
                {
                    this.value = string.Empty;
                }

                return this.value;
            }

            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return value;
        }
    }

}