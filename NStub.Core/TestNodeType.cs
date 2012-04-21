// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestNodeType.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Core
{
    /// <summary>
    /// Defines the data type of a <see cref="TestNode"/>.
    /// </summary>
    public enum TestNodeType
    {
        Root, 
        Assembly, 
        Module, 
        Class, 
        Method
    }
}