namespace NStub.CSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp;
    
    
    [TestFixture()]
    public partial class ExceptionsTest
    {
        
        [Test()]
        public void StringContent()
        {
            Assert.IsNotEmpty(Exceptions.DirectoryCannotBeFound);
            Assert.IsNotEmpty(Exceptions.ParameterCannotBeNull);
            Assert.IsNotEmpty(Exceptions.StringCannotBeEmpty);
        }
    }
}
