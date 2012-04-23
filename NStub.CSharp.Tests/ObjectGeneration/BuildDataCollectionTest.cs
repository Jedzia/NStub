namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks;
    using NStub.CSharp.ObjectGeneration.Builders;

    [TestFixture()]
    public partial class BuildDataDictionaryTest
    {

        private EmptyMultiBuildParameters item1;
        private EmptyMultiBuildParameters item2;
        private EmptyMultiBuildParameters item3;
        private BuildDataDictionary testObject;

        [SetUp()]
        public void SetUp()
        {
            this.testObject = new BuildDataDictionary();
            this.item1 = new EmptyMultiBuildParameters() { Enabled = true };
            this.item2 = new EmptyMultiBuildParameters() { Id = new Guid("2764B5BE-6E56-4694-B1A1-5C105420CB7F") };
            this.item3 = new EmptyMultiBuildParameters() { Enabled = true, Id = new Guid("0AE4E28A-9B78-43bf-ABD2-9F7CB5F3833B") };
        }

        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }

        [Test()]
        public void ConstructTest()
        {
            this.testObject = new BuildDataDictionary();
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

            AddDefaultTestItems();
            expected = 3;
            actual = testObject.Count;
            Assert.AreEqual(expected, actual);
        }

        public void AddDefaultTestItems()
        {
            testObject.AddDataItem("Moo", "FirstOne", item1);
            testObject.AddDataItem("InGeneral", item1);
            testObject.AddDataItem("NotInGeneral", "X-Key", item2);
            testObject.AddDataItem("General", "ReallyCool", item3);
        }

        [Test()]
        public void PropertyEntryCountNormalBehavior()
        {
            // Test read access of 'EntryCount' Property.
            var expected = 0;
            var actual = testObject.EntryCount;
            Assert.AreEqual(expected, actual);

            testObject.AddDataItem("Moo", "Key", item1);
            expected = 1;
            Assert.AreEqual(expected, testObject.EntryCount);

            testObject.AddDataItem("YesInGeneral", item2);
            expected = 2;
            Assert.AreEqual(expected, testObject.EntryCount);

            testObject.AddDataItem("NotInGeneral", "My-Key", item3);
            expected = 3;
            Assert.AreEqual(expected, testObject.EntryCount);

            // Moo.FirstOne -> item1; General.InGeneral -> item1; NotInGeneral.X-Key -> item2; General.ReallyCool -> item3
            // [1+G2] item1 = { Enabled = true };
            // [   3] item2 = { Id = new Guid("2764B5BE-6E56-4694-B1A1-5C105420CB7F") };
            // [  G4] item3 = { Enabled = true, Id = new Guid("0AE4E28A-9B78-43bf-ABD2-9F7CB5F3833B") };
            AddDefaultTestItems();
            expected = 7;
            Assert.AreEqual(expected, testObject.EntryCount);
        }

        [Test()]
        public void PropertyGeneralNormalBehavior()
        {
            // Test read access of 'General' Property.
            var expected = testObject["General"].Keys;
            var actual = testObject.General.Keys;
            var expectedV = testObject["General"].Values;
            var actualV = testObject.General.Values;
            Assert.AreElementsSame(expected, actual);
            Assert.AreElementsSame(expectedV, actualV);

            // Moo.FirstOne -> item1; General.InGeneral -> item1; NotInGeneral.X-Key -> item2; General.ReallyCool -> item3
            // [1+G2] item1 = { Enabled = true };
            // [   3] item2 = { Id = new Guid("2764B5BE-6E56-4694-B1A1-5C105420CB7F") };
            // [  G4] item3 = { Enabled = true, Id = new Guid("0AE4E28A-9B78-43bf-ABD2-9F7CB5F3833B") };
            AddDefaultTestItems();

            Assert.AreEqual(2, testObject.General.Count);
            expected = testObject["General"].Keys;
            actual = testObject.General.Keys;
            expectedV = testObject["General"].Values;
            actualV = testObject.General.Values;
            Assert.AreElementsSame(expected, actual);
            Assert.AreElementsSame(expectedV, actualV);
        }

        [Test()]
        public void PropertyIsDirtyNormalBehavior()
        {
            // Test read access of 'IsDirty' Property.
            var expected = false;
            var actual = testObject.IsDirty;
            Assert.AreEqual(expected, actual);

            testObject.AddDataItem("Moo", "Key", item1);
            expected = true;
            actual = testObject.IsDirty;
            Assert.AreEqual(expected, actual);

            this.testObject = new BuildDataDictionary();
            testObject.AddDataItem("YesInGeneral", item2);
            actual = testObject.IsDirty;
            Assert.AreEqual(expected, actual);

            this.testObject = new BuildDataDictionary();
            testObject.AddDataItem("NotInGeneral", "My-Key", item3);
            actual = testObject.IsDirty;
            Assert.AreEqual(expected, actual);

            testObject.Save();
            expected = false;
            actual = testObject.IsDirty;
            Assert.AreEqual(expected, actual);

            // replacing the existing from above should not affect the dirty flag.
            testObject.AddDataItem("NotInGeneral", "My-Key", item3, true);
            actual = testObject.IsDirty;
            Assert.AreEqual(expected, actual);

            testObject.AddDataItem("NotInGeneral", "My-OtherKey", item3, true);
            expected = true;
            actual = testObject.IsDirty;
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
        public void AddMoreDataItems()
        {
            // Moo.FirstOne -> item1; General.InGeneral -> item1; NotInGeneral.X-Key -> item2; General.ReallyCool -> item3
            // [1+G2] item1 = { Enabled = true };
            // [   3] item2 = { Id = new Guid("2764B5BE-6E56-4694-B1A1-5C105420CB7F") };
            // [  G4] item3 = { Enabled = true, Id = new Guid("0AE4E28A-9B78-43bf-ABD2-9F7CB5F3833B") };
            AddDefaultTestItems();

            var expected = item1;
            var actual = testObject["Moo"]["FirstOne"];
            Assert.AreEqual(expected, actual);

            actual = testObject.General["InGeneral"];
            Assert.AreEqual(expected, actual);

            expected = item2;
            actual = testObject["NotInGeneral"]["X-Key"];
            Assert.AreEqual(expected, actual);

            expected = item3;
            actual = testObject["General"]["ReallyCool"];
            Assert.AreEqual(expected, actual);
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

            // Moo.FirstOne -> item1; General.InGeneral -> item1; NotInGeneral.X-Key -> item2; General.ReallyCool -> item3
            // [1+G2] item1 = { Enabled = true };
            // [   3] item2 = { Id = new Guid("2764B5BE-6E56-4694-B1A1-5C105420CB7F") };
            // [  G4] item3 = { Enabled = true, Id = new Guid("0AE4E28A-9B78-43bf-ABD2-9F7CB5F3833B") };
            AddDefaultTestItems();

            expected = item1;
            actual = testObject["Moo"]["FirstOne"];
            Assert.AreSame(expected, actual);

            actual = testObject["General"]["InGeneral"];
            Assert.AreSame(expected, actual);

            expected = item2;
            actual = testObject["NotInGeneral"]["X-Key"];
            Assert.AreSame(expected, actual);
            
            expected = item3;
            actual = testObject["General"]["ReallyCool"];
            Assert.AreSame(expected, actual);
        }

        [Test()]
        public void PropertyItemIndexerThrows()
        {
            // Moo.FirstOne -> item1; General.InGeneral -> item1; NotInGeneral.X-Key -> item2; General.ReallyCool -> item3
            // [1+G2] item1 = { Enabled = true };
            // [   3] item2 = { Id = new Guid("2764B5BE-6E56-4694-B1A1-5C105420CB7F") };
            // [  G4] item3 = { Enabled = true, Id = new Guid("0AE4E28A-9B78-43bf-ABD2-9F7CB5F3833B") };
            AddDefaultTestItems();

            IBuilderData expected = null;
            IBuilderData actual = null;
            Assert.Throws<KeyNotFoundException>(() => actual = testObject["Moo"]["XXX-Tripple-XXX"]);
            Assert.AreEqual(expected, actual);

            Assert.Throws<KeyNotFoundException>(() => actual = testObject["General"]["XXInGeneralXX"]);
            Assert.AreEqual(expected, actual);

            Assert.Throws<NullReferenceException>(() => actual = testObject["XXXGeneral"]["X-Key"]);
            Assert.AreEqual(expected, actual);

            Assert.Throws<KeyNotFoundException>(() => actual = testObject.General["What.The..."]);
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PropertyItemIndexerGivesNullCategoryWithoutToThrow()
        {
            // Moo.FirstOne -> item1; General.InGeneral -> item1; NotInGeneral.X-Key -> item2; General.ReallyCool -> item3
            // [1+G2] item1 = { Enabled = true };
            // [   3] item2 = { Id = new Guid("2764B5BE-6E56-4694-B1A1-5C105420CB7F") };
            // [  G4] item3 = { Enabled = true, Id = new Guid("0AE4E28A-9B78-43bf-ABD2-9F7CB5F3833B") };
            AddDefaultTestItems();

            var actual = testObject["XXXGeneralXXX"];
            Assert.IsNull(actual);
        }

        [Test()]
        public void GetEnumeratorTest()
        {
            // Moo.FirstOne -> item1; General.InGeneral -> item1; NotInGeneral.X-Key -> item2; General.ReallyCool -> item3
            // [1+G2] item1 = { Enabled = true };
            // [   3] item2 = { Id = new Guid("2764B5BE-6E56-4694-B1A1-5C105420CB7F") };
            // [  G4] item3 = { Enabled = true, Id = new Guid("0AE4E28A-9B78-43bf-ABD2-9F7CB5F3833B") };
            AddDefaultTestItems();

            // *** Class iterator ***
            var actual = testObject.GetEnumerator();
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<IEnumerator<IBuilderData>>(actual);
            Assert.AreEqual(null, actual.Current);
            
            // move to the first item.
            Assert.IsTrue(actual.MoveNext());
            Assert.AreEqual(item1, actual.Current);
            // one forward to the last group 'General' should be item3.
            Assert.IsTrue(actual.MoveNext());
            Assert.AreEqual(item3, actual.Current);
            // one forward, nothing left.
            Assert.IsFalse(actual.MoveNext());
            Assert.AreEqual(null, actual.Current);


            // *** General iterator ***
            Assert.IsNotNull(testObject.General.GetEnumerator());
            Assert.IsInstanceOfType<IEnumerator<KeyValuePair<string, IBuilderData>>>(testObject.General.GetEnumerator());

            Assert.IsNotNull(testObject.Data().GetEnumerator());
            Assert.IsInstanceOfType<IEnumerator<KeyValuePair<string, IReadOnlyDictionary<string, IBuilderData>>>>(
                testObject.Data().GetEnumerator());

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
            IDictionary<string, IBuilderData> actual;
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
