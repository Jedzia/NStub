namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using System.CodeDom;
    using System.Reflection;
    
    
    public partial class AssignmentInfoCollectionTest
    {
        
        private AssignmentInfoCollection testObject;
        
        [SetUp()]
        public void SetUp()
        {
            // ToDo: Implement SetUp logic here 
            this.testObject = new AssignmentInfoCollection();
        }
        
        [TearDown()]
        public void TearDown()
        {
            // ToDo: Implement TearDown logic here 
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersTest()
        {
            this.testObject = new AssignmentInfoCollection();
        }
        
        [Test()]
        public void PropertyCountNormalBehavior()
        {
            // Test read access of 'Count' Property.
            var expected = 0;
            var actual = testObject.Count;
            Assert.AreEqual(expected, actual);

            testObject.AddAssignment(new ConstructorAssignment("Para", new CodeAssignStatement(), new CodeMemberField()));

            expected = 1;
            actual = testObject.Count;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyItemIndexNormalBehavior()
        {
            // Test read access of 'Indexer' Property.
            var expected = new ConstructorAssignment("ParameterName", new CodeAssignStatement(), new CodeMemberField());
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

            var item = new ConstructorAssignment("ParameterName", new CodeAssignStatement(), new CodeMemberField());
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
            var expected = new ConstructorAssignment("ParameterName", new CodeAssignStatement(), new CodeMemberField());
            testObject.AddAssignment(expected);
            Assert.AreEqual(1, testObject.Count);
            var actual = testObject["ParameterName"];
            Assert.AreEqual(expected, actual);

            var expected2 = new ConstructorAssignment("OtherParameterName", new CodeAssignStatement(), new CodeMemberField());
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
            var item = new ConstructorAssignment("ParameterName", new CodeAssignStatement(), new CodeMemberField());
            testObject.AddAssignment(item);
            Assert.AreEqual(1, testObject.Count);

            Assert.Throws<ArgumentException>(() => testObject.AddAssignment(item));
            Assert.AreEqual(1, testObject.Count);

            var item2 = new ConstructorAssignment("ParameterName", new CodeAssignStatement(), new CodeMemberField());
            Assert.Throws<ArgumentException>(() => testObject.AddAssignment(item2));
            Assert.AreEqual(1, testObject.Count);
        }

        [Test()]
        public void EmptyTest()
        {
            // TODO: Implement unit test for Empty

            testObject.
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetEnumeratorTest()
        {
            // TODO: Implement unit test for GetEnumerator

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void EqualsTest()
        {
            // TODO: Implement unit test for Equals

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetHashCodeTest()
        {
            // TODO: Implement unit test for GetHashCode

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetTypeTest()
        {
            // TODO: Implement unit test for GetType

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void ToStringTest()
        {
            // TODO: Implement unit test for ToString

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
