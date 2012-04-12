// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorAssignment.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System.CodeDom;

    /// <summary>
    /// Holds a mapping from parameter name to code creation expressions.
    /// </summary>
    internal class ConstructorAssignment
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorAssignment"/> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="assignStatement">The assign statement for the parameter.</param>
        /// <param name="memberField">The related member field of the parameter.</param>
        public ConstructorAssignment(
            string parameterName, CodeAssignStatement assignStatement, CodeMemberField memberField)
        {
            Guard.NotNullOrEmpty(() => parameterName, parameterName);
            Guard.NotNull(() => assignStatement, assignStatement);
            Guard.NotNull(() => memberField, memberField);

            this.ParameterName = parameterName;
            this.AssignStatement = assignStatement;
            this.MemberField = memberField;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the assign statement for the parameter.
        /// </summary>
        public CodeAssignStatement AssignStatement { get; private set; }

        /// <summary>
        /// Gets the related member field of the parameter.
        /// </summary>
        public CodeMemberField MemberField { get; private set; }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name of the parameter.
        /// </value>
        public string ParameterName { get; private set; }

        #endregion
    }
}