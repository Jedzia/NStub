// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultMethodEraser.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    using System.Linq;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Test method generator for static method members.
    /// </summary>
    public class DefaultMethodEraser : MemberBuilder
    {
        #region Fields

        private static readonly string[] TestsToRemove = new[] { "Equals", "GetType", "GetHashCode", "ToString", };

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMethodEraser"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public DefaultMethodEraser(IMemberSetupContext context)
            : base(context)
        {
        }

        #endregion

        /// <summary>
        /// Determines whether this instance can handle a specified build context.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// <c>true</c> if this instance can handle the specified context; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanHandleContext(IMemberBuildContext context)
        {
            return !context.IsConstructor && !context.IsProperty && !context.IsEvent && context.MemberInfo != null &&
                   !context.MemberInfo.IsStatic;
        }

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
        /// Runs before anything else on the test method.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        protected override void PreBuild(IMemberPreBuildContext context)
        {
            if (TestsToRemove.Any(e => e == context.MemberInfo.Name))
            {
                context.PreBuildResult.ExcludeMember = true;
            }
        }
    }
}