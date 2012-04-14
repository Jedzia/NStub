namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.BuildContext;
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
            // ToDo: Implement SetUp logic here 
            this.codeNamespace = new System.CodeDom.CodeNamespace();
            this.testClassDeclaration = new System.CodeDom.CodeTypeDeclaration();
            this.typeMember = new System.CodeDom.CodeTypeMember();

            MethodInfo methodInfo = this.GetType().GetMethod("SetUp");
            typeMember.UserData["MethodMemberInfo"] = methodInfo;

            this.buildData = new NStub.CSharp.ObjectGeneration.BuildDataCollection();
            this.mocks = new MockRepository();
            this.setUpTearDownContext = this.mocks.StrictMock<NStub.CSharp.BuildContext.ISetupAndTearDownContext>();
            this.testObject = new MemberBuildContext(this.codeNamespace, this.testClassDeclaration, this.typeMember, this.buildData, this.setUpTearDownContext);

        }
        
        [TearDown()]
        public void TearDown()
        {
            // ToDo: Implement TearDown logic here 
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
        public void PropertyIsEventWhenTypeMemberIsEvent()
        {
            MethodInfo methodInfo = typeof(InfoApe).GetMethod("add_PublicEventObject");
            typeMember.UserData["MethodMemberInfo"] = methodInfo;

            // Test read access of 'IsEvent' Property.
            mocks.ReplayAll();
            var expected = true;
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
        public void PropertyIsPropertyWhenTypeMemberIsPropertyGet()
        {
            MethodInfo methodInfo = typeof(InfoApe).GetMethod("get_PublicPropertyGetSetInt");
            typeMember.UserData["MethodMemberInfo"] = methodInfo;

            // Test read access of 'IsProperty' Property.
            mocks.ReplayAll();
            var expected = true;
            var actual = testObject.IsProperty;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }

        [Test()]
        public void PropertyIsPropertyWhenTypeMemberIsPropertySet()
        {
            MethodInfo methodInfo = typeof(InfoApe).GetMethod("set_PublicPropertyGetSetInt");
            typeMember.UserData["MethodMemberInfo"] = methodInfo;

            // Test read access of 'IsProperty' Property.
            mocks.ReplayAll();
            var expected = true;
            var actual = testObject.IsProperty;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }

        [Test()]
        public void PropertyMemberInfoNormalBehavior()
        {
            // TODO: Implement unit test for PropertyMemberInfo

            // Test read access of 'MemberInfo' Property.
            var expected = "Insert expected object here";
            var actual = testObject.MemberInfo;
            //Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyNormalBehaviorClassDeclarationNormalBehavior()
        {
            // TODO: Implement unit test for PropertyTestClassDeclaration

            // Test read access of 'TestClassDeclaration' Property.
            var expected = new System.CodeDom.CodeTypeDeclaration();
            var actual = testObject.TestClassDeclaration;
            //Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyNormalBehaviorKeyNormalBehavior()
        {
            // TODO: Implement unit test for PropertyTestKey

            // Test read access of 'TestKey' Property.
            var expected = "Insert expected object here";
            var actual = testObject.TestKey;
            //Assert.AreEqual(expected, actual);

            // Test write access of 'TestKey' Property.
            expected = "Insert setter object here";
            testObject.TestKey = expected;
            actual = testObject.TestKey;
            //Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyNormalBehaviorObjectTypeNormalBehavior()
        {
            // TODO: Implement unit test for PropertyTestObjectType

            // Test read access of 'TestObjectType' Property.
            var expected = "Insert expected object here";
            var actual = testObject.TestObjectType;
            //Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetBuilderDataTest()
        {
            // TODO: Implement unit test for GetBuilderData

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertySetUpTearDownContextNormalBehavior()
        {
            // TODO: Implement unit test for PropertySetUpTearDownContext

            // Test read access of 'SetUpTearDownContext' Property.
            var expected = "Insert expected object here";
            var actual = testObject.SetUpTearDownContext;
            //Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void PropertyTypeMemberNormalBehavior()
        {
            // TODO: Implement unit test for PropertyTypeMember

            // Test read access of 'TypeMember' Property.
            var expected = new System.CodeDom.CodeTypeMember();
            var actual = testObject.TypeMember;
            //Assert.AreEqual(expected, actual);

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void EqualsTest()
        {
            // TODO: Implement unit test for Equals

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetHashCodeTest()
        {
            // TODO: Implement unit test for GetHashCode

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void GetTypeTest()
        {
            // TODO: Implement unit test for GetType

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void ToStringTest()
        {
            // TODO: Implement unit test for ToString

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
