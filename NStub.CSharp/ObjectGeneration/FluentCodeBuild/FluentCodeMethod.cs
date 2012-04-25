// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentCodeMethod.cs" company="EvePanix">
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
using System.Linq.Expressions;

    /// <summary>
    /// Provides Fluent code construction for <see cref="CodeMemberMethod"/> declared methods.
    /// </summary>
    public static class FluentCodeMethod
    {
        /// <summary>
        /// Add a blank line to the method body.
        /// </summary>
        /// <param name="method">The method to add a blank line to.</param>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public static CodeMemberMethod AddBlankLine(this CodeMemberMethod method)
        {
            method.Statements.Add(new CodeSnippetStatement(string.Empty));
            return method;
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
        /// Add a reference to a static class to the method body. Like '<c>Assert</c>' or '<c>DateTime</c>'.
        /// </summary>
        /// <param name="method">The method to add the statements to.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
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
        public static CodeLocalVariableBinder Var(
            this CodeMemberMethod method, string variableName, bool createVariable)
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
    }

    /// <summary>
    /// Provides Fluent code construction for static classes.
    /// </summary>
    public static class StaticClass
    {
        /// <summary>
        /// Create a reference to a static class Like '<c>System.String</c>' or '<c>DateTime</c>'.
        /// </summary>
        /// <param name="className">Name of the static class.</param>
        /// <returns>
        /// A reference to the specified data type.
        /// </returns>
        public static CodeTypeReferenceExpression Of(string className)
        {
            // "Assert"
            var staticexpr = new CodeTypeReferenceExpression(className);
            // var result = new CodeTypeReferenceBinder(method, staticexpr);
            return staticexpr;
        }

        /// <summary>
        /// Create a reference to the specified property of a static class Like '<c>string.Empty</c>' or '<c>DateTime.Now</c>'.
        /// </summary>
        /// <typeparam name="T">Type of the static class to access.</typeparam>
        /// <param name="property">The expression defining the property. For example
        /// this can be '() => string.Empty' or '() => DateTime.Now'.</param>
        /// <returns>A reference expression to the value of the specified property.</returns>
        public static CodePropertyReferenceExpression Property<T>(Expression<Func<T>> property)
        {
            var member = (MemberExpression)property.Body;
            var propertyName = member.Member.Name;
            var staticexpr = new CodeTypeReferenceExpression(typeof(T).FullName);
            var result = new CodePropertyReferenceExpression(staticexpr, propertyName);
            return result;
        }
    }

    /// <summary>
    /// Provides Fluent code construction for static classes.
    /// </summary>
    /// <typeparam name="T">Type of the static class to access.</typeparam>
    public static class StaticClass<T>
    {
        /// <summary>
        /// Creates a reference to the value of the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>A reference expression to the value of the specified property.</returns>
        public static CodePropertyReferenceExpression Property(string propertyName)
        {
            var staticexpr = new CodeTypeReferenceExpression(typeof(T).FullName);
            var result = new CodePropertyReferenceExpression(staticexpr, propertyName);
            return result;
        }
    }
}