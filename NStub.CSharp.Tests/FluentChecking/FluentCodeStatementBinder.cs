namespace NStub.CSharp.Tests.FluentChecking
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Gallio.Framework.Assertions;
    using NStub.Core;

    /// <summary>
    /// Build a reference type from fluent parameters.
    /// </summary>
    public class FluentCodeStatementBinder<T>
        where T : CodeStatement
    {
        private readonly IEnumerable<T> initialExpression;
        //private readonly CodeTypeReferenceExpression reference;
        //private CodeMethodInvokeExpression invoker;

        public int Result
        {
            get { return foundTimes; }
        }

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
        int foundTimes = 0;
        int totalItems = 0;
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
            detected = false;
            IEnumerable<CompareResult> compResult = null;
            if (typeof(T) == typeof(CodeExpressionStatement))
            {
                var detailed = this.initialExpression.Cast<CodeExpressionStatement>();
                if (detailed.Count() == 0)
                {
                    globalResult = false;
                    foundTimes = -1;
                    Log("Expression: By comparing " + typeof(K) + "´s contained in " + typeof(T) +
                                " elements an empty expression list always returns false");
                    return this;
                }

                // check type !
                compResult = detailed.Where(e => e.Expression is K).Select((e) => f((K)e.Expression));
            }
            else if (typeof(T) == typeof(CodeFieldReferenceExpression))
            {
                var detailed = this.initialExpression.Cast<CodeFieldReferenceExpression>();
                if (detailed.Count() == 0)
                {
                    globalResult = false;
                    foundTimes = -1;
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
                    foundTimes = -1;
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
                    foundTimes = -1;
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

            totalItems += compResult.Count();
            foundTimes += compResult.Count(e => e.Result == true);
            //globalResult &= compResult.Any(e => e.Result == true);

            WriteErrorMsg<K>(compResult, "");
            return this;
        }

        /*/// <summary>
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodname">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public FluentCodeStatementBinder<T> ExpressionLeft<K>(Func<K, CompareResult> f)
            where K : CodeExpression
        {
            detected = false;
            IEnumerable<CompareResult> compResult = null;
            if (typeof(T) == typeof(CodeAssignStatement))
            {
                var detailed = this.initialExpression.Cast<CodeAssignStatement>();
                if (detailed.Count() == 0)
                {
                    globalResult = false;
                    foundTimes = -1;
                    Log("ExpressionLeft: By comparing " + typeof(K) + "´s contained in " + typeof(T) +
                                " elements an empty expression list always returns false");
                    return this;
                }

                compResult = detailed.Select((e) => f((K)e.Left));
            }
            //else if (typeof(T) == typeof(CodeVariableDeclarationStatement))
            //{
              //  var detailed = this.initialExpression.Cast<CodeVariableDeclarationStatement>();
             //   if (detailed.Count() == 0)
             //   {
              //      globalResult = false;
              //      Log("ExpressionLeft: By comparing " + typeof(K) + "´s contained in " + typeof(T) +
             //                   " elements an empty expression list always returns false");
             //       return this;
            //    }
            //
            //     compResult = detailed.Select((e) => f((K)e.InitExpression));
            //}
            else
            {
                throw new AssertionException("ExpressionLeft<K>(...) ist not capable of comparing `" +
                    typeof(T) + "` elements.");
            }

            totalItems += compResult.Count();
            foundTimes += compResult.Count(e => e.Result == true);
            //globalResult &= compResult.Any(e => e.Result == true);

            WriteErrorMsg<K>(compResult, "left side");
            return this;
        }*/

        /// <summary>
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodname">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public FluentCodeStatementBinder<T> ExpressionLeft<K>(Func<K, CompareResult> f)
            where K : CodeExpression
        {
            return ExpressionSided<K>(f, (e) => e.Left);
        }

        /// <summary>
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodname">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public FluentCodeStatementBinder<T> ExpressionRight<K>(Func<K, CompareResult> f)
            where K : CodeExpression
        {
            return ExpressionSided<K>(f, (e) => e.Right);
        }

        /// <summary>
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodname">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        private FluentCodeStatementBinder<T> ExpressionSided<K>(Func<K, CompareResult> f, Func<CodeAssignStatement, CodeExpression> selector)
            where K : CodeExpression
        {
            detected = false;
            IEnumerable<CompareResult> compResult = null;
            if (typeof(T) == typeof(CodeAssignStatement))
            {
                var detailed = this.initialExpression.Cast<CodeAssignStatement>();
                if (detailed.Count() == 0)
                {
                    globalResult = false;
                    foundTimes = -1;
                    Log("ExpressionLeft: By comparing " + typeof(K) + "´s contained in " + typeof(T) +
                                " elements an empty expression list always returns false");
                    return this;
                }
                compResult = detailed.Select((e) => f((K)selector(e)));
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

            totalItems += compResult.Count();
            foundTimes += compResult.Count(e => e.Result == true);
            //globalResult &= compResult.Any(e => e.Result == true);

            WriteErrorMsg<K>(compResult, "left side");
            return this;
        }


        private void WriteErrorMsg<K>(IEnumerable<CompareResult> compResult, string msgforT) where K : CodeExpression
        {
            if (!compResult.Any(e => e.Result == true))
            {
                if (compResult.Count() == 0)
                {
                    Log("By comparing " + typeof(K) + "´s contained in " + typeof(T) + " " + msgforT +
                            " no " + typeof(K) + " elements where found.");
                    return;
                }

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


        /*public Expression<Func<bool>> IsEmpty()
        {
            return () => result == 0;;
        }*/

        /*public Expression<Func<bool>> IsNotEmpty()
        {
            return () => result > 0; ;
        }*/

        private FluentCodeStatementBinder<T> EndDetect()
        {
            foundTimes = 0;
            totalItems = 0;
            detected = true;
            return this;
        }
        bool detected;

        public FluentCodeStatementBinder<T> WasFound()
        {
            var detect = foundTimes > 0;
            if (!detect)
            {
                Log(string.Format("WasFound(> 0) on {0} total items with {1} found items.", totalItems, foundTimes));
            }
            globalResult &= detect;
            return EndDetect();
        }

        public FluentCodeStatementBinder<T> WasNotFound()
        {
            var detect = foundTimes == 0;
            if (!detect)
            {
                Log(string.Format("WasFound(== 0) on {0} total items with {1} found items.", totalItems, foundTimes));
            }
            globalResult &= detect;
            return EndDetect();
        }
        public FluentCodeStatementBinder<T> WasFound(int times)
        {
            // genau times mal gefunden
            var detect = foundTimes == times;
            if (!detect)
            {
                Log(string.Format("WasFound({2}) on {0} total items with {1} found items.", totalItems, foundTimes, times));
            }
            globalResult &= detect;
            return EndDetect();
        }

        public FluentCodeStatementBinder<T> WasNotFound(int times)
        {
            // genau times mal nicht gefunden
            var detect = (totalItems - foundTimes) == times;
            if (!detect)
            {
                Log(string.Format("WasNotFound({2}) on {0} total items with {1} not found items.", totalItems, totalItems - foundTimes, times));
            }
            globalResult &= detect;
            return EndDetect();
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
            if (!detected)
            {
                // when no WasNotFound, WasFound, etc. was called, then use WasFound as standard and assert.
                WasFound();
                Log("Auto WasFound() called.");
                detected = false;
            }
            Expression<Func<bool>> returnValue =
                () =>
                globalResult;
            return returnValue;
            //return initialExpression;
        }



        internal FluentCodeStatementBinder<T> And(Expression<Func<bool>> expression)
        {
            globalResult &= expression.Compile().Invoke();
            return this;
        }
    }
}