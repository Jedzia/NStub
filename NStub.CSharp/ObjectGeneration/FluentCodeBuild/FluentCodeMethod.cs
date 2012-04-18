namespace NStub.CSharp.ObjectGeneration.FluentCodeBuild
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.CodeDom;

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
        /// Creates and initializes a local variable. var test = object.DoSomething("parameter").
        /// </summary>
        /// <param name="method">The method to add the statements to.</param>
        /// <param name="variableName">Name of the local variable.</param>
        /// <param name="createVariable">if set to <c>true</c> a local variable is created; otherwise it is only referenced.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public static CodeLocalVariableBinder Var(this CodeMemberMethod method, string variableName, bool createVariable)
        {
            // "Assert"
            if (createVariable)
            {
                var localDecl = new CodeVariableDeclarationStatement("var", variableName);
                var result = new CodeLocalVariableBinder(method, localDecl);
                return result;
            }
            else
            {
                var staticexpr = new CodeVariableReferenceExpression(variableName);
                var result = new CodeLocalVariableBinder(method, staticexpr);
                return result;
            }
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
}
