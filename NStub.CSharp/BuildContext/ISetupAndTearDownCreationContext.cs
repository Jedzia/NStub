// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISetupAndTearDownCreationContext.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.BuildContext
{
    using NStub.CSharp.ObjectGeneration;

    /// <summary>
    /// Represents the data used in SetUp and TearDown test-method generation.
    /// </summary>
    public interface ISetupAndTearDownCreationContext : ISetupAndTearDownContext
    {
        #region Properties

        /// <summary>
        /// Gets the build data dictionary that stores generation wide category/key/value properties.
        /// </summary>
        BuildDataDictionary BuildData { get; }

        /// <summary>
        /// Gets the test object member field creator.
        /// </summary>
        /// <remarks>
        /// Contains the test object member field initialization expression ( this.testObject = new Foo( ... ) )
        /// of the test SetUp method.
        /// </remarks>
        ITestObjectComposer TestObjectCreator { get; }

        #endregion
    }
}