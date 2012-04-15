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
        /// <returns>A new CodeMemberField with the specified data.</returns>
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
        /// <returns>A new CodeMemberField with the specified data.</returns>
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
        /// A new CodeMemberField with the specified data.
        /// </returns>
        public static CodeMemberField Create<T>(string memberFieldName)
        {
            return Create(memberFieldName, typeof(T));
        }

    }
}
