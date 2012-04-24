// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMultiBuildParameters.cs" company="EvePanix">
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
    /// Stores information about user provided parameters of a test method for an <see cref="IMultiBuilder"/>.
    /// </summary>
    public interface IMultiBuildParameters : IMemberBuildParameters
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identification of the Builder.
        /// </summary>
        Guid Id { get; set; }

        #endregion
    }
}