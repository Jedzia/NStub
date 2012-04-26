// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodeGeneratorSetup.cs" company="EvePanix">
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

    /// <summary>
    /// Represents the setup data for a <see cref="ICodeGenerator"/>.
    /// </summary>
    public interface ICodeGeneratorSetup : /*ICodeGeneratorSetupBase,*/ ICloneable
    {
        /*}

    public interface ICodeGeneratorSetupBase 
    {*/
        #region Properties

        /// <summary>
        /// Gets or sets the method generators level of detail .
        /// </summary>
        /// <value>
        /// The method generators level of detail.
        /// </value>
        MemberVisibility MethodGeneratorLevelOfDetail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to produce setup and tear down methods.
        /// </summary>
        /// <value>
        /// <c>true</c> if [use setup and tear down]; otherwise, <c>false</c>.
        /// </value>
        bool UseSetupAndTearDown { get; set; }

        #endregion
    }
}