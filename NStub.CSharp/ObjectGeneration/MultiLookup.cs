// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiLookup.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System;

    /// <summary>
    /// Result storage class for <see cref="IMultiBuilder"/> type to <see cref="IMultiBuildParameters"/>
    /// relations.
    /// </summary>
    public class MultiLookup
    {
        #region Fields

        /// <summary>
        /// The exact type of the <see cref="IMultiBuilder"/>.
        /// </summary>
        public Type BuilderType;

        /// <summary>
        /// Available user data for the <see cref="BuilderType"/>.
        /// </summary>
        public IMultiBuildParameters Parameters;

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            // TODO: write your implementation of ToString() here
            return this.BuilderType + " {" + this.Parameters.Id + "}";
        }
    }
}