// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberVisibility.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Core
{
    using System;

    /// <summary>
    /// Type member visibility.
    /// </summary>
    // [Flags]
    public enum MemberVisibility
    {
        /// <summary>
        /// All public types.
        /// </summary>
        Public = 1 << 0, 

        /// <summary>
        /// All internal types.
        /// </summary>
        Internal = 1 << 1, 

        /// <summary>
        /// All private types.
        /// </summary>
        Private = 1 << 2, 
    }
}