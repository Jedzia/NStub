namespace NStub.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core;
    using System.Collections;
    
    
    [TestFixture()]
    public partial class ReadOnlyDictionaryTest
    {

        private Dictionary<string, int> baseObject;
        private ReadOnlyDictionary<string, int> testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.baseObject = new Dictionary<string, int>();
            this.baseObject.Add("Key", 42);
            this.testObject = new ReadOnlyDictionary<string, int>(baseObject);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
            this.baseObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersDictionaryTest()
        {
            this.testObject = new ReadOnlyDictionary<string, int>(baseObject);
            this.baseObject.Add("Key1", 142);
            this.baseObject.Add("Key2", 342);
            this.baseObject.Add("Key3", -42);
            this.baseObject.Add("Key4", int.MaxValue);
            Assert.AreElementsEqual(baseObject, testObject);

            Assert.Throws<ArgumentNullException>(() => new ReadOnlyDictionary<string, int>(null));
        }
        
        [Test()]
        public void PropertyCountNormalBehavior()
        {
            // Test read access of 'Count' Property.
            var expected = 1;
            var actual = testObject.Count;
            Assert.AreEqual(expected, actual);

            this.baseObject.Add("OtherKey", 666);
            this.testObject = new ReadOnlyDictionary<string, int>(baseObject);
            expected = 2;
            actual = testObject.Count;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyItemNormalBehavior()
        {
            // Test read access of 'Item' Property.
            var expected = 42;
            var actual = this.testObject["Key"];
            Assert.AreEqual(expected, actual);

            // Property or indexer 'NStub.Core.ReadOnlyDictionary<string,int>.this[string]' 
            // cannot be assigned to -- it is read only
            // this.testObject["Key"] = 99;
        }
        
        [Test()]
        public void PropertyKeysNormalBehavior()
        {
            // Test read access of 'Keys' Property.
            var expected = baseObject.Keys;
            var actual = testObject.Keys;
            Assert.AreEqual(expected, actual);

            Assert.Throws<NotSupportedException>(()=> testObject.Keys.Add("Bla"));
        }
        
        [Test()]
        public void PropertyValuesNormalBehavior()
        {
            // Test read access of 'Values' Property.
            var expected = baseObject.Values;
            var actual = testObject.Values;
            Assert.AreEqual(expected, actual);

            Assert.Throws<NotSupportedException>(() => testObject.Values.Add(123));
        }
        
        [Test()]
        public void ContainsKeyTest()
        {
            Assert.IsTrue(testObject.ContainsKey("Key"));
            Assert.IsFalse(testObject.ContainsKey("NotA_Key"));
        }
        
        [Test()]
        public void ContainsTest()
        {
            Assert.IsTrue(testObject.Contains(new KeyValuePair<string, int>("Key", 42)));
            Assert.IsFalse(testObject.Contains(new KeyValuePair<string, int>("Key", 43)));
            Assert.IsFalse(testObject.Contains(new KeyValuePair<string, int>("KeyX", 42)));
        }
        
        [Test()]
        public void CopyToTest()
        {
            var destination = new KeyValuePair<string, int>[1];
            testObject.CopyTo(destination, 0);
            Assert.AreElementsEqual(destination, testObject);
        }
        
        [Test()]
        public void GetEnumeratorTest()
        {
            Assert.AreEqual(baseObject.GetEnumerator(), testObject.GetEnumerator());
            Assert.AreEqual(baseObject.GetEnumerator(), ((IEnumerable)testObject).GetEnumerator());
        }
        
        [Test()]
        public void TryGetValueTest()
        {
            int result = 0;
            var found = testObject.TryGetValue("NotPresent", out result);
            Assert.IsFalse(found);
            Assert.AreEqual(0, result);

            found = testObject.TryGetValue("Key", out result);
            Assert.IsTrue(found);
            Assert.AreEqual(42, result);
        }
    }
}
