// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestBuilderFactory.cs" company="EvePanix">
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
    using System.Collections.Generic;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Provides builders for test method generation. 
    /// </summary>
    public class TestBuilderFactory : ITestBuilderFactory
    {
        // private readonly EventBuilder eventBuilder;
        // private readonly MethodBuilder methodBuilder;
        // private readonly PropertyBuilder propertyBuilder;
        // private readonly Dictionary<Type, IMemberBuilder> builders = new Dictionary<Type, IMemberBuilder>();
        #region Fields

        private readonly List<IBuildHandler> handlers = new List<IBuildHandler>();

        #endregion

        /*/// <summary>
        /// Initializes a new instance of the <see cref="TestBuilderFactory"/> class.
        /// </summary>
        /// <param name="propertyBuilder">The property builder.</param>
        /// <param name="eventBuilder">The event builder.</param>
        /// <param name="methodBuilder">The method builder.</param>
        public TestBuilderFactory(
            PropertyBuilder propertyBuilder, EventBuilder eventBuilder, MethodBuilder methodBuilder)
        {
            Guard.NotNull(() => propertyBuilder, propertyBuilder);
            Guard.NotNull(() => eventBuilder, eventBuilder);
            Guard.NotNull(() => methodBuilder, methodBuilder);

            this.propertyBuilder = propertyBuilder;
            this.eventBuilder = eventBuilder;
            this.methodBuilder = methodBuilder;
        }*/
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBuilderFactory"/> class.
        /// </summary>
        public TestBuilderFactory()
        {
            this.handlers.Add(new BuildHandler(typeof(PropertyBuilder), PropertyBuilder.CanHandleContext));
            this.handlers.Add(new BuildHandler(typeof(PropertyGetBuilder), PropertyGetBuilder.CanHandleContext));
            this.handlers.Add(new BuildHandler(typeof(PropertySetBuilder), PropertySetBuilder.CanHandleContext));
            this.handlers.Add(new BuildHandler(typeof(EventBuilder), EventBuilder.CanHandleContext));
            this.handlers.Add(new BuildHandler(typeof(ConstructorBuilder), ConstructorBuilder.CanHandleContext));
            this.handlers.Add(new BuildHandler(typeof(StaticMethodBuilder), StaticMethodBuilder.CanHandleContext));
        }

        #endregion

        /// <summary>
        /// Adds the specified handler to the factory.
        /// </summary>
        /// <param name="handler">The handler to be added.</param>
        public void AddHandler(IBuildHandler handler)
        {
            this.handlers.Add(handler);
        }

        /*/// <summary>
        /// Gets the event builder.
        /// </summary>
        public EventBuilder EventBuilder
        {
            get
            {
                return this.eventBuilder;
            }
        }

        /// <summary>
        /// Gets the method builder.
        /// </summary>
        public MethodBuilder MethodBuilder
        {
            get
            {
                return this.methodBuilder;
            }
        }

        /// <summary>
        /// Gets the property builder.
        /// </summary>
        public PropertyBuilder PropertyBuilder
        {
            get
            {
                return this.propertyBuilder;
            }
        }*/

        /// <summary>
        /// Tries to get the builder for the specified context.
        /// </summary>
        /// <param name="context">The context of the current test object.</param>
        /// <returns>
        /// A list of member builders that can handle the request or an <c>empty</c> list if no one can be found.
        /// </returns>
        public IEnumerable<IMemberBuilder> GetBuilder(IMemberBuildContext context)
        {
            foreach(var buildHandler in this.handlers)
            {
                var canHandleContext = buildHandler.Handler(context);
                if (canHandleContext)
                {
                    var memberBuilder = buildHandler.CreateInstance(context);
                    yield return memberBuilder;
                }
            }
        }
    }

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