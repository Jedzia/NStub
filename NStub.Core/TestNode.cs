// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestNode.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Tree-structure data for the representation of class and member types in an assembly.
    /// </summary>
    public class TestNode
    {
        #region Fields

        private IList<TestNode> nodes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestNode"/> class.
        /// </summary>
        /// <param name="text">The text of the node.</param>
        /// <param name="testNodeType">Type of the test node.</param>
        /// <param name="tagValue">The tag value.</param>
        /// <remarks>
        /// The <paramref name="tagValue"/> is put into the <see cref="TestNode.MethodInfo"/> field as
        /// <see cref="MethodInfo"/> or into the <see cref="ClrType"/>, when it is a <see cref="Type"/>.
        /// </remarks>
        public TestNode(string text, TestNodeType testNodeType, object tagValue)
        {
            Guard.NotNullOrEmpty(() => text, text);

            // this.Checked = @checked;
            this.Checked = true;
            this.Text = text;
            this.TestNodeType = testNodeType;

            if (tagValue is MethodInfo)
            {
                this.MethodInfo = tagValue as MethodInfo;
            }
            else if (tagValue is Type)
            {
                this.ClrType = tagValue as Type;
            }

            // <exception cref="ArgumentOutOfRangeException"><c>tagValue</c> is out of range, it has to be of 
            // <see cref="MethodInfo"/> or <see cref="System.Type"/> type.</exception>
            /*else
            {
                throw new ArgumentOutOfRangeException("tagValue", "The supplied tagValue has to be of MethodInfo or System.Type type.");
            }*/
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestNode"/> class.
        /// </summary>
        internal TestNode()
        {
            this.TestNodeType = TestNodeType.Root;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TestNode"/> is marked to be processed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to processed; otherwise, <c>false</c>.
        /// </value>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets the type of the class.
        /// </summary>
        /// <value>
        /// The type of the class.
        /// </value>
        public Type ClrType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is representing a class.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is representing class; otherwise, <c>false</c>.
        /// </value>
        public bool IsClass
        {
            get
            {
                return this.ClrType != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is representing a method.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is representing a method; otherwise, <c>false</c>.
        /// </value>
        public bool IsMethod
        {
            get
            {
                return this.MethodInfo != null;
            }
        }

        /// <summary>
        /// Gets the method info of the member.
        /// </summary>
        public MethodInfo MethodInfo { get; private set; }

        /// <summary>
        /// Gets the list of child nodes.
        /// </summary>
        public IList<TestNode> Nodes
        {
            get
            {
                return this.nodes ?? (this.nodes = new List<TestNode>());
            }
        }

        /// <summary>
        /// Gets the type of the test node.
        /// </summary>
        /// <value>
        /// The type of the test node.
        /// </value>
        public TestNodeType TestNodeType { get; private set; }

        /// <summary>
        /// Gets the full qualified name of the type.
        /// </summary>
        public string Text { get; internal set; }

        #endregion
    }
}