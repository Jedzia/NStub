// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberBuilder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    using NStub.Core;
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
        protected MemberBuilder(IMemberSetupContext context)
        {
            Guard.NotNull(() => context, context);
            this.SetupContext = context;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets build context of the test method member.
        /// </summary>
        public IMemberSetupContext SetupContext { get; private set; }

        #endregion

        /*/// <summary>
        /// Gets or sets the <see cref="T:System.Object"/> at the specified index.
        /// </summary>
        /// <param name="propertyName">The index of the element to get or set.</param>
        /// <value>The <see cref="T:System.Object"/> at the specified index.</value>
        public IBuilderData this[string propertyName]
        {
            get
            {
                // TODO: return the specified index here
                var propData = this.Context.BuildData[this.Context.TestKey];
                var result = propData[propertyName];
                return result;
            }

            //set
            //{
                // TODO: set the specified index to value here 
            //}
        }*/

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
        /// <param name="originalName">The initial name of the test method member.</param>
        /// <returns>
        /// The name of the test method.
        /// </returns>
        public string GetTestName(IMemberBuildContext context, string originalName)
        {
            return DetermineTestName(context, originalName);
        }

        /// <summary>
        /// Builds the test method member with the specified context.
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
        /// <param name="originalName">The initial name of the test method member.</param>
        /// <returns>
        /// The name of the test method.
        /// </returns>
        /// <remarks>The builders are called one after one, so an integral name resolution happens.</remarks>
        protected virtual string DetermineTestName(IMemberSetupContext context, string originalName)
        {
            return originalName;
        }
    }
}