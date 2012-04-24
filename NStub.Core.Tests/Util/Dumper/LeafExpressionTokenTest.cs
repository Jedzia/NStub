namespace NStub.Core.Tests.Util.Dumper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core.Util.Dumper;
    using System.Text;
    
    
    [TestFixture()]
    public partial class LeafExpressionTokenTest
    {
        
        private LeafExpressionToken testObject;
        private string text;
        
        [SetUp()]
        public void SetUp()
        {
            this.text = "Value of text";
            this.testObject = new LeafExpressionToken(this.text);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersTextTest()
        {
            this.text = "Value of text";
            this.testObject = new LeafExpressionToken(this.text);
            this.testObject = new LeafExpressionToken(null);
        }
        
        [Test()]
        public void PropertyLengthNormalBehavior()
        {
            // Test read access of 'Length' Property.
            var expected = this.text.Length;
            var actual = testObject.Length;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyMultiLineNormalBehavior()
        {
            // Test read access of 'MultiLine' Property.
            var expected = false;
            var actual = testObject.MultiLine;
            Assert.AreEqual(expected, actual);

            // Test write access of 'MultiLine' Property.
            expected = false;
            testObject.MultiLine = true;
            actual = testObject.MultiLine;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void WriteTest()
        {
            var sb = new StringBuilder();
            var expected = this.text;
            testObject.Write(sb, 0);
            var actual = sb.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
