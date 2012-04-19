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
    using NStub.Core;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Provides builders for test method generation. 
    /// </summary>
    public abstract class TestBuilderFactory : ITestBuilderFactory
    {
        #region Fields

        private readonly List<IBuildHandler> handlers = new List<IBuildHandler>();
        private static ITestBuilderFactory defaultfactory;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default <see cref="TestBuilderFactory"/>.
        /// </summary>
        /// <value>
        /// The default <see cref="TestBuilderFactory"/>.
        /// </value>
        /// <remarks>
        /// <para>The default <see cref="TestBuilderFactory"/> can only set once, and that before accessing it.</para>
        /// <para>To achieve a different behavior for the functionality provided by another implementation
        /// of an <see cref="ITestBuilderFactory"/>, derive from this class and override the abstract 
        /// <see cref="TestBuilderFactory.Factory"/> getter.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Cannot set the default <see cref="ITestBuilderFactory"/> twice. Maybe you accessed it before you've written to it.</exception>
        public static ITestBuilderFactory Default
        {
            get
            {
                return defaultfactory ?? (defaultfactory = new DefaultTestBuilderFactory());
            }

            set
            {
                Guard.NotNull(() => value, value);
                if (defaultfactory != null)
                {
                    throw new InvalidOperationException(
                        "Cannot set the default ITestBuilderFactory twice. Maybe you accessed it before you've written to it.");
                }

                defaultfactory = value;
            }
        }

        /// <summary>
        /// Gets the factory service of this instance.
        /// </summary>
        public abstract ITestBuilderFactory Factory { get; }

        /// <summary>
        /// Gets the list of build handlers.
        /// </summary>
        protected List<IBuildHandler> Handlers
        {
            get
            {
                return this.handlers;
            }
        }

        #endregion

        /// <summary>
        /// Adds the specified handler to the factory.
        /// </summary>
        /// <param name="handler">The handler to be added.</param>
        public void AddHandler(IBuildHandler handler)
        {
            this.Handlers.Add(handler);
        }

        /// <summary>
        /// Tries to get the builder for the specified context.
        /// </summary>
        /// <param name="context">The context of the current test object.</param>
        /// <returns>
        /// A list of member builders that can handle the request or an <c>empty</c> list if no one can be found.
        /// </returns>
        public IEnumerable<IMemberBuilder> GetBuilder(IMemberBuildContext context)
        {
            // Todo: maybe cache em.
            foreach(var buildHandler in this.Handlers)
            {
                var canHandleContext = buildHandler.CanHandle(context);
                if (!canHandleContext)
                {
                    continue;
                }

                var memberBuilder = buildHandler.CreateInstance(context);
                yield return memberBuilder;
            }
        }
    }
}