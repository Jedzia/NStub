// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodBuilder.cs" company="EvePanix">
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
    /// Test method generator for method type members.
    /// </summary>
    public class MethodBuilder : MemberBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public MethodBuilder(IMemberBuildContext context)
            : base(context)
        {
        }

        #endregion

        /// <summary>
        /// Builds the specified context.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// <c>true</c> on success.
        /// </returns>
        protected override bool BuildMember(IMemberBuildContext context)
        {
            return true;
        }

        /// <summary>
        /// Determines the name of the test method.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// The name of the test method.
        /// </returns>
        protected override string DetermineTestName(IMemberBuildContext context)
        {
            return context.TypeMember.Name;
        }
    }
}