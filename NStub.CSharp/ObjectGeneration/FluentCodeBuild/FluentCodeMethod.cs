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
        /// Determines whether the specified method contains specified type of statement.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <param name="statementType">The matching statement type.</param>
        /// <returns>
        /// A Linq-Expression for use in <see cref="Assert.That"/>, checking the truth of the assertion.
        /// </returns>
        public static IEnumerable<T> StatementsOf<T>(this CodeMemberMethod method)
        {
            /*if (method.Statements.Count == 0)
            {
                throw new AssertionException("The method's statement list is empty. Can't find a '" +
                    typeof(T).ToString() + "' type on a method with no statements.");
            }*/

            IEnumerable<T> returnValue = method.Statements.OfType<T>();
            return returnValue;
        }

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
        /// <typeparam name="T">Type of the method return type.</typeparam>
        /// <param name="method">The method which return type is set.</param>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public static CodeMemberMethod WithReturnType<T>(this CodeMemberMethod method)
        {
            return WithReturnType(method, typeof(T));
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
        /// Add a reference to a static class to the method body. Like 'Assert' or 'DateTime'.
        /// </summary>
        /// <param name="method">The method to add the statements to.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public static CodeTypeReferenceBinder StaticClass(this CodeMemberMethod method, string className)
        {
            // "Assert"
            var staticexpr = new CodeTypeReferenceExpression(className);
            var result = new CodeTypeReferenceBinder(method, staticexpr);
            return result;
        }

        /// <summary>
        /// Add and assign a reference to a member field to the method body. Like 'this.myField = "Hello world;"'.
        /// </summary>
        /// <param name="method">The method to add the assignment to.</param>
        /// <param name="fieldName">Name of the field to create or reference.</param>
        /// <returns>A fluent interface to build up field reference types.</returns>
        public static CodeFieldReferenceBinder Assign(this CodeMemberMethod method, string fieldName)
        {
            var fieldRef1 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName);
            var result = new CodeFieldReferenceBinder(method, fieldRef1);
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

    /// <summary>
    /// Summary
    /// </summary>
    [global::System.Serializable]
    public class CodeTypeReferenceException : Exception
    {
        private CodeTypeReferenceBinder binder;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CodeTypeReferenceException"/> class
        /// </summary>
        /// <param name="binder">The binder associated with this exception.</param>
        public CodeTypeReferenceException(CodeTypeReferenceBinder binder)
        {
            Guard.NotNull(() => binder, binder);
            this.binder = binder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CodeTypeReferenceException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the error. The content of message is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        /// <param name="binder">The binder associated with this exception.</param>
        public CodeTypeReferenceException(CodeTypeReferenceBinder binder, string message)
            : base(message)
        {
            Guard.NotNull(() => binder, binder);
            this.binder = binder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CodeTypeReferenceException"/> class
        /// </summary>
        /// <param name="binder">The binder associated with this exception.</param>
        /// <param name="message">A <see cref="T:System.String"/> that describes the error. The content of message is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public CodeTypeReferenceException(CodeTypeReferenceBinder binder, string message, Exception inner)
            : base(message, inner)
        {
            Guard.NotNull(() => binder, binder);
            this.binder = binder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CodeTypeReferenceException"/> class
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected CodeTypeReferenceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// Summary
    /// </summary>
    [global::System.Serializable]
    public class CodeFieldReferenceException : Exception
    {
        private CodeFieldReferenceBinder binder;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CodeFieldReferenceException"/> class
        /// </summary>
        /// <param name="binder">The binder associated with this exception.</param>
        public CodeFieldReferenceException(CodeFieldReferenceBinder binder)
        {
            Guard.NotNull(() => binder, binder);
            this.binder = binder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CodeFieldReferenceException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the error. The content of message is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        /// <param name="binder">The binder associated with this exception.</param>
        public CodeFieldReferenceException(CodeFieldReferenceBinder binder, string message)
            : base(message)
        {
            Guard.NotNull(() => binder, binder);
            this.binder = binder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CodeFieldReferenceException"/> class
        /// </summary>
        /// <param name="binder">The binder associated with this exception.</param>
        /// <param name="message">A <see cref="T:System.String"/> that describes the error. The content of message is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public CodeFieldReferenceException(CodeFieldReferenceBinder binder, string message, Exception inner)
            : base(message, inner)
        {
            Guard.NotNull(() => binder, binder);
            this.binder = binder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CodeFieldReferenceException"/> class
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected CodeFieldReferenceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
