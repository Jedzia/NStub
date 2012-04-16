namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.Tests.FluentChecking;
    
    
    public partial class CompareResultTest
    {
        
        private string comparer;
        private string name;
        private bool result;
        private CompareResult testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.result = true;
            this.name = "Value of name";
            this.comparer = "Value of comparer";
            this.testObject = new CompareResult(this.result, this.name, this.comparer);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithNullParametersShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new CompareResult(this.result, null, this.comparer));
            Assert.Throws<ArgumentNullException>(() => new CompareResult(this.result, this.name, null));
        }
        
        [Test()]
        public void PropertyComparerNormalBehavior()
        {
            // Test read access of 'Comparer' Property.
            var expected = "Value of comparer";
            var actual = testObject.Comparer;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyNameNormalBehavior()
        {
            // Test read access of 'Name' Property.
            var expected = "Value of name";
            var actual = testObject.Name;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyResultNormalBehavior()
        {
            // Test read access of 'Result' Property.
            var expected = true;
            var actual = testObject.Result;
            Assert.AreEqual(expected, actual);
        }
    }
}
