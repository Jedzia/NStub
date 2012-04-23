namespace NStub.Core.Tests.Util.Dumper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core.Util.Dumper;
    
    
    [TestFixture()]
    public partial class HeadingPresenterTest
    {
        
        private object content;
        private string heading;
        private HeadingPresenter testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.heading = "Value of heading";
            this.content = "Value of content";
            this.testObject = new HeadingPresenter(this.heading, this.content);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersHeadingContentTest()
        {
            this.heading = "Value of heading";
            this.content = "Value of content";
            this.testObject = new HeadingPresenter(this.heading, this.content);
            this.testObject = new HeadingPresenter(null, this.content);
            this.testObject = new HeadingPresenter(this.heading, null);
            this.testObject = new HeadingPresenter(null, null);
        }
        
        [Test()]
        public void PropertyHeadingNormalBehavior()
        {
            // Test read access of 'Heading' Property.
            var expected = this.heading;
            var actual = testObject.Heading;
            Assert.AreEqual(expected, actual);

            // Test write access of 'Heading' Property.
            expected = "Insert setter object here";
            testObject.Heading = expected;
            actual = testObject.Heading;
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void FieldContentNormalBehavior()
        {
            // Test read access of 'Heading' Property.
            var expected = this.content;
            var actual = testObject.Content;
            Assert.AreEqual(expected, actual);

            // Test write access of 'Heading' Property.
            expected = "Insert setter object here";
            testObject.Heading = expected;
            actual = testObject.Heading;
            Assert.AreEqual(expected, actual);
        }

    }
}
