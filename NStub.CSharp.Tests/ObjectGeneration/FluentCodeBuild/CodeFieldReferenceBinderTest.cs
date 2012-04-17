namespace NStub.CSharp.Tests.ObjectGeneration
{
    using global::MbUnit.Framework;
    using System.CodeDom;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;
    using System;

    public partial class CodeFieldReferenceBinderTest
    {

        private CodeFieldReferenceBinder testObject;
        private CodeTypeDeclaration declaration;
        private CodeMemberMethod method;
        
        [SetUp()]
        public void SetUp()
        {
            this.declaration = new CodeTypeDeclaration("MyClass");
            this.method = new CodeMemberMethod();
            this.testObject = this.method.Assign("myField");
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.declaration = null;
            this.testObject = null;
            this.method = null;
        }
        
        [Test()]
        public void PropertyFieldAssignmentNormalBehavior()
        {
            // Test read access of 'FieldAssignment' Property.
            var actual = testObject.FieldAssignment;
            Assert.IsNull(actual);

            testObject.With(123);
            actual = testObject.FieldAssignment;
            Assert.IsNotNull(actual);
        }
        
        [Test()]
        public void AndCreateInTest()
        {
            // TODO: Implement unit test for AndCreateIn

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void CommitTest()
        {
            // TODO: Implement unit test for Commit

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void WithTest()
        {
            var actual = testObject.With("assignment text");
            var fieldAssignment = testObject.FieldAssignment;
            Assert.AreSame(testObject, actual);
            Assert.IsNotNull(fieldAssignment);

        }
    }
}
