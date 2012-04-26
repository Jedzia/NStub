// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberBuildContextTest.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.Tests.BuildContext
{
    using System;
    using System.CodeDom;
    using System.Reflection;
    using global::MbUnit.Framework;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.Tests.Stubs;
    using Rhino.Mocks;
    using NStub.Core;

    public class MemberBuildContextTest
    {
        #region Fields

        private BuildDataDictionary buildData;
        private CodeNamespace codeNamespace;
        private MockRepository mocks;
        private ISetupAndTearDownContext setUpTearDownContext;
        private CodeTypeDeclaration testClassDeclaration;
        private MemberBuildContext testObject;
        private CodeTypeMember typeMember;
        private const string TheKey = "TheKey";

        #endregion

        [Test]
        public void ConstructWithAnyParameterIsNullShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(
                () => this.testObject = new MemberBuildContext(
                                            null, 
                                            this.testClassDeclaration, 
                                            this.typeMember, 
                                            this.buildData,
                                            this.setUpTearDownContext, TheKey));

            Assert.Throws<ArgumentNullException>(
                () => this.testObject = new MemberBuildContext(
                                            this.codeNamespace, 
                                            null, 
                                            this.typeMember, 
                                            this.buildData,
                                            this.setUpTearDownContext, TheKey));

            Assert.Throws<ArgumentNullException>(
                () => this.testObject = new MemberBuildContext(
                                            this.codeNamespace, 
                                            this.testClassDeclaration, 
                                            null, 
                                            this.buildData,
                                            this.setUpTearDownContext, TheKey));

            Assert.Throws<ArgumentNullException>(
                () => this.testObject = new MemberBuildContext(
                                            this.codeNamespace, 
                                            this.testClassDeclaration, 
                                            this.typeMember, 
                                            null,
                                            this.setUpTearDownContext, TheKey));

            Assert.Throws<ArgumentNullException>(
                () => this.testObject = new MemberBuildContext(
                                            this.codeNamespace,
                                            this.testClassDeclaration,
                                            this.typeMember,
                                            this.buildData,
                                            null, TheKey));
            Assert.Throws<ArgumentNullException>(
                () => this.testObject = new MemberBuildContext(
                                            this.codeNamespace,
                                            this.testClassDeclaration,
                                            this.typeMember,
                                            this.buildData,
                                            this.setUpTearDownContext, null));
        }

        [Test]
        public void ConstructWithParametersCodeNamespaceTestClassDeclarationTypeMemberBuildDataSetUpTearDownContextTest(
            )
        {
            // TODO: Implement unit test for ConstructWithParametersCodeNamespaceTestClassDeclarationTypeMemberBuildDataSetUpTearDownContext
            this.codeNamespace = new CodeNamespace();
            this.testClassDeclaration = new CodeTypeDeclaration();
            this.typeMember = new CodeTypeMember();
            this.buildData = new BuildDataDictionary();
            this.testObject = new MemberBuildContext(
                this.codeNamespace, 
                this.testClassDeclaration, 
                this.typeMember, 
                this.buildData, 
                this.setUpTearDownContext, TheKey);
        }

        [Test]
        public void GetBuilderDataTest()
        {
            var expected = this.mocks.StrictMock<IBuilderData>();
            this.mocks.ReplayAll();

            // no data in buildData at all.
            // this.testObject.TestKey = "TheKey";
            var actual = this.testObject.GetBuilderData("CAT");
            Assert.IsNull(actual);

            // add item with key "TheKey" to category 'CAT' and request it through GetBuilderData.
            this.buildData.AddDataItem("CAT", "TheKey", expected);
            actual = this.testObject.GetBuilderData("CAT");
            Assert.AreEqual(expected, actual);

            this.mocks.VerifyAll();
        }

        [Test]
        public void GetBuilderDataGeneric()
        {
            var builder = this.mocks.StrictMock<IMemberBuilder>();
            var expected = this.mocks.StrictMock<IBuilderData>();
            this.mocks.ReplayAll();

            // no data in buildData at all.
            // this.testObject.TestKey = "TheKey";
            var actual = this.testObject.GetBuilderData<IBuilderData>(builder);
            Assert.IsNull(actual);

            // add item with key "builder Type.FullName" to category of 'General' and request it through GetBuilderData<T>.
            this.buildData.AddDataItem(builder.GetType().FullName, expected);
            actual = this.testObject.GetBuilderData<IBuilderData>(builder);
            Assert.AreEqual(expected, actual);

            this.mocks.VerifyAll();
        }

        [Test]
        public void PropertyBuildDataNormalBehavior()
        {
            // Test read access of 'BuildData' Property.
            this.mocks.ReplayAll();
            var expected = this.buildData;
            var actual = this.testObject.BuildData;
            Assert.AreEqual(expected, actual);
            this.mocks.VerifyAll();
        }

        [Test]
        public void PropertyCodeNamespaceNormalBehavior()
        {
            // Test read access of 'CodeNamespace' Property.
            this.mocks.ReplayAll();
            var expected = this.codeNamespace;
            var actual = this.testObject.CodeNamespace;
            Assert.AreEqual(expected, actual);
            this.mocks.VerifyAll();
        }

        [Test]
        public void PropertyIsConstructorNormalBehavior()
        {
            // Test read access of 'IsConstructor' Property.
            this.mocks.ReplayAll();
            var expected = false;
            var actual = this.testObject.IsConstructor;
            Assert.AreEqual(expected, actual);
            this.mocks.VerifyAll();

            this.typeMember = new CodeConstructor();
            this.testObject = this.mocks.StrictMock<MemberBuildContext>(
                this.codeNamespace, 
                this.testClassDeclaration, 
                this.typeMember, 
                this.buildData, 
                this.setUpTearDownContext, TheKey);
            this.mocks.ReplayAll();
            expected = true;
            actual = this.testObject.IsConstructor;
            Assert.AreEqual(expected, actual);
            this.mocks.VerifyAll();
        }

        [Test]
        public void PropertyIsEventNormalBehavior()
        {
            // Test read access of 'IsEvent' Property.
            this.mocks.ReplayAll();
            var expected = false;
            var actual = this.testObject.IsEvent;
            Assert.AreEqual(expected, actual);
            this.mocks.VerifyAll();
        }

        [Test]
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
            this.mocks.ReplayAll();
            var actual = this.testObject.IsEvent;
            Assert.AreEqual(expected, actual);
            this.mocks.VerifyAll();
        }

        [Test]
        public void PropertyIsPropertyNormalBehavior()
        {
            // Test read access of 'IsProperty' Property.
            this.mocks.ReplayAll();
            var expected = false;
            var actual = this.testObject.IsProperty;
            Assert.AreEqual(expected, actual);
            this.mocks.VerifyAll();
        }

        [Test]
        [Row("--NULL--", false)]
        [Row("PublicVoidMethodVoid", false)]
        [Row("add_PublicEventObject", false)]
        [Row("get_PublicPropertyGetSetInt", true)]
        [Row("set_PublicPropertyGetSetString", true)]
        public void PropertyIsPropertyWhenTypeMemberIsProperty(string signature, bool expected)
        {
            SetUserData(this.typeMember, signature);

            // Test read access of 'IsProperty' Property.
            this.mocks.ReplayAll();
            var actual = this.testObject.IsProperty;
            Assert.AreEqual(expected, actual);
            this.mocks.VerifyAll();
        }


        [Test]
        public void PropertyMemberInfoNormalBehavior()
        {
            // Test read access of 'MemberInfo' Property.
            this.mocks.ReplayAll();
            var actual = this.testObject.MemberInfo;
            Assert.IsNull(actual);

            var expected = SetUserData(this.typeMember, "set_PublicPropertyGetSetInt");
            actual = this.testObject.MemberInfo;
            Assert.AreEqual(expected, actual);

            this.mocks.VerifyAll();
        }

        [Test]
        public void PropertyNormalBehaviorClassDeclarationNormalBehavior()
        {
            // Test read access of 'TestClassDeclaration' Property.
            this.mocks.ReplayAll();
            var expected = this.testClassDeclaration;
            var actual = this.testObject.TestClassDeclaration;
            Assert.AreEqual(expected, actual);

            this.mocks.VerifyAll();
        }

        [Test]
        public void PropertySetUpTearDownContextNormalBehavior()
        {
            // Test read access of 'SetUpTearDownContext' Property.
            this.mocks.ReplayAll();
            var expected = this.setUpTearDownContext;
            var actual = this.testObject.SetUpTearDownContext;
            Assert.AreEqual(expected, actual);
            this.mocks.VerifyAll();
        }

        [Test]
        public void PropertyTestKeyNormalBehavior()
        {
            // Test read access of 'TestKey' Property.
            this.mocks.ReplayAll();
            var actual = this.testObject.TestKey;
            Assert.AreEqual(TheKey, actual);

            this.mocks.VerifyAll();
        }

        [Test]
        public void PropertyTestObjectTypeNormalBehavior()
        {
            // Test read access of 'TestObjectType' Property.
            this.mocks.ReplayAll();
            var actual = this.testObject.TestObjectType;
            Assert.IsNull(actual);

            var expected = typeof(InfoApe);
            this.testClassDeclaration.UserData[NStubConstants.UserDataClassTypeKey] = expected;
            actual = this.testObject.TestObjectType;
            Assert.AreEqual(expected, actual);

            this.mocks.VerifyAll();
        }

        [Test]
        public void PropertyTypeMemberNormalBehavior()
        {
            // Test read access of 'TypeMember' Property.
            this.mocks.ReplayAll();
            var expected = this.typeMember;
            var actual = this.testObject.TypeMember;
            Assert.AreEqual(expected, actual);
            this.mocks.VerifyAll();
        }

        [SetUp]
        public void SetUp()
        {
            this.codeNamespace = new CodeNamespace();
            this.testClassDeclaration = new CodeTypeDeclaration();
            this.typeMember = new CodeTypeMember();

            // MethodInfo methodInfo = this.GetType().GetMethod("SetUp");
            // typeMember.UserData[NStubConstants.TestMemberMethodInfoKey] = methodInfo;
            this.buildData = new BuildDataDictionary();
            this.mocks = new MockRepository();
            this.setUpTearDownContext = this.mocks.StrictMock<ISetupAndTearDownContext>();
            this.testObject = new MemberBuildContext(
                this.codeNamespace, 
                this.testClassDeclaration, 
                this.typeMember, 
                this.buildData, 
                this.setUpTearDownContext, TheKey);
        }

        [TearDown]
        public void TearDown()
        {
            this.testObject = null;
        }

        /// <summary>
        /// Set the user data with key 'MethodMemberInfo' of the specified CodeObject to a method 
        /// from the <see cref="InfoApe"/> class. The MethodInfo resolution is done
        /// through a <see cref="MethodInfo.GetMethod(string)"/> signature.
        /// </summary>
        /// <param name="codeTypeMember">The code type member which gets the <see cref="MethodInfo.UserData"/> 'MethodMemberInfo' set.</param>
        /// <param name="methodSignature">The signature of the desired method.</param>
        private static MethodInfo SetUserData(CodeObject codeTypeMember, string methodSignature)
        {
            MethodInfo methodInfo = typeof(InfoApe).GetMethod(methodSignature);
            codeTypeMember.UserData[NStubConstants.TestMemberMethodInfoKey] = methodInfo;
            return methodInfo;
        }
    }
}