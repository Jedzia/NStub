using System;
using System.Linq;
using System.Text;
using System.CodeDom;
using MbUnit.Framework;
using System.Linq.Expressions;
using Gallio.Framework.Assertions;

namespace NStub.CSharp.Tests.FluentChecking
{
    /// <summary>
    /// Provides fluent assertions for <see cref="CodeMemberMethod"/> types.
    /// </summary>
    public static class FluentCodeMethodChecking
    {
        /// <summary>
        /// Determines whether the specified method contains a comment of the specified text.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <param name="comment">The matching comment text.</param>
        /// <returns>A Linq-Expression for use in <see cref="Assert.That"/>, checking the truth of the assertion.</returns>
        public static Expression<Func<bool>> ContainsComment(this CodeMemberMethod method, string comment)
        {
            if (method.Statements.Count == 0)
            {
                throw new AssertionException("The method's statement list is empty. Can't find comment '" +
                    comment + "' in a method with no statements.");
            }

            Expression<Func<bool>> returnValue;
            returnValue = () => method.Statements
                .OfType<CodeCommentStatement>()
                .Select(e => e.Comment.Text)
                .Any(e => e == comment);
            return returnValue;
        }

        /// <summary>
        /// Builds an error message for use with <see cref="ContainsComment"/> assertions.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>
        /// An error message for use with <see cref="ContainsComment"/> assertions.
        /// </returns>
        public static string ContainsCommentMsg(this CodeMemberMethod method)
        {
            if (method.Statements.Count == 0)
            {
                return string.Empty;
            }

            var count = 0;
            var result = method.Statements.OfType<CodeCommentStatement>()
                .Select(e => e.Comment.Text)
                .Aggregate("[", (a, b) =>
                {
                    var separator = string.Empty;
                    if (count > 0)
                    {
                        separator = ", ";
                    }
                    count++;
                    return a + separator + "{" + b + "}";
                });

            return result + "]";
        }

        /// <summary>
        /// Determines whether the method contains a specified attribute.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <param name="attribute">The matching attribute name.</param>
        /// <returns>
        /// A Linq-Expression for use in <see cref="Assert.That"/>, checking the truth of the assertion.
        /// </returns>
        public static Expression<Func<bool>> ContainsAttribute(this CodeMemberMethod method, string attribute)
        {
            if (method.CustomAttributes.Count == 0)
            {
                throw new AssertionException("The method's attribute list is empty. Can't find attribute '" +
                    attribute + "' in a method with no attached attributes.");
            }

            Expression<Func<bool>> returnValue;
            returnValue = () => method.CustomAttributes
                .OfType<CodeAttributeDeclaration>()
                .Select(e => e.Name)
                .Any(e => e == attribute);
            return returnValue;
        }

        /// <summary>
        /// Builds an error message for use with <see cref="ContainsAttribute"/> assertions.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>An error message for use with <see cref="ContainsAttribute"/> assertions.</returns>
        public static string ContainsAttributeMsg(this CodeMemberMethod method)
        {
            if (method.CustomAttributes.Count == 0)
            {
                return string.Empty;
            }

            var count = 0;
            var result = method.CustomAttributes.OfType<CodeAttributeDeclaration>()
                .Select(e => e.Name)
                .Aggregate("[", (a, b) =>
                {
                    var separator = string.Empty;
                    if (count > 0)
                    {
                        separator = ", ";
                    }
                    count++;
                    return a + separator + "{" + b + "}";
                });
            return result + "]";
        }

        /// <summary>
        /// Determines whether the method has a specified return type.
        /// </summary>
        /// <typeparam name="T">The expected return type of the method.</typeparam>
        /// <param name="method">The method to check.</param>
        /// <returns>
        /// A Linq-Expression for use in <see cref="Assert.That"/>, checking the truth of the assertion.
        /// </returns>
        public static Expression<Func<bool>> HasReturnType<T>(this CodeMemberMethod method)
        {
            return HasReturnTypeOf(method, typeof(T));
        }

        /// <summary>
        /// Determines whether the method has a specified return type.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <param name="expected">The expected return type of the method.</param>
        /// <returns>
        /// A Linq-Expression for use in <see cref="Assert.That"/>, checking the truth of the assertion.
        /// </returns>
        public static Expression<Func<bool>> HasReturnTypeOf(this CodeMemberMethod method, Type expected)
        {
            var methodReturnType = method.ReturnType.BaseType;
            var actualReturnType = methodReturnType.ToString();
            var expectedReturnType = expected.ToString();
            Expression<Func<bool>> returnValue;
            returnValue = () => expectedReturnType == actualReturnType;
            return returnValue;
        }

        /// <summary>
        /// Determines whether the specified method contains specified type of statement.
        /// </summary>
        /// <typeparam name="T">The matching statement type.</typeparam>
        /// <param name="method">The method to check.</param>
        /// <returns>
        /// A Linq-Expression for use in <see cref="Assert.That"/>, checking the truth of the assertion.
        /// </returns>
        public static Expression<Func<bool>> ContainsStatement<T>(this CodeMemberMethod method)
        {
            return ContainsStatement(method, typeof(T));
        }

        /// <summary>
        /// Determines whether the specified method contains specified type of statement.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <param name="statementType">The matching statement type.</param>
        /// <returns>
        /// A Linq-Expression for use in <see cref="Assert.That"/>, checking the truth of the assertion.
        /// </returns>
        public static Expression<Func<bool>> ContainsStatement(this CodeMemberMethod method, Type statementType)
        {
            if (method.Statements.Count == 0)
            {
                throw new AssertionException("The method's statement list is empty. Can't find a '" +
                    statementType.ToString() + "' type on a method with no statements.");
            }

            Expression<Func<bool>> returnValue;
            returnValue = () => method.Statements
                .OfType<CodeStatement>()
                //.Select(e => e.Expression)
                .Any(e => e.GetType() == statementType);
            return returnValue;
        }


        /*/// <summary>
        /// Builds an error message for use with <see cref="HasComment"/> assertions.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>
        /// An error message for use with <see cref="HasComment"/> assertions.
        /// </returns>
        public static string HasCommentMsgX(this CodeMemberMethod method)
        {
            if (method.Statements.Count == 0)
            {
                return string.Empty;
            }

            var count = 0;
            var result = method.Statements.OfType<CodeCommentStatement>()
                .Select(e => e.Comment.Text)
                .Aggregate("[", (a, b) =>
                {
                    var separator = string.Empty;
                    if (count > 0)
                    {
                        separator = ", ";
                    }
                    count++;
                    return a + separator + "{" + b + "}";
                });

            return result;
        }*/
    }
}
