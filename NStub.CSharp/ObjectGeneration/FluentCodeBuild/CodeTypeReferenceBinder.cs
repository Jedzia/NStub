namespace NStub.CSharp.ObjectGeneration.FluentCodeBuild
{
    using System.CodeDom;

    /// <summary>
    /// Build a reference type from fluent parameters.
    /// </summary>
    public class CodeTypeReferenceBinder
    {
        /*private static void TestThis()
        {
            var cm = new CodeMemberMethod();
            cm.StaticClass("Assert").Invoke("Inconclusive").With("Thisone").Commit();
        }*/

        private readonly CodeMemberMethod method;
        private readonly CodeExpression reference;
        private CodeMethodInvokeExpression invoker;

        internal CodeMethodInvokeExpression Invoker
        {
            get { return invoker; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a type reference. At the moment checks
        /// for 'reference is CodeTypeReferenceExpression';
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is type reference; otherwise, <c>false</c>.
        /// </value>
        public bool IsTypeReference
        {
            get { return reference is CodeTypeReferenceExpression; }
        }

        /// <summary>
        /// Gets the expression to the referenced type.
        /// </summary>
        public CodeTypeReferenceExpression TypeReference
        {
            get { return reference as CodeTypeReferenceExpression; }
        }

        /// <summary>
        /// Gets the expression to the referenced type.
        /// </summary>
        public CodeExpression Expression
        {
            get { return reference; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTypeReferenceBinder"/> class.
        /// </summary>
        /// <param name="method">The method to add a CodeTypeReference to.</param>
        /// <param name="reference">The reference to a variable or type.</param>
        internal CodeTypeReferenceBinder(CodeMemberMethod method, CodeExpression reference)
        {
            Guard.NotNull(() => method, method);
            Guard.NotNull(() => reference, reference);
            this.method = method;
            this.reference = reference;
        }

        /// <summary>
        /// Add a primitive parameter to the method invocation.
        /// </summary>
        /// <param name="value">The content of the primitive expression.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
        public CodeTypeReferenceBinder With(object value)
        {
            if (invoker == null)
            {
                throw new CodeTypeReferenceException(this, "Cannot add parameter to a method that is not defined." +
                                                           "Use Invoke(...) to specify the method." );
            }
            var primitive = new CodePrimitiveExpression(value);
            invoker.Parameters.Add(primitive);
            return this;
        }

        /// <summary>
        /// Add a parameter with a reference to a local variable of the specified name to the method invocation.
        /// </summary>
        /// <param name="variableName">The name of the referenced local variable.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
        public CodeTypeReferenceBinder WithReference(string variableName)
        {
            // Todo: add WithThisReference
            if (invoker == null)
            {
                throw new CodeTypeReferenceException(this, "Cannot add parameter to a method that is not defined." +
                                                           "Use Invoke(...) to specify the method.");
            }
            var varRef = new CodeVariableReferenceExpression(variableName);
            invoker.Parameters.Add(varRef);
            return this;
        }

        /// <summary>
        /// Add multiple parameters with a reference to a local variables of the specified name to the method invocation.
        /// </summary>
        /// <param name="variableNames">The list of local variable names.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
        public CodeTypeReferenceBinder WithReference(params string[] variableNames)
        {
            // Todo: add WithThisReference
            if (invoker == null)
            {
                throw new CodeTypeReferenceException(this, "Cannot add parameter to a method that is not defined." +
                                                           "Use Invoke(...) to specify the method.");
            }

            foreach (var variableName in variableNames)
            {
                var varRef = new CodeVariableReferenceExpression(variableName);
                invoker.Parameters.Add(varRef);
            }

            return this;
        }

        /// <summary>
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodname">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public CodeTypeReferenceBinder Invoke(string methodname)
        {
            invoker = new CodeMethodInvokeExpression();
            invoker.Method = new CodeMethodReferenceExpression(reference, methodname);
            return this;
        }

        /// <summary>
        /// Completes the creation of the reference type with an assignment of a local variable.
        /// </summary>
        /// <param name="variableName">Name of the local variable that gets assigned.</param>
        /// <param name="createVariable">if set to <c>true</c> a local variable is created with the assignment.</param>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public CodeMemberMethod AssignLocal(string variableName, bool createVariable)
        {
            // Todo: member checking.
            if (createVariable)
            {
                var localDecl = new CodeVariableDeclarationStatement("var", variableName, invoker);
                method.Statements.Add(localDecl);
            }
            else
            {
                var localRef = new CodeVariableReferenceExpression(variableName);
                var as1 = new CodeAssignStatement(localRef, invoker);
                method.Statements.Add(as1);
            }
            return method;
        }

        /// <summary>
        /// Completes the creation of the reference type with an assignment of a member field.
        /// </summary>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public CodeMemberMethod AssignField(string fieldName)
        {
            // Todo: member checking.
            var fieldRef1 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName);
            var as1 = new CodeAssignStatement(fieldRef1, invoker);
            method.Statements.Add(as1);
            return method;
        }

        /// <summary>
        /// Completes the creation of the reference type.
        /// </summary>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public CodeMemberMethod Commit()
        {
            // Todo: member checking.
            if (localVar != null)
            {
                var temp = localVar;
                localVar = null;
                return temp.Commit(this);
            }

            method.Statements.Add(invoker);
            return method;
        }

        private CodeLocalVariableBinder localVar;

        /// <summary>
        /// Gets or sets the reference to the memorized local variable.
        /// </summary>
        /// <value>
        /// The local variable declaration.
        /// </value>
        internal CodeLocalVariableBinder LocalVar
        {
            get { return localVar; }
            set { localVar = value; }
        }

        /*public CodeMemberMethod Assign()
        {
            if (localVar == null)
            {

            }
            localVar.Commit(this);
            //method.Statements.Add(invoker);
            return method;
        }*/

    }
}