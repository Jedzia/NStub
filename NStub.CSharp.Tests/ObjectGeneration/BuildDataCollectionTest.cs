namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks;

    public partial class BuildDataCollectionTest
    {
        
        private BuildDataCollection testObject;
        
        public BuildDataCollectionTest()
        {
        }
        
        [SetUp()]
        public void SetUp()
        {
            this.testObject = new BuildDataCollection();
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructTest()
        {
            this.testObject = new BuildDataCollection();
            // unit test for Empty list. Always has the 'General' category.
            Assert.AreEqual(1, testObject.Count);
            Assert.IsNotEmpty(testObject);
            var lookup = testObject["General"];
            Assert.IsNotNull(lookup);
        }
        
        [Test()]
        public void PropertyCountNormalBehavior()
        {
            // Test read access of 'Count' Property. Always has one, the 'General', category.
            var expected = 1;
            var actual = testObject.Count;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void AddDataItemTest()
        {
            // add to general category.
            var expected1 = MockRepository.GenerateStrictMock<IBuilderData>();
            testObject.AddDataItem("TheKey", expected1);
            Assert.AreEqual(1, testObject.Count);
            var actual1 = testObject["General"]["TheKey"];
            Assert.AreEqual(expected1, actual1);

            // add to a explicitely specified category.
            var expected2 = MockRepository.GenerateStrictMock<IBuilderData>();
            testObject.AddDataItem("OtherCategory", "TheKey", expected2);
            Assert.AreEqual(2, testObject.Count);
            var actual2 = testObject["OtherCategory"]["TheKey"];
            Assert.AreEqual(expected2, actual2);
            Assert.AreNotSame(actual1, actual2);

            // add to general category, but with different key. count should not differ, as it counts the categories.
            var expected3 = MockRepository.GenerateStrictMock<IBuilderData>();
            testObject.AddDataItem("TheOtherKey", expected3);
            var actual3 = testObject["General"]["TheOtherKey"];
            Assert.AreEqual(2, testObject.Count);
            Assert.AreEqual(expected3, actual3);
            Assert.AreNotSame(actual1, actual3);
            Assert.AreNotSame(actual2, actual3);

            expected1.VerifyAllExpectations();
            expected2.VerifyAllExpectations();
            expected3.VerifyAllExpectations();
        }

        [Test()]
        public void AddDataItemTheSameShouldThrow()
        {
            var expected1 = MockRepository.GenerateStrictMock<IBuilderData>();
            testObject.AddDataItem("TheKey", expected1);

            Assert.Throws<ArgumentException>(() => testObject.AddDataItem("TheKey", expected1));
            Assert.AreEqual(1, testObject.Count);
        }

        [Test()]
        public void PropertyItemIndexerNormalBehavior()
        {
            // Test read access of 'Indexer' Property.
            var expected = MockRepository.GenerateStrictMock<IBuilderData>();
            testObject.AddDataItem("TheKey", expected);
            var lookup = testObject["General"];
            Assert.IsNotNull(lookup);
            var actual = lookup["TheKey"];
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void GetEnumeratorTest()
        {
            Assert.IsNotNull(testObject.GetEnumerator());
        }

        [Test()]
        public void GeneralCategoryShouldBeEmpty()
        {
            var actual = testObject["General"];
            Assert.IsNotNull(actual);
            Assert.IsEmpty(actual);
        }

        [Test()]
        public void TryGetCategoryTest()
        {
            // category not in list.
            Dictionary<string, IBuilderData> actual;
            var found = testObject.TryGetCategory("NotPresentCategory", out actual);
            Assert.IsFalse(found);
            Assert.IsNull(actual);

            // standard 'General' category is always in list.
            found = testObject.TryGetCategory("General", out actual);
            Assert.IsTrue(found);
            Assert.IsNotNull(actual);
            Assert.IsEmpty(actual);

            // added 'OtherCategory' category.
            var expected = MockRepository.GenerateStrictMock<IBuilderData>();
            testObject.AddDataItem("OtherCategory", "TheKey", expected);
            Assert.AreEqual(2, testObject.Count);
            found = testObject.TryGetCategory("OtherCategory", out actual);
            Assert.IsTrue(found);
            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual["TheKey"]);
        }
        
        [Test()]
        public void TryGetValueFromGeneral()
        {
            IBuilderData actual;
            var found = testObject.TryGetValue("NotPresentValue", out actual);
            Assert.IsFalse(found);
            Assert.IsNull(actual);

            var expected = MockRepository.GenerateStrictMock<IBuilderData>();
            testObject.AddDataItem("TheKey", expected);
            found = testObject.TryGetValue("TheKey", out actual);
            Assert.IsTrue(found);
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void TryGetValueFromSpecifiedCategory()
        {
            IBuilderData actual;
            var found = testObject.TryGetValue("NotPresentCategory", "NotPresentValue", out actual);
            Assert.IsFalse(found);
            Assert.IsNull(actual);

            var expected = MockRepository.GenerateStrictMock<IBuilderData>();
            testObject.AddDataItem("TheKey", expected);
            found = testObject.TryGetValue("General", "TheKey", out actual);
            Assert.IsTrue(found);
            Assert.AreEqual(expected, actual);

            found = testObject.TryGetValue("General", "NotPresentValue", out actual);
            Assert.IsFalse(found);
            Assert.IsNull(actual);

            found = testObject.TryGetValue("NotPresentCategory", "TheKey", out actual);
            Assert.IsFalse(found);
            Assert.IsNull(actual);

            var expected2 = MockRepository.GenerateStrictMock<IBuilderData>();
            testObject.AddDataItem("OtherCategory", "TheKey", expected2);
            found = testObject.TryGetValue("OtherCategory", "TheKey", out actual);
            Assert.IsTrue(found);
            Assert.AreEqual(expected2, actual);
        }
    }
}
