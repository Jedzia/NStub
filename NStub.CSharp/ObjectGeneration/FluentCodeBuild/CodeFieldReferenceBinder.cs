// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeFieldReferenceBinder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.FluentCodeBuild
{
    using System;
    using System.CodeDom;
    using NStub.Core;

    /// <summary>
    /// Build a reference to a code field from fluent parameters.
    /// </summary>
    public class CodeFieldReferenceBinder
    {
        /*private static void TestThis()
        {
            var ctdecl = new CodeTypeDeclaration("MyClass");
            var cm = new CodeMemberMethod();
            cm.Assign("myField").AndCreateIn<IAsyncResult>(ctdecl).With(123);
        }*/
        #region Fields

        private readonly CodeFieldReferenceExpression fieldReference;
        private readonly CodeMemberMethod method;
        private CodeAssignStatement fieldAssignment;
        private CodeMemberField memberField;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeFieldReferenceBinder"/> class.
        /// </summary>
        /// <param name="method">The method to add a <see cref="CodeFieldReferenceExpression"/> to.</param>
        /// <param name="fieldReference">The field reference to add.</param>
        internal CodeFieldReferenceBinder(CodeMemberMethod method, CodeFieldReferenceExpression fieldReference)
        {
            Guard.NotNull(() => method, method);
            Guard.NotNull(() => fieldReference, fieldReference);
            this.method = method;
            this.fieldReference = fieldReference;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the field assignment, e.g. 'this.myField = "Foo";' statement.
        /// </summary>
        public CodeAssignStatement FieldAssignment
        {
            get
            {
                return this.fieldAssignment;
            }
        }

        /// <summary>
        /// Gets the reference to the member field.
        /// </summary>
        public CodeFieldReferenceExpression FieldReference
        {
            get
            {
                return this.fieldReference;
            }
        }

        #endregion

        /// <summary>
        /// Create the field in the specified class.
        /// </summary>
        /// <param name="owningClass">The owning class.</param>
        /// <param name="fieldType">CLR-Type of the field.</param>
        /// <returns>
        /// A fluent interface to build up member field types.
        /// </returns>
        /// <exception cref="CodeFieldReferenceException">Cannot create a member field twice.</exception>
        public CodeFieldReferenceBinder AndCreateIn(CodeTypeDeclaration owningClass, Type fieldType)
        {
            if (this.memberField != null)
            {
                throw new CodeFieldReferenceException(this, "Cannot create a memberfield twice.");
            }

            this.memberField = new CodeMemberField(fieldType, this.fieldReference.FieldName);
            owningClass.Members.Add(this.memberField);
            return this;
        }

        /// <summary>
        /// Create the field in the specified class.
        /// </summary>
        /// <typeparam name="T">CLR-Type of the field.</typeparam>
        /// <param name="owningClass">The owning class.</param>
        /// <returns>
        /// A fluent interface to build up member field types.
        /// </returns>
        public CodeFieldReferenceBinder AndCreateIn<T>(CodeTypeDeclaration owningClass)
        {
            return this.AndCreateIn(owningClass, typeof(T));
        }

        /// <summary>
        /// Completes the creation of the field assign statement.
        /// </summary>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        /// <exception cref="CodeFieldReferenceException">There are no assignments to the field. 
        /// Nothing to commit, use <see cref="With"/>(...) to assign values to the field.</exception>
        public CodeMemberMethod Commit()
        {
            if (this.fieldAssignment == null)
            {
                const string Msg = "There are no assignments to the field. Nothing to commit" +
                                   ", use With(...) to assign values to the field.";

                throw new CodeFieldReferenceException(this, Msg);
            }

            this.method.Statements.Add(this.fieldAssignment);
            return this.method;
        }

        /// <summary>
        /// Add a primitive parameter to the method invocation.
        /// </summary>
        /// <param name="value">The object to represent by the primitive expression.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
        public CodeFieldReferenceBinder With(object value)
        {
            var primitive = new CodePrimitiveExpression(value);
            this.fieldAssignment = new CodeAssignStatement(this.fieldReference, primitive);
            return this;
        }
    }
}