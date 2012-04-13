// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberBuilder.cs" company="EvePanix">
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
    /// Base class for a test method processing class.
    /// </summary>
    public abstract class MemberBuilder : IMemberBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public MemberBuilder(IMemberBuildContext context)
        {
            this.Context = context;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets build context of the test method member.
        /// </summary>
        public IMemberBuildContext Context { get; private set; }

        #endregion

        /// <summary>
        /// Builds the specified context.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// <c>true</c> on success.
        /// </returns>
        public bool Build(IMemberBuildContext context)
        {
            return BuildMember(context);
        }

        /// <summary>
        /// Determines the name of the test method.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// The name of the test method.
        /// </returns>
        public string GetTestName(IMemberBuildContext context)
        {
            return DetermineTestName(context);
        }


        /// <summary>
        /// Builds the specified context.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// <c>true</c> on success.
        /// </returns>
        protected abstract bool BuildMember(IMemberBuildContext context);

        /// <summary>
        /// Determines the name of the test method.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// The name of the test method.
        /// </returns>
        protected abstract string DetermineTestName(IMemberBuildContext context);


    }
}