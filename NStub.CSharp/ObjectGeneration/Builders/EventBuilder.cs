// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventBuilder.cs" company="EvePanix">
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
    using System.CodeDom;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Test method generator for event type members.
    /// </summary>
    public class EventBuilder : MemberBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public EventBuilder(IMemberSetupContext context)
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
            return context.IsEvent;


            // return context.TypeMember.Name.StartsWith("add_") || context.TypeMember.Name.StartsWith("remove_");
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
            var typeMember = context.TypeMember;
            var eventName = typeMember.Name;
            this.ComputeCodeMemberEvent(typeMember as CodeMemberMethod, eventName);
            return true;
        }

        /// <summary>
        /// Determines the name of the test method.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <param name="originalName">The initial name of the test method member.</param>
        /// <returns>
        /// The name of the test method.
        /// </returns>
        protected override string DetermineTestName(IMemberSetupContext context, string originalName)
        {
            var typeMemberName = originalName;
            if (typeMemberName.Contains("add_"))
            {
                typeMemberName = typeMemberName.Replace("add_", "Event");
            }
            else if (typeMemberName.Contains("remove_"))
            {
                typeMemberName = typeMemberName.Replace("remove_", "Event");
            }
            
            typeMemberName = typeMemberName.Replace("Test", "AddAndRemove");
            return typeMemberName;
        }

        /// <summary>
        /// Handle event related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="eventName">Name of the event.</param>
        protected void ComputeCodeMemberEvent(CodeMemberMethod typeMember, string eventName)
        {
            //var propertyName = typeMemberName.Replace("get_", string.Empty).Replace("set_", string.Empty);

            /*if (typeMember.Name.Contains("add_"))
            {
                typeMember.Name = typeMember.Name.Replace("add_", "Event");
            }
            else if (typeMember.Name.Contains("remove_"))
            {
                typeMember.Name = typeMember.Name.Replace("remove_", "Event");
            }

            */
            BaseCSharpCodeGenerator.ReplaceTestInTestName(typeMember, "AddAndRemove");
        }


    }
}