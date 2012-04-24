// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultMemberBuilderFactory.cs" company="EvePanix">
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
    /// Default implementation of a <see cref="IMemberBuilderFactory"/>.
    /// </summary>
    internal class DefaultMemberBuilderFactory : MemberBuilderFactory
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMemberBuilderFactory"/> class.
        /// </summary>
        protected internal DefaultMemberBuilderFactory()
            : base(new BuilderSerializer())
        {
            AddHandler(
                new BuildHandler(
                    typeof(PropertyBuilder), PropertyBuilder.CanHandleContext, typeof(BuildParametersOfPropertyBuilder)));
            AddHandler(
                new BuildHandler(
                    typeof(PropertyGetBuilder), 
                    PropertyGetBuilder.CanHandleContext, 
                    typeof(EmptyBuildParameters)));
            AddHandler(
                new BuildHandler(
                    typeof(PropertySetBuilder), 
                    PropertySetBuilder.CanHandleContext,
                    typeof(EmptyBuildParameters)));
            AddHandler(
                new BuildHandler(
                    typeof(EventBuilder), EventBuilder.CanHandleContext, typeof(EmptyBuildParameters)));
            AddHandler(
                new BuildHandler(
                    typeof(ConstructorBuilder), 
                    ConstructorBuilder.CanHandleContext,
                    typeof(EmptyBuildParameters)));
            AddHandler(
                new BuildHandler(
                    typeof(StaticMethodBuilder), 
                    StaticMethodBuilder.CanHandleContext,
                    typeof(EmptyBuildParameters)));
            AddHandler(
                new BuildHandler(
                    typeof(DefaultMethodEraser), 
                    DefaultMethodEraser.CanHandleContext,
                    typeof(EmptyBuildParameters)));
            AddHandler(
                new BuildHandler(
                    typeof(RenamingBuilder),
                    RenamingBuilder.CanHandleContext,
                    typeof(MultiBuildParametersOfRenamingBuilder)));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the service of this instance.
        /// </summary>
        public override IMemberBuilderFactory Factory
        {
            get
            {
                // return this.taskService ?? (this.taskService = TaskRegistry.GetInstance());
                return new DefaultMemberBuilderFactory();
            }
        }

        #endregion
    }
}