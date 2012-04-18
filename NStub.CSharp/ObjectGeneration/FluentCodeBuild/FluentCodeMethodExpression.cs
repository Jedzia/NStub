// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentCodeMethodExpression.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.FluentCodeBuild
{
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides Fluent <see cref="CodeMemberMethod"/> investigation.
    /// </summary>
    public static class FluentCodeMethodExpression
    {
        /// <summary>
        /// Get the statements of a method by the specified type.
        /// </summary>
        /// <typeparam name="T">The matching statement type.</typeparam>
        /// <param name="method">The method to check.</param>
        /// <returns>
        /// A Linq-Expression for use in <i>Assert.That</i>, checking the truth of the assertion.
        /// </returns>
        public static IEnumerable<T> StatementsOfType<T>(this CodeMemberMethod method)
        {
            /*if (method.Statements.Count == 0)
            {
                throw new AssertionException("The method's statement list is empty. Can't find a '" +
                    typeof(T).ToString() + "' type on a method with no statements.");
            }*/
            IEnumerable<T> returnValue = method.Statements.OfType<T>();
            return returnValue;
        }
    }
}