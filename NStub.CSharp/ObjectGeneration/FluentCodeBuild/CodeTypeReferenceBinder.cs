// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeTypeReferenceBinder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.FluentCodeBuild
{
    using System.CodeDom;
    using NStub.Core;

    /// <summary>
    /// Build a reference type from fluent parameters.
    /// </summary>
    public class CodeTypeReferenceBinder
    {
        /*private static void TestThis()
        {
            var cm = new CodeMemberMethod();
            cm.StaticClass("Assert").Invoke("Inconclusive").With("Thisone").Commit();
        }*/
        #region Fields

        private readonly CodeMemberMethod method;
        private CodeExpression reference;
        private CodeMethodInvokeExpression invoker;
        private CodeLocalVariableBinder localVar;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTypeReferenceBinder"/> class.
        /// </summary>
        /// <param name="method">The method to add a <see cref="CodeTypeReference"/> to.</param>
        /// <param name="reference">The reference to a variable or type.</param>
        internal CodeTypeReferenceBinder(CodeMemberMethod method, CodeExpression reference)
        {
            Guard.NotNull(() => method, method);
            Guard.NotNull(() => reference, reference);
            this.method = method;
            this.reference = reference;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expression to the referenced type.
        /// </summary>
        public CodeExpression Expression
        {
            get
            {
                return this.reference;
            }

            set
            {
                this.reference = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a type reference. At the moment checks
        /// for 'reference is <see cref="CodeTypeReferenceExpression"/>';
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is type reference; otherwise, <c>false</c>.
        /// </value>
        public bool IsTypeReference
        {
            get
            {
                return this.reference is CodeTypeReferenceExpression;
            }
        }

        /// <summary>
        /// Gets the expression to the referenced type.
        /// </summary>
        public CodeTypeReferenceExpression TypeReference
        {
            get
            {
                return this.reference as CodeTypeReferenceExpression;
            }
        }

        /// <summary>
        /// Gets the invoker expression of the type.
        /// </summary>
        internal CodeMethodInvokeExpression Invoker
        {
            get
            {
                return this.invoker;
            }
        }

        /// <summary>
        /// Gets or sets the reference to the memorized local variable.
        /// </summary>
        /// <value>
        /// The local variable declaration.
        /// </value>
        internal CodeLocalVariableBinder LocalVar
        {
            get
            {
                return this.localVar;
            }

            set
            {
                this.localVar = value;
            }
        }

        #endregion

        /// <summary>
        /// Completes the creation of the reference type with an assignment of a member field.
        /// </summary>
        /// <param name="fieldName">Name of the member field to assign.</param>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public CodeMemberMethod AssignField(string fieldName)
        {
            // Todo: member checking.
            var fieldRef1 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName);
            var as1 = new CodeAssignStatement(fieldRef1, this.invoker);
            this.method.Statements.Add(as1);
            return this.method;
        }

        /// <summary>
        /// Completes the creation of the reference type with an assignment of a local variable.
        /// </summary>
        /// <param name="variableName">Name of the local variable that gets assigned.</param>
        /// <param name="createVariable">if set to <c>true</c> a local variable is created with the assignment.</param>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public CodeMemberMethod AssignLocal(string variableName, bool createVariable)
        {
            // Todo: member checking.
            if (createVariable)
            {
                var localDecl = new CodeVariableDeclarationStatement("var", variableName, this.invoker);
                this.Assignment = localDecl;
                this.method.Statements.Add(localDecl);
            }
            else
            {
                var localRef = new CodeVariableReferenceExpression(variableName);
                var as1 = new CodeAssignStatement(localRef, this.invoker);
                this.Assignment = as1;
                this.method.Statements.Add(as1);
            }

            return this.method;
        }

        /// <summary>
        /// The summary.
        /// </summary>
        private CodeStatement assignment;

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public CodeStatement Assignment
        {
            get
            {
                return this.assignment;
            }

            set
            {
                this.assignment = value;
            }
        }

        /// <summary>
        /// Completes the creation of the reference type.
        /// </summary>
        /// <returns>
        /// A fluent interface to build up methods.
        /// </returns>
        public CodeMemberMethod Commit()
        {
            // Todo: member checking.
            if (this.localVar != null)
            {
                var temp = this.localVar;
                this.localVar = null;
                return temp.Commit(this);
            }

            this.method.Statements.Add(this.invoker);
            return this.method;
        }

        /// <summary>
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public CodeTypeReferenceBinder Invoke(string methodName)
        {
            this.invoker = new CodeMethodInvokeExpression
                               {
                                   Method = new CodeMethodReferenceExpression(this.reference, methodName)
                               };
            return this;
        }

        /// <summary>
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public CodeTypeReferenceBinder Invoke<T>(string methodName)
        {
            this.invoker = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression(this.reference, methodName)
            };
            var generic = typeof(T);
            this.invoker.Method.TypeArguments.Add(generic);
            return this;
        }

        /// <summary>
        /// Specify the name of the method to invoke.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <returns>A fluent interface to build up reference types.</returns>
        public CodeTypeReferenceBinder Invoke(string methodName, params string[] typearguments)
        {
            this.invoker = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression(this.reference, methodName)
            };
            
            if (typearguments != null)
            {
                foreach (var type in typearguments)
                {
                    this.invoker.Method.TypeArguments.Add(type);
                }
            }
            
            return this;
        }


        /// <summary>
        /// Add a primitive parameter to the method invocation.
        /// </summary>
        /// <param name="value">The content of the primitive expression.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
        /// <exception cref="CodeTypeReferenceException">Cannot add parameter to a method that is not defined.Use Invoke(...) to specify the method.</exception>
        public CodeTypeReferenceBinder With(object value)
        {
            if (this.invoker == null)
            {
                const string Msg = "Cannot add parameter to a method that is not defined." +
                                   "Use Invoke(...) to specify the method.";
                throw new CodeTypeReferenceException(this, Msg);
            }

            var primitive = new CodePrimitiveExpression(value);
            this.invoker.Parameters.Add(primitive);
            return this;
        }

        /// <summary>
        /// Add a specified expression as parameter to the method invocation.
        /// </summary>
        /// <param name="value">The code expression to add.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
        /// <exception cref="CodeTypeReferenceException">Cannot add parameter to a method that is not defined.Use Invoke(...) to specify the method.</exception>
        public CodeTypeReferenceBinder With(CodeExpression expression)
        {
            if (this.invoker == null)
            {
                const string Msg = "Cannot add parameter to a method that is not defined." +
                                   "Use Invoke(...) to specify the method.";
                throw new CodeTypeReferenceException(this, Msg);
            }

            this.invoker.Parameters.Add(expression);
            return this;
        }

        /// <summary>
        /// Add a parameter with a reference to a local variable of the specified name to the method invocation.
        /// </summary>
        /// <param name="variableName">The name of the referenced local variable.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
        /// <exception cref="CodeTypeReferenceException">Cannot add parameter to a method that is not defined.Use Invoke(...) to specify the method.</exception>
        public CodeTypeReferenceBinder WithReference(string variableName)
        {
            // Todo: add WithThisReference
            if (this.invoker == null)
            {
                const string Msg = "Cannot add parameter to a method that is not defined." +
                                   "Use Invoke(...) to specify the method.";
                throw new CodeTypeReferenceException(this, Msg);
            }

            var varRef = new CodeVariableReferenceExpression(variableName);
            this.invoker.Parameters.Add(varRef);
            return this;
        }

        /// <summary>
        /// Add multiple parameters with a reference to a local variables of the specified name to the method invocation.
        /// </summary>
        /// <param name="variableNames">The list of local variable names.</param>
        /// <returns>
        /// A fluent interface to build up reference types.
        /// </returns>
        /// <exception cref="CodeTypeReferenceException">Cannot add parameter to a method that is not defined.Use Invoke(...) to specify the method.</exception>
        public CodeTypeReferenceBinder WithReference(params string[] variableNames)
        {
            // Todo: add WithThisReference
            if (this.invoker == null)
            {
                const string Msg = "Cannot add parameter to a method that is not defined." +
                                   "Use Invoke(...) to specify the method.";
                throw new CodeTypeReferenceException(this, Msg);
            }

            foreach (var variableName in variableNames)
            {
                var varRef = new CodeVariableReferenceExpression(variableName);
                this.invoker.Parameters.Add(varRef);
            }

            return this;
        }

        /*public CodeMemberMethod Assign()
        {
            if (localVar == null)
            {

            }
            localVar.Commit(this);
            //method.Statements.Add(invoker);
            return method;
        }*/
    }
}