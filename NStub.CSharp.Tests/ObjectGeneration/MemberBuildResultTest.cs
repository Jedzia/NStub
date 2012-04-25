namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using System.CodeDom;
    
    
    [TestFixture()]
    public partial class MemberBuildResultTest
    {
        private MemberBuildResult testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.testObject = new MemberBuildResult();
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersTest()
        {
            this.testObject = new MemberBuildResult();
        }
        
        [Test()]
        public void PropertyClassMethodsToAddNormalBehavior()
        {
            // Test read access of 'ClassMethodsToAdd' Property.
            var actual = testObject.ClassMethodsToAdd;
            Assert.IsEmpty(actual);
            Assert.IsNotNull(actual);
        }
        
        [Test()]
        public void PropertyExcludeMemberNormalBehavior()
        {
            // Test read access of 'ExcludeMember' Property.
            var expected = false;
            var actual = testObject.ExcludeMember;
            Assert.AreEqual(expected, actual);

            // Test write access of 'ExcludeMember' Property.
            expected = true;
            testObject.ExcludeMember = expected;
            actual = testObject.ExcludeMember;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void ResetTest()
        {
            testObject.ExcludeMember = true;
            testObject.ClassMethodsToAdd.Add(new CodeMemberMethod());
            Assert.IsNotEmpty(testObject.ClassMethodsToAdd);
            testObject.Reset();
            Assert.AreEqual(false, testObject.ExcludeMember);
            Assert.IsEmpty(testObject.ClassMethodsToAdd);
            Assert.IsNotNull(testObject.ClassMethodsToAdd);
        }
    }
}
