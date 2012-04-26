// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberBuilderDecoratorAttribute.cs" company="EvePanix">
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
    using NStub.Core;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Marks an <see cref="IMemberBuilder"/> to intercept another <see cref="IMemberBuilder"/> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class MemberBuilderDecoratorAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberBuilderDecoratorAttribute"/> class,
        /// which describes a decorator relationship between <see cref="IMemberBuilder"/> types.
        /// </summary>
        /// <param name="decoratedType">The type of the <see cref="IMemberBuilder"/> to intercept with the
        /// attributed class.</param>
        /// <remarks>
        /// Pinpoints this type to the specified <paramref name="decoratedType"/>. The decorating type
        /// has to implement the <see cref="IMemberBuilder"/> interface. Further, the constructor of the
        /// decorator has to be in the form of
        /// <para>DecoratorType(<see cref="IMemberBuilder"/> <paramref name="decoratedType"/>,
        /// <see cref="IMemberSetupContext"/> context).</para>
        /// <para>That allows the decorator to intercept the decorated type.</para>
        /// </remarks>
        public MemberBuilderDecoratorAttribute(Type decoratedType)
        {
            Guard.CanBeAssigned(() => decoratedType, decoratedType, typeof(IMemberBuilder));
            this.DecoratedType = decoratedType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the decorated <see cref="IMemberBuilder"/>.
        /// </summary>
        public Type DecoratedType { get; private set; }

        #endregion
    }
}