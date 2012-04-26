// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllIwantToTest.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp
{
    using System;
    using NStub.CSharp.ObjectGeneration;
    using System.Collections.Generic;

    /// <summary>
    /// For testing purposes. Todo: DeleteME after this project is done *g*
    /// </summary>
    public class AllIwantToTest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AllIwantToTest"/> class.
        /// </summary>
        public AllIwantToTest(IEnumerable<string> myIe)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AllIwantToTest"/> class.
        /// </summary>
        public AllIwantToTest(bool myBool)
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
        public AllIwantToTest(short myShort, Type myType, IBuildHandler myHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AllIwantToTest"/> class.
        /// </summary>
        public AllIwantToTest(
            int myInt, string myString, bool myBool, short myShort, Type myType, IBuildHandler myHandler)
        {
            if (this.PublicEventObject != null)
            {
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [public event object].
        /// </summary>
        public event EventHandler PublicEventObject;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [my bool].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [my bool]; otherwise, <c>false</c>.
        /// </value>
        public bool MyBool { get; set; }

        /// <summary>
        /// Gets or sets my handler.
        /// </summary>
        /// <value>
        /// My handler.
        /// </value>
        public IBuildHandler MyHandler { get; set; }

        /// <summary>
        /// Gets or sets my int.
        /// </summary>
        /// <value>
        /// My int.
        /// </value>
        public int MyInt { get; set; }

        /// <summary>
        /// Gets or sets my short.
        /// </summary>
        /// <value>
        /// My short.
        /// </value>
        public short MyShort { get; set; }

        /// <summary>
        /// Gets or sets my static string.
        /// </summary>
        /// <value>
        /// My static string.
        /// </value>
        public static string MyStaticString { get; set; }

        /// <summary>
        /// Gets or sets my string.
        /// </summary>
        /// <value>
        /// My string.
        /// </value>
        public string MyString { get; set; }

        /// <summary>
        /// Gets or sets my type.
        /// </summary>
        /// <value>
        /// My type.
        /// </value>
        public Type MyType { get; set; }

        #endregion
    }
}