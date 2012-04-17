namespace NStub.CSharp.Tests.FluentChecking
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Gallio.Framework.Assertions;

    /// <summary>
    /// Build a reference type from fluent parameters.
    /// </summary>
    public class FluentCodeStatementBinder<T>
        where T : CodeStatement
    {
        private readonly IEnumerable<T> initialExpression;
        //private readonly CodeTypeReferenceExpression reference;
        //private CodeMethodInvokeExpression invoker;

        /*/// <summary>
        /// Gets the expression to the referenced type.
        /// </summary>
        public CodeTypeReferenceExpression TypeReference
        {
            get { return reference; }
        }*/

        public string Error { get; private set; }

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

        /*/// <summary>
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
        }*/

        bool globalResult = true;
        //int globalResult = 0;
        private int logCount;
        private void Log(string text)
        {
            Error += "[" + logCount + "]" + text + Environment.NewLine;
            logCount++;
        }

        /*public FluentCodeStatementBinder<CodeMethodInvokeExpression> Expression<T1>(Func<CodeMethodInvokeExpression, CompareResult> func)
        {
            throw new NotImplementedException();
        }*/

        /// <summary>
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodname">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public FluentCodeStatementBinder<T> Expression<K>(Func<K, CompareResult> f) where K : CodeExpression
        {
            IEnumerable<CompareResult> compResult = null;
            if (typeof(T) == typeof(CodeExpressionStatement))
            {
                var detailed = this.initialExpression.Cast<CodeExpressionStatement>();
                if (detailed.Count() == 0)
                {
                    globalResult = false;
                    Log("Expression: By comparing " + typeof(K) + "´s contained in " + typeof(T) +
                                " elements an empty expression list always returns false");
                    return this;
                }

                compResult = detailed.Select((e) => f((K)e.Expression));
            }
            else if (typeof(T) == typeof(CodeFieldReferenceExpression))
            {
                var detailed = this.initialExpression.Cast<CodeFieldReferenceExpression>();
                if (detailed.Count() == 0)
                {
                    globalResult = false;
                    Log("Expression: By comparing " + typeof(K) + "´s contained in " + typeof(T) +
                                " elements an empty expression list always returns false");
                    return this;
                }

                compResult = detailed.Select((e) => f((K)e.TargetObject));
            }
            else if (typeof(T) == typeof(CodePropertyReferenceExpression))
            {
                var detailed = this.initialExpression.Cast<CodePropertyReferenceExpression>();
                if (detailed.Count() == 0)
                {
                    globalResult = false;
                    Log("Expression: By comparing " + typeof(K) + "´s contained in " + typeof(T) +
                                " elements an empty expression list always returns false");
                    return this;
                }

                compResult = detailed.Select((e) => f((K)e.TargetObject));
            }
            else if (typeof(T) == typeof(CodeVariableDeclarationStatement))
            {
                var detailed = this.initialExpression.Cast<CodeVariableDeclarationStatement>();
                if (detailed.Count() == 0)
                {
                    globalResult = false;
                    Log("Expression: By comparing " + typeof(K) + "´s contained in " + typeof(T) +
                                " elements an empty expression list always returns false");
                    return this;
                }

                compResult = detailed.Select((e) => f((K)e.InitExpression));
            }
            else
            {
                throw new AssertionException("Expression<K>(...) ist not capable of comparing `" +
                    typeof(T) + "` elements.");
            }

            globalResult &= compResult.Any(e => e.Result == true);

            // replace with -> WriteErrorMsg<K>(compResult, "");
            if (!globalResult)
            {
                // i think there cant be an empty compResult when the above checks for empty input list.
                /*if (compResult.Count() == 0)
                {
                    Error = string.Empty;
                }
                else*/
                //{
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
                Log("By comparing " + typeof(K) + "´s contained in " + typeof(T) +
                        " elements, the value `" + lastComparer + "` was not found in the checked items: " + result + "]");
                //}


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
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodname">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public FluentCodeStatementBinder<T> ExpressionLeft<K>(Func<K, CompareResult> f)
            where K : CodeExpression
        {
            IEnumerable<CompareResult> compResult = null;
            if (typeof(T) == typeof(CodeAssignStatement))
            {
                var detailed = this.initialExpression.Cast<CodeAssignStatement>();
                if (detailed.Count() == 0)
                {
                    globalResult = false;
                    Log("ExpressionLeft: By comparing " + typeof(K) + "´s contained in " + typeof(T) +
                                " elements an empty expression list always returns false");
                    return this;
                }

                compResult = detailed.Select((e) => f((K)e.Left));
            }
            /*else if (typeof(T) == typeof(CodeVariableDeclarationStatement))
            {
                var detailed = this.initialExpression.Cast<CodeVariableDeclarationStatement>();
                if (detailed.Count() == 0)
                {
                    globalResult = false;
                    Log("ExpressionLeft: By comparing " + typeof(K) + "´s contained in " + typeof(T) +
                                " elements an empty expression list always returns false");
                    return this;
                }

                compResult = detailed.Select((e) => f((K)e.InitExpression));
            }*/
            else
            {
                throw new AssertionException("ExpressionLeft<K>(...) ist not capable of comparing `" +
                    typeof(T) + "` elements.");
            }

            globalResult &= compResult.Any(e => e.Result == true);

            WriteErrorMsg<K>(compResult, "left side");
            return this;
        }

        private void WriteErrorMsg<K>(IEnumerable<CompareResult> compResult, string msgforT) where K : CodeExpression
        {
            if (!globalResult)
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
                Log("By comparing " + typeof(K) + "´s contained in " + typeof(T) + " " + msgforT +
                        " elements, the value `" + lastComparer + "` was not found in the checked items: " + result + "]");
            }
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
}