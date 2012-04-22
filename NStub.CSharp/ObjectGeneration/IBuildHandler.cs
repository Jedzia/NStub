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
        /// Gets a value indicating whether this instance is a multi builder type.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a multi builder type; otherwise, <c>false</c>.
        /// </value>
        bool IsMultiBuilder { get; }

        /// <summary>
        /// Gets the handler, that checks, if the associated <see cref="Type"/> can process an <see cref="IMemberBuildContext"/> assignment.
        /// </summary>
        Func<IMemberBuildContext, bool> CanHandle { get; }

        /// <summary>
        /// Gets the associated <see cref="IMemberBuilder"/> type that can handle the request specified in the <see cref="CanHandle"/> method.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the type of the parameter data.
        /// </summary>
        /// <value>
        /// The type of the parameter data.
        /// </value>
        Type ParameterDataType { get; }

        /// <summary>
        /// Gets the description of the builder.
        /// </summary>
        string Description { get; }

        #endregion

        /// <summary>
        /// Creates a new instance of the <see cref="IMemberBuilder"/> specified in the <see cref="Type"/> property with 
        /// the specified context data.
        /// </summary>
        /// <param name="context">The context of the current test object.</param>
        /// <returns>
        /// A test member builder that can handle the request specified in the <see cref="CanHandle"/> method.
        /// </returns>
        IMemberBuilder CreateInstance(IMemberBuildContext context);
    }
}