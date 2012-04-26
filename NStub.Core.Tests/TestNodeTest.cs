namespace NStub.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core;
    using System.Reflection;
    using System.CodeDom;
    using NStub.Core.Tests.Stubs;


    [TestFixture()]
    public partial class TestNodeTest
    {

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

        private Type tagValueClrType;
        private MethodInfo tagValueMethodInfo;
        private NStub.Core.TestNodeType testNodeType;
        private TestNode testObjectClr;
        private TestNode testObjectMethod;
        private string text;

        [SetUp()]
        public void SetUp()
        {
            this.text = "Value of text";
            this.testNodeType = TestNodeType.Root;
            // = Class, ClrType
            this.tagValueClrType = typeof(TestNodeTest);
            // = Method, MethodInfo
            this.tagValueMethodInfo = typeof(InfoApe).GetMethod("PublicVoidMethodVoid");
            this.testObjectClr = new TestNode(this.text, this.testNodeType, this.tagValueClrType);
            this.testObjectMethod = new TestNode(this.text, this.testNodeType, this.tagValueMethodInfo);
        }

        [TearDown()]
        public void TearDown()
        {
            this.testObjectClr = null;
        }

        [Test()]
        public void ConstructWithParametersTextTestNodeTypeTagValueTest()
        {
            this.text = "Value of text";
            this.testObjectClr = new TestNode(this.text, this.testNodeType, this.tagValueClrType);
            Assert.Throws<ArgumentNullException>(() => new TestNode(null, this.testNodeType, this.tagValueClrType));
            Assert.Throws<ArgumentException>(() => new TestNode(string.Empty, this.testNodeType, this.tagValueClrType));

            //Assert.Throws<ArgumentOutOfRangeException>(() => new TestNode(this.text, this.testNodeType, null));
            //Assert.Throws<ArgumentOutOfRangeException>(() => new TestNode(this.text, this.testNodeType, "other Type"));
        }

        [Test()]
        public void PropertyCheckedNormalBehavior()
        {
            // Test read access of 'Checked' Property.
            var expected = true;
            var actual = testObjectClr.Checked;
            Assert.AreEqual(expected, actual);

            // Test write access of 'Checked' Property.
            expected = false;
            testObjectClr.Checked = expected;
            actual = testObjectClr.Checked;
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PropertyClrTypeNormalBehavior()
        {
            // Test read access of 'ClrType' Property.
            var expected = this.tagValueClrType;
            var actual = testObjectClr.ClrType;
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PropertyIsClassNormalBehavior()
        {
            // Test read access of 'IsClass' Property.
            var expected = true;
            var actual = testObjectClr.IsClass;
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = testObjectMethod.IsClass;
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PropertyIsMethodNormalBehavior()
        {
            // Test read access of 'IsMethod' Property.
            var expected = false;
            var actual = testObjectClr.IsMethod;
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = testObjectMethod.IsMethod;
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PropertyMethodInfoNormalBehavior()
        {
            // Test read access of 'MethodInfo' Property.
            MethodInfo expected = null;
            var actual = testObjectClr.MethodInfo;
            Assert.AreEqual(expected, actual);

            expected = this.tagValueMethodInfo;
            actual = testObjectMethod.MethodInfo;
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PropertyNodesNormalBehavior()
        {
            // Test read access of 'Nodes' Property.
            var actual = testObjectClr.Nodes;
            Assert.IsNotNull(actual);
            Assert.IsEmpty(actual);

            var expected = testObjectMethod;
            testObjectClr.Nodes.Add(expected);
            actual = testObjectClr.Nodes;
            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual[0]);
        }

        [Test()]
        public void PropertyNormalBehaviorNodeTypeNormalBehavior()
        {
            // Test read access of 'TestNodeType' Property.
            var expected = this.testNodeType;
            var actual = testObjectClr.TestNodeType;
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PropertyTextNormalBehavior()
        {
            // Test read access of 'Text' Property.
            var expected = this.text;
            var actual = testObjectClr.Text;
            Assert.AreEqual(expected, actual);
        }
    }
}
