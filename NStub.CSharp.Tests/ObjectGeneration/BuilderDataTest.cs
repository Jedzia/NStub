namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks;
    
    
    [TestFixture()]
    public partial class BuilderDataTest
    {
        
        private string dataObject;
        private BuilderData<string> testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.dataObject = "MyData";
            this.testObject = new BuilderData<string>(this.dataObject);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersDataObjectTest()
        {
            this.testObject = new BuilderData<string>(this.dataObject);
            
            Assert.Throws<ArgumentNullException>(() => new BuilderData<string>(null));
        }
        
        [Test()]
        public void PropertyDataNormalBehavior()
        {
            // Test read access of 'Data' Property.
            var expected = this.dataObject;
            var actual = testObject.Data;
            Assert.AreSame(expected, actual);
        }
        
        [Test()]
        public void PropertyIsCompleteNormalBehavior()
        {
            // Test read access of 'IsComplete' Property.
            var expected = true;
            var actual = testObject.IsComplete;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void GetDataTest()
        {
            var expected = dataObject;
            var actual = testObject.GetData();
            Assert.AreSame(expected, actual);
        }
        
        [Test()]
        public void HasDataForTypeTest()
        {

            var builder = MockRepository.GenerateStrictMock<IMemberBuilder>();
            builder.Replay();

            var expected = true;
            var actual = testObject.HasDataForType(builder);
            Assert.AreEqual(expected, actual);

            builder.VerifyAllExpectations();
        }

        [Test()]
        public void SetDataTest()
        {
            var expected = "New Value";
            testObject.SetData(expected);
            var actual = testObject.GetData();
            Assert.AreSame(expected, actual);

            var wrongType = 123d;
            Assert.Throws<ArgumentException>(() => testObject.SetData(wrongType));
            actual = testObject.GetData();
            Assert.AreSame(expected, actual);
        }


        private class Animal { }
        private class Mammal : Animal { }
        private class Chimpanzee : Mammal { }

        [Test()]
        public void SetDataInheritance()
        {
            var testAnimal = new BuilderData<Mammal>(new Mammal());
            testAnimal.SetData(new Chimpanzee());
            Assert.Throws<ArgumentException>(() => testAnimal.SetData(new Animal()));
        }
    }
}
