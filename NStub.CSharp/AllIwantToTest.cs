using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NStub.CSharp.ObjectGeneration;

namespace NStub.CSharp
{
    /// <summary>
    /// For testing purposes. Todo: DeleteME after this project is done *g*
    /// </summary>
    public class AllIwantToTest
    {
        /// <summary>
        /// Occurs when [public event object].
        /// </summary>
        public event EventHandler PublicEventObject;

        /// <summary>
        /// Gets or sets my int.
        /// </summary>
        /// <value>
        /// My int.
        /// </value>
        public int MyInt { get; set; }
       
        /// <summary>
        /// Gets or sets my string.
        /// </summary>
        /// <value>
        /// My string.
        /// </value>
        public string MyString { get; set; }
       
        /// <summary>
        /// Gets or sets a value indicating whether [my bool].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [my bool]; otherwise, <c>false</c>.
        /// </value>
        public bool MyBool { get; set; }

        /// <summary>
        /// Gets or sets my short.
        /// </summary>
        /// <value>
        /// My short.
        /// </value>
        public short MyShort { get; set; }

        /// <summary>
        /// Gets or sets my type.
        /// </summary>
        /// <value>
        /// My type.
        /// </value>
        public Type MyType { get; set; }

        /// <summary>
        /// Gets or sets my handler.
        /// </summary>
        /// <value>
        /// My handler.
        /// </value>
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
            if (this.PublicEventObject != null)
            {
            }
        }
    }
}
