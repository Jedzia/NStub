namespace NStub.CSharp.ObjectGeneration.Tests.ObjectGeneration.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration.Builders;
    using Rhino.Mocks;
    using NStub.CSharp.BuildContext;
    using System.CodeDom;
    using NStub.CSharp.Tests.Stubs;
    using Rhino.Mocks.Constraints;
    
    
    public partial class ConstructorBuilderTest
    {

        private NStub.CSharp.BuildContext.IMemberBuildContext context;
        private MockRepository mocks;
        private ConstructorBuilder testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.mocks = new MockRepository();
            this.context = this.mocks.StrictMock<NStub.CSharp.BuildContext.IMemberBuildContext>();
            this.testObject = new ConstructorBuilder(this.context);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
            this.context = null;
            this.mocks = null;
        }
        
        [Test()]
        public void ConstructWithParametersContextTest()
        {
            this.testObject = new ConstructorBuilder(this.context);
            Assert.Throws<ArgumentNullException>(() => new ConstructorBuilder(null));
        }
        
        [Test()]
        public void CanHandleContextTest()
        {
            Expect.Call(this.context.IsConstructor).Return(true);
            Expect.Call(this.context.IsConstructor).Return(false);
            mocks.ReplayAll();

            var expected = true;
            var actual = ConstructorBuilder.CanHandleContext(this.context);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = ConstructorBuilder.CanHandleContext(this.context);
            Assert.AreEqual(expected, actual);
            
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertySetupContextNormalBehavior()
        {
            // Test read access of 'SetupContext' Property.
            var expected = this.context;
            var actual = testObject.SetupContext;
            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOfType<IMemberBuildContext>(actual);
            Assert.IsInstanceOfType<IMemberSetupContext>(actual);
        }
        
        [Test()]
        public void BuildWithContextSetUpTearDownContextIsNullShouldThrow()
        {
            Expect.Call(this.context.SetUpTearDownContext).Return(null);
            mocks.ReplayAll();
            Assert.Throws<ArgumentNullException>(() => testObject.Build(this.context));
            mocks.VerifyAll();
        }

        [Test()]
        public void BuildWithContextSetUpTearDownContextIsOnlyISetupAndTearDownContextShouldThrow()
        {
            var stcontext = this.mocks.StrictMock<NStub.CSharp.BuildContext.ISetupAndTearDownContext>();
            Expect.Call(this.context.SetUpTearDownContext).Return(stcontext);
            mocks.ReplayAll();
            Assert.Throws<ArgumentNullException>(() => testObject.Build(this.context));
            mocks.VerifyAll();
        }

        [Test()]
        public void BuildWithEmptyAssignments()
        {
            var assignments = new[] { new AssignmentInfoCollection() };

            var tobuilder = this.mocks.StrictMock<ITestObjectBuilder>();
            Expect.Call(tobuilder.Assignments).Return(assignments);

            var stcontext = this.mocks.StrictMock<ISetupAndTearDownCreationContext>();
            Expect.Call(stcontext.TestObjectCreator).Return(tobuilder);
            
            Expect.Call(this.context.SetUpTearDownContext).Return(stcontext);
            mocks.ReplayAll();
            var expected = true;
            var actual = testObject.Build(this.context);
            Assert.AreEqual(expected, actual);

            mocks.VerifyAll();
        }

        [Test()]
        public void BuildWithAssignmentsSkipsAssignmentsWithOneAssignment()
        {
            var assignments = new[] { new AssignmentInfoCollection()};
            var memberField = new CodeMemberField(typeof(int), "myField");
            var assignStatement = new CodeAssignStatement();
            var ctorAssignment = new ConstructorAssignment("theParameter", assignStatement, memberField);
            assignments[0].AddAssignment(ctorAssignment);
            //assignments[0].UsedConstructor = 

            var tobuilder = this.mocks.StrictMock<ITestObjectBuilder>();
            Expect.Call(tobuilder.Assignments).Return(assignments);

            var stcontext = this.mocks.StrictMock<ISetupAndTearDownCreationContext>();
            Expect.Call(stcontext.TestObjectCreator).Return(tobuilder);

            Expect.Call(this.context.SetUpTearDownContext).Return(stcontext);//.Repeat.Any();
            mocks.ReplayAll();
            var expected = true;
            var actual = testObject.Build(this.context);
            Assert.AreEqual(expected, actual);

            mocks.VerifyAll();
        }

        [Test()]
        public void BuildWithAssignmentUsedConstructorIsNullShouldThrow()
        {
            var assignments = new[] { new AssignmentInfoCollection(), new AssignmentInfoCollection() };
            var tobuilder = this.mocks.StrictMock<ITestObjectBuilder>();
            Expect.Call(tobuilder.Assignments).Return(assignments);

            var stcontext = this.mocks.StrictMock<ISetupAndTearDownCreationContext>();
            Expect.Call(stcontext.TestObjectCreator).Return(tobuilder);

            Expect.Call(this.context.SetUpTearDownContext).Return(stcontext);
            mocks.ReplayAll();
            Assert.Throws<InvalidOperationException>(() => testObject.Build(this.context));
            mocks.VerifyAll();
        }

        [Test()]
        public void BuildWithAssignmentsNoMatchingParameters()
        {
            var method = new CodeMemberMethod();
            var assCol1 = new AssignmentInfoCollection();
            var assCol2 = new AssignmentInfoCollection();
            var assignments = new[] { assCol1, assCol2 };
            var memberField = new CodeMemberField(typeof(int), "myField");
            var assignStatement = new CodeAssignStatement();
            var ctorAssignment = new ConstructorAssignment("theParameter", assignStatement, memberField);
            assignments[0].AddAssignment(ctorAssignment);
            var helpctors = typeof(ConstructorBuilderHelpType).GetConstructors();
            assignments[0].UsedConstructor = helpctors[0];
            assignments[1].UsedConstructor = helpctors[0];

            var tobuilder = this.mocks.StrictMock<ITestObjectBuilder>();
            Expect.Call(tobuilder.Assignments).Return(assignments);

            var stcontext = this.mocks.StrictMock<ISetupAndTearDownCreationContext>();
            Expect.Call(stcontext.TestObjectCreator).Return(tobuilder);

            Expect.Call(this.context.SetUpTearDownContext).Return(stcontext);//.Repeat.Any();
            Expect.Call(this.context.TestObjectType).Return(typeof(int)).Repeat.Any();
            var ctdecl = new CodeTypeDeclaration("TheTestClass");
            Expect.Call(this.context.TestClassDeclaration).Return(ctdecl).Repeat.Any();

            Expect.Call(delegate { tobuilder.AssignExtra(ctdecl, method, null, assCol1); })
                //.Constraints(Is.Equal(ctdecl), Is.Equal(method), Is.Anything(), Is.Equal(assCol1)
                .Constraints(Is.Equal(ctdecl), Is.Anything(), Is.Anything(), Is.Equal(assCol1))
                .Do((Action<CodeTypeDeclaration,CodeMemberMethod,CodeObjectCreateExpression ,AssignmentInfoCollection>)
                delegate(CodeTypeDeclaration testClassDeclaration, 
            CodeMemberMethod testMethod, 
            CodeObjectCreateExpression testObjectConstructor, 
            AssignmentInfoCollection ctorAssignments)
            {
                var a = testClassDeclaration;
                var b = testMethod;
                Assert.IsInstanceOfType<CustomConstructorCodeMemberMethod>(testMethod);
                Assert.AreEqual("ConstructWithParametersTheIntParameterTest", testMethod.Name);
                var c = testObjectConstructor;
                Assert.IsEmpty(testObjectConstructor.Parameters);
                Assert.AreEqual("Int32", testObjectConstructor.CreateType.BaseType);
                var d = ctorAssignment;
                
                var e = ctorAssignment;
            });
            Expect.Call(delegate { tobuilder.AssignExtra(ctdecl, method, null, assCol1); })
                //.Constraints(Is.Equal(ctdecl), Is.Equal(method), Is.Anything(), Is.Equal(assCol1)
                .Constraints(Is.Equal(ctdecl), Is.Anything(), Is.Anything(), Is.Equal(assCol2)
                );
            mocks.ReplayAll();
            var expected = true;
            var actual = testObject.Build(this.context);
            Assert.AreEqual(expected, actual);

            mocks.VerifyAll();
        }


        [Test()]
        public void GetTestNameTest()
        {
            var expected = "TheOrigName";
            var actual = testObject.GetTestName(this.context, "TheOrigName");
            Assert.AreEqual(expected, actual);
        }
     }
}
