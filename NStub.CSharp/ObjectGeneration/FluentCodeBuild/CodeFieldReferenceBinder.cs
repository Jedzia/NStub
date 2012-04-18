namespace NStub.CSharp.ObjectGeneration.FluentCodeBuild
{
    using System;
    using System.CodeDom;

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

        private readonly CodeMemberMethod method;
        private readonly CodeFieldReferenceExpression fieldReference;
        private CodeAssignStatement fieldAssignment;
        private CodeMemberField memberField;

        /// <summary>
        /// Gets the reference to the member field.
        /// </summary>
        public CodeFieldReferenceExpression FieldReference
        {
            get { return fieldReference; }
        }

        /// <summary>
        /// Gets the field assignment, e.g. 'this.myField = "Foo";' statement.
        /// </summary>
        public CodeAssignStatement FieldAssignment
        {
            get { return fieldAssignment; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTypeReferenceBinder"/> class.
        /// </summary>
        /// <param name="method">The method to add a CodeTypeReference to.</param>
        /// <param name="fieldReference">The field reference to add.</param>
        internal CodeFieldReferenceBinder(CodeMemberMethod method, CodeFieldReferenceExpression fieldReference)
        {
            Guard.NotNull(() => method, method);
            Guard.NotNull(() => fieldReference, fieldReference);
            this.method = method;
            this.fieldReference = fieldReference;
        }

        /// <summary>
        /// Create the field in the specified class.
        /// </summary>
        /// <param name="owningClass">The owning class.</param>
        /// <param name="fieldType">CLR-Type of the field.</param>
        /// <returns>
        /// A fluent interface to build up member field types.
        /// </returns>
        public CodeFieldReferenceBinder AndCreateIn(CodeTypeDeclaration owningClass, Type fieldType)
        {
            if (this.memberField != null)
            {
                throw new CodeFieldReferenceException(this, "Cannot create a memberfield twice.");
            }
            this.memberField = new CodeMemberField(fieldType, fieldReference.FieldName);
            owningClass.Members.Add(memberField);
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
            return AndCreateIn(owningClass, typeof(T));
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
            fieldAssignment = new CodeAssignStatement(fieldReference, primitive);
            return this;
        }

        /// <summary>
        /// Completes the creation of the field assign statement.
        /// </summary>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public CodeMemberMethod Commit()
        {
            if (this.fieldAssignment == null)
            {
                throw new CodeFieldReferenceException(this, "There are no assignments to the field. Nothing to commit"+
                                                            ", use With(...) to assign values to the field.");
            }
            method.Statements.Add(fieldAssignment);
            return method;
        }

    }
}