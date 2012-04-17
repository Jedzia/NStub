namespace NStub.CSharp.Tests.FluentChecking
{
    using System;
    using System.Linq;
    using System.CodeDom;

    public static class Contains
    {
        public static Func<CodeMethodInvokeExpression, CompareResult> MethodPrimitiveParameter(object primitiveValue)
        {
            return (e) =>
            {
                var primitives = e.Parameters.OfType<CodePrimitiveExpression>();
                //var result = primitives.Any(p => object.ReferenceEquals(p.Value, primitiveValue));
                var result = primitives.Any(p => p.Value.Equals(primitiveValue));
                //var resultParas = primitives.(p => p.Value == primitiveValue);
                int count = 0;
                var resultParas = primitives
                    .Select(p => p.Value)
                    .Aggregate("PARAMETERS[", (a, b) =>
                    {
                        var separator = string.Empty;
                        if (count > 0)
                        {
                            separator = ", ";
                        }
                        count++;
                        return a + separator + "{" + b + "}";
                    }) + "]";
                return new CompareResult(result, e.Method.MethodName + " " + resultParas + " ", primitiveValue.ToString());
            };
        }
    }

    public static class Is
    {
        /*public static Func<T, CompareResult> Named<T>(string methodName) where T : CodeMethodInvokeExpression
        {
            Func<T, CompareResult> func = (e) => new CompareResult(e.Method.MethodName == methodName, e.Method.MethodName, methodName);
            return func;
        }*/

        /*public static Func<T, bool> Named<T>(string methodName) where T : CodeMethodReferenceExpression
        {
            Func<T, bool> func = (e) => e.MethodName == methodName;
            return func;
        }*/

        public static bool Dings<T>(T expression) where T : CodeMethodInvokeExpression
        {

            return true;
        }

        public static Func<CodeMethodInvokeExpression, CompareResult> MethodNamed(string methodName)
        {
            return (e) => new CompareResult(e.Method.MethodName == methodName, e.Method.MethodName, methodName);
        }


        public static Func<T, CompareResult> Named<T>(Func<T, CompareResult> func) where T : CodeExpression
        {
            //return (e) => new CompareResult(e.Method.MethodName == methodName, e.Method.MethodName, methodName);
            return func;
        }

        public static Func<CodeFieldReferenceExpression, CompareResult> FieldNamed(string fieldName)// where T : CodeFieldReferenceExpression
        {
            return (e) => new CompareResult(e.FieldName == fieldName, e.FieldName, fieldName);
        }

        public static Func<CodeVariableReferenceExpression, CompareResult> VarRefNamed(string fieldName)// where T : CodeFieldReferenceExpression
        {
            return (e) => new CompareResult(e.VariableName == fieldName, e.VariableName, fieldName);
        }

        public static Func<CodeVariableReferenceExpression, CompareResult> VarRefAssigned(string fieldName)// where T : CodeFieldReferenceExpression
        {
            return (e) => new CompareResult(e.VariableName == fieldName, e.VariableName, fieldName);
        }

        public static Func<CodeVariableDeclarationStatement, CompareResult> VarNamed(string fieldName)// where T : CodeFieldReferenceExpression
        {
            return (e) => new CompareResult(e.Name == fieldName, e.Name, fieldName);
        }

        public static Func<CodePrimitiveExpression, CompareResult> Primitve(object value)// where T : CodeFieldReferenceExpression
        {
            return (e) => new CompareResult(e.Value.Equals(value), e.Value.ToString(), value.ToString());
        }

        /*public static Func<CodeFieldReferenceExpression, CompareResult> Namedx(CodeFieldReferenceExpression x, string fieldName)
        {
            return (e) => new CompareResult(e.FieldName == fieldName, e.FieldName, fieldName);
        }*/

    }
}