namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using global::MbUnit.Framework;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks;
    using NStub.CSharp.Tests.Stubs;
    using NStub.CSharp.ObjectGeneration.Builders;

    public partial class BuildHandlerTest
    {

        private MockRepository mocks;
        private BuildHandler testObject;
        private System.Type type;
        private Func<IMemberBuildContext, bool> handler;
        private IMemberBuildContext buildContext;
        private IMemberBuilder builder;
        private bool handlerReturn;
        
        [SetUp()]
        public void SetUp()
        {
            this.mocks = new MockRepository();

            this.buildContext = this.mocks.StrictMock<IMemberBuildContext>();
            this.builder = this.mocks.StrictMock<IMemberBuilder>();
            this.type = this.builder.GetType();
            this.handlerReturn = false;
            handler = (e) => { return this.handlerReturn; };
            this.testObject = new BuildHandler(this.type, handler, MemberBuilder.EmptyParameters.GetType());
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.handler = null;
            this.buildContext = null;
            this.testObject = null;
            this.mocks = null;
        }
        
        [Test()]
        public void ConstructWithParameterTypeIsNotIMemberBuilderShouldThrow()
        {
            // all other construction types than IMemberBuilder should throw.
            this.type = typeof(object);
            Assert.Throws<ArgumentException>(() => new BuildHandler(this.type, handler, MemberBuilder.EmptyParameters.GetType()));
        }
        
        [Test()]
        public void PropertyHandlerNormalBehavior()
        {
            // Test read access of 'Handler' Property.
            mocks.ReplayAll();
            var expected = this.handler;
            var actual = testObject.CanHandle;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertyTypeNormalBehavior()
        {
            // Test read access of 'Type' Property.
            mocks.ReplayAll();
            var expected = this.type;
            var actual = testObject.Type;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void CreateInstanceTest()
        {
            this.type = typeof(MyMemberBuilder);
            this.testObject = new BuildHandler(this.type, handler, MemberBuilder.EmptyParameters.GetType());
            mocks.ReplayAll();

            var actual = testObject.CreateInstance(this.buildContext);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<MyMemberBuilder>(actual);
            var actualMemberBuilder = (MyMemberBuilder)actual;
            Assert.AreEqual(this.buildContext, actualMemberBuilder.Context);
            Assert.AreEqual(0, actualMemberBuilder.BuildCalled);
            Assert.AreEqual(0, actualMemberBuilder.GetTestNameCalled);

            mocks.VerifyAll();
        }
    }
}
