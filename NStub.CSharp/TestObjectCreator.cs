using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;

namespace NStub.CSharp
{
    /// <summary>
    /// TestObjectCreator.
    /// </summary>
    [Serializable]
    internal class TestObjectCreator : IEquatable<TestObjectCreator>
    {
        #region Properties

        /// <summary>
        /// Gets the SetUpMethod.
        /// </summary>
        /// <value>The SetUpMethod.</value>
        internal CodeMemberMethod SetUpMethod { get; private set; }

        /// <summary>
        /// Gets the TestObjectMemberField.
        /// </summary>
        /// <value>The TestObjectMemberField.</value>
        internal CodeMemberField TestObjectMemberField { get; private set; }

        /// <summary>
        /// Gets the TestObjectName.
        /// </summary>
        /// <value>The TestObjectName.</value>
        internal string TestObjectName { get; private set; }

        /// <summary>
        /// Gets the type of the test object.
        /// </summary>
        /// <value>
        /// The type of the test object.
        /// </value>
        internal Type TestObjectType { get; private set; }

        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TestObjectCreator"/> class.
        /// </summary>
        internal TestObjectCreator() { }
        /// <summary>
        /// Initializes a new fully specified instance of the <see cref="TestObjectCreator"/> class.
        /// </summary>
        /// <param name="SetUpMethod">The SetUpMethod</param>
        /// <param name="TestObjectMemberField">The TestObjectMemberField</param>
        /// <param name="TestObjectName">The TestObjectName</param>
        /// <param name="testObjectType">Type of the test object.</param>
        internal TestObjectCreator(CodeMemberMethod SetUpMethod, CodeMemberField TestObjectMemberField, string TestObjectName, Type testObjectType)
        {
            Guard.NotNull(() => SetUpMethod, SetUpMethod);
            Guard.NotNull(() => TestObjectMemberField, TestObjectMemberField);
            Guard.NotNull(() => TestObjectName, TestObjectName);
            Guard.NotNull(() => testObjectType, testObjectType);

            this.SetUpMethod = SetUpMethod;
            this.TestObjectMemberField = TestObjectMemberField;
            this.TestObjectName = TestObjectName;
            this.TestObjectType = testObjectType;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            TestObjectCreator other = obj as TestObjectCreator;
            if (other != null)
                return Equals(other);
            return false;
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TestObjectCreator other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return
              SetUpMethod == other.SetUpMethod &&
              TestObjectMemberField == other.TestObjectMemberField &&
              TestObjectName == other.TestObjectName;
        }
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SetUpMethod = " + SetUpMethod + ";");
            sb.Append("TestObjectMemberField = " + TestObjectMemberField + ";");
            sb.Append("TestObjectName = " + TestObjectName);
            return sb.ToString();
        }
        #endregion


        /// <summary>
        /// Creates a code generation expression for an object to test with a member field and initialization
        /// in the previous specified <see cref="SetUpMethod"/> method.
        /// </summary>
        /// <returns>
        /// The initialization expression of the object under test.
        /// </returns>
        public virtual CodeObjectCreateExpression BuildTestObject()
        {
            /*var invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                //new CodePrimitiveExpression("expected")
                new CodeFieldReferenceExpression(testObjectMemberField, "bla")
                , new CodeVariableReferenceExpression("actual"));*/
            var fieldRef1 =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.TestObjectMemberField.Name);

            var testObjectMemberFieldCreate = new CodeObjectCreateExpression(this.TestObjectName, new CodeExpression[] { });
            this.testObjectMemberFieldCreateExpression = testObjectMemberFieldCreate;
            var as1 = new CodeAssignStatement(fieldRef1, testObjectMemberFieldCreate);
            this.assignments = AddParametersToConstructor(as1);

            // Creates a statement using a code expression.
            // var expressionStatement = new CodeExpressionStatement(fieldRef1);
            AddCreationStatement(as1);


            return testObjectMemberFieldCreate;
            //return mms;
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
                return assignments != null && assignments.Count > 0;
            }
        }

        private ConstructorAssignmentList assignments;

        /// <summary>
        /// Gets the assignments related to this instance.
        /// </summary>
        public IEnumerable<AssignmentInfo> Assignments
        {
            get
            {
                return this.assignments;
            }
        }

        private CodeObjectCreateExpression testObjectMemberFieldCreateExpression;

        /// <summary>
        /// Gets the member field create expression for the object under test.
        /// </summary>
        public CodeObjectCreateExpression TestObjectMemberFieldCreateExpression
        {
            get
            {
                return this.testObjectMemberFieldCreateExpression;
            }
        }

        /// <summary>
        /// Add the parameter assignments to the specified constructor expression.
        /// </summary>
        /// <param name="as1">The expression with the constructor to add parameter initialization to.</param>
        /// <returns>A structure with data about parameter initialization for the type of test object of this instance.</returns>
        private ConstructorAssignmentList AddParametersToConstructor(CodeAssignStatement as1)
        {

            //Type[] parameters = { /*typeof(int)*/ };

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
            var testObjectConstructors = this.TestObjectType.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public);
            //var ctorParameterTypes = new List<ParameterInfo>();

            if (testObjectConstructors.Length < 1)
            {
                return null;
            }


            // for now, use to ctor with the most parameters.
            // foreach testObjectConstructors
            //var most = testObjectConstructors.Max(e => e.GetParameters().Length);
            //var constructor = testObjectConstructors.Where(e => e.GetParameters().Length == most).First();
            var tempAssignments = new List<AssignmentInfo>();
            int most = -1;
            AssignmentInfo mostAssignmentInfo = null;
            foreach (var constructor in testObjectConstructors)
            {
                var assignments = BuildAssignmentInfoForConstructor(constructor);
                int parameterAmount = constructor.GetParameters().Length;
                if (parameterAmount > most)
                {
                    most = parameterAmount;
                    mostAssignmentInfo = assignments;
                }
                else
                {
                    tempAssignments.Add(assignments);
                }
            }

            if (mostAssignmentInfo == null)
            {
                throw new InvalidOperationException("No preferred constructor found, but there should one.");
            }

            var result = new ConstructorAssignmentList(mostAssignmentInfo);
            result.AddConstructorAssignment(tempAssignments);

            return result;

        }

        /// <summary>
        /// Builds a assignment info for the specified constructor.
        /// </summary>
        /// <param name="constructor">The constructor to build the parameter assignment info's for.</param>
        /// <returns>An <see cref="AssignmentInfo"/> intitialized with the data from the specified 
        /// <paramref name="constructor"/>.</returns>
        private AssignmentInfo BuildAssignmentInfoForConstructor(ConstructorInfo constructor)
        {
            //var ctorParameterTypesInterface = new List<ParameterInfo>();
            //var ctorParameterTypesStd = new List<ParameterInfo>();
            var ctorParameterTypes = new ParameterInfoList();
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


            //var mockRepositoryMemberField = this.AddMockRepository(
            //    testClassDeclaration, setUpMethod, testObjectName, null, "mocks");

            // BuildAssignmentInfoForConstructor

            var assignments = new AssignmentInfo();
            assignments.UsedConstructor = constructor;
            foreach (var paraInfo in ctorParameterTypes.StandardTypes)
            {
                var memberField = BaseCSharpCodeGenerator.CreateMemberField(
                    paraInfo.ParameterType.FullName, paraInfo.Name);
                var fieldAssignment = this.CreateInitializeMemberField(paraInfo.ParameterType, paraInfo.Name);
                var assignment = new ConstructorAssignment(paraInfo.Name, fieldAssignment, memberField);
                assignments.AddAssignment(assignment);
            }



            return assignments;
        }


        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to the constructor create
        /// expression stored in <see cref="TestObjectMemberFieldCreateExpression"/>.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        public void AssignParameters(CodeTypeDeclaration testClassDeclaration)
        {
            this.AssignParameters(testClassDeclaration, this.TestObjectMemberFieldCreateExpression);
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

                var result = true;
                var ctorparameters = this.assignments.PreferredConstructor.UsedConstructor.GetParameters();
                foreach (var para in ctorparameters)
                {
                    var assignment = this.assignments.PreferredConstructor[para.Name];
                    if (assignment == null)
                    {
                        return false;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to the specified constructor create
        /// expression.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="testObjectConstructor">The object constructor to create the parameter initializers for.</param>
        public void AssignParameters(CodeTypeDeclaration testClassDeclaration, CodeObjectCreateExpression testObjectConstructor)
        {
            Guard.NotNull(() => testClassDeclaration, testClassDeclaration);
            Guard.NotNull(() => testObjectConstructor, testObjectConstructor);
            //Guard.NotNull(() => this.assignments, this.assignments);
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
                    AddCreationStatement(assignment.AssignStatement);
                    // Add the local variable to the constructor initializer in the object create expression 
                    // (e.g. SetUp method, test object constructor) of the specified method.
                    testObjectConstructor.Parameters.Add(assignment.AssignStatement.Left);
                }

                // reorder the testObject initializer to the bottom of the SetUp method.
                ReorderSetupStatement(testObjectInitializerPosition);
            }
        }

        /// <summary>
        /// Reorders the statement at the specified position to the last position of the code generation statements
        /// in the <see cref="SetUpMethod"/>.
        /// </summary>
        /// <param name="testObjectInitializerPosition">The position of the statement to push to the bottom.</param>
        private void ReorderSetupStatement(int testObjectInitializerPosition)
        {
            var removedTypedec = this.SetUpMethod.Statements[testObjectInitializerPosition];
            this.SetUpMethod.Statements.RemoveAt(testObjectInitializerPosition);
            this.SetUpMethod.Statements.Add(removedTypedec);
        }

        /// <summary>
        /// Creates a reference to a member field and initializes it with a new instance of the specified parameter type.
        /// </summary>
        /// <param name="type">Defines the type of the new object.</param>
        /// <param name="memberField">Name of the referenced member field.</param>
        /// <returns></returns>
        /// <remarks>Produces a statement like: 
        /// <code>this.project = new Microsoft.Build.BuildEngine.Project();</code>.</remarks>
        private CodeAssignStatement CreateInitializeMemberField(Type type, string memberField)
        {
            var fieldRef1 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), memberField);
            var objectCreate1 = new CodeObjectCreateExpression(type.FullName, new CodeExpression[] { });
            return new CodeAssignStatement(fieldRef1, objectCreate1);
        }


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

        private void AddCreationStatement(CodeAssignStatement as1)
        {
            this.SetUpMethod.Statements.Add(as1);
        }

    }
}
