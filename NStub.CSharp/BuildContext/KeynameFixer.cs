// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeynameFixer.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.BuildContext
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.CodeDom;

    /// <summary>
    /// Fix the Test-Key of a MemberBuildContext.
    /// </summary>
    internal sealed class KeynameFixer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeynameFixer"/> class.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="typeMember">The type member.</param>
        public KeynameFixer(CodeNamespace codeNamespace,
            CodeTypeDeclaration testClassDeclaration,
            CodeTypeMember typeMember)
        {

        }

        /// <summary>
        /// Fixes the specified key.
        /// </summary>
        /// <param name="initial">The initial key value.</param>
        /// <returns>
        /// The fixed and normalized key.
        /// </returns>
        /// <remarks>
        /// At this time, simply "get_", "set_", "add_" and "remove_" is removed from the key, to
        /// allow build properties for both sides of CLR-Properties and CLR-Events.
        /// </remarks>
        public string Fix(string initial)
        {
            var result = initial;
            if (initial.StartsWith("get_"))
            {
                result = initial.Replace("get_", string.Empty);
            }
            else if (initial.StartsWith("set_"))
            {
                result = initial.Replace("set_", string.Empty);
            }
            else if (initial.StartsWith("add_"))
            {
                result = initial.Replace("add_", string.Empty);
            }
            else if (initial.StartsWith("remove_"))
            {
                result = initial.Replace("remove_", string.Empty);
            }

            return result;
        }
    }
}
