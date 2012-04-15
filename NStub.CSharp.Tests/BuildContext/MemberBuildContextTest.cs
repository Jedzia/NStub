namespace NStub.CSharp.Tests.BuildContext
{
    using System;
    using global::MbUnit.Framework;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks;
    using System.Reflection;
    using NStub.CSharp.Tests.Stubs;

    public partial class MemberBuildContextTest
    {

        private NStub.CSharp.ObjectGeneration.BuildDataCollection buildData;
        private System.CodeDom.CodeNamespace codeNamespace;
        private MockRepository mocks;
        private NStub.CSharp.BuildContext.ISetupAndTearDownContext setUpTearDownContext;
        private System.CodeDom.CodeTypeDeclaration testClassDeclaration;
        private MemberBuildContext testObject;
        private System.CodeDom.CodeTypeMember typeMember;


        public MemberBuildContextTest()
        {
        }

        [SetUp()]
        public void SetUp()
        {
            this.codeNamespace = new System.CodeDom.CodeNamespace();
            this.testClassDeclaration = new System.CodeDom.CodeTypeDeclaration();
            this.typeMember = new System.CodeDom.CodeTypeMember();

            //MethodInfo methodInfo = this.GetType().GetMethod("SetUp");
            //typeMember.UserData["MethodMemberInfo"] = methodInfo;

            this.buildData = new NStub.CSharp.ObjectGeneration.BuildDataCollection();
            this.mocks = new MockRepository();
            this.setUpTearDownContext = this.mocks.StrictMock<NStub.CSharp.BuildContext.ISetupAndTearDownContext>();
            this.testObject = new MemberBuildContext(this.codeNamespace, this.testClassDeclaration, this.typeMember, this.buildData, this.setUpTearDownContext);

        }

        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }

        [Test()]
        public void ConstructWithParametersCodeNamespaceTestClassDeclarationTypeMemberBuildDataSetUpTearDownContextTest()
        {
            // TODO: Implement unit test for ConstructWithParametersCodeNamespaceTestClassDeclarationTypeMemberBuildDataSetUpTearDownContext
            this.codeNamespace = new System.CodeDom.CodeNamespace();
            this.testClassDeclaration = new System.CodeDom.CodeTypeDeclaration();
            this.typeMember = new System.CodeDom.CodeTypeMember();
            this.buildData = new NStub.CSharp.ObjectGeneration.BuildDataCollection();
            this.testObject = new MemberBuildContext(this.codeNamespace, this.testClassDeclaration, this.typeMember, this.buildData, this.setUpTearDownContext);
        }

        [Test()]
        public void ConstructWithAnyParameterIsNullShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => this.testObject = new MemberBuildContext(
                null,
                this.testClassDeclaration,
                this.typeMember,
                this.buildData,
                this.setUpTearDownContext));

            Assert.Throws<ArgumentNullException>(() => this.testObject = new MemberBuildContext(
                this.codeNamespace,
                null,
                this.typeMember,
                this.buildData,
                this.setUpTearDownContext));

            Assert.Throws<ArgumentNullException>(() => this.testObject = new MemberBuildContext(
                this.codeNamespace,
                this.testClassDeclaration,
                null,
                this.buildData,
                this.setUpTearDownContext));

            Assert.Throws<ArgumentNullException>(() => this.testObject = new MemberBuildContext(
                this.codeNamespace,
                this.testClassDeclaration,
                this.typeMember,
                null,
                this.setUpTearDownContext));

            Assert.Throws<ArgumentNullException>(() => this.testObject = new MemberBuildContext(
                this.codeNamespace,
                this.testClassDeclaration,
                this.typeMember,
                this.buildData,
                null));
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
            this.testObject = this.mocks.StrictMock<MemberBuildContext>(this.codeNamespace, this.testClassDeclaration, this.typeMember, this.buildData, this.setUpTearDownContext);
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
        [Row("--NULL--", false)]
        [Row("PublicVoidMethodVoid", false)]
        [Row("add_PublicEventObject", true)]
        [Row("remove_PublicEventObject", true)]
        [Row("get_PublicPropertyGetSetInt", false)]
        [Row("set_PublicPropertyGetSetString", false)]
        public void PropertyIsEventWhenTypeMemberIsEvent(string signature, bool expected)
        {
            SetUserData(this.typeMember, signature);

            // Test read access of 'IsEvent' Property.
            mocks.ReplayAll();
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

        /// <summary>
        /// Set the user data with key 'MethodMemberInfo' of the specified CodeObject to a method 
        /// from the <see cref="InfoApe"/> class. The MethodInfo resolution is done
        /// through a <see cref="MethodInfo.GetMethod(string)"/> signature.
        /// </summary>
        /// <param name="codeTypeMember">The code type member which gets the <see cref="MethodInfo.UserData"/> 'MethodMemberInfo' set.</param>
        /// <param name="methodSignature">The signature of the desired method.</param>
        private static MethodInfo SetUserData(System.CodeDom.CodeObject codeTypeMember, string methodSignature)
        {
            MethodInfo methodInfo = typeof(InfoApe).GetMethod(methodSignature);
            codeTypeMember.UserData["MethodMemberInfo"] = methodInfo;
            return methodInfo;
        }

        [Test()]
        [Row("--NULL--", false)]
        [Row("PublicVoidMethodVoid", false)]
        [Row("add_PublicEventObject", false)]
        [Row("get_PublicPropertyGetSetInt", true)]
        [Row("set_PublicPropertyGetSetString", true)]
        public void PropertyIsPropertyWhenTypeMemberIsProperty(string signature, bool expected)
        {
            SetUserData(this.typeMember, signature);

            // Test read access of 'IsProperty' Property.
            mocks.ReplayAll();
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
            Assert.IsNull(actual);

            var expected = SetUserData(this.typeMember, "set_PublicPropertyGetSetInt");
            actual = testObject.MemberInfo;
            Assert.AreEqual(expected, actual);

            mocks.VerifyAll();
        }

        [Test()]
        public void PropertyNormalBehaviorClassDeclarationNormalBehavior()
        {
            // Test read access of 'TestClassDeclaration' Property.
            mocks.ReplayAll();
            var expected = this.testClassDeclaration;
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
            Assert.IsNull(actual);

            // Test write access of 'TestKey' Property.
            var expected = "My Test Key";
            testObject.TestKey = expected;
            actual = testObject.TestKey;
            Assert.AreEqual(expected, actual);

            mocks.VerifyAll();
        }

        [Test()]
        public void PropertyTestObjectTypeNormalBehavior()
        {
            // Test read access of 'TestObjectType' Property.
            mocks.ReplayAll();
            var actual = testObject.TestObjectType;
            Assert.IsNull(actual);

            var expected = typeof(InfoApe);
            this.testClassDeclaration.UserData["TestObjectClassType"] = expected;
            actual = testObject.TestObjectType;
            Assert.AreEqual(expected, actual);

            mocks.VerifyAll();
        }

        [Test()]
        public void GetBuilderDataTestThrowsWithOutTestKeySet()
        {
            mocks.ReplayAll();
            Assert.Throws<InvalidOperationException>(() => testObject.GetBuilderData("CAT"));
            mocks.VerifyAll();
        }

        [Test()]
        public void GetBuilderDataTest()
        {
            var expected = this.mocks.StrictMock<IBuilderData>();
            mocks.ReplayAll();

            // no data in buildData at all.
            testObject.TestKey = "TheKey";
            var actual = testObject.GetBuilderData("CAT");
            Assert.IsNull(actual);

            // add item with key "TheKey" to category 'CAT' and request it through GetBuilderData.
            this.buildData.AddDataItem("CAT", "TheKey", expected);
            actual = testObject.GetBuilderData("CAT");
            Assert.AreEqual(expected, actual);

            // request with a key that is not in buildData.
            testObject.TestKey = "OtherKey";
            actual = testObject.GetBuilderData("CAT");
            Assert.IsNull(actual);

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
            var expected = typeMember;
            var actual = testObject.TypeMember;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }
    }
}
