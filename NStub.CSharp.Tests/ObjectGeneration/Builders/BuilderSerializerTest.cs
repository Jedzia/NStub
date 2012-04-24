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
    using System.Xml;

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

        [Test()]
        public void DeserializeAllSetupDataMinimalDefaultMethodEraser()
        {
            var xml = TestDataProvider.BuildParametersMinimalDefaultMethodEraserXml();

            Expect.Call(handler.Type).Return(typeof(EmptyBuildParameters));
            mocks.ReplayAll();
            var result = testObject.DeserializeAllSetupData(xml, properties, new[] { handler });
            Assert.AreElementsEqual(new[] { new EmptyBuildParameters() }, result);
            Assert.IsEmpty(properties.General.Keys);
            mocks.VerifyAll();
        }

        [Test()]
        public void DeserializeAllSetupDataMinimalDefaultMethodEraserNotPresentHandlerParameterType()
        {
            var xml = TestDataProvider.BuildParametersMinimalDefaultMethodEraserXml();

            // In case no matching handler is found, EmptyBuildParameters has to be instantiated and returned.
            Expect.Call(handler.Type).Return(typeof(string));
            mocks.ReplayAll();
            var result = testObject.DeserializeAllSetupData(xml, properties, new[] { handler });
            Assert.AreElementsEqual(new[] { new EmptyBuildParameters() }, result);
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

            Assert.AreEqual(1, properties.EntryCount);

            // ensure there is a global entry for the PropertyBuilder containing an EmptyBuildParameters instance.
            Assert.Contains(properties.General, new KeyValuePair<string, IBuilderData>(expectedBuilderType.FullName, expected));

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

        [Test()]
        public void DeserializeAllSetupDataWithWrongXmlTypeThrows()
        {
            var xml = TestDataProvider.BuildParametersMinimalDefaultMethodEraserXml();

            Expect.Call(handler.Type).Return(typeof(DefaultMethodEraser)).Repeat.Any();
            Expect.Call(handler.IsMultiBuilder).Return(false).Repeat.Any();
            Expect.Call(handler.ParameterDataType).Return(typeof(EmptyMultiBuildParameters)).Repeat.Any();
            mocks.ReplayAll();
            Assert.Throws<ArgumentOutOfRangeException>(()=> testObject.DeserializeAllSetupData(xml, properties, new[] { handler }));
            mocks.VerifyAll();
        }

        [Test()]
        public void DeserializeAllSetupDataWithWrongXmlMultiTypeThrows()
        {
            var xml = TestDataProvider.BuildParametersWrongMultiMinimalDefaultMethodEraserXml();

            Expect.Call(handler.Type).Return(typeof(DefaultMethodEraser)).Repeat.Any();
            Expect.Call(handler.IsMultiBuilder).Return(true).Repeat.Any();
            Expect.Call(handler.ParameterDataType).Return(typeof(EmptyBuildParameters)).Repeat.Any();
            mocks.ReplayAll();
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.DeserializeAllSetupData(xml, properties, new[] { handler }));
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
            // <NStub.CSharp.ObjectGeneration.Builders.DefaultMethodEraser><EmptyBuildParameters>
            var fragment = GetBuildParametersFragment(TestDataProvider.BuildParametersMinimalDefaultMethodEraserXml());
            var xml = fragment.InnerXml;
            // Todo: hier weiter
            Expect.Call(handler.Type).Return(typeof(DefaultMethodEraser)).Repeat.Any();
            Expect.Call(handler.IsMultiBuilder).Return(false).Repeat.Any();
            Expect.Call(handler.ParameterDataType).Return(typeof(string)).Repeat.Any();
            mocks.ReplayAll();
            var result = testObject.DetermineIMemberBuilderFromXmlFragment(xml, new[] { handler });
            Assert.AreSame(handler, result);
            mocks.VerifyAll();
        }

        public static XmlElement GetBuildParametersFragment(string xmlDocumentContent)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlDocumentContent);
            var fragment = xmlDoc[BuilderConstants.BuildParametersXmlId];
            return fragment;
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
