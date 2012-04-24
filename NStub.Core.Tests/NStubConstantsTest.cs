namespace NStub.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core;
    
    
    [TestFixture()]
    public partial class NStubConstantsTest
    {
        [Test]
        public void TestMemberMethodInfoKey()
        {
            var actual = NStubConstants.TestMemberMethodInfoKey;
            Assert.DoesNotContain(actual, "{0}");
        }
        
        [Test]
        public void UserDataClassTypeKey()
        {
            var actual = NStubConstants.UserDataClassTypeKey;
            Assert.DoesNotContain(actual, "{0}");
        }
    }
}
