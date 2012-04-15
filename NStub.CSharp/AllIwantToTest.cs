using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NStub.CSharp.ObjectGeneration;

namespace NStub.CSharp
{
    public class AllIwantToTest
    {
        public event EventHandler PublicEventObject;

        public int MyInt { get; set; }
        public string MyString { get; set; }
        public bool MyBool { get; set; }
        public short MyShort { get; set; }
        public Type MyType { get; set; }
        public IBuildHandler MyHandler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AllIwantToTest"/> class.
        /// </summary>
        public AllIwantToTest( bool myBool)
        {

        }
       
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AllIwantToTest"/> class.
        /// </summary>
        public AllIwantToTest(int myInt, string myString, bool myBool)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AllIwantToTest"/> class.
        /// </summary>
        public AllIwantToTest( short myShort, Type myType, IBuildHandler myHandler)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AllIwantToTest"/> class.
        /// </summary>
        public AllIwantToTest(int myInt, string myString, bool myBool, short myShort, Type myType, IBuildHandler myHandler)
        {
            
        }
    }
}
