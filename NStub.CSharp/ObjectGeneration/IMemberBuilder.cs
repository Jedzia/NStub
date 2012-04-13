// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberBuilder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Implements a test method generator for type members.
    /// </summary>
    public interface IMemberBuilder
    {
        /// <summary>
        /// Builds the specified context.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns><c>true</c> on success.</returns>
        bool Build(IMemberBuildContext context);

        /// <summary>
        /// Determines the name of the test method.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <param name="originalName">The initial name of the test method member.</param>
        /// <returns>
        /// The name of the test method.
        /// </returns>
        string GetTestName(IMemberBuildContext context, string originalName);

    }
}