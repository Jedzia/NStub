// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberBuildResult.cs" company="EvePanix">
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
    /// Feedback object for the pre build phase of the object generation.
    /// </summary>
    internal class MemberBuildResult : IMemberBuildResult
    {
        #region Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MemberBuildResult"/> class.
        /// </summary>
        public MemberBuildResult()
        {
            ClassMethodsToAdd = new List<CodeMemberMethod>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether to exclude the member from the test generation.
        /// </summary>
        /// <value>
        /// <c>true</c> if excluding the member from the test generation; otherwise, <c>false</c>.
        /// </value>
        public bool ExcludeMember { get; set; }

        /// <summary>
        /// Gets the class methods to add to the test class under Build-Phase.
        /// </summary>
        public ICollection<CodeMemberMethod> ClassMethodsToAdd { get; private set; }

        #endregion

        public virtual void Reset()
        {
            ExcludeMember = false;
            ClassMethodsToAdd.Clear();
        }
    }
}