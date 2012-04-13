// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberBuildContext.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.BuildContext
{
    using System.CodeDom;
    using NStub.CSharp.ObjectGeneration;

    /// <summary>
    /// Represents the data used to create new unit tests.
    /// </summary>
    public interface IMemberBuildContext
    {
        #region Properties

        /// <summary>
        /// Gets the code namespace of the test.
        /// </summary>
        CodeNamespace CodeNamespace { get; }

        /// <summary>
        /// Gets test class declaration.( early testObject ).
        /// </summary>
        CodeTypeDeclaration TestClassDeclaration { get; }

        /// <summary>
        /// Gets a reference to the test SetUp method.
        /// </summary>
        CodeTypeMember TypeMember { get; }

        /// <summary>
        /// Gets a reference to the test TearDown method.
        /// </summary>
        CodeMemberMethod TearDownMethod { get; }

        /// <summary>
        /// Gets the test object member field creator.
        /// </summary>
        /// <remarks>
        /// Contains the test object member field initialization expression ( this.testObject = new Foo( ... ) )
        /// of the test SetUp method.
        /// </remarks>
        ITestObjectBuilder TestObjectCreator { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is a property.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a property; otherwise, <c>false</c>.
        /// </value>
        bool IsProperty { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is an event.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is an event; otherwise, <c>false</c>.
        /// </value>
        bool IsEvent { get; }

        #endregion
    }
}