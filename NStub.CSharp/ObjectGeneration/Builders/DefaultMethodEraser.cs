// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticMethodBuilder.cs" company="EvePanix">
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
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;
    using NStub.Core;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;

    /// <summary>
    /// Test method generator for static method members.
    /// </summary>
    public class DefaultMethodEraser : MemberBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticMethodBuilder"/> class.
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
            if (context.MemberInfo != null)
            {
            }

            if (context.TestKey != null)
            {
                if (context.TestObjectType.Name.Contains("AllIwantToTest"))
                {
                    if (context.TestKey.Contains("Static"))
                    {
                        // var abc = context.TestKey;
                        // var older = context.TestObjectType.Name;
                        // var mstatic = context.MemberInfo;
                    }
                }
            }

            return !context.IsConstructor && !context.IsProperty && !context.IsEvent && context.MemberInfo != null &&
                   !context.MemberInfo.IsStatic;

            // return context.TypeMember.Name.StartsWith("get_") || context.TypeMember.Name.StartsWith("set_");
        }

        protected override void PreBuild(IMemberSetupContext context)
        {
            if (context.MemberInfo.Name == "GetHashCode")
            {
                // context.TestClassDeclaration.Members.Remove(context.TypeMember);
            }
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
            /*Guard.NotNull(() => context, context);
            var typeMember = context.TypeMember as CodeMemberMethod;
            if (context.MemberInfo.Name == "GetHashCode")
            {
                context.TestClassDeclaration.Members.Remove(context.TypeMember);
            }*/
            
            return true;
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}