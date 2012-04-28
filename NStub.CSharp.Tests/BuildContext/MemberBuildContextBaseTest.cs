namespace NStub.CSharp.Tests.BuildContext
{
    using System;
    using global::MbUnit.Framework;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks;

    [Ignore]
    public partial class MemberBuildContextBaseTest
    {

        private NStub.CSharp.ObjectGeneration.BuildDataDictionary buildData;
        private System.CodeDom.CodeNamespace codeNamespace;
        private MockRepository mocks;
        private NStub.CSharp.BuildContext.ISetupAndTearDownContext setUpTearDownContext;
        private System.CodeDom.CodeTypeDeclaration testClassDeclaration;
        private MemberBuildContextBase testObject;
        private System.CodeDom.CodeTypeMember typeMember;
        private IBuilderData builderData;
        
        public MemberBuildContextBaseTest()
        {
        }
        
        [SetUp()]
        public void SetUp()
        {
            this.codeNamespace = new System.CodeDom.CodeNamespace();
            this.testClassDeclaration = new System.CodeDom.CodeTypeDeclaration();
            this.typeMember = new System.CodeDom.CodeTypeMember();
            this.buildData = new NStub.CSharp.ObjectGeneration.BuildDataDictionary();
            this.mocks = new MockRepository();
            this.builderData = this.mocks.StrictMock<IBuilderData>();
            this.buildData.AddDataItem("CAT", "TheKey", builderData);
            this.setUpTearDownContext = this.mocks.StrictMock<NStub.CSharp.BuildContext.ISetupAndTearDownContext>();
            this.testObject = this.mocks.Stub<MemberBuildContextBase>(this.codeNamespace, this.testClassDeclaration, this.typeMember, this.buildData, this.setUpTearDownContext);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void PropertyBuildDataNormalBehavior()
        {
            // Test read access of 'BuildData' Property.
            mocks.ReplayAll();
            var expected = buildData;
            var actual = testObject.BuildData;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertyCodeNamespaceNormalBehavior()
        {
            // Test read access of 'CodeNamespace' Property.
            mocks.ReplayAll();
            var expected = this.codeNamespace;
            var actual = testObject.CodeNamespace;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertyIsConstructorNormalBehavior()
        {
            // Test read access of 'IsConstructor' Property.
            mocks.ReplayAll();
            var expected = false;
            var actual = testObject.IsConstructor;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();

            this.typeMember = new System.CodeDom.CodeConstructor();
            this.testObject = this.mocks.StrictMock<MemberBuildContextBase>(this.codeNamespace, this.testClassDeclaration, this.typeMember, this.buildData, this.setUpTearDownContext);
            mocks.ReplayAll();
            expected = true;
            actual = testObject.IsConstructor;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertyIsEventNormalBehavior()
        {
            // Test read access of 'IsEvent' Property.
            mocks.ReplayAll();
            var expected = false;
            var actual = testObject.IsEvent;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertyIsPropertyNormalBehavior()
        {
            // Test read access of 'IsProperty' Property.
            mocks.ReplayAll();
            var expected = false;
            var actual = testObject.IsProperty;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertyMemberInfoNormalBehavior()
        {
            // Test read access of 'MemberInfo' Property.
            mocks.ReplayAll();
            var actual = testObject.MemberInfo;
            Assert.AreEqual(null, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertyTestClassDeclarationNormalBehavior()
        {
            // Test read access of 'TestClassDeclaration' Property.
            mocks.ReplayAll();
            var expected = testClassDeclaration;
            var actual = testObject.TestClassDeclaration;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertyTestKeyNormalBehavior()
        {
            // Test read access of 'TestKey' Property.
            mocks.ReplayAll();
            var actual = testObject.TestKey;
            Assert.IsNull(null, actual);
        }
        
        [Test()]
        public void PropertyTestObjectTypeNormalBehavior()
        {
            // Test read access of 'TestObjectType' Property.
            mocks.ReplayAll();
            // var expected = "Insert expected object here";
            var actual = testObject.TestObjectType;
            Assert.AreEqual(null, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void GetBuilderDataTest()
        {
            mocks.ReplayAll();
            // without a key ... throw!
            Assert.Throws<InvalidOperationException>(() => testObject.GetBuilderData("CAT"));

            var expected = this.builderData;
            var actual = testObject.GetBuilderData("CAT");
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertySetUpTearDownContextNormalBehavior()
        {
            // Test read access of 'SetUpTearDownContext' Property.
            mocks.ReplayAll();
            var expected = setUpTearDownContext;
            var actual = testObject.SetUpTearDownContext;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
        
        [Test()]
        public void PropertyTypeMemberNormalBehavior()
        {
            // Test read access of 'TypeMember' Property.
            mocks.ReplayAll();
            var expected = this.typeMember;
            var actual = testObject.TypeMember;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
    }
}
