// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImplicitVariableDeclarationStatement.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp
{
    using System.CodeDom;

    /// <summary>
    /// Represents a implicit(var) variable declaration
    /// </summary>
    public class ImplicitVariableDeclarationStatement : CodeVariableDeclarationStatement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitVariableDeclarationStatement"/> class
        /// using the specified data type, variable name, and initialization expression.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="initExpression">A <see cref="System.CodeDom.CodeExpression"/> that indicates
        /// the initialization expression for the variable.</param>
        public ImplicitVariableDeclarationStatement(string name, CodeExpression initExpression)
            : base("var", name, initExpression)
        {
        }

        #endregion
    }
}