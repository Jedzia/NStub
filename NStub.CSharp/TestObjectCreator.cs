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
        /// Compose additional items of the test setup method.
        /// </summary>
        /// <param name="setUpMethod">The test setup method.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <returns>
        /// The initialization expression of the object under test.
        /// Is <c>null</c>, when none is created.
        /// </returns>
        //public virtual CodeObjectCreateExpression ComposeTestSetupMethod(
        public virtual CodeObjectCreateExpression ComposeTestSetupMethod(
            /*CodeMemberMethod setUpMethod,
            CodeMemberField testObjectMemberField,
            string testObjectName*/
                                   )
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
            var as1 = new CodeAssignStatement(fieldRef1, testObjectMemberFieldCreate);
            this.creationMembers = AddParametersToConstructor(as1);

            // Creates a statement using a code expression.
            // var expressionStatement = new CodeExpressionStatement(fieldRef1);
            AddCreationStatement(as1);
            return testObjectMemberFieldCreate;
            //return mms;
        }

        /// <summary>
        /// The summary.
        /// </summary>
        private List<CodeMemberField> creationMembers;

        /// <summary>
        /// Gets or sets the CreationMembers.
        /// </summary>
        /// <value>The CreationMembers.</value>
        public IEnumerable<CodeMemberField> CreationMembers
        {
            get
            {
                return this.creationMembers;
            }
        }

        public bool HasCreationMembers
        {
            get
            {
                return creationMembers != null && creationMembers.Count > 0;
            }
        }


        private List<CodeMemberField> AddParametersToConstructor(CodeAssignStatement as1)
        {

            Type[] parameters = { /*typeof(int)*/ };

            // Get the constructor that takes an integer as a parameter.
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
                return new List<CodeMemberField>();
            }

            // else
            // {
            // outputBlock.Text += 
            // "The public constructor of MyClass that takes an integer as a parameter is:\n"; 
            // outputBlock.Text += ctor.ToString() + "\n";
            // }
            var testObjectConstructors = this.TestObjectType.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public);
            bool hasInterfaceInCtorParameters = false;
            var ctorParameterTypesInterface = new List<ParameterInfo>();
            var ctorParameterTypesStd = new List<ParameterInfo>();
            //var ctorParameterTypes = new List<ParameterInfo>();

            foreach (var constructor in testObjectConstructors)
            {
                var ctorParameters = constructor.GetParameters();
                foreach (var para in ctorParameters)
                {
                    if (!para.ParameterType.IsGenericType)
                    {
                        if (para.ParameterType.IsInterface)
                        {
                            hasInterfaceInCtorParameters = true;
                            ctorParameterTypesInterface.Add(para);
                        }
                        else
                        {
                            ctorParameterTypesStd.Add(para);
                        }
                    }
                }
            }

            if (ctorParameterTypesStd.Count > 0)
            {

            }

            if (!hasInterfaceInCtorParameters)
            {
                //return new CodeAssignStatement[0];
                return new List<CodeMemberField>();
            }

            //var testObjectInitializerPosition = setUpMethod.Statements.Count - 1;

            //var mockRepositoryMemberField = this.AddMockRepository(
            //    testClassDeclaration, setUpMethod, testObjectName, null, "mocks");

            var mockMemberFields = new List<CodeMemberField>();
            var mockAssignments = new List<CodeAssignStatement>();
            foreach (var paraInfo in ctorParameterTypesStd)
            {
                var mockMemberField = BaseCSharpCodeGenerator.CreateMemberField(
                    paraInfo.ParameterType.FullName, paraInfo.Name);
                var mockAssignment = this.AddConstructorObject(
                    this.SetUpMethod, mockMemberField, this.TestObjectName, paraInfo, paraInfo.Name);
                //mockAssignments.Add(mockAssignment);
                mockMemberFields.Add(mockMemberField);
            }

            // reorder the testObject initializer to the bottom of the SetUp method.
            //var removedTypedec = setUpMethod.Statements[testObjectInitializerPosition];
            //setUpMethod.Statements.RemoveAt(testObjectInitializerPosition);
            //setUpMethod.Statements.Add(removedTypedec);

            //return mockAssignments;
            return mockMemberFields;

        }

        private CodeAssignStatement AddConstructorObject(
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
            //var invokeExpression = new CodeMethodInvokeExpression(mockRef, "StrictMock");
            //invokeExpression.Method.TypeArguments.Add(paraType.FullName);

            // Creates a statement using a code expression.
            //var expressionStatement = new CodeExpressionStatement(invokeExpression);
            var objectCreate1 = new CodeObjectCreateExpression(paraType.FullName, new CodeExpression[] { });
            var as1 = new CodeAssignStatement(fieldRef1, objectCreate1);
            setUpMethod.Statements.Add(as1);

            // var testObject = TestMemberFieldLookup["testObject"];
            // setUpMethod.Statements.Add(as1);
            return as1;
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
