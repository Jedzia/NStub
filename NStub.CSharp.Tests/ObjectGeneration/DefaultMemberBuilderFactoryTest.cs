namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks;
    
    
    [TestFixture()]
    public partial class DefaultMemberBuilderFactoryTest
    {
        private System.Collections.Generic.IEnumerable<NStub.CSharp.ObjectGeneration.IBuildHandler> handlers;

        private NStub.CSharp.ObjectGeneration.IBuildHandler handlersItem;

        private MockRepository mocks;

        private NStub.CSharp.ObjectGeneration.Builders.IBuilderSerializer serializer;

        private DefaultMemberBuilderFactory testObject;

        public DefaultMemberBuilderFactoryTest()
        {
        }

        [SetUp()]
        public void SetUp()
        {
            // ToDo: Implement SetUp logic here 
            this.mocks = new MockRepository();
            this.serializer = this.mocks.StrictMock<NStub.CSharp.ObjectGeneration.Builders.IBuilderSerializer>();
            this.handlersItem = this.mocks.StrictMock<IBuildHandler>();
            this.handlers = new[] { handlersItem };
            this.testObject = new DefaultMemberBuilderFactory(this.serializer, this.handlers);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TearDown()]
        public void TearDown()
        {
            // ToDo: Implement TearDown logic here 
            this.testObject = null;

            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [Test()]
        public void ConstructWithParametersSerializerHandlersTest()
        {
            // TODO: Implement unit test for ConstructWithParametersSerializerHandlers
            // this.serializer = new NStub.CSharp.ObjectGeneration.Builders.IBuilderSerializer();
            // this.handlers = new System.Collections.Generic.IEnumerable<NStub.CSharp.ObjectGeneration.IBuildHandler>(this.handlersItem);
            // this.testObject = new DefaultMemberBuilderFactory(this.serializer, this.handlers);

            Assert.Throws<ArgumentNullException>(() => new DefaultMemberBuilderFactory(null, this.handlers));
            Assert.Throws<ArgumentNullException>(() => new DefaultMemberBuilderFactory(this.serializer, null));
        }

        [Test()]
        public void ConstructWithParametersTest()
        {
            // TODO: Implement unit test for ConstructWithParameters
            this.testObject = new DefaultMemberBuilderFactory();


            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyBuilderNamesNormalBehavior()
        {
            // Test read access of 'BuilderNames' Property.
            var actual = testObject.BuilderNames;
            Assert.IsNotEmpty(actual);
            Assert.Count(8, actual);
        }
        
        [Test()]
        public void PropertyBuilderTypesNormalBehavior()
        {
            // Test read access of 'BuilderTypes' Property.
            var actual = testObject.BuilderTypes;
            Assert.IsNotEmpty(actual);
            Assert.Count(8, actual);
        }
        
        [Test()]
        public void PropertyFactoryNormalBehavior()
        {
            // Test read access of 'Factory' Property.
            var actual = testObject.Factory;
            Assert.IsNotNull(actual);
        }
        
        [Test()]
        public void AddHandlerTest()
        {
            // TODO: Implement unit test for AddHandler
            //testObject.AddHandler(new BuildHandler(typeof(string), (e) => true, typeof(int)));
            //testObject.AddHandler(new BuildHandler(typeof(string), (e) => true, typeof(int)));

            Assert.Throws<ArgumentException>(() => testObject.AddHandler(new BuildHandler(typeof(string), (e) => true, typeof(int))));
            //Assert.Throws<ArgumentException>(() => testObject.AddHandler(new BuildHandler(typeof(IMemberBuilder), (e) => true, typeof(int))));

            // Todo: type checking for instances (in BuildHandler)
            testObject.AddHandler(new BuildHandler(typeof(IMemberBuilder), (e) => true, typeof(IMemberBuildParameters)));
        }
        
        [Test()]
        public void DeserializeAllSetupDataTest()
        {
            // TODO: Implement unit test for DeserializeAllSetupData

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetBuilderDescriptionTest()
        {
            // TODO: Implement unit test for GetBuilderDescription

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetBuilderTest()
        {
            // TODO: Implement unit test for GetBuilder

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetMultiParameterTest()
        {
            // TODO: Implement unit test for GetMultiParameter

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetParametersTest()
        {
            // TODO: Implement unit test for GetParameters

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetSampleSetupDataTest()
        {
            // TODO: Implement unit test for GetSampleSetupData

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void MultiParametersTest()
        {
            // TODO: Implement unit test for MultiParameters

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void SerializeAllSetupDataTest()
        {
            // TODO: Implement unit test for SerializeAllSetupData

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void SerializeSetupDataTest()
        {
            // TODO: Implement unit test for SerializeSetupData

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void SetParametersTest()
        {
            // TODO: Implement unit test for SetParameters

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
