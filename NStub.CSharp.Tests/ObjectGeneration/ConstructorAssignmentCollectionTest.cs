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
            this.preferredConstructor = new NStub.CSharp.ObjectGeneration.AssignmentInfoCollection();
            this.testObject = new ConstructorAssignmentCollection(this.preferredConstructor);

            Assert.Throws<ArgumentNullException>(() => new ConstructorAssignmentCollection(null));
        }
        
        [Test()]
        public void PropertyCountNormalBehavior()
        {
            // Test read access of 'Count' Property.
            var expected = 1;
            var actual = testObject.Count;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyPreferredConstructorNormalBehavior()
        {
            // Test read access of 'PreferredConstructor' Property.
            var expected = this.preferredConstructor;
            var actual = testObject.PreferredConstructor;
            Assert.AreSame(expected, actual);
        }
        
        [Test()]
        public void AddConstructorAssignmentTest()
        {
            var expected = 1;
            var actual = testObject.Count;
            Assert.AreEqual(expected, actual);

            var asgn = new NStub.CSharp.ObjectGeneration.AssignmentInfoCollection();
            testObject.AddConstructorAssignment(asgn);

            expected = 2;
            actual = testObject.Count;
            Assert.AreEqual(expected, actual);

            // add the same again.
            testObject.AddConstructorAssignment(asgn);
            expected = 3;
            actual = testObject.Count;
            Assert.AreEqual(expected, actual);

            // add a range of assignments.
            testObject.AddConstructorAssignment(new[] { asgn, asgn, asgn, asgn });
            expected = 7;
            actual = testObject.Count;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void GetEnumeratorTest()
        {
            Assert.IsNotNull(testObject.GetEnumerator());
            Assert.IsNotNull(((System.Collections.IEnumerable)testObject).GetEnumerator());
            Assert.IsInstanceOfType<IEnumerator<AssignmentInfoCollection>>(testObject.GetEnumerator());
        }
    }
}
