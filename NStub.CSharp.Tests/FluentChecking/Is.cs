namespace NStub.CSharp.Tests.FluentChecking
{
    using System;
    using System.CodeDom;

    public static class Is
    {
        public static Func<T, CompareResult> Named<T>(string methodName) where T : CodeMethodInvokeExpression
        {
            Func<T, CompareResult> func = (e) => new CompareResult(e.Method.MethodName == methodName, e.Method.MethodName, methodName);
            return func;
        }

        /*public static Func<T, bool> Named<T>(string methodName) where T : CodeMethodReferenceExpression
        {
            Func<T, bool> func = (e) => e.MethodName == methodName;
            return func;
        }*/

        public static bool Dings<T>(T expression) where T : CodeMethodInvokeExpression
        {
            
            return true;
        }

        public static Func<CodeMethodInvokeExpression, CompareResult> Named(string methodName)
        {
            return (e) => new CompareResult(e.Method.MethodName == methodName, e.Method.MethodName, methodName);
        }
    }
}