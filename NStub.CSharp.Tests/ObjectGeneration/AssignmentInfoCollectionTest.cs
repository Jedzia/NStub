namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using global::MbUnit.Framework;
    using System.CodeDom;
    using NStub.CSharp.ObjectGeneration;

    public partial class AssignmentInfoCollectionTest
    {
        
        private AssignmentInfoCollection testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.testObject = new AssignmentInfoCollection();
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructTest()
        {
            this.testObject = new AssignmentInfoCollection();
            Assert.AreEqual(0, this.testObject.Count);
            Assert.IsEmpty(this.testObject);
        }
        
        [Test()]
        public void PropertyCountNormalBehavior()
        {
            // Test read access of 'Count' Property.
            var expected = 0;
            var actual = testObject.Count;
            Assert.AreEqual(expected, actual);

            testObject.AddAssignment(new ConstructorAssignment("Para", new CodeAssignStatement(), new CodeMemberField(), typeof(string)));

            expected = 1;
            actual = testObject.Count;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyItemIndexNormalBehavior()
        {
            // Test read access of 'Indexer' Property.
            var expected = new ConstructorAssignment("ParameterName", new CodeAssignStatement(), new CodeMemberField(), typeof(string));
            testObject.AddAssignment(expected);
            var actual = testObject["ParameterName"];
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PropertyItemIndexWithNotPresentKey()
        {
            // Test read access of 'Indexer' Property with a key that is not present.
            var actual = testObject["NotPresent"];
            Assert.IsNull(actual);

            var item = new ConstructorAssignment("ParameterName", new CodeAssignStatement(), new CodeMemberField(), typeof(string));
            testObject.AddAssignment(item);
            actual = testObject["AnotherNotPresent"];
            Assert.IsNull(actual);
        }

        [Test()]
        public void PropertyUsedConstructorNormalBehavior()
        {
            // Test read access of 'UsedConstructor' Property.
            var actual = testObject.UsedConstructor;
            Assert.IsNull(actual);
        }
        
        [Test()]
        public void AddAssignmentTest()
        {
            var expected = new ConstructorAssignment("ParameterName", new CodeAssignStatement(), new CodeMemberField(), typeof(string));
            testObject.AddAssignment(expected);
            Assert.AreEqual(1, testObject.Count);
            var actual = testObject["ParameterName"];
            Assert.AreEqual(expected, actual);

            var expected2 = new ConstructorAssignment("OtherParameterName", new CodeAssignStatement(), new CodeMemberField(), typeof(string));
            testObject.AddAssignment(expected2);
            actual = testObject["OtherParameterName"];
            Assert.AreEqual(2, testObject.Count);
            Assert.AreEqual(expected2, actual);
            actual = testObject["ParameterName"];
            Assert.AreEqual(expected, actual);

        }

        [Test()]
        public void AddAssignmentWithSameKeyShouldThrow()
        {
            var item = new ConstructorAssignment("ParameterName", new CodeAssignStatement(), new CodeMemberField(), typeof(string));
            testObject.AddAssignment(item);
            Assert.AreEqual(1, testObject.Count);

            Assert.Throws<ArgumentException>(() => testObject.AddAssignment(item));
            Assert.AreEqual(1, testObject.Count);

            var item2 = new ConstructorAssignment("ParameterName", new CodeAssignStatement(), new CodeMemberField(), typeof(string));
            Assert.Throws<ArgumentException>(() => testObject.AddAssignment(item2));
            Assert.AreEqual(1, testObject.Count);
        }

        [Test()]
        public void EmptyTest()
        {
            // unit test for Empty list.
            testObject = AssignmentInfoCollection.Empty();
            Assert.AreEqual(0, testObject.Count);
            Assert.IsEmpty(testObject);
        }
        
        [Test()]
        public void GetEnumeratorTest()
        {
            Assert.IsNotNull(testObject.GetEnumerator());
        }
    }
}
