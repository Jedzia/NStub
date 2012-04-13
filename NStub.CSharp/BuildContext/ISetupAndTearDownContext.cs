// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISetupAndTearDownContext.cs" company="EvePanix">
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
    /// Represents the data used in SetUp and TearDown test-method generation.
    /// </summary>
    public interface ISetupAndTearDownCreationContext : ISetupAndTearDownContext
    {
        /// <summary>
        /// Gets the build data dictionary that stores generation wide category/key/value properties.
        /// </summary>
        BuildDataCollection BuildData { get; }

        /// <summary>
        /// Gets the test object member field creator.
        /// </summary>
        /// <remarks>
        /// Contains the test object member field initialization expression ( this.testObject = new Foo( ... ) )
        /// of the test SetUp method.
        /// </remarks>
        ITestObjectBuilder TestObjectCreator { get; }
    }

    /// <summary>
    /// Stores data used by SetUp and TearDown test-methods.
    /// </summary>
    public interface ISetupAndTearDownContext 
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
        CodeMemberMethod SetUpMethod { get; }

        /// <summary>
        /// Gets a reference to the test TearDown method.
        /// </summary>
        CodeMemberMethod TearDownMethod { get; }

        #endregion
    }
}