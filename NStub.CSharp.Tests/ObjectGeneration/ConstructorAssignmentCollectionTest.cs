namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    
    
    [TestFixture()]
    public partial class ConstructorAssignmentCollectionTest
    {
        
        private NStub.CSharp.ObjectGeneration.AssignmentInfoCollection preferredConstructor;
        private ConstructorAssignmentCollection testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.preferredConstructor = new NStub.CSharp.ObjectGeneration.AssignmentInfoCollection();
            this.testObject = new ConstructorAssignmentCollection(this.preferredConstructor);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersPreferredConstructorTest()
        {
            // TODO: Implement unit test for ConstructWithParametersPreferredConstructor
            this.preferredConstructor = new NStub.CSharp.ObjectGeneration.AssignmentInfoCollection();
            this.testObject = new ConstructorAssignmentCollection(this.preferredConstructor);

            Assert.Throws<ArgumentNullException>(() => new ConstructorAssignmentCollection(null));
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyCountNormalBehavior()
        {
            // Test read access of 'Count' Property.
            var expected = 1;
            var actual = testObject.Count;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyPreferredConstructorNormalBehavior()
        {
            // TODO: Implement unit test for PropertyPreferredConstructor

            // Test read access of 'PreferredConstructor' Property.
            var expected = new NStub.CSharp.ObjectGeneration.AssignmentInfoCollection();
            var actual = testObject.PreferredConstructor;
            Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void AddConstructorAssignmentTest()
        {
            // TODO: Implement unit test for AddConstructorAssignment

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetEnumeratorTest()
        {
            // TODO: Implement unit test for GetEnumerator

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
