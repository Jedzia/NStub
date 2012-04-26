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
        /// <summary>
        /// Top parent in the tree. All branches start here.
        /// </summary>
        Root,
       
        /// <summary>
        /// The node identifies an assembly.
        /// </summary>
        Assembly,
       
        /// <summary>
        /// The node identifies a module of an assembly.
        /// </summary>
        Module,
       
        /// <summary>
        /// The node is associated with a class and provides a class type member info.
        /// </summary>
        Class,
        
        /// <summary>
        /// The node is associated with a method and provides a class method member info.
        /// </summary>
        Method
    }
}