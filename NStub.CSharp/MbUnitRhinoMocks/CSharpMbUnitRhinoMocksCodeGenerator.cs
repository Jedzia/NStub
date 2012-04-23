// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpMbUnitRhinoMocksCodeGenerator.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.MbUnitRhinoMocks
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NStub.Core;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.MbUnit;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="CSharpMbUnitRhinoMocksCodeGenerator"/> is responsible for the generation of the individual
    /// class files with <see cref="MbUnit"/> and Rhino.Mocks support which will make up the actual test project.  For information
    /// regarding the generation of the project file, see 
    /// <see cref="CSharpProjectGenerator"></see> and <see cref="MockRepository"/>.
    /// </summary>
    public class CSharpMbUnitRhinoMocksCodeGenerator : CSharpMbUnitCodeGenerator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpMbUnitRhinoMocksCodeGenerator"/> class
        /// based the given <see cref="CodeNamespace"/> which will output to the given directory.
        /// </summary>
        /// <param name="buildSystem">The build system.</param>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="testBuilders">The test builder repository.</param>
        /// <param name="configuration">The configuration of the generator.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="codeNamespace"/> or
        /// <cref name="ICodeGeneratorParameters.OutputDirectory"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException"><cref name="ICodeGeneratorParameters.OutputDirectory"/> is an
        /// empty string.</exception>
        /// <exception cref="ApplicationException"><cref name="ICodeGeneratorParameters.OutputDirectory"/>
        /// cannot be found.</exception>
        public CSharpMbUnitRhinoMocksCodeGenerator(
            IBuildSystem buildSystem,
            CodeNamespace codeNamespace,
            IMemberBuilderFactory testBuilders,
            ICodeGeneratorParameters configuration)
            : base(buildSystem, codeNamespace, testBuilders, configuration)
        {
        }

        #endregion

        /*/// <summary>
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
            //var invokeExpression = new CodeMethodInvokeExpression(
              //  new CodeTypeReferenceExpression("Assert"),
              //  "AreEqual",
                //new CodePrimitiveExpression("expected")
             //   new CodeFieldReferenceExpression(testObjectMemberField, "bla")
             //   , new CodeVariableReferenceExpression("actual"));
            var fieldRef1 =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), testObjectMemberField.Name);

            var testObjectMemberFieldCreate = new CodeObjectCreateExpression(testObjectName, new CodeExpression[] { });
            var as1 = new CodeAssignStatement(fieldRef1, testObjectMemberFieldCreate);

            // Creates a statement using a code expression.
            // var expressionStatement = new CodeExpressionStatement(fieldRef1);
            setUpMethod.Statements.Add(as1);
            return testObjectMemberFieldCreate;
        }*/

        /// <summary>
        /// Compose additional items of the test setup method.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.( early testObject ).</param>
        /// <param name="setUpMethod">The test setup method.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <returns>
        /// The list of assigned mock objects.
        /// </returns>
        protected virtual IEnumerable<CodeAssignStatement> ComposeTestSetupMockery(
            CodeTypeDeclaration testClassDeclaration,
            CodeMemberMethod setUpMethod,
            CodeMemberField testObjectMemberField,
            string testObjectName)
        {
            // Todo: only the Type is necs, the CodeTypeDeclaration is to much knowledge.
            var testObjectClassType = (Type)testClassDeclaration.UserData[NStubConstants.UserDataClassTypeKey];

            Type[] parameters = { /*typeof(int)*/ };

            // Get the constructor that takes an integer as a parameter.
            var ctor = testObjectClassType.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                Type.DefaultBinder,
                parameters,
                null);

            if (ctor == null)
            {
                // outputBlock.Text += 
                // "There is no public constructor of MyClass that takes an integer as a parameter.\n";
            }

            // else
            // {
            // outputBlock.Text += 
            // "The public constructor of MyClass that takes an integer as a parameter is:\n"; 
            // outputBlock.Text += ctor.ToString() + "\n";
            // }
            var testObjectConstructors = testObjectClassType.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public);
            bool hasInterfaceInCtorParameters = false;
            var ctorParameterTypes = new List<ParameterInfo>();

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
            }

            if (!hasInterfaceInCtorParameters)
            {
                return new CodeAssignStatement[0];
            }

            var testObjectInitializerPosition = setUpMethod.Statements.Count - 1;

            var mockRepositoryMemberField = this.AddMockRepository(
                testClassDeclaration, setUpMethod, testObjectName, null, "mocks");

            var mockAssignments = new List<CodeAssignStatement>();
            foreach (var paraInfo in ctorParameterTypes)
            {
                var mockMemberField = AddTestMemberField(
                    testClassDeclaration, paraInfo.ParameterType.FullName, paraInfo.Name);
                var mockAssignment = this.AddMockObject(
                    setUpMethod, mockRepositoryMemberField, testObjectName, paraInfo, paraInfo.Name);
                mockAssignments.Add(mockAssignment);
            }

            // reorder the testObject initializer to the bottom of the SetUp method.
            var removedTypedec = setUpMethod.Statements[testObjectInitializerPosition];
            setUpMethod.Statements.RemoveAt(testObjectInitializerPosition);
            setUpMethod.Statements.Add(removedTypedec);

            return mockAssignments;
        }

        /// <summary>
        /// Generates the setup and tear down additional members and fields.
        /// </summary>
        /// <param name="context">Contains data specific to SetUp and TearDown test-method generation.</param>
        /// <param name="testObjectName">Name of the test object.</param>
        /// <param name="testObjectMemberField">The test object member field.</param>
        protected override void GenerateSetupAndTearDownAdditional(
            ISetupAndTearDownCreationContext context, string testObjectName, CodeMemberField testObjectMemberField)
        {
            var assignedMockObjects = this.ComposeTestSetupMockery(
                context.TestClassDeclaration, context.SetUpMethod, testObjectMemberField, testObjectName);
            if (assignedMockObjects.Count() > 0)
            {
                foreach (var mockObject in assignedMockObjects)
                {
                    // Todo: maybe use the creator here to add all the stuff
                    context.TestObjectCreator.TestObjectMemberFieldCreateExpression.Parameters.Add(mockObject.Left);
                }

                string rhinoImport = typeof(MockRepository).Namespace;
                context.CodeNamespace.Imports.Add(new CodeNamespaceImport(rhinoImport));
            }
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

        /// <summary>
        /// Adds the mock repository.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="setUpMethod">The test SetUp method.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <param name="paraInfo">The parameter info of the parameter to mock.</param>
        /// <param name="paraName">Name of the mock parameter.</param>
        /// <returns>The declaration of the created mock member field of the test class.</returns>
        private CodeMemberField AddMockRepository(
            CodeTypeDeclaration testClassDeclaration,
            CodeMemberMethod setUpMethod,
            string testObjectName,
            ParameterInfo paraInfo,
            string paraName)
        {
            var mockMemberField = AddTestMemberField(testClassDeclaration, typeof(MockRepository).Name, "mocks");

            var fieldRef1 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), paraName);

            var objectCreate1 = new CodeObjectCreateExpression(typeof(MockRepository).Name, new CodeExpression[] { });
            var as1 = new CodeAssignStatement(fieldRef1, objectCreate1);

            // Creates a statement using a code expression.
            // var expressionStatement = new CodeExpressionStatement(fieldRef1);
            setUpMethod.Statements.Add(as1);
            return mockMemberField;
        }

        /*private void GenerateSetupAndTearDown(
            CodeNamespace codeNamespace,
            CodeTypeDeclaration codeTypeDeclaration,
            string testObjectName,
            CodeMemberField testObjectMemberField)
        {
            var setUpMethod = CreateCustomCodeMemberMethodWithSameNameAsAttribute("SetUp");
            var testObjectMemberFieldCreate = this.ComposeTestSetupMethod(
                setUpMethod, testObjectMemberField, testObjectName);
            
            GenerateSetupAndTearDownAdditional(codeNamespace, codeTypeDeclaration, testObjectName, testObjectMemberField, setUpMethod, testObjectMemberFieldCreate);

            codeTypeDeclaration.Members.Add(setUpMethod);
            var tearDownMethod = CreateCustomCodeMemberMethodWithSameNameAsAttribute("TearDown");
            this.ComposeTestTearDownMethod(tearDownMethod, testObjectMemberField, testObjectName);
            codeTypeDeclaration.Members.Add(tearDownMethod);
        }*/
    }
}