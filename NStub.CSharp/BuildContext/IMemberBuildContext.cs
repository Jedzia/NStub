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
    using NStub.CSharp.ObjectGeneration;

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
        /// <returns>The builder data with the <see cref="TestKey"/> or <c>null</c> if nothing is found.</returns>
        IBuilderData GetBuilderData(string category);

        T GetBuilderData<T>(IMemberBuilder builder) where T : class, IBuilderData;

        /// <summary>
        /// Gets the data specific to SetUp and TearDown test-methods.
        /// </summary>
        /// <remarks>
        /// Contains the SetUp and TearDown initialization.
        /// </remarks>
        ISetupAndTearDownContext SetUpTearDownContext { get; }

    }
}