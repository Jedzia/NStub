namespace NStub.CSharp.ObjectGeneration
{
    using System;

    /// <summary>
    /// Marks an <see cref="IMemberBuilder"/> to intercept another <see cref="IMemberBuilder"/> class.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class MemberBuilderDecoratorAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the decorated <see cref="IMemberBuilder"/>.
        /// </summary>
        public Type DecoratedType
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberBuilderDecoratorAttribute"/> class,
        /// which describes a decorator relationship between <see cref="IMemberBuilder"/> types.
        /// </summary>
        /// <param name="decoratedType">The type of the <see cref="IMemberBuilder"/> to intercept with the
        /// attributed class.</param>
        /// <remarks>
        /// Pinpoints this type to the specified <paramref name="decoratorType"/>. The decorating type
        /// has to implement the <see cref="IMemberBuilder"/> interface. Further, the constructor of the
        /// decorator has to be in the form of
        /// <para>DecoratorType(IMemberBuilder decoratedType, <see cref="IMemberSetupContext"/> context).</para>
        /// <para>That allows the decorator to intercept the decorated type.</para>
        /// </remarks>
        public MemberBuilderDecoratorAttribute(Type decoratedType)
        {
            Guard.CanBeAssigned(() => decoratedType, typeof(IMemberBuilder), decoratedType);
            this.DecoratedType = decoratedType;
        }
    }
}