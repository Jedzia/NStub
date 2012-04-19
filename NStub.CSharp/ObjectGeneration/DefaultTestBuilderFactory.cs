// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultTestBuilderFactory.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using NStub.CSharp.ObjectGeneration.Builders;

    /// <summary>
    /// Default implementation of a <see cref="ITestBuilderFactory"/>.
    /// </summary>
    internal class DefaultTestBuilderFactory : TestBuilderFactory
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTestBuilderFactory"/> class.
        /// </summary>
        protected internal DefaultTestBuilderFactory()
        {
            Handlers.Add(new BuildHandler(typeof(PropertyBuilder), PropertyBuilder.CanHandleContext));
            Handlers.Add(new BuildHandler(typeof(PropertyGetBuilder), PropertyGetBuilder.CanHandleContext));
            Handlers.Add(new BuildHandler(typeof(PropertySetBuilder), PropertySetBuilder.CanHandleContext));
            Handlers.Add(new BuildHandler(typeof(EventBuilder), EventBuilder.CanHandleContext));
            Handlers.Add(new BuildHandler(typeof(ConstructorBuilder), ConstructorBuilder.CanHandleContext));
            Handlers.Add(new BuildHandler(typeof(StaticMethodBuilder), StaticMethodBuilder.CanHandleContext));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the service of this instance.
        /// </summary>
        public override ITestBuilderFactory Factory
        {
            get
            {
                // return this.taskService ?? (this.taskService = TaskRegistry.GetInstance());
                return new DefaultTestBuilderFactory();
            }
        }

        #endregion
    }
}