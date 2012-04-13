// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuildHandler.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Checks if a registered <see cref="IMemberBuilder"/> can handle a <see cref="IMemberBuildContext"/> request.
    /// </summary>
    public interface IBuildHandler
    {
        #region Properties

        /// <summary>
        /// Gets the handler, that checks, if the associated <see cref="Type"/> can process an <see cref="IMemberBuildContext"/> arbeit Todo: translate.
        /// </summary>
        Func<IMemberBuildContext, bool> Handler { get; }

        /// <summary>
        /// Gets the associated <see cref="IMemberBuilder"/> type.
        /// </summary>
        Type Type { get; }

        #endregion
    }
}