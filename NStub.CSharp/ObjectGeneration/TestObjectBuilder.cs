// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestObjectBuilder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Used to create new test member fields with the matching initialization for them.
    /// </summary>
    [Serializable]
    internal class TestObjectBuilder : TestObjectBuilderBase, IEquatable<TestObjectBuilder>
    {
        /*/// <summary>
        /// Initializes a new instance of the <see cref="TestObjectCreator"/> class.
        /// </summary>
        internal TestObjectCreator() { }*/
        #region Fields

        private ConstructorAssignmentCollection assignments;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestObjectBuilder"/> class.
        /// </summary>
        /// <param name="setUpMethod">The SetUpMethod</param>
        /// <param name="testObjectMemberField">The TestObjectMemberField</param>
        /// <param name="testObjectName">The TestObjectName</param>
        /// <param name="testObjectType">Type of the test object.</param>
        internal TestObjectBuilder(
            CodeMemberMethod setUpMethod,
            CodeMemberField testObjectMemberField,
            string testObjectName,
            Type testObjectType)
            : base(setUpMethod, testObjectMemberField, testObjectName, testObjectType)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the assignments related to this instance.
        /// </summary>
        public IEnumerable<AssignmentInfoCollection> Assignments
        {
            get
            {
                return this.assignments;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has ready initialized parameter assignments.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has parameter assignments; otherwise, <c>false</c>.
        /// </value>
        public bool HasParameterAssignments
        {
            get
            {
                return this.assignments != null && this.assignments.Count > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance constructor parameters are completely assigned.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is completely assigned; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompletelyAssigned
        {
            get
            {
                if (!this.HasParameterAssignments)
                {
                    return true;
                }

                var ctorparameters = this.assignments.PreferredConstructor.UsedConstructor.GetParameters();
                foreach (var para in ctorparameters)
                {
                    var assignment = this.assignments.PreferredConstructor[para.Name];
                    if (assignment == null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        #endregion

        /// <summary>
        /// Creates a reference to a member field and initializes it with a new instance of the specified parameter type.
        /// </summary>
        /// <param name="type">Defines the type of the new object.</param>
        /// <param name="memberField">Name of the referenced member field.</param>
        /// <returns>An assignment statement for the specified member field.</returns>
        /// <remarks>Produces a statement like: 
        /// <code>this.project = new Microsoft.Build.BuildEngine.Project();</code>.</remarks>
        public static CodeAssignStatement CreateInitializeMemberField(Type type, string memberField)
        {
            var fieldRef1 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), memberField);
            var objectCreate1 = new CodeObjectCreateExpression(type.FullName, new CodeExpression[] { });
            return new CodeAssignStatement(fieldRef1, objectCreate1);
        }

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to the specified constructor create
        /// expression.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="testObjectConstructor">The object constructor to create the parameter initializers for.</param>
        public override void AssignParameters(
            CodeTypeDeclaration testClassDeclaration, CodeObjectCreateExpression testObjectConstructor)
        {
            Guard.NotNull(() => testClassDeclaration, testClassDeclaration);
            Guard.NotNull(() => testObjectConstructor, testObjectConstructor);

            // Guard.NotNull(() => this.assignments, this.assignments);
            if (testClassDeclaration.Name.StartsWith("Jedzia.SamCat.Model.Tasks.TaskComposer"))
            {
            }

            if (this.HasParameterAssignments)
            {
                var testObjectInitializerPosition = SetUpMethod.Statements.Count - 1;

                var ctorparameters = this.assignments.PreferredConstructor.UsedConstructor.GetParameters();
                foreach (var para in ctorparameters)
                {
                    var assignment = this.assignments.PreferredConstructor[para.Name];
                    if (assignment == null)
                    {
                        continue;
                    }

                    // Add the member field to the test class.
                    testClassDeclaration.Members.Add(assignment.MemberField);

                    // Add a local variable for the constructor parameter.
                    AddAssignStatement(assignment.AssignStatement);

                    // Add the local variable to the constructor initializer in the object create expression 
                    // (e.g. SetUp method, test object constructor) of the specified method.
                    testObjectConstructor.Parameters.Add(assignment.AssignStatement.Left);
                }

                // reorder the testObject initializer to the bottom of the SetUp method.
                this.ReorderSetupStatement(testObjectInitializerPosition);
            }
        }

        /// <summary>
        /// Creates a code generation expression for an object to test with a member field and initialization
        /// in the constructor specified <see cref="TestObjectBuilderBase.SetUpMethod"/> method.
        /// </summary>
        /// <returns>
        /// The initialization expression of the object under test.
        /// </returns>
        public override CodeObjectCreateExpression BuildTestObject()
        {
            /*var invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                //new CodePrimitiveExpression("expected")
                new CodeFieldReferenceExpression(testObjectMemberField, "bla")
                , new CodeVariableReferenceExpression("actual"));*/
            var fieldRef1 =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), TestObjectMemberField.Name);

            var testObjectMemberFieldCreate = new CodeObjectCreateExpression(TestObjectName, new CodeExpression[] { });
            TestObjectMemberFieldCreateExpression = testObjectMemberFieldCreate;
            var as1 = new CodeAssignStatement(fieldRef1, testObjectMemberFieldCreate);
            this.assignments = this.AddParametersToConstructor();

            // Creates a statement using a code expression.
            // var expressionStatement = new CodeExpressionStatement(fieldRef1);
            AddAssignStatement(as1);

            return testObjectMemberFieldCreate;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is <c>null</c>.</exception>
        public override bool Equals(object obj)
        {
            var other = obj as TestObjectBuilder;
            return other != null && this.Equals(other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(TestObjectBuilder other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return
                SetUpMethod == other.SetUpMethod &&
                TestObjectMemberField == other.TestObjectMemberField &&
                TestObjectName == other.TestObjectName;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.assignments != null ? this.assignments.GetHashCode() : 0;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("SetUpMethod = " + SetUpMethod + ";");
            sb.Append("TestObjectMemberField = " + TestObjectMemberField + ";");
            sb.Append("TestObjectName = " + TestObjectName);
            return sb.ToString();
        }

        /*
                private CodeAssignStatement AddMockObject(
                    CodeMemberMethod setUpMethod,
                    CodeMemberField mockRepositoryMemberField,
                    string testObjectName,
                    ParameterInfo paraInfo,
                    string paraName)
                {
                    var paraType = paraInfo.ParameterType;

                    var mockRef =
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), mockRepositoryMemberField.Name);

                    var fieldRef1 =
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), paraName);

                    // Creates a statement using a code expression.
                    // var expressionStatement = new CodeExpressionStatement(fieldRef1);

                    // Creates a code expression for a CodeExpressionStatement to contain.
                    var invokeExpression = new CodeMethodInvokeExpression(mockRef, "StrictMock");
                    invokeExpression.Method.TypeArguments.Add(paraType.FullName);

                    // Creates a statement using a code expression.
                    // var expressionStatement = new CodeExpressionStatement(invokeExpression);
                    // var objectCreate1 = new CodeObjectCreateExpression(testObjectName, new CodeExpression[] { });
                    var as1 = new CodeAssignStatement(fieldRef1, invokeExpression);
                    setUpMethod.Statements.Add(as1);

                    // var testObject = TestMemberFieldLookup["testObject"];
                    // setUpMethod.Statements.Add(as1);
                    return as1;
                }
        */

        /// <summary>
        /// Add the parameter assignments to the specified constructor expression.
        /// </summary>
        /// <returns>A structure with data about parameter initialization for the type of test object of this instance.</returns>
        /// <exception cref="InvalidOperationException">No preferred constructor found, but there should one.</exception>
        private ConstructorAssignmentCollection AddParametersToConstructor()
        {
            // Type[] parameters = { /*typeof(int)*/ };

            /*// Get the constructor that takes an integer as a parameter.
            var ctor = this.TestObjectType.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                Type.DefaultBinder,
                parameters,
                null);

            if (ctor == null)
            {
                // outputBlock.Text += 
                // "There is no public constructor of MyClass that takes an integer as a parameter.\n";
            }
            else
            {
                // there's only a default ctor, return.
                return assignments;
            }*/

            // else
            // {
            // outputBlock.Text += 
            // "The public constructor of MyClass that takes an integer as a parameter is:\n"; 
            // outputBlock.Text += ctor.ToString() + "\n";
            // }
            var testObjectConstructors = TestObjectType.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public);

            // var ctorParameterTypes = new List<ParameterInfo>();
            if (testObjectConstructors.Length < 1)
            {
                return null;
            }

            // for now, use to ctor with the most parameters.
            // foreach testObjectConstructors
            // var most = testObjectConstructors.Max(e => e.GetParameters().Length);
            // var constructor = testObjectConstructors.Where(e => e.GetParameters().Length == most).First();
            var tempAssignments = new List<AssignmentInfoCollection>();
            int most = -1;
            AssignmentInfoCollection mostAssignmentInfoCollection = null;
            foreach (var constructor in testObjectConstructors)
            {
                var assignmentInfoCollection = this.BuildAssignmentInfoForConstructor(constructor);
                int parameterAmount = constructor.GetParameters().Length;
                if (parameterAmount > most)
                {
                    most = parameterAmount;
                    mostAssignmentInfoCollection = assignmentInfoCollection;
                }
                else
                {
                    tempAssignments.Add(assignmentInfoCollection);
                }
            }

            if (mostAssignmentInfoCollection == null)
            {
                throw new InvalidOperationException("No preferred constructor found, but there should one.");
            }

            var result = new ConstructorAssignmentCollection(mostAssignmentInfoCollection);
            result.AddConstructorAssignment(tempAssignments);

            return result;
        }

        /// <summary>
        /// Builds a assignment info for the specified constructor.
        /// </summary>
        /// <param name="constructor">The constructor to build the parameter assignment info's for.</param>
        /// <returns>An <see cref="AssignmentInfoCollection"/> initialized with the data from the specified 
        /// <paramref name="constructor"/>.</returns>
        private AssignmentInfoCollection BuildAssignmentInfoForConstructor(ConstructorInfo constructor)
        {
            // var ctorParameterTypesInterface = new List<ParameterInfo>();
            // var ctorParameterTypesStd = new List<ParameterInfo>();
            var ctorParameterTypes = new ParameterInfoCollection();
            var ctorParameters = constructor.GetParameters();
            foreach (var para in ctorParameters)
            {
                if (!para.ParameterType.IsGenericType)
                {
                    ctorParameterTypes.AddParameterInfo(para);

                    /*if (para.ParameterType.IsInterface)
                                        {
                                            hasInterfaceInCtorParameters = true;
                                            ctorParameterTypesInterface.Add(para);
                                        }
                                        else
                                        {
                                            ctorParameterTypesStd.Add(para);
                                        }*/
                }
            }

            /*if (ctorParameterTypes.CountOfStandardTypes > 0)
            {

            }*/

            /*if (!ctorParameterTypes.HasInterfaces)
            {
                //return new CodeAssignStatement[0];
                return assignments;
            }*/

            // var mockRepositoryMemberField = this.AddMockRepository(
            // testClassDeclaration, setUpMethod, testObjectName, null, "mocks");

            // BuildAssignmentInfoForConstructor
            var assignmentInfoCollection = new AssignmentInfoCollection { UsedConstructor = constructor };
            foreach (var paraInfo in ctorParameterTypes.StandardTypes)
            {
                var memberField = BaseCSharpCodeGenerator.CreateMemberField(
                    paraInfo.ParameterType.FullName, paraInfo.Name);
                var fieldAssignment = CreateInitializeMemberField(paraInfo.ParameterType, paraInfo.Name);
                var assignment = new ConstructorAssignment(paraInfo.Name, fieldAssignment, memberField);
                assignmentInfoCollection.AddAssignment(assignment);
            }

            return assignmentInfoCollection;
        }

        /// <summary>
        /// Reorders the statement at the specified position to the last position of the code generation statements
        /// in the <see cref="TestObjectBuilderBase.SetUpMethod"/>.
        /// </summary>
        /// <param name="testObjectInitializerPosition">The position of the statement to push to the bottom.</param>
        private void ReorderSetupStatement(int testObjectInitializerPosition)
        {
            var removedTypedec = SetUpMethod.Statements[testObjectInitializerPosition];
            SetUpMethod.Statements.RemoveAt(testObjectInitializerPosition);
            SetUpMethod.Statements.Add(removedTypedec);
        }
    }
}