namespace NStub.CSharp.Tests.ObjectGeneration.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration.Builders;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks;
    using NStub.CSharp.Tests.Data;
    using NStub.Core.Util.Dumper;

    [TestFixture]
    public partial class BuilderSerializerTest
    {

        private MockRepository mocks;
        private IBuildHandler handler;
        private BuildDataDictionary properties;
        private BuilderSerializer testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.mocks = new MockRepository();
            this.handler = this.mocks.StrictMock<IBuildHandler>();
            this.properties = new BuildDataDictionary();
            this.testObject = new BuilderSerializer();
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
            this.properties = null;
        }
        
        [Test()]
        public void ConstructWithParametersTest()
        {
            // TODO: Implement unit test for ConstructWithParameters
            this.testObject = new BuilderSerializer();
        }
        
        [Test()]
        public void DeserializeAllSetupDataTest()
        {
            //var xxx = TestDataProvider.BuildParametersXml();
            // std, EmptyBuildParameters
            var xml = TestDataProvider.BuildParametersMinimalDefaultMethodEraserXml();

            //Server.Default.LambdaFormatter = Console.Out;
            Server.Default.LambdaFormatter = global::Gallio.Framework.TestLog.Warnings;
            //Server.ToConsoleOut();

            Expect.Call(handler.Type).Return(typeof(string));
            mocks.ReplayAll();

            var result = testObject.DeserializeAllSetupData(xml, properties, new[] { handler });
            //typeof(BuilderSerializerTest).Assembly.Dump(3);
            //result.Dump("Headline", 15);
            //global::Gallio.Framework.TestLog.ConsoleOutput.WriteLine("Arscg");
            //global::Gallio.Framework.TestLog.Warnings.WriteLine("Arscg");
            //var xx = properties;
           // Assert.IsEmpty(result, "VAR: {0}", "Moo");
            
            //var xxx = Assert.XmlSerialize(result.First());
            //var xxx = Assert.XmlSerialize(new EmptyMultiBuildParameters());
            //Assert.AreEqual("", xxx);

            // Assert.AreElementsSame(new[] { new EmptyBuildParameters() }, result);
            mocks.VerifyAll();

        }

        [Test(), Ignore]
        public void DeserializeAllSetupDataMinimal()
        {
            var xml = TestDataProvider.BuildParametersMinimalDefaultMethodEraserXml();

            Expect.Call(handler.Type).Return(typeof(string));
            mocks.ReplayAll();
            var result = testObject.DeserializeAllSetupData(xml, properties, new[] { handler });
            Assert.AreElementsSame(new[] { new EmptyBuildParameters() }, result);
            mocks.VerifyAll();
        }

        [Test()]
        public void DeserializeAllSetupDataWithPropertyBuilderAndEmptyBuildParameters()
        {
            // <NStub.CSharp.ObjectGeneration.Builders.DefaultMethodEraser><EmptyBuildParameters>
            var expectedBuilderType = typeof(PropertyBuilder);
            var expectedParameterType = typeof(EmptyBuildParameters);
            var expectedEnabled = true;
            string buildParametersXmlId = "BuildParameters";
            var xml = TestDataProvider.BuildParametersMinimal(buildParametersXmlId, expectedBuilderType, expectedParameterType, expectedEnabled);

            Expect.Call(handler.Type).Return(expectedBuilderType).Repeat.Any();
            Expect.Call(handler.IsMultiBuilder).Return(false).Repeat.Any();
            //Expect.Call(handler.ParameterDataType).Return(typeof(string)).Repeat.Any();
            Expect.Call(handler.ParameterDataType).Return(expectedParameterType).Repeat.Any();
            mocks.ReplayAll();
            var result = testObject.DeserializeAllSetupData(xml, properties, new[] { handler });
            var expected = new EmptyBuildParameters();
            expected.Enabled = expectedEnabled;
            //Assert.AreElementsEqual(new[] { expected }, result,
              //  (x, y) => x.Enabled == y.Enabled && x.GetType() == y.GetType());
            Assert.AreElementsEqual(new[] { expected }, result);

            mocks.VerifyAll();
        }

        [Test()]
        public void DeserializeAllSetupDataWithEmptyBuildParametersHandlerParameterDataType()
        {
            // <NStub.CSharp.ObjectGeneration.Builders.DefaultMethodEraser><EmptyBuildParameters>
            var xml = TestDataProvider.BuildParametersMinimalDefaultMethodEraserXml();

            Expect.Call(handler.Type).Return(typeof(DefaultMethodEraser)).Repeat.Any();
            Expect.Call(handler.IsMultiBuilder).Return(false).Repeat.Any();
            //Expect.Call(handler.ParameterDataType).Return(typeof(string)).Repeat.Any();
            Expect.Call(handler.ParameterDataType).Return(typeof(EmptyBuildParameters)).Repeat.Any();
            mocks.ReplayAll();
            var result = testObject.DeserializeAllSetupData(xml, properties, new[] { handler });
            var expected = new EmptyBuildParameters();
            expected.Enabled = true;
            Assert.AreElementsEqual(new[] { expected }, result, 
                (x,y)=>x.Enabled == y.Enabled && x.GetType() == y.GetType());
            mocks.VerifyAll();
        }

        [Test(), Ignore]
        public void DeserializeAllSetupDataWithEmptyMultiBuildParametersHandlerParameterDataType()
        {
            // <NStub.CSharp.ObjectGeneration.Builders.DefaultMethodEraser><EmptyBuildParameters>
            var xml = TestDataProvider.BuildParametersMinimalDefaultMethodEraserXml();

            Expect.Call(handler.Type).Return(typeof(DefaultMethodEraser)).Repeat.Any();
            Expect.Call(handler.IsMultiBuilder).Return(false).Repeat.Any();
            //Expect.Call(handler.ParameterDataType).Return(typeof(string)).Repeat.Any();
            Expect.Call(handler.ParameterDataType).Return(typeof(EmptyMultiBuildParameters)).Repeat.Any();
            mocks.ReplayAll();
            var result = testObject.DeserializeAllSetupData(xml, properties, new[] { handler });
            Assert.AreElementsSame(new[] { new EmptyBuildParameters() }, result);
            mocks.VerifyAll();
        }

        [Test(), Ignore]
        public void DeserializeAllSetupDataWithWrongHandlerParameterDataType()
        {
            // <NStub.CSharp.ObjectGeneration.Builders.DefaultMethodEraser><EmptyBuildParameters>
            var xml = TestDataProvider.BuildParametersMinimalDefaultMethodEraserXml();

            Expect.Call(handler.Type).Return(typeof(DefaultMethodEraser)).Repeat.Any();
            Expect.Call(handler.IsMultiBuilder).Return(false).Repeat.Any();
            //Expect.Call(handler.ParameterDataType).Return(typeof(string)).Repeat.Any();
            Expect.Call(handler.ParameterDataType).Return(typeof(EmptyMultiBuildParameters)).Repeat.Any();
            mocks.ReplayAll();
            var result = testObject.DeserializeAllSetupData(xml, properties, new[] { handler });
            Assert.AreElementsSame(new[] { new EmptyBuildParameters() }, result);
            mocks.VerifyAll();
        }

        [Test(), Ignore]
        public void DeserializeAllSetupDataWithPropertyBuilder()
        {
            // <NStub.CSharp.ObjectGeneration.Builders.DefaultMethodEraser><EmptyBuildParameters>
            var xml = TestDataProvider.BuildParametersMinimalDefaultMethodEraserXml();

            Expect.Call(handler.Type).Return(typeof(DefaultMethodEraser)).Repeat.Any();
            Expect.Call(handler.IsMultiBuilder).Return(false).Repeat.Any();
            Expect.Call(handler.ParameterDataType).Return(typeof(string)).Repeat.Any();
            mocks.ReplayAll();
            var result = testObject.DeserializeAllSetupData(xml, properties, new[] { handler });
            Assert.AreElementsSame(new[] { new EmptyBuildParameters() }, result);
            mocks.VerifyAll();
        }

        [Test()]
        public void DetermineIMemberBuilderFromXmlFragmentTest()
        {
            // TODO: Implement unit test for DetermineIMemberBuilderFromXmlFragment

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetMultiParametersTest()
        {
            // TODO: Implement unit test for GetMultiParameters

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
        public void SerializeAllHandlersTest()
        {
            // TODO: Implement unit test for SerializeAllHandlers

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void SerializeParametersForBuilderTypeTest()
        {
            // TODO: Implement unit test for SerializeParametersForBuilderType

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
