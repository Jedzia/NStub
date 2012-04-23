namespace NStub.Core.Tests.Util.Dumper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core.Util.Dumper;
    
    
    [TestFixture()]
    public partial class ServerTest
    {
        
        private Server testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.testObject = Server.Default;
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void EventTextChangedAddAndRemove()
        {
            string eventText = string.Empty;
            Action<object, TextWrittenEventArgs> body = (o, e) => eventText += e.Text;
            
            var formatter = new XhtmlWriter();
            Server.Default.LambdaFormatter = formatter;
            testObject.TextChanged += new EventHandler<TextWrittenEventArgs>(body);
            var text = "Buh";
            var expected = text + formatter.NewLine;
            formatter.WriteLine(text);
            Assert.AreEqual(expected, eventText);

            testObject.TextChanged -= new EventHandler<TextWrittenEventArgs>(body);
            formatter.WriteLine(text);
            Assert.AreEqual(expected, eventText);
        }
        
        [Test()]
        public void PropertyDefaultNormalBehavior()
        {
            // Test read access of 'Default' Property.
            var actual = Server.Default;
            Assert.IsNotNull(actual);
        }
        
        [Test()]
        public void PropertyLambdaFormatterNormalBehavior()
        {
            // Test read access of 'LambdaFormatter' Property.
            var actual = testObject.LambdaFormatter;
            Assert.IsNotNull(actual);

            // Test write access of 'LambdaFormatter' Property.
            var expected = new XhtmlWriter();
            testObject.LambdaFormatter = expected;
            actual = testObject.LambdaFormatter;
            Assert.AreSame(expected, actual);
        }
        
        [Test()]
        public void ToConsoleOutTest()
        {
            var expected = Console.Out;
            Server.ToConsoleOut();
            var actual = testObject.LambdaFormatter;
            Assert.AreSame(expected, actual);
        }
    }
}
