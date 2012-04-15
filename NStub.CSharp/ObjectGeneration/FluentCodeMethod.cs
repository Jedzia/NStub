using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace NStub.CSharp.ObjectGeneration
{
    /// <summary>
    /// Provides Fluent <see cref="CodeMemberMethod"/> construction.
    /// </summary>
    public static class FluentCodeMethod
    {
        /// <summary>
        /// Add a comment to the method body.
        /// </summary>
        /// <param name="method">The method to add a comment to.</param>
        /// <param name="comment">The text of the comment.</param>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public static CodeMemberMethod AddComment(this CodeMemberMethod method, string comment)
        {
            method.Statements.Add(new CodeCommentStatement(comment));
            return method;
        }

        /// <summary>
        /// Add a blank line to the method body.
        /// </summary>
        /// <param name="method">The method to add a blank line to.</param>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public static CodeMemberMethod AddBlankLine(this CodeMemberMethod method)
        {
            method.Statements.Add(new CodeSnippetStatement(""));
            return method;
        }

        /// <summary>
        /// Adds an attribute to the method body.
        /// </summary>
        /// <param name="method">The method to add the attribute to.</param>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public static CodeMemberMethod AddMethodAttribute(this CodeMemberMethod method, string attribute)
        {
            var testAttr = new CodeAttributeDeclaration(new CodeTypeReference(attribute));
            method.CustomAttributes.Add(testAttr);
            return method;
        }

        /// <summary>
        /// Sets the name of the method body.
        /// </summary>
        /// <param name="method">The method to set the name on.</param>
        /// <param name="name">The name of the method.</param>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public static CodeMemberMethod SetName(this CodeMemberMethod method, string name)
        {
            method.Name = name;
            return method;
        }

        /// <summary>
        /// Set the method return type to the specified value.
        /// </summary>
        /// <param name="method">The method which return type is set.</param>
        /// <param name="returnType">Type of the method return type.</param>
        /// <returns> A fluent interface to build up methods.</returns>
        public static CodeMemberMethod WithReturnType(this CodeMemberMethod method, Type returnType)
        {
            method.ReturnType = new CodeTypeReference(returnType);
            return method;
        }

        /// <summary>
        /// Clear the parameters of the method.
        /// </summary>
        /// <param name="method">The method with the parameters to clear.</param>
        /// <returns> A fluent interface to build up methods.</returns>
        public static CodeMemberMethod ClearParameters(this CodeMemberMethod method)
        {
            method.Parameters.Clear();
            return method;
        }

        /// <summary>
        /// Add a static class statement to the method body.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public static CodeTypeReferenceBinder StaticClass(this CodeMemberMethod method, string className)
        {
            // "Assert"
            var staticexpr = new CodeTypeReferenceExpression(className);
            var result = new CodeTypeReferenceBinder(method, staticexpr);
            return result;
        }

    }

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
        /*public CodeMemberMethod Method
        {
            get { return method; }
        }*/

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
