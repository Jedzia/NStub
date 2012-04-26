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
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using NStub.Core;

    /// <summary>
    /// Holds a mapping from parameter name to code creation expressions.
    /// </summary>
    public class ConstructorAssignment
    {
        #region Fields

        private readonly CodeMemberField memberField;
        private ICollection<ConstructorAssignment> createAssignments;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorAssignment"/> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="assignStatement">The assign statement for the parameter.</param>
        /// <param name="memberField">The related member field of the parameter.</param>
        /// <param name="type">The type of the field.</param>
        public ConstructorAssignment(
            string parameterName, CodeAssignStatement assignStatement, CodeMemberField memberField, Type type)
        {
            Guard.NotNullOrEmpty(() => parameterName, parameterName);
            Guard.NotNull(() => assignStatement, assignStatement);
            Guard.NotNull(() => memberField, memberField);
            Guard.NotNull(() => type, type);

            this.ParameterName = parameterName;
            this.AssignStatement = assignStatement;
            this.memberField = memberField;
            this.MemberType = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the assign statement for the parameter.
        /// </summary>
        public CodeAssignStatement AssignStatement { get; private set; }

        /// <summary>
        /// Gets the additional assignments used to create this constructor assignment.
        /// </summary>
        public ICollection<ConstructorAssignment> CreateAssignments
        {
            get
            {
                return this.createAssignments ?? (this.createAssignments = new List<ConstructorAssignment>());
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has creation assignments.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has creation assignments; otherwise, <c>false</c>.
        /// </value>
        public bool HasCreationAssignments
        {
            get
            {
                return this.createAssignments != null && this.createAssignments.Count > 0;
            }
        }

        /// <summary>
        /// Gets the related member field of the parameter.
        /// </summary>
        public CodeMemberField MemberField
        {
            get
            {
                return this.memberField;
            }
        }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <returns>
        /// The name of the member.
        /// </returns>
        public string MemberFieldName
        {
            get
            {
                return this.memberField.Name;
            }

            set
            {
                this.memberField.Name = value;
            }
        }

        /// <summary>
        /// Gets the type of the member field.
        /// </summary>
        /// <value>
        /// The type of the member field.
        /// </value>
        public Type MemberType { get; private set; }

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