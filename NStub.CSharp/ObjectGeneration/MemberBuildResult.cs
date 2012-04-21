using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NStub.CSharp.ObjectGeneration
{

    /// <summary>
    /// Feedback object for the pre build phase of the object generation.
    /// </summary>
    internal class MemberBuildResult : IMemberBuildResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether to exclude the member from the test generation.
        /// </summary>
        /// <value>
        /// <c>true</c> if excluding the member from the test generation; otherwise, <c>false</c>.
        /// </value>
        public bool ExcludeMember { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberBuildResult"/> class.
        /// </summary>
        public MemberBuildResult()
        {

        }
    }
}
