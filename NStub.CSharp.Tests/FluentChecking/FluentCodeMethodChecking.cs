using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using MbUnit.Framework;
using System.Linq.Expressions;

namespace NStub.CSharp.Tests.FluentChecking
{
    public static class FluentCodeMethodChecking
    {
        public static Expression<Func<bool>> HasComment(this CodeMemberMethod method, string comment)
        {
            Expression<Func<bool>> returnValue;
            returnValue = () => method.Statements
                .OfType<CodeCommentStatement>()
                .Select(e => e.Comment.Text)
                .Any(e => e == comment);
            return returnValue;
        }

        public static Expression<Func<bool>> HasAttribute(this CodeMemberMethod method, string attribute)
        {
            //var testAttr = new CodeAttributeDeclaration(new CodeTypeReference(attribute));
            //method.CustomAttributes.Add(testAttr);
            //return method;

            Expression<Func<bool>> returnValue;
            returnValue = () => method.CustomAttributes
                .OfType<CodeAttributeDeclaration>()
                .Select(e => e.Name)
                .Any(e => e == attribute);
            return returnValue;
        }

        public static string HasAttributeMsg(this CodeMemberMethod method)
        {
            var result = method.CustomAttributes.OfType < CodeAttributeDeclaration>()
                .Select(e=>e.Name)
                .Aggregate((a,b)=>a + ", " + b);
            return result;
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
