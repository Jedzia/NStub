using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using NStub.Core;
using NStub.CSharp;
using MbUnit.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace NStub.CSharp
{

    public class ImplicitVariableDeclarationStatement : CodeVariableDeclarationStatement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitVariableDeclarationStatement"/> class
        /// using the specified data type, variable name, and initialization expression.
        /// </summary>
        // /// <param name="type">The name of the data type of the variable.</param>
        /// <param name="name">The name of the variable.</param>
        /// <param name="initExpression">A <see cref="System.CodeDom.CodeExpression"/> that indicates
        /// the initialization expression for the variable.</param>
        public ImplicitVariableDeclarationStatement(string name, CodeExpression initExpression)
            : base("var", name, initExpression)
        {

        }
    }

    /// <summary>
    /// The CSharpCodeGenerator is responsible for the generation of the individual
    /// class files which will make up the actual test project.  For information
    /// regarding the generation of the project file, see 
    /// <see cref="CSharpProjectGenerator">CSharpProjectGenerator</see>.
    /// </summary>
    public class CSharpMbUnitCodeGenerator : BaseCSharpCodeGenerator, Core.ICodeGenerator
    {
        #region Fields (Private)

        //private CodeNamespace UnitCodeNamespace;
        //private string UnitOutputDirectory;

        #endregion Fields (Private)

        #region Constructor (Public)

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpCodeGenerator"/> class
        /// based the given CodeNamespace which will output to the given directory.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="outputDirectory">The output directory.</param>
        /// <exception cref="System.ArgumentNullException">codeNamepsace or
        /// outputDirectory is null.</exception>
        /// <exception cref="System.ArgumentException">outputDirectory is an
        /// empty string.</exception>
        /// <exception cref="DirectoryNotFoundException">outputDirectory
        /// cannot be found.</exception>
        public CSharpMbUnitCodeGenerator(CodeNamespace codeNamespace, string outputDirectory)
            : base(codeNamespace, outputDirectory)
        {
        }

        #endregion Constructor (Public)

        #region Properties (Public)


        #endregion Properties (Public)

        #region Methods (Public)

        /// <summary>
        /// Handle property related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ComputeCodeMemberProperty(CodeMemberMethod typeMember, string propertyName)
        {

            var methodMemberInfo = typeMember.UserData["MethodMemberInfo"];

            typeMember.Name = "Property" + propertyName + "NormalBehavior";
            /*if (typeMember.Name.Contains("get_"))
            {
                typeMember.Name = typeMember.Name.Replace("get_", "Property");
            }
            else if (typeMember.Name.Contains("set_"))
            {
                typeMember.Name = typeMember.Name.Replace("set_", "Property");
            }*/
            //typeMember.Name += "NormalBehavior";
            //ReplaceTestInTestName(typeMember, "NormalBehavior");


            var variableDeclaration = new CodeVariableDeclarationStatement(
                // Type of the variable to declare.
                //typeof(string),
                "var",
                // Name of the variable to declare.
                "expected",
                // Optional initExpression parameter initializes the variable.
                new CodePrimitiveExpression("Testing"));

            // A C# code generator produces the following source code for the preceeding example code:

            // string TestString = "Testing";
            typeMember.Statements.Add(variableDeclaration);

            variableDeclaration = new ImplicitVariableDeclarationStatement(
                "actual", new CodePrimitiveExpression("Testing"));
            typeMember.Statements.Add(variableDeclaration);

            // Creates a code expression for a CodeExpressionStatement to contain.
            var invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                //new CodePrimitiveExpression("expected")
                new CodeVariableReferenceExpression("expected")
                , new CodeVariableReferenceExpression("actual"));

            // Creates a statement using a code expression.
            var expressionStatement = new CodeExpressionStatement(invokeExpression);

            // A C# code generator produces the following source code for the preceeding example code:

            // Console.Write( "Example string" );
            typeMember.Statements.Add(invokeExpression);
        }

        /// <summary>
        /// Compose additional items of the test teardown method.
        /// </summary>
        /// <param name="teardownMethod">A reference to the teardown method of the test.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        protected override void ComposeTestTearDownMethod(
            CodeMemberMethod teardownMethod, 
            CodeMemberField testObjectMemberField, 
            string testObjectName)
        {
            /*var invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                //new CodePrimitiveExpression("expected")
                new CodeFieldReferenceExpression(testObjectMemberField, "bla")
                , new CodeVariableReferenceExpression("actual"));*/

            CodeFieldReferenceExpression fieldRef1 =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), testObjectMemberField.Name);

            CodeObjectCreateExpression objectCreate1 =
                new CodeObjectCreateExpression(testObjectName, new CodeExpression[] { });
            CodeAssignStatement as1 =
                new CodeAssignStatement(fieldRef1, new CodePrimitiveExpression(null));
            //new CodeAssignStatement(fieldRef1, objectCreate1);


            // Creates a statement using a code expression.
            //var expressionStatement = new CodeExpressionStatement(fieldRef1);
            teardownMethod.Statements.Add(as1);
        }

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
        protected override CodeObjectCreateExpression ComposeTestSetupMethod(
                    CodeMemberMethod setUpMethod,
                    CodeMemberField testObjectMemberField,
                    string testObjectName)
        {
            /*var invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                //new CodePrimitiveExpression("expected")
                new CodeFieldReferenceExpression(testObjectMemberField, "bla")
                , new CodeVariableReferenceExpression("actual"));*/

            CodeFieldReferenceExpression fieldRef1 =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), testObjectMemberField.Name);

            var testObjectMemberFieldCreate = new CodeObjectCreateExpression(testObjectName, new CodeExpression[] { });
            CodeAssignStatement as1 =
                //new CodeAssignStatement(fieldRef1, new CodePrimitiveExpression(10));
            new CodeAssignStatement(fieldRef1, testObjectMemberFieldCreate);


            // Creates a statement using a code expression.
            //var expressionStatement = new CodeExpressionStatement(fieldRef1);
            setUpMethod.Statements.Add(as1);
            return testObjectMemberFieldCreate;
        }

        /// <summary>
        /// Compose additional items of the test setup method.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.( early testObject ).</param>
        /// <param name="setUpMethod">The test setup method.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        protected override IEnumerable<CodeAssignStatement> ComposeTestSetupMockery(CodeTypeDeclaration testClassDeclaration,
            CodeMemberMethod setUpMethod,
            CodeMemberField testObjectMemberField,
            string testObjectName)
        {
            var testObjectClassType = (Type)testClassDeclaration.UserData["TestObjectClassType"];


            Type[] parameters = { /*typeof(int)*/ };

            // Get the constructor that takes an integer as a parameter.
            ConstructorInfo ctor = testObjectClassType.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                Type.DefaultBinder,
                parameters,
                null);

            if (ctor == null)
            {
                //outputBlock.Text += 
                //    "There is no public constructor of MyClass that takes an integer as a parameter.\n";
            }
            else
            {
                //outputBlock.Text += 
                //    "The public constructor of MyClass that takes an integer as a parameter is:\n"; 
                //outputBlock.Text += ctor.ToString() + "\n";
            }



            var testObjectConstructors = testObjectClassType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            bool hasInterfaceInCtorParameters = false;
            var ctorParameterTypes = new List<ParameterInfo>();
            //if (testObjectConstructors.Length > 1)
            //{
            foreach (var constructor in testObjectConstructors)
            {
                var ctorParameters = constructor.GetParameters();
                foreach (var para in ctorParameters)
                {
                    if (para.ParameterType.IsInterface && !para.ParameterType.IsGenericType)
                    {
                        hasInterfaceInCtorParameters = true;
                        ctorParameterTypes.Add(para);
                    }
                }
                //ConstructorInfo
            }
            //}
            if (!hasInterfaceInCtorParameters)
            {
                return new CodeAssignStatement[0];
            }

            var testObjectInitializerPosition = setUpMethod.Statements.Count - 1;

            var mockRepositoryMemberField = AddMockRepository(testClassDeclaration, setUpMethod, testObjectName, null, "mocks");

            List<CodeAssignStatement> mockAssignments = new List<CodeAssignStatement>();
            foreach (var paraInfo in ctorParameterTypes)
            {
                var mockMemberField = AddTestMemberField(testClassDeclaration, paraInfo.ParameterType.FullName, paraInfo.Name);
                var mockAssignment = AddMockObject(setUpMethod, mockRepositoryMemberField, testObjectName, paraInfo, paraInfo.Name);
                mockAssignments.Add(mockAssignment);
            }

            // reorder the testObject initializer to the bottom of the SetUp method.
            var removedTypedec = setUpMethod.Statements[testObjectInitializerPosition];
            setUpMethod.Statements.RemoveAt(testObjectInitializerPosition);
            setUpMethod.Statements.Add(removedTypedec);

            return mockAssignments;
        }

        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        private CodeMemberField AddMockRepository(
            CodeTypeDeclaration testClassDeclaration, 
            CodeMemberMethod setUpMethod, 
            string testObjectName,
            ParameterInfo paraInfo, 
            string paraName)
        {
            var mockMemberField = AddTestMemberField(testClassDeclaration, typeof(Rhino.Mocks.MockRepository).Name, "mocks");

            //testClassDeclaration.us
            //var paraType = paraInfo.ParameterType;
            CodeFieldReferenceExpression fieldRef1 =
               new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), paraName);

            CodeObjectCreateExpression objectCreate1 =
                new CodeObjectCreateExpression(typeof(Rhino.Mocks.MockRepository).Name, new CodeExpression[] { });
            CodeAssignStatement as1 =
                //new CodeAssignStatement(fieldRef1, new CodePrimitiveExpression(10));
            new CodeAssignStatement(fieldRef1, objectCreate1);


            // Creates a statement using a code expression.
            //var expressionStatement = new CodeExpressionStatement(fieldRef1);
            setUpMethod.Statements.Add(as1);
            return mockMemberField;
        }
        
        private CodeAssignStatement AddMockObject(
            CodeMemberMethod setUpMethod,
            CodeMemberField mockRepositoryMemberField,
            string testObjectName, 
            ParameterInfo paraInfo, 
            string paraName)
        {
            var paraType = paraInfo.ParameterType;

            CodeFieldReferenceExpression mockRef =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), mockRepositoryMemberField.Name);

            CodeFieldReferenceExpression fieldRef1 =
               new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), paraName);



            // Creates a statement using a code expression.
            //var expressionStatement = new CodeExpressionStatement(fieldRef1);


            // Creates a code expression for a CodeExpressionStatement to contain.
            var invokeExpression = new CodeMethodInvokeExpression(
                mockRef,
                "StrictMock"
                //new CodePrimitiveExpression("expected")
                );
            invokeExpression.Method.TypeArguments.Add(paraType.FullName);
            // Creates a statement using a code expression.
            //var expressionStatement = new CodeExpressionStatement(invokeExpression);

            CodeObjectCreateExpression objectCreate1 =
                new CodeObjectCreateExpression(testObjectName, new CodeExpression[] { });
            
            CodeAssignStatement as1 =
                //new CodeAssignStatement(fieldRef1, new CodePrimitiveExpression(10));
            new CodeAssignStatement(fieldRef1, invokeExpression);

            // A C# code generator produces the following source code for the preceeding example code:

            // Console.Write( "Example string" );
            setUpMethod.Statements.Add(as1);


            //var testObject = TestMemberFieldLookup["testObject"];
            //setUpMethod.Statements.Add(as1);
            return as1;
        }

        /// <summary>
        /// Handle event related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="eventName">Name of the event.</param>
        protected override void ComputeCodeMemberEvent(CodeMemberMethod typeMember, string eventName)
        {
            if (typeMember.Name.Contains("add_"))
            {
                typeMember.Name = typeMember.Name.Replace("add_", "Event");
            }
            else if (typeMember.Name.Contains("remove_"))
            {
                typeMember.Name = typeMember.Name.Replace("remove_", "Event");
            }
            ReplaceTestInTestName(typeMember, "AddAndRemove");
        }

        #endregion Methods (Public)

        #region Helper Methods (Private)

        /// <summary>
        /// Add namespace imports to the main compilation unit.
        /// </summary>
        /// <returns>
        /// A list of code name spaces, to be added to the compilation unit.
        /// </returns>
        protected override IEnumerable<CodeNamespaceImport> RetrieveNamespaceImports()
        {
            return new[] 
            { 
                new CodeNamespaceImport("System"),
                new CodeNamespaceImport("System.Collections.Generic"),
                new CodeNamespaceImport("System.Linq"),
                new CodeNamespaceImport(typeof(TestAttribute).Namespace),
            };
        }

        #endregion Helper Methods (Private)
    }
}
