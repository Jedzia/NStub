// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITestBuilderFactory.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System.Collections.Generic;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Provides access to builders used in test method generation.
    /// </summary>
    public interface ITestBuilderFactory
    {
        /*/// <summary>
        /// Gets the event builder.
        /// </summary>
        EventBuilder EventBuilder { get; }

        /// <summary>
        /// Gets the method builder.
        /// </summary>
        MethodBuilder MethodBuilder { get; }

        /// <summary>
        /// Gets the property builder.
        /// </summary>
        PropertyBuilder PropertyBuilder { get; }*/

        /// <summary>
        /// Tries to get the builder for the specified context.
        /// </summary>
        /// <param name="context">The context of the current test object.</param>
        /// <returns>A list of member builders that can handle the request or an <c>empty</c> list if no one can be found.</returns>
        IEnumerable<IMemberBuilder> GetBuilder(IMemberBuildContext context);
    }
}