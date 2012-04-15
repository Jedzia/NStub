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

            return result;
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


        /// <summary>
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
        }


    }

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


    public class FluentCodeWrapper<T, K>
    {
        private readonly T initialExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTypeReferenceBinder"/> class.
        /// </summary>
        /// <param name="method">The method to add a CodeTypeReference to.</param>
        /// <param name="reference">The reference to add.</param>
        internal FluentCodeWrapper(T initialExpression)
        {
            Guard.NotNull(() => initialExpression, initialExpression);
            this.initialExpression = initialExpression;
        }
    }

    /// <summary>
    /// Build a reference type from fluent parameters.
    /// </summary>
    public class FluentCodeStatementBinder<T> where T : CodeStatement
    {
        private static void TestThis()
        {
            var cm = new CodeMemberMethod();
            //cm.StaticClass("Assert").Invoke("Inconclusive").With("Thisone").Commit();
        }
        private readonly IEnumerable<T> initialExpression;
        private readonly CodeTypeReferenceExpression reference;
        private CodeMethodInvokeExpression invoker;

        /*/// <summary>
        /// Gets the expression to the referenced type.
        /// </summary>
        public CodeTypeReferenceExpression TypeReference
        {
            get { return reference; }
        }*/

        public string Error { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTypeReferenceBinder"/> class.
        /// </summary>
        /// <param name="method">The method to add a CodeTypeReference to.</param>
        /// <param name="reference">The reference to add.</param>
        internal FluentCodeStatementBinder(IEnumerable<T> method, CodeTypeReferenceExpression reference)
        {
            Guard.NotNull(() => method, method);
            //Guard.NotNull(() => reference, reference);
            this.initialExpression = method;
            //this.reference = reference;
        }

        /// <summary>
        /// Add a primitive parameter to the method invocation.
        /// </summary>
        /// <param name="text">The content of the primitive expression.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
        public FluentCodeStatementBinder<T> With(string text)
        {
            if (invoker == null)
            {
                //throw new CodeTypeReferenceException(this, "Cannot add parameter to a method that is not defined." +
                //"Use Invoke(...) to specify the method.");
            }
            var primitive = new CodePrimitiveExpression(text);
            invoker.Parameters.Add(primitive);
            return this;
        }

        bool globalResult = true;
        /// <summary>
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodname">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public FluentCodeStatementBinder<T> Expression<K>(Func<K, CompareResult> f) where K : CodeExpression
        {
            if (typeof(T) == typeof(CodeExpressionStatement))
            {
                var detailed = this.initialExpression.Cast<CodeExpressionStatement>();
                //globalResult = detailed.All((e) => f((K)e.Expression));
                var compResult = detailed.Select((e) => f((K)e.Expression));
                globalResult = compResult.All(e => e.Result);

                if (!globalResult)
                {
                    if (compResult.Count() == 0)
                    {
                        Error = string.Empty;
                    }
                    else
                    {
                        var count = 0;
                        var result = compResult
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
                        var lastComparer = compResult.Last().Comparer;
                        Error = "By comparing " +typeof(K) + "´s contained in " + typeof(T) +
                            " elements, the value `" + lastComparer + "` was not found in the checked items: " + result + "]";
                    }
                }

                //globalResult = detailed.All((e) => f(null, (K)e.Expression));

                /*foreach (var item in detailed)
                {
                    var result = f((K)item.Expression);
                    globalResult &= result;
                    //f((K)item.Expression);
                }*/

                /*if (typeof(K) == typeof(CodeMethodInvokeExpression))
                {
                    var subdetailed = detailed.OfType<CodeExpressionStatement>().Select(e => e.Expression);
                    var x1 = subdetailed.OfType<CodeMethodInvokeExpression>().ToArray();
                }*/
            }
            //this.initialExpression
            //    .Any(e=>e.

            //invoker = new CodeMethodInvokeExpression();
            //invoker.Method = new CodeMethodReferenceExpression(reference, methodname);
            return this;
        }

        /// <summary>
        /// Completes the creation of the reference type.
        /// </summary>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public Expression<Func<bool>> Assert()
        {
            // Todo: member checking.
            //method.Statements.Add(invoker);
            //Error = "Hello nerd";
            Expression<Func<bool>> returnValue = 
                () => 
                    globalResult; 
            return returnValue;
            //return initialExpression;
        }
    }

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

    public class CompareResult
    {

        private readonly bool result;
        private readonly string name;
        private readonly string comparer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="CompareResult"/> is result.
        /// </summary>
        /// <value>
        ///   <c>true</c> if result; otherwise, <c>false</c>.
        /// </value>
        public bool Result
        {
            get { return result; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the comparer.
        /// </summary>
        public string Comparer
        {
            get { return comparer; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareResult"/> class.
        /// </summary>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="name">The name.</param>
        /// <param name="comparer">The comparer.</param>
        public CompareResult(bool result, string name, string comparer)
        {
            //Guard.NotNullOrEmpty(() => name, name);
            //Guard.NotNullOrEmpty(() => comparer, comparer);
            this.result = result;
            this.name = name;
            this.comparer = comparer;
        }
    }
}
