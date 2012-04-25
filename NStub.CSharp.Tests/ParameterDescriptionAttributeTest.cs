namespace NStub.CSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp;
    using NStub.CSharp.Tests.Stubs;
    
    
    [TestFixture()]
    public partial class ParameterDescriptionAttributeTest
    {
        
        private string description;
        private ParameterDescriptionAttribute testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.description = "Value of description";
            this.testObject = new ParameterDescriptionAttribute(this.description);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersDescriptionTest()
        {
            this.description = "Value of description";
            this.testObject = new ParameterDescriptionAttribute(this.description);
            this.testObject = new ParameterDescriptionAttribute(null);
            this.testObject = new ParameterDescriptionAttribute(string.Empty);
        }
        
        [Test()]
        public void PropertyDescriptionNormalBehavior()
        {
            // Test read access of 'Description' Property.
            var expected = description;
            var actual = testObject.Description;
            Assert.AreEqual(expected, actual);

            // Test write access of 'Description' Property.
            expected = "A new value";
            testObject.Description = expected;
            actual = testObject.Description;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyTypeIdNormalBehavior()
        {
            // Test read access of 'TypeId' Property.
            var expected = testObject.GetType();
            var actual = testObject.TypeId;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void IsDefaultAttributeTest()
        {
            Assert.IsFalse(testObject.IsDefaultAttribute());
        }
        
        [Test()]
        public void MatchTest()
        {
            Assert.IsFalse(testObject.Match(this));
            Assert.IsTrue(testObject.Match(testObject));
            //Assert.IsTrue(testObject.Match(new InfoApe()));
        }
    }
}
