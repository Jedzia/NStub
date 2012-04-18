// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentCodeMemberField.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.FluentCodeBuild
{
    using System;
    using System.CodeDom;

    /// <summary>
    /// Provides Fluent <see cref="CodeMemberField"/> construction.
    /// </summary>
    public static class FluentCodeMemberField
    {
        /// <summary>
        /// Creates the specified member field by name and type.
        /// </summary>
        /// <param name="memberFieldName">Name of the member field.</param>
        /// <param name="memberFieldType">Type of the member field.</param>
        /// <returns>A new <see cref="CodeMemberField"/> with the specified data.</returns>
        public static CodeMemberField Create(string memberFieldName, string memberFieldType)
        {
            var memberField = new CodeMemberField(memberFieldType, memberFieldName)
                                  {
                                      Attributes = MemberAttributes.Private
                                  };
            return memberField;
        }

        /// <summary>
        /// Creates the specified member field by name and type.
        /// </summary>
        /// <param name="memberFieldName">Name of the member field.</param>
        /// <param name="memberFieldType">Type of the member field.</param>
        /// <returns>A new <see cref="CodeMemberField"/> with the specified data.</returns>
        public static CodeMemberField Create(string memberFieldName, Type memberFieldType)
        {
            var memberField = new CodeMemberField(memberFieldType, memberFieldName)
                                  {
                                      Attributes = MemberAttributes.Private
                                  };
            return memberField;
        }

        /// <summary>
        /// Creates the specified member field by name and type.
        /// </summary>
        /// <typeparam name="T">Type of the member field</typeparam>
        /// <param name="memberFieldName">Name of the member field.</param>
        /// <returns>
        /// A new <see cref="CodeMemberField"/> with the specified data.
        /// </returns>
        public static CodeMemberField Create<T>(string memberFieldName)
        {
            return Create(memberFieldName, typeof(T));
        }
    }
}