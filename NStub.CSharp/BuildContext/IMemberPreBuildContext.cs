// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberPreBuildContext.cs" company="EvePanix">
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
    /// Represents the data used to setup unit tests in the pre-build phase.
    /// </summary>
    public interface IMemberPreBuildContext : IMemberSetupContext
    {
        #region Properties

        /// <summary>
        /// Gets the build result.
        /// </summary>
        IMemberPreBuildResult PreBuildResult { get; }

        #endregion
    }
}