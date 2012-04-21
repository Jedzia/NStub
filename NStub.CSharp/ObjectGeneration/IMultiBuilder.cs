// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMultiBuilder.cs" company="EvePanix">
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
    /// Implements a test method generator for type members.
    /// </summary>
    public interface IMultiBuilder : IMemberBuilder
    {
        #region Properties

        /// <summary>
        /// Gets the identification number of this instance.
        /// </summary>
        Guid Id { get; }

        #endregion
    }
}