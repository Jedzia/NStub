namespace NStub.CSharp.Tests.ObjectGeneration
{
    using global::MbUnit.Framework;
    using System.CodeDom;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;

    public partial class CodeFieldReferenceBinderTest
    {
        
        private CodeFieldReferenceBinder testObject;
        
        [SetUp()]
        public void SetUp()
        {
            var ctdecl = new CodeTypeDeclaration("MyClass");
            var cm = new CodeMemberMethod();

            this.testObject = cm.Assign("myField");
        }
        
        [TearDown()]
        public void TearDown()
        {
            // ToDo: Implement TearDown logic here 
            this.testObject = null;
        }
        
        [Test()]
        public void PropertyFieldAssignmentNormalBehavior()
        {
            // TODO: Implement unit test for PropertyFieldAssignment

            // Test read access of 'FieldAssignment' Property.
            var expected = "Insert expected object here";
            var actual = testObject.FieldAssignment;
            //Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
            //cm.Assign("myField").AndCreateIn<IAsyncResult>(ctdecl).With(123);
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
            // TODO: Implement unit test for With

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
