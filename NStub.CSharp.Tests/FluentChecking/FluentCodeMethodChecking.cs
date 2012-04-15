using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using MbUnit.Framework;
using System.Linq.Expressions;
using Gallio.Framework.Assertions;

namespace NStub.CSharp.Tests.FluentChecking
{
    public static class FluentCodeMethodChecking
    {
        public static Expression<Func<bool>> HasComment(this CodeMemberMethod method, string comment)
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

        public static string HasCommentMsg(this CodeMemberMethod method)
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
        }

        public static Expression<Func<bool>> HasAttribute(this CodeMemberMethod method, string attribute)
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

        public static string HasAttributeMsg(this CodeMemberMethod method)
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

        public static Expression<Func<bool>> HasReturnType<T>(this CodeMemberMethod method)
        {
            return HasReturnTypeOf(method, typeof(T));
        }

        public static Expression<Func<bool>> HasReturnTypeOf(this CodeMemberMethod method, Type expected)
        {
            var methodReturnType = method.ReturnType.BaseType;
            var actualReturnType = methodReturnType.ToString();
            var expectedReturnType = expected.ToString();
            Expression<Func<bool>> returnValue;
            returnValue = () => expectedReturnType == actualReturnType;
            return returnValue;
        }

        public static CodeMemberMethod ClearParameters(this CodeMemberMethod method)
        {
            method.Parameters.Clear();
            return method;
        }

    }
}
