// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiBuilder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    using System;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Base class for a test method processing class that can have multiple instantiations and user defined parameters.
    /// </summary>
    public abstract class MultiBuilder : MemberBuilder, IMultiBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        protected MultiBuilder(IMemberSetupContext context)
            : base(context)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parameters associated with this builder.
        /// </summary>
        /// <value>
        /// The user defined parameters of the builder.
        /// </value>
        public IMultiBuildParameters Parameters { get; set; }

        /// <summary>
        /// Gets the identification of the Builder.
        /// </summary>
        public Guid Id
        {
            get
            {
                return Parameters.Id;
            }
        }

        #endregion

        /// <summary>
        /// Builds the test method member with the specified context.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// <c>true</c> on success.
        /// </returns>
        protected abstract override bool BuildMember(IMemberBuildContext context);
    }
}