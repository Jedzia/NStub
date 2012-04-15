namespace NStub.CSharp.Tests.FluentChecking
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public static class FluentCodeStatementBinderExtender
    {
        public static FluentCodeStatementBinder<T> Where<T>(this IEnumerable<T> collection) where T : CodeStatement
        {
            var binder = new FluentCodeStatementBinder<T>(collection, null);
            return binder;
        }

        public static void TestWhut(this Expression<Func<bool>> expression)
        {
            var met = new CodeMemberMethod();

            //var res = met.ContainsStatement<CodeExpressionStatement>().Whut();
            //var res = met.StatementsOf<CodeExpressionStatement>().Where();
            //var binder = new FluentCodeStatementBinder<CodeExpressionStatement>(res, null);

            //var abc = Whut<CodeExpressionStatement>(expression);
        }

        public static FluentCodeStatementBinder<T> Where<T>(this Expression<Func<bool>> expression) where T : CodeStatement
        {
            //var binder = new FluentCodeStatementBinder(expression, null);
            return null;
            /*var firstResult = expression.Compile().Invoke();

            Expression<Func<bool>> returnValue;
            returnValue = () => firstResult && method.Statements
                .OfType<CodeStatement>()
                //.Select(e => e.Expression)
                .Any(e => e.GetType() == typeof(string)
                );

            return returnValue;*/
        }
    }
}