namespace NStub.CSharp.ObjectGeneration.FluentCodeBuild
{
    using System.CodeDom;

    /// <summary>
    /// Build a reference type from fluent parameters.
    /// </summary>
    public class CodeTypeReferenceBinder
    {
        private static void TestThis()
        {
            var cm = new CodeMemberMethod();
            cm.StaticClass("Assert").Invoke("Inconclusive").With("Thisone").Commit();
        }
        private readonly CodeMemberMethod method;
        private readonly CodeTypeReferenceExpression reference;
        private CodeMethodInvokeExpression invoker;

        /// <summary>
        /// Gets the expression to the referenced type.
        /// </summary>
        public CodeTypeReferenceExpression TypeReference
        {
            get { return reference; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTypeReferenceBinder"/> class.
        /// </summary>
        /// <param name="method">The method to add a CodeTypeReference to.</param>
        /// <param name="reference">The reference to add.</param>
        internal CodeTypeReferenceBinder(CodeMemberMethod method, CodeTypeReferenceExpression reference)
        {
            Guard.NotNull(() => method, method);
            Guard.NotNull(() => reference, reference);
            this.method = method;
            this.reference = reference;
        }

        /// <summary>
        /// Add a primitive parameter to the method invocation.
        /// </summary>
        /// <param name="text">The content of the primitive expression.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
        public CodeTypeReferenceBinder With(string text)
        {
            if (invoker == null)
            {
                throw new CodeTypeReferenceException(this, "Cannot add parameter to a method that is not defined." +
                                                           "Use Invoke(...) to specify the method." );
            }
            var primitive = new CodePrimitiveExpression(text);
            invoker.Parameters.Add(primitive);
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
        /// Completes the creation of the reference type.
        /// </summary>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public CodeMemberMethod Commit()
        {
            // Todo: member checking.
            method.Statements.Add(invoker);
            return method;
        }
    }
}