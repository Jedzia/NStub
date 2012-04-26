namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    
    
    [TestFixture()]
    public partial class TestObjectComposerTest
    {
        private NStub.CSharp.ObjectGeneration.BuildDataDictionary buildData;
        private System.CodeDom.CodeMemberMethod setUpMethod;
        private TestObjectComposer testObject;
        private System.CodeDom.CodeMemberField testObjectMemberField;
        private string testObjectName;
        private System.Type testObjectType;
        
        [SetUp()]
        public void SetUp()
        {
            this.buildData = new NStub.CSharp.ObjectGeneration.BuildDataDictionary();
            this.setUpMethod = new System.CodeDom.CodeMemberMethod();
            this.testObjectMemberField = new System.CodeDom.CodeMemberField();
            this.testObjectName = "Value of testObjectName";
            this.testObjectType = typeof(object);
            this.testObject = new TestObjectComposer(this.buildData, this.setUpMethod, this.testObjectMemberField, this.testObjectName, this.testObjectType);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersBuildDataSetUpMethodTestObjectMemberFieldTestObjectNameTestObjectTypeTest()
        {
            this.buildData = new NStub.CSharp.ObjectGeneration.BuildDataDictionary();
            this.setUpMethod = new System.CodeDom.CodeMemberMethod();
            this.testObjectMemberField = new System.CodeDom.CodeMemberField();
            this.testObjectName = "Value of testObjectName";
            this.testObjectType = typeof(object);
            this.testObject = new TestObjectComposer(this.buildData, this.setUpMethod, this.testObjectMemberField, this.testObjectName, this.testObjectType);

            Assert.Throws<ArgumentNullException>(() => new TestObjectComposer(null, this.setUpMethod, this.testObjectMemberField, this.testObjectName, this.testObjectType));
            Assert.Throws<ArgumentNullException>(() => new TestObjectComposer(this.buildData, null, this.testObjectMemberField, this.testObjectName, this.testObjectType));
            Assert.Throws<ArgumentNullException>(() => new TestObjectComposer(this.buildData, this.setUpMethod, null, this.testObjectName, this.testObjectType));
            Assert.Throws<ArgumentNullException>(() => new TestObjectComposer(this.buildData, this.setUpMethod, this.testObjectMemberField, null, this.testObjectType));
            Assert.Throws<ArgumentException>(() => new TestObjectComposer(this.buildData, this.setUpMethod, this.testObjectMemberField, string.Empty, this.testObjectType));
            Assert.Throws<ArgumentNullException>(() => new TestObjectComposer(this.buildData, this.setUpMethod, this.testObjectMemberField, this.testObjectName, null));
        }
        
        [Test()]
        public void PropertyHasParameterAssignmentsNormalBehavior()
        {
            // TODO: Implement unit test for PropertyHasParameterAssignments

            // Test read access of 'HasParameterAssignments' Property.
            var expected = true;
            var actual = testObject.HasParameterAssignments;
            Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyIsCompletelyAssignedNormalBehavior()
        {
            // TODO: Implement unit test for PropertyIsCompletelyAssigned

            // Test read access of 'IsCompletelyAssigned' Property.
            var expected = true;
            var actual = testObject.IsCompletelyAssigned;
            Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void AssignExtraTest()
        {
            // TODO: Implement unit test for AssignExtra

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void AssignOnlyTest()
        {
            // TODO: Implement unit test for AssignOnly

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void AssignParametersTest()
        {
            // TODO: Implement unit test for AssignParameters

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void BuildTestObjectTest()
        {
            // TODO: Implement unit test for BuildTestObject

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyAssignmentsNormalBehavior()
        {
            // TODO: Implement unit test for PropertyAssignments

            // Test read access of 'Assignments' Property.
            var expected = new ConstructorAssignmentCollection(new AssignmentInfoCollection());
            var actual = testObject.Assignments;
            Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyBuildDataNormalBehavior()
        {
            // TODO: Implement unit test for PropertyBuildData

            // Test read access of 'BuildData' Property.
            var expected = this.buildData;
            var actual = testObject.BuildData;
            Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyNormalBehaviorObjectMemberFieldCreateExpressionNormalBehavior()
        {
            // TODO: Implement unit test for PropertyTestObjectMemberFieldCreateExpression

            // Test read access of 'TestObjectMemberFieldCreateExpression' Property.
            var expected = new System.CodeDom.CodeObjectCreateExpression();
            var actual = testObject.TestObjectMemberFieldCreateExpression;
            Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyNormalBehaviorObjectMemberFieldNormalBehavior()
        {
            // TODO: Implement unit test for PropertyTestObjectMemberField

            // Test read access of 'TestObjectMemberField' Property.
            var expected = this.testObjectMemberField;
            var actual = testObject.TestObjectMemberField;
            Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyNormalBehaviorObjectNameNormalBehavior()
        {
            // TODO: Implement unit test for PropertyTestObjectName

            // Test read access of 'TestObjectName' Property.
            var expected = this.testObjectName;
            var actual = testObject.TestObjectName;
            Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyNormalBehaviorObjectTypeNormalBehavior()
        {
            // TODO: Implement unit test for PropertyTestObjectType

            // Test read access of 'TestObjectType' Property.
            var expected = this.testObjectType;
            var actual = testObject.TestObjectType;
            Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertySetUpMethodNormalBehavior()
        {
            // TODO: Implement unit test for PropertySetUpMethod

            // Test read access of 'SetUpMethod' Property.
            var expected = this.setUpMethod;
            var actual = testObject.SetUpMethod;
            Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void TryFindConstructorAssignmentTest()
        {
            // TODO: Implement unit test for TryFindConstructorAssignment

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
