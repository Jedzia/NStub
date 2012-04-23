namespace NStub.Core.Tests.Util.Dumper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core.Util.Dumper;
    using System.Text;
    using System.Globalization;
    
    
    [TestFixture()]
    public partial class XhtmlWriterTest
    {
        
        private XhtmlWriter testObject;
        private string eventText;
        
        [SetUp()]
        public void SetUp()
        {
            this.testObject = new XhtmlWriter();
            this.eventText = string.Empty;
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void Construct()
        {
            this.testObject = new XhtmlWriter();
        }

        [Test()]
        public void EventTextChangedAddAndRemove()
        {
            //testObject.TextChanged += (o, e) => eventText += e.Text;
            testObject.TextChanged += TestObjectTextChanged;

            var expected = "Text";
            testObject.Write(expected);
            Assert.IsEmpty(eventText);
            testObject.Flush();
            Assert.AreEqual(expected, eventText);

            testObject.TextChanged -= TestObjectTextChanged;
            testObject.Write(expected);
            testObject.Flush();
            Assert.AreEqual(expected, eventText);
        }

        private void TestObjectTextChanged(object sender, TextWrittenEventArgs e)
        {
            this.eventText += e.Text;
        }

        [Test()]
        public void EventTextChangedNewLineFlushBehavior()
        {
            testObject.TextChanged += TestObjectTextChanged;

            var text1 = "Text";
            var expected1 = text1 + testObject.NewLine;
            testObject.Write(text1);
            Assert.IsEmpty(eventText);

            // Check that auto flush-event-call is raised on a written NewLine.
            testObject.Write(testObject.NewLine);
            Assert.AreEqual(expected1, eventText);

            eventText = string.Empty;

            // custom NewLine must behave the same.
            testObject.NewLine = "XYZ";
            text1 = "Text2";
            expected1 = text1 + testObject.NewLine;
            testObject.Write(text1);
            Assert.IsEmpty(eventText);

            testObject.Write(testObject.NewLine);
            Assert.AreEqual(expected1, eventText);
        }

        [Test()]
        public void EventTextChangedNewLineFlushBehaviorWriteChar()
        {
            testObject.TextChanged += TestObjectTextChanged;

            var text1 = "Text";
            var expected1 = text1 + testObject.NewLine;
            testObject.Write(text1);
            Assert.IsEmpty(eventText);

            // Check that auto flush-event-call is raised on a written NewLine by Write(char).
            var newlineChars = testObject.NewLine.ToCharArray();
            foreach (var character in newlineChars)
            {
                if (newlineChars.Length > 1)
                {
                    Assert.IsEmpty(eventText);
                }
                testObject.Write(character);
            }
            Assert.AreEqual(expected1, eventText);
        }

        [Test()]
        public void EventTextChangedNewLineFlushBehaviorWriteCharArray()
        {
            testObject.TextChanged += TestObjectTextChanged;

            var text1 = "Text";
            var expected1 = text1 + testObject.NewLine;
            testObject.Write(text1);
            Assert.IsEmpty(eventText);

            // Check that auto flush-event-call is raised on a written NewLine by Write(char).
            var newlineChars = testObject.NewLine.ToCharArray();
            for (int index = 0; index < newlineChars.Length; index++)
			{
                if (newlineChars.Length > 1)
                {
                    Assert.IsEmpty(eventText);
                }
                testObject.Write(newlineChars, index, 1);
			}
            Assert.AreEqual(expected1, eventText);
        }

        [Test()]
        public void CloseTest()
        {
            var expected = "Bla";
            testObject.Write(expected);
            testObject.Close();

            Assert.Throws<System.ObjectDisposedException>(() => testObject.Write(expected));
            testObject.Close();
            testObject.Dispose();
        }
        
        [Test()]
        public void PropertyEncodingNormalBehavior()
        {
            // Test read access of 'Encoding' Property.
            var expected = Encoding.Unicode;
            var actual = testObject.Encoding;
            Assert.AreEqual(expected.CodePage, actual.CodePage);
        }
        
        [Test()]
        public void PropertyFormatProviderNormalBehavior()
        {
            // Test read access of 'FormatProvider' Property.
            var expected = CultureInfo.CurrentCulture;
            var actual = testObject.FormatProvider;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void DisposeTest()
        {
            var expected = "Bla";
            testObject.Write(expected);
            testObject.Dispose();
            var actual = testObject.ToString();
            Assert.AreEqual(expected, actual);

            Assert.Throws<System.ObjectDisposedException>(() => testObject.Write(expected));
            testObject.Dispose();
        }
        
        [Test()]
        public void FlushTest()
        {
            var expected = "Bla";
            testObject.Write(expected);
            testObject.Flush();
            var actual = testObject.ToString();
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void GetStringBuilderTest()
        {
            var expected = "Bla\r\n and more.";
            testObject.Write(expected);
            var actual = testObject.GetStringBuilder();
            Assert.AreEqual(expected, actual.ToString());
            actual.Append("ABC");
            var actualStr = testObject.ToString();
            Assert.AreEqual(expected + "ABC", actualStr);
        }
        
        [Test()]
        public void PropertyNewLineNormalBehavior()
        {
            // Test read access of 'NewLine' Property.
            var expected = Environment.NewLine;
            var actual = testObject.NewLine;
            Assert.AreEqual(expected, actual);

            // Test write access of 'NewLine' Property.
            expected = "MyNewLine";
            testObject.NewLine = expected;
            actual = testObject.NewLine;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void WriteLineTest()
        {
            var line1 = "The Text";
            var expected = line1 + testObject.NewLine;
            testObject.WriteLine(line1);
            var actual = testObject.ToString();
            Assert.AreEqual(expected, actual);

            var line2 = "Another line.";
            expected = line1 + testObject.NewLine + line2 + testObject.NewLine;
            testObject.WriteLine(line2);
            actual = testObject.ToString();
            Assert.AreEqual(expected, actual);

            var line3 = "The third.";
            expected = line1 + testObject.NewLine + line2 + testObject.NewLine + line3 + "XYZ";
            testObject.NewLine = "XYZ";
            testObject.WriteLine(line3);
            actual = testObject.ToString();
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void WriteTest()
        {
            var expectedStr = "The Text";
            testObject.Write(expectedStr);
            var actual = testObject.ToString();
            Assert.AreEqual(expectedStr, actual);

            testObject.GetStringBuilder().Length = 0;
            expectedStr = "The Text";
            testObject.Write(expectedStr.ToCharArray());
            actual = testObject.ToString();
            Assert.AreEqual(expectedStr, actual);

            testObject.GetStringBuilder().Length = 0;
            var expectedInt = 5;
            testObject.Write(expectedInt);
            actual = testObject.ToString();
            Assert.AreEqual(expectedInt.ToString(CultureInfo.InvariantCulture), actual);

            testObject.GetStringBuilder().Length = 0;
            var expectedChar = 'X';
            testObject.Write(expectedChar);
            actual = testObject.ToString();
            Assert.AreEqual(expectedChar.ToString(CultureInfo.InvariantCulture), actual);

        }
    }
}
