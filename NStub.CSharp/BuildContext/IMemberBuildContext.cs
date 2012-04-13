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
    using System;
    using System.Reflection;

    /// <summary>
    /// Represents the data used to create new unit tests.
    /// </summary>
    public interface IMemberBuildContext : IMemberSetupContext
    {
        /// <summary>
        /// Gets or sets the key associated with the test.
        /// </summary>
        /// <value>
        /// The key associated with the test.
        /// </value>
        string TestKey { get; }

        /// <summary>
        /// Gets the builder data specific to this builders key.
        /// </summary>
        /// <param name="category">Name of the category to request.</param>
        /// <returns>The builder data with the <see cref="Context.TestKey"/> or <c>null</c> if nothing is found.</returns>
        IBuilderData GetBuilderData(string category);
    }

    /// <summary>
    /// Represents the data used to setup new unit tests.
    /// </summary>
    public interface IMemberSetupContext
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
        /// Contains information about the build members in a dictionary form.
        /// </summary>
        BuildDataCollection BuildData { get; }

        /// <summary>
        /// Gets the test object member field creator.
        /// </summary>
        /// <remarks>
        /// Contains the test object member field initialization expression ( this.testObject = new Foo( ... ) )
        /// of the test SetUp method.
        /// </remarks>
        [Obsolete("This is not defined, only for testing purposes.")]
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

        /// <summary>
        /// Gets the member info about the current test method.
        /// </summary>
        MethodInfo MemberInfo { get; }

        /// <summary>
        /// Gets type of the object under test.
        /// </summary>
        Type TestObjectType { get; }

        #endregion
    }
}