namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.ObjectGeneration.Builders;
    using Rhino.Mocks;
    using System.CodeDom;

    [TestFixture]
    public partial class PropertyBuilderTest
    {

        private NStub.CSharp.BuildContext.IMemberSetupContext context;
        private NStub.CSharp.BuildContext.IMemberBuildContext buildcontext;

        private MockRepository mocks;

        private PropertyBuilder testObject;

        [SetUp()]
        public void SetUp()
        {
            this.mocks = new MockRepository();
            this.context = this.mocks.StrictMock<NStub.CSharp.BuildContext.IMemberSetupContext>();
            this.buildcontext = this.mocks.StrictMock<NStub.CSharp.BuildContext.IMemberBuildContext>();
            this.testObject = new PropertyBuilder(this.context);
        }

        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }

        [Test()]
        public void CanHandleContextTest()
        {
            Expect.Call(buildcontext.IsProperty).Return(false);
            Expect.Call(buildcontext.IsProperty).Return(true);
            mocks.ReplayAll();

            var expected = false;
            var actual = PropertyBuilder.CanHandleContext(this.buildcontext);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = PropertyBuilder.CanHandleContext(this.buildcontext);
            Assert.AreEqual(expected, actual);

            mocks.VerifyAll();
        }

        [Test()]
        public void PropertySetupContextNormalBehavior()
        {
            // Test read access of 'SetupContext' Property.
            var expected = this.context;
            var actual = testObject.SetupContext;
            Assert.AreSame(expected, actual);
        }

        [Test()]
        public void BuildTest()
        {
            var typeMember = new CodeMemberMethod() { Name = "TypeMemberTest" };
            var propData = mocks.Stub<IBuilderData>();
            Expect.Call(buildcontext.TypeMember).Return(typeMember);
            //Expect.Call(buildcontext.GetBuilderData("Property")).Return(propData);
            
            // is not relevant ... just a devel-testing call to 
            // "var userData = context.GetBuilderData<BuildParametersOfPropertyBuilder>(this);"
            Expect.Call(buildcontext.GetBuilderData<BuildParametersOfPropertyBuilder>(testObject)).Return(null);
            Expect.Call(buildcontext.GetBuilderData("Property")).Return(propData).Repeat.Any();
            //Expect.Call(buildcontext.IsProperty).Return(true);
            mocks.ReplayAll();

            testObject.Build(this.buildcontext);

            // Todo: check if only the ending "Test" gets replaced.
            var expected = "TypeMemberNormalBehavior";
            var actual = typeMember.Name;
            Assert.AreEqual(expected, actual);

            mocks.VerifyAll();
        }

        [Test()]
        public void BuildArgumentContextIsNullShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Build(null));
        }

        [Test()]
        public void BuildArgumentContextOfWrongTypeMemberShouldThrow()
        {
            var typeMember = new CodeTypeMember() { Name = "TestTypeMember" };
            var propData = mocks.Stub<IBuilderData>();
            Expect.Call(buildcontext.TypeMember).Return(typeMember);
            mocks.ReplayAll();

            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Build(this.buildcontext));

            mocks.VerifyAll();
        }

        [Test()]
        public void GetTestNameTest()
        {
            // The TypeMembers name isn't relevant. the information is taken from the
            // originalName parameter of the GetTestName method.
            // var typeMember = new CodeMemberMethod() { Name = "NameIsNotRelevant" };
            // Expect.Call(buildcontext.TypeMember).Return(typeMember).Repeat.Any();
            mocks.ReplayAll();

            // The 'get_' or 'set_' prefix of Property methods should be transformed into
            // a 'Property' prefix for the test name of that property.
            var initial = "get_OtherTypeMemberTest";
            var actual = testObject.GetTestName(this.buildcontext, initial);
            var expected = "PropertyOtherTypeMemberTest";
            Assert.AreEqual(expected, actual);

            initial = "set_OtherTypeMemberTest";
            actual = testObject.GetTestName(this.buildcontext, initial);
            expected = "PropertyOtherTypeMemberTest";
            Assert.AreEqual(expected, actual);

            mocks.VerifyAll();
        }
    }
}
