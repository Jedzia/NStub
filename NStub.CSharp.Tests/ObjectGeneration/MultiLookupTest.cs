namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.ObjectGeneration.Builders;
    
    
    [TestFixture()]
    public partial class MultiLookupTest
    {
        
        private MultiLookup testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.testObject = new MultiLookup();
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }

        [Test()]
        public void Construct()
        {
            this.testObject = new MultiLookup();
        }

        [Test()]
        public void FieldBuilderType()
        {
            var actual = testObject.BuilderType;
            Assert.IsNull(actual);

            var expected = typeof(string);
            testObject.BuilderType = expected;
            actual = testObject.BuilderType;
            Assert.AreSame(expected, actual);
        }

        [Test()]
        public void FieldParameters()
        {
            var actual = testObject.Parameters;
            Assert.IsNull(actual);

            var expected = new EmptyMultiBuildParameters();
            testObject.Parameters = expected;
            actual = testObject.Parameters;
            Assert.AreSame(expected, actual);
        }

        [Test()]
        public void ToStringTest()
        {
            var builderType = typeof(string);
            var parameters = new EmptyMultiBuildParameters();
            testObject.BuilderType = builderType;
            testObject.Parameters = parameters;

            var expected = builderType + " {" + parameters.Id + "}";
            var actual = testObject.ToString();
            Assert.AreEqual(expected, actual);
        }

    }
}
