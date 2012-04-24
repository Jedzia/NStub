// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberBuildResult.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.CodeDom;
namespace NStub.CSharp.ObjectGeneration
{
    /// <summary>
    /// Provides feedback in the pre build phase of test object generation.
    /// </summary>
    public interface IMemberPreBuildResult
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to exclude the member from the test generation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if excluding the member from the test generation; otherwise, <c>false</c>.
        /// </value>
        bool ExcludeMember { get; set; }


        #endregion
    }

    /// <summary>
    /// Provides feedback in the build phase of test object generation.
    /// </summary>
    public interface IMemberBuildResult : IMemberPreBuildResult
    {
        /// <summary>
        /// Gets the class methods to add to the test class under Build-Phase.
        /// </summary>
        ICollection<CodeMemberMethod> ClassMethodsToAdd { get; }
    }
}