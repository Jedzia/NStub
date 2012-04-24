namespace NStub.Core.Tests.Util.Dumper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core.Util.Dumper;
    using System.IO;
    using System.Globalization;
    using System.Collections;
    using System.Linq.Expressions;


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
            Server.Default.LambdaFormatter = this.myConsoleOut;

            object element = "My String";
            var expected = "[String]" + myConsoleOut.NewLine + element.ToString() + myConsoleOut.NewLine;
            ObjectDumper.Write(element);
            var actual = myConsoleOut.ToString();
            Assert.AreEqual(expected, actual);
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump());
            Assert.AreEqual(actual, myConsoleOut.ToString());


            myConsoleOut.GetStringBuilder().Length = 0;
            element = 5.32d;
            expected = "[Double]" + myConsoleOut.NewLine + element.ToString() + myConsoleOut.NewLine;
            ObjectDumper.Write(element, 1);
            actual = myConsoleOut.ToString();
            Assert.AreEqual(expected, actual);
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump(1));
            Assert.AreEqual(actual, myConsoleOut.ToString());

            myConsoleOut.GetStringBuilder().Length = 0;
            element = new CultureInfo("en-US");
            expected = "[CultureInfo]" + myConsoleOut.NewLine + "Parent={ }";
            ObjectDumper.Write(element, 1);
            actual = myConsoleOut.ToString();
            Assert.Contains(actual, expected);
            Assert.Contains(actual, "en-US");
            Assert.Contains(actual, "(United States)");
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump(1));
            Assert.AreEqual(actual, myConsoleOut.ToString());

            myConsoleOut.GetStringBuilder().Length = 0;
            element = new CultureInfo("en-US");
            expected = "[Object[]]" + myConsoleOut.NewLine + "Parent={ }";
            var elementArray = new[] { element, element, element, element };
            ObjectDumper.Write(elementArray, 1, 2);
            actual = myConsoleOut.ToString();
            Assert.Contains(actual, expected);
            Assert.Contains(actual, "en-US");
            Assert.Contains(actual, "maximum Dump Count of [2] reached");
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(elementArray, elementArray.Dump(1, 2));
            Assert.AreEqual(actual, myConsoleOut.ToString());

            var txtwrt = new StringWriter();
            element = new CultureInfo("en-US");
            expected = "[Object[]]" + myConsoleOut.NewLine + "Parent={ }";
            elementArray = new[] { element, element, element, element };
            ObjectDumper.Write(elementArray, 1, 2, txtwrt);
            actual = txtwrt.ToString();
            Assert.Contains(actual, expected);
            Assert.Contains(actual, "en-US");
            Assert.Contains(actual, "maximum Dump Count of [2] reached");

            txtwrt = new StringWriter();
            element = new CultureInfo("en-US");
            expected = "[Object[]]" + myConsoleOut.NewLine + "[CultureInfo]Prefix:Parent={ }";
            elementArray = new[] { element, element, element, element };
            ObjectDumper.Write("Prefix:", elementArray, 1, 12, txtwrt);
            actual = txtwrt.ToString();
            Assert.Contains(actual, expected);
            Assert.Contains(actual, "en-US");
            Assert.Contains(actual, "maximum Dump Count of [12] reached");
        }

        public class NumerableOne : IEnumerable
        {
            public class NumeratorOne : IEnumerator
            {
                string[] values = { "Yes", "No" };
                int counter = 0;
                #region IEnumerator Members

                public IEnumerable Thing
                {
                    get
                    {
                        var result = new NumerableOne();
                        return result;
                    }
                }

                public object Current
                {
                    get
                    {
                        var result = values[counter];
                        return result;
                    }
                }

                public bool MoveNext()
                {
                    if (counter < 2)
                    {
                        counter++;
                    }
                    return counter < 2;
                }

                public void Reset()
                {
                    counter = 0;
                }

                #endregion
            }
            #region IEnumerable Members

            public IEnumerator GetEnumerator()
            {
                return new NumeratorOne();
            }

            #endregion
        }

        public class MyDeep
        {
            public IEnumerable XXX = new NumerableOne();
        }

        [Test()]
        public void WriteMoreTest()
        {
            ObjectDumper.Write(null);
            ObjectDumper.Write(DateTime.Now);

            int[][][] a1 = new int[4][][];
            int[][] b1 = new int[4][];
            int[] c1 = new int[3];
            b1[2] = c1;
            ObjectDumper.Write(a1.ToList(), int.MaxValue);

            var ie = new uint[] { 4, 5, 7 }.AsEnumerable();
            ObjectDumper.Write(ie, int.MaxValue);

            var enumerable = new object[] { "abc", new NumerableOne(), 7 }.AsEnumerable();
            ObjectDumper.Write(enumerable, int.MaxValue);

            ObjectDumper.Write(new List<NumerableOne>() { new NumerableOne() }.Select((e) => new { e, Nose = "xxx" }).AsEnumerable(), int.MaxValue/*, int.MaxValue, this.myConsoleOut*/);

            ObjectDumper.Write(new List<object>() { 5, null, DateTime.Now, new NumerableOne() }, int.MaxValue);
            ObjectDumper.Write(new List<int>() { 1, 2, 3, 4, 5, 6 }, 5);
            ObjectDumper.Write(new List<List<int>>() { new List<int>() { 1, 2, 3, 4, 5, 6 }, new List<int>() { 11, 12, 13, 14, 15, 16 } }, 5);

            Gallio.Framework.TestLog.DebugTrace.Write(this.myConsoleOut.ToString());
        }

        [Test()]
        public void WriteProblematic()
        {
            ObjectDumper.Write(typeof(ObjectDumper).GetMethods(), 2);
            Assert.Contains(myConsoleOut.ToString(), "Name=Write");

            ObjectDumper.Write(new MyDeep(), 2);
        }

        public enum MyEnum { ValueOne, ValueTwo }

        public class ValEnum
        {
            public MyEnum TheEnum = MyEnum.ValueOne;
        }

        public struct ValOne : IEnumerable
        {
            public ValueType TheValue;
            public IEnumerable<MyEnum> TheEnum;

            public ValOne(int x)
            {
                TheValue = x;
                TheEnum = new[] { MyEnum.ValueOne, MyEnum.ValueTwo };
            }

            public IEnumerator GetEnumerator()
            {
                return new NumerableOne.NumeratorOne();
            }
        }

        public class ValTwo
        {
            public ValueType MyValue;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:ValTwo"/> class.
            /// </summary>
            public ValTwo(int x)
            {
                MyValue = new ValOne(x);
            }
        }

        [Test()]
        public void WriteValueTypes()
        {
            ObjectDumper.Write(new ValTwo(12345), 2);

            var val1 = MyEnum.ValueTwo;
            ObjectDumper.Write(val1, 2);

            var valIe = new object[] { MyEnum.ValueTwo, MyEnum.ValueOne };
            ObjectDumper.Write(valIe, 2);

            ObjectDumper.Write(new ValEnum(), 2);

            ObjectDumper.Write(new ValOne(12345), 2);


            //Assert.Contains(myConsoleOut.ToString(), "Name=Write");

            //ObjectDumper.Write(new MyDeep(), 2);
        }

        [Test()]
        public void WriteBitArray()
        {

            Server.Default.LambdaFormatter = this.myConsoleOut;

            var ba = new BitArray(8);
            ba.Set(3, true);
            ba.Set(5, true);

            var element = new object[] { ba, new { My=234, TH=ba, EN=MyEnum.ValueTwo } }.AsEnumerable();
            Assert.AreSame(element, element.Dump(5));
            //Assert.AreEqual("[BitArray]\r\nFalse\r\nFalse\r\nFalse\r\nTrue\r\nFalse\r\nTrue\r\nFalse\r\nFalse\r\n", myConsoleOut.ToString());
            Assert.Contains(myConsoleOut.ToString(), "False\r\n  False\r\n  False\r\n  True\r\n  False\r\n  True\r\n  False\r\n  False");
            //Assert.Contains(myConsoleOut.ToString(), "System.Char[]");


            //Assert.Contains(myConsoleOut.ToString(), "Name=Write");

            //ObjectDumper.Write(new MyDeep(), 2);
        }


        [Test()]
        public void ExtensionDumpMaxMinValue()
        {
            Server.Default.LambdaFormatter = this.myConsoleOut;

            object element = "My String";
            var expected = "[String]" + myConsoleOut.NewLine + element.ToString() + myConsoleOut.NewLine;
            ObjectDumper.Write(element, 21);
            var actual = myConsoleOut.ToString();
            Assert.AreEqual(expected, actual);
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump("Description", 21));
            Assert.Contains(myConsoleOut.ToString(), element.ToString());
            Assert.Contains(myConsoleOut.ToString(), "Description");
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump("Description", -1));
            Assert.Contains(myConsoleOut.ToString(), element.ToString());
            Assert.Contains(myConsoleOut.ToString(), "Description");
        }

        [Test()]
        public void ExtensionDumpWithDescription()
        {
            Server.Default.LambdaFormatter = this.myConsoleOut;

            object element = "My String";
            var expected = "[String]" + myConsoleOut.NewLine + element.ToString() + myConsoleOut.NewLine;
            ObjectDumper.Write(element);
            var actual = myConsoleOut.ToString();
            Assert.AreEqual(expected, actual);
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump("Description"));
            Assert.Contains(myConsoleOut.ToString(), element.ToString());
            Assert.Contains(myConsoleOut.ToString(), "Description");
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump("Description", -1, -1));
            Assert.Contains(myConsoleOut.ToString(), "maximum Dump Count of [0] reached");


            myConsoleOut.GetStringBuilder().Length = 0;
            element = 5.32d;
            expected = "[Double]" + myConsoleOut.NewLine + element.ToString() + myConsoleOut.NewLine;
            ObjectDumper.Write(element, 1);
            actual = myConsoleOut.ToString();
            Assert.AreEqual(expected, actual);
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump("Description", 1));
            Assert.Contains(myConsoleOut.ToString(), element.ToString());
            Assert.Contains(myConsoleOut.ToString(), "Description");

            myConsoleOut.GetStringBuilder().Length = 0;
            element = new CultureInfo("en-US");
            expected = "[Object[]]" + myConsoleOut.NewLine + "Parent={ }";
            var elementArray = new[] { element, element, element, element };
            ObjectDumper.Write(elementArray, 1, 2);
            actual = myConsoleOut.ToString();
            Assert.Contains(actual, expected);
            Assert.Contains(actual, "en-US");
            Assert.Contains(actual, "maximum Dump Count of [2] reached");
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump("Description", 1, 2));
            Assert.Contains(myConsoleOut.ToString(), element.ToString());
            Assert.Contains(myConsoleOut.ToString(), "Description");
            Assert.Contains(myConsoleOut.ToString(), "maximum Dump Count of [2] reached");

            myConsoleOut.GetStringBuilder().Length = 0;
            element = new CultureInfo("en-US");
            expected = "[Object[]]" + myConsoleOut.NewLine + "[CultureInfo]Prefix:" + "Parent={ }";
            elementArray = new[] { element, element, element, element };
            ObjectDumper.Write("Prefix:", elementArray, 1, 2, this.myConsoleOut);
            actual = myConsoleOut.ToString();
            Assert.Contains(actual, expected);
            Assert.Contains(actual, "en-US");
            Assert.Contains(actual, "maximum Dump Count of [2] reached");
            myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump("Prefix:", "Description", 1, 2));
            Assert.Contains(myConsoleOut.ToString(), element.ToString());
            Assert.Contains(myConsoleOut.ToString(), "Description");
            Assert.Contains(myConsoleOut.ToString(), "maximum Dump Count of [2] reached");

        }

        [Test()]
        public void WriteWithIQueriable()
        {
            var elements = new object[] { "A", 22.4d, "C", 6, new DateTime(2012, 4, 23) }.AsQueryable();
            var expected = "[EnumerableQuery`1]\r\nA\r\n22,4\r\nC\r\n6\r\n23.04.2012\r\n";
            ObjectDumper.Write(elements);
            var actual = myConsoleOut.ToString();
            Assert.AreEqual(expected, actual);

            myConsoleOut.GetStringBuilder().Length = 0;
            elements = new object[] { "A", 22.4d, "C", 6, new DateTime(2012, 4, 23) }.AsQueryable();
            expected = "[EnumerableQuery`1]\r\nA\r\n22,4\r\nC\r\n6\r\n23.04.2012\r\n";
            ObjectDumper.Write(elements, int.MaxValue);
            actual = myConsoleOut.ToString();
            Assert.AreEqual(expected, actual);
            myConsoleOut.GetStringBuilder().Length = 0;

            myConsoleOut.GetStringBuilder().Length = 0;
            elements = new object[] { new object[] { 4, 44.3f, "A" }.AsEnumerable(), 22.4d, "C", 6, new DateTime(2012, 4, 23) }.AsQueryable();
            expected = "[EnumerableQuery`1]\r\n...\r\n  4\r\n  44,3\r\n  A\r\n22,4\r\nC\r\n6\r\n23.04.2012\r\n";
            ObjectDumper.Write(elements, int.MaxValue);
            actual = myConsoleOut.ToString();
            Assert.AreEqual(expected, actual);
            myConsoleOut.GetStringBuilder().Length = 0;

        }

        [Test()]
        public void ExtensionDumpIQueryable()
        {
            Server.Default.LambdaFormatter = this.myConsoleOut;

            var element = "My String".ToCharArray().AsQueryable();
            //var expected = "[String]" + myConsoleOut.NewLine + element.ToString() + myConsoleOut.NewLine;
            //myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(element, element.Dump(2));
            Assert.Contains(myConsoleOut.ToString(), element.ToString());
            Assert.Contains(myConsoleOut.ToString(), "System.Char[]");
        }

        [Test()]
        public void ExtensionDumpExpression()
        {
            Server.Default.LambdaFormatter = this.myConsoleOut;

            ConstantExpression constExpr = Expression.Constant(
                Expression.Constant(42)
                );

            //var expected = "[String]" + myConsoleOut.NewLine + element.ToString() + myConsoleOut.NewLine;
            //myConsoleOut.GetStringBuilder().Length = 0;
            Assert.AreSame(constExpr, constExpr.Dump("Hello Dump", 2));
            Assert.Contains(constExpr.ToString(), constExpr.ToString());
            Assert.Contains(myConsoleOut.ToString(), "NodeType=Constant");
        }
        public static void ThrowIt()
        {
            throw new ApplicationException("Boo");
        }

        [Test()]
        public void ExtensionDumpExpressionThrows()
        {
            Server.Default.LambdaFormatter = this.myConsoleOut;

            int n = 0;
            Expression<Func<bool>> returnValue;
            returnValue = () => 5 == (9 / n);

            Assert.AreSame(returnValue, returnValue.Dump("Hello Dump", 2));
            Assert.Contains(returnValue.ToString(), returnValue.ToString());
            Assert.Contains(myConsoleOut.ToString(), "Dump ExpressionToken Visit \r\nSystem.InvalidOperationException");
            Assert.Contains(myConsoleOut.ToString(), "at NStub.Core.Util.Dumper.ExpressionToken.Visit");
            Assert.Contains(myConsoleOut.ToString(), "NodeType=Constant");

            //var expected = "[String]" + myConsoleOut.NewLine + element.ToString() + myConsoleOut.NewLine;
            //myConsoleOut.GetStringBuilder().Length = 0;

            ConstantExpression constExpr = Expression.Constant(
                //Expression.Call(typeof(ObjectDumperTest), "ThrowIt", null, null)
                Expression.Lambda(returnValue)
                );
            Assert.AreSame(constExpr, constExpr.Dump("Hello Dump", 2));
            Assert.Contains(constExpr.ToString(), constExpr.ToString());
            //Assert.Contains(returnValue.ToString(), "asdasd");
            Assert.Contains(myConsoleOut.ToString(), "NodeType=Constant");
        }

    }
}
