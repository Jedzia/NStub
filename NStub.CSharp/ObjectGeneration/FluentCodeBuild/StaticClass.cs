// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticClass.cs" company="EvePanix">
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
    using System.Linq.Expressions;

    /// <summary>
    /// Provides Fluent code construction for static classes.
    /// </summary>
    /// <typeparam name="T">Type of the static class to access.</typeparam>
    public static class StaticClass<T>
    {
        /// <summary>
        /// Creates a reference to the value of the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>A reference expression to the value of the specified property.</returns>
        public static CodePropertyReferenceExpression Property(string propertyName)
        {
            var staticexpr = new CodeTypeReferenceExpression(typeof(T).FullName);
            var result = new CodePropertyReferenceExpression(staticexpr, propertyName);
            return result;
        }
    }

    /// <summary>
    /// Provides Fluent code construction for static classes.
    /// </summary>
    public static class StaticClass
    {
        /// <summary>
        /// Create a reference to a static class Like '<c>System.String</c>' or '<c>DateTime</c>'.
        /// </summary>
        /// <param name="className">Name of the static class.</param>
        /// <returns>
        /// A reference to the specified data type.
        /// </returns>
        public static CodeTypeReferenceExpression Of(string className)
        {
            // "Assert"
            var staticexpr = new CodeTypeReferenceExpression(className);

            // var result = new CodeTypeReferenceBinder(method, staticexpr);
            return staticexpr;
        }

        /// <summary>
        /// Create a reference to the specified property of a static class Like '<c>string.Empty</c>' or '<c>DateTime.Now</c>'.
        /// </summary>
        /// <typeparam name="T">Type of the static class to access.</typeparam>
        /// <param name="property">The expression defining the property. For example
        /// this can be '() => string.Empty' or '() => DateTime.Now'.</param>
        /// <returns>A reference expression to the value of the specified property.</returns>
        public static CodePropertyReferenceExpression Property<T>(Expression<Func<T>> property)
        {
            var member = (MemberExpression)property.Body;
            var propertyName = member.Member.Name;
            var staticexpr = new CodeTypeReferenceExpression(typeof(T).FullName);
            var result = new CodePropertyReferenceExpression(staticexpr, propertyName);
            return result;
        }
    }
}