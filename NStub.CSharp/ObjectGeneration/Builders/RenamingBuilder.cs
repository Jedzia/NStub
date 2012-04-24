// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenamingBuilder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    using System.CodeDom;
    using System.ComponentModel;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;

    /// <summary>
    /// Rename text during the <see cref="IMemberBuilder.Build"/> process.
    /// </summary>
    [Description("Renaming component for use in the Build-Step.\r\nUse with caution, because it's hot hot hot!")]
    public class RenamingBuilder : MultiBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RenamingBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public RenamingBuilder(IMemberSetupContext context)
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
            return true;

            // return !context.IsConstructor && !context.IsProperty && !context.IsEvent && context.MemberInfo != null &&
            // !context.MemberInfo.IsStatic;
        }

        /// <summary>
        /// Builds the test method member with the specified context.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// <c>true</c> on success.
        /// </returns>
        protected override bool BuildMember(IMemberBuildContext context)
        {
            var typeMember = context.TypeMember as CodeMemberMethod;
            typeMember.AddComment("From RenamingBuilder {" + Parameters.Id + "}");
            typeMember.AddBlankLine();

            // context.GetBuilderData
            return true;
        }
    }
}