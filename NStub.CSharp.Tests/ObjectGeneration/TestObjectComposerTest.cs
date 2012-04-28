namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using NStub.Core;
    using NStub.CSharp.Tests.Stubs;
    using System.CodeDom;


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
            CreateTestObject(typeof(ComposeMeCtorVoid));
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
            this.testObjectName = "myTestObjectName";
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
            // Test read access of 'HasParameterAssignments' Property.
            var expected = false;
            var actual = testObject.HasParameterAssignments;
            Assert.AreEqual(expected, actual);

            testObject.BuildTestObject(MemberVisibility.Public);
            expected = true;
            actual = testObject.HasParameterAssignments;
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PropertyIsCompletelyAssignedNormalBehavior()
        {
            // Test read access of 'IsCompletelyAssigned' Property.
            var expected = true;
            var actual = testObject.IsCompletelyAssigned;
            Assert.AreEqual(expected, actual);
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
        public void AssignmentListsAreSameAtTheBase()
        {
            CreateTestObject(typeof(ComposeMeTwoCtor));
            testObject.BuildTestObject(MemberVisibility.Public);
            Assert.AreSame(testObject.Assignments, testObject.CtorAssignments);
            Assert.IsInstanceOfType<IEnumerable<AssignmentInfoCollection>>(testObject.Assignments);
            Assert.IsInstanceOfType<ConstructorAssignmentCollection>(testObject.CtorAssignments);
        }

        [Test()]
        public void PropertyAssignmentsNormalBehavior()
        {
            // Test read access of 'Assignments' Property.
            var actual = testObject.Assignments;
            Assert.IsNull(actual);

            testObject.BuildTestObject(MemberVisibility.Public);
            actual = testObject.Assignments;
            Assert.IsNotEmpty(actual);
        }

        [Test()]
        public void PropertyBuildDataNormalBehavior()
        {
            // Test read access of 'BuildData' Property.
            var expected = this.buildData;
            var actual = testObject.BuildData;
            Assert.AreSame(expected, actual);
        }

        [Test()]
        public void PropertyTestObjectMemberFieldCreateExpressionNormalBehavior()
        {
            // Test read access of 'TestObjectMemberFieldCreateExpression' Property.
            var actual = testObject.TestObjectMemberFieldCreateExpression;
            Assert.IsNull(actual);

            testObject.BuildTestObject(MemberVisibility.Public);
            actual = testObject.TestObjectMemberFieldCreateExpression;
            Assert.IsNotNull(actual);
        }

        [Test()]
        public void PropertyTestObjectMemberFieldNormalBehavior()
        {
            // Test read access of 'TestObjectMemberField' Property.
            var expected = this.testObjectMemberField;
            var actual = testObject.TestObjectMemberField;
            Assert.AreSame(expected, actual);
        }

        [Test()]
        public void PropertyTestObjectNameNormalBehavior()
        {
            // Test read access of 'TestObjectName' Property.
            var expected = this.testObjectName;
            var actual = testObject.TestObjectName;
            Assert.AreSame(expected, actual);
        }

        [Test()]
        public void PropertyTestObjectTypeNormalBehavior()
        {
            // Test read access of 'TestObjectType' Property.
            var expected = this.testObjectType;
            var actual = testObject.TestObjectType;
            Assert.AreSame(expected, actual);
        }

        [Test()]
        public void PropertySetUpMethodNormalBehavior()
        {
            // Test read access of 'SetUpMethod' Property.
            var expected = this.setUpMethod;
            var actual = testObject.SetUpMethod;
            Assert.AreSame(expected, actual);
        }

        [Test()]
        public void TryFindConstructorAssignmentTest()
        {
            CreateTestObject(typeof(ComposeMeCtorString));
            testObject.BuildTestObject(MemberVisibility.Public);

            ConstructorAssignment ctorAssignment;
            var found = testObject.TryFindConstructorAssignment("para2", out ctorAssignment, false);
            Assert.IsTrue(found);
            Assert.IsNotNull(ctorAssignment);

            found = testObject.TryFindConstructorAssignment("para1", out ctorAssignment, false);
            Assert.IsTrue(found);
            Assert.IsNotNull(ctorAssignment);

            found = testObject.TryFindConstructorAssignment("pArA2", out ctorAssignment, false);
            Assert.IsTrue(found);
            Assert.IsNotNull(ctorAssignment);

            found = testObject.TryFindConstructorAssignment("pArA2", out ctorAssignment, true);
            Assert.IsFalse(found);
            Assert.IsNull(ctorAssignment);

            found = testObject.TryFindConstructorAssignment("NoOneLoveMe", out ctorAssignment, false);
            Assert.IsFalse(found);
            Assert.IsNull(ctorAssignment);

            Assert.Throws<ArgumentNullException>(() => testObject.TryFindConstructorAssignment(null, out ctorAssignment, false));
            Assert.Throws<ArgumentException>(() => testObject.TryFindConstructorAssignment(string.Empty, out ctorAssignment, false));
        }

        [Test()]
        public void EqualsTest()
        {
            var second = new TestObjectComposer(
                this.buildData, 
                this.setUpMethod, 
                this.testObjectMemberField,
                this.testObjectName,
                this.testObjectType);

            Assert.IsTrue(testObject.Equals(second));
            Assert.IsTrue(second.GetHashCode() == testObject.GetHashCode());

            second = new TestObjectComposer(
                this.buildData,
                this.setUpMethod,
                this.testObjectMemberField,
                this.testObjectName + "XYZ",
                this.testObjectType);

            Assert.IsFalse(testObject.Equals(second));
            testObject.BuildTestObject(MemberVisibility.Public);
            Assert.IsFalse(second.GetHashCode() == testObject.GetHashCode());
        }
    }
}
