namespace NStub.Core.Tests.Util.Dumper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core.Util.Dumper;
using System.IO;
    using System.Globalization;
    
    
    [TestFixture()]
    public partial class ObjectDumperTest
    {

        private TextWriter storedConsoleOut;
        private StringWriter myConsoleOut;
        
        [SetUp()]
        public void SetUp()
        {
            storedConsoleOut = Console.Out;
            myConsoleOut = new StringWriter();
            Console.SetOut(myConsoleOut);
        }
        
        [TearDown()]
        public void TearDown()
        {
            Console.SetOut(storedConsoleOut);
        }
        
        [Test()]
        public void WriteTest()
        {
            //var formatter = new XhtmlWriter();
            //Server.Default.LambdaFormatter = formatter;

            object element = "Use a better implementation of Object selector";
            var expected = "[String]";
            ObjectDumper.Write(element);
            var actual = myConsoleOut.ToString();
            Assert.AreEqual(expected, actual);

            myConsoleOut.GetStringBuilder().Length = 0;
            element = 5.32d;
            expected = "[Double]" + element.ToString() + myConsoleOut.NewLine;
            ObjectDumper.Write(element, 1);
            actual = myConsoleOut.ToString();
            Assert.AreEqual(expected, actual);

            myConsoleOut.GetStringBuilder().Length = 0;
            element = new CultureInfo("en-US");
            expected = "[CultureInfo]Parent={ }";
            ObjectDumper.Write(element, 1);
            actual = myConsoleOut.ToString();
            Assert.Contains(actual, expected);
            Assert.Contains(actual, "en-US");
            Assert.Contains(actual, "(United States)");

            myConsoleOut.GetStringBuilder().Length = 0;
            element = new CultureInfo("en-US");
            expected = "[Object[]]Parent={ }";
            ObjectDumper.Write(new[] { element, element, element, element }, 1, 2);
            actual = myConsoleOut.ToString();
            Assert.Contains(actual, expected);
            Assert.Contains(actual, "en-US");
            Assert.Contains(actual, "maximum Dump Count of [2] reached");

            var txtwrt = new StringWriter();
            element = new CultureInfo("en-US");
            expected = "[Object[]]Parent={ }";
            ObjectDumper.Write(new[] { element, element, element, element }, 1, 2, txtwrt);
            actual = txtwrt.ToString();
            Assert.Contains(actual, expected);
            Assert.Contains(actual, "en-US");
            Assert.Contains(actual, "maximum Dump Count of [2] reached");
        }

        [Test()]
        public void WriteMoreTest()
        {
            ObjectDumper.Write(null);
            ObjectDumper.Write(DateTime.Now);
            ObjectDumper.Write(new List<object>() { 5, null });
            ObjectDumper.Write(new List<int>() { 1, 2, 3, 4, 5, 6 });
            ObjectDumper.Write(new List<List<int>>() { new List<int>() { 1, 2, 3, 4, 5, 6 }, new List<int>() { 11, 12, 13, 14, 15, 16 } });
        }
}
}
