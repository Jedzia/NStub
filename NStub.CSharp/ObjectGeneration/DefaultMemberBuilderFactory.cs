﻿// --------------------------------------------------------------------------------------------------------------------
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
using System.Collections.Generic;

    /// <summary>
    /// Default implementation of a <see cref="IMemberBuilderFactory"/>.
    /// </summary>
    internal class DefaultMemberBuilderFactory : MemberBuilderFactory
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMemberBuilderFactory"/> class. DeleteME
        /// </summary>
        /// <param name="serializer">The serializer capable of serializing the registered <see cref="IMemberBuilder"/>'s to xml.</param>
        /// <param name="handlers">The list of available build handlers holding the information how to 
        /// create the associated <see cref="IMemberBuilder"/>.</param>
        /// <param name="noWay">Just a private and not used differentiator.</param>
        private DefaultMemberBuilderFactory(IBuilderSerializer serializer, IEnumerable<IBuildHandler> handlers, string noWay)
            : base(serializer)
        {
            foreach (var handler in handlers)
            {
                AddHandler(handler);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMemberBuilderFactory"/> class
        /// for testing purposes.
        /// </summary>
        /// <param name="serializer">The serializer capable of serializing the registered <see cref="IMemberBuilder"/>'s to xml.</param>
        /// <param name="handlers">The list of available build handlers holding the information how to
        /// create the associated <see cref="IMemberBuilder"/>.</param>
        internal DefaultMemberBuilderFactory(IBuilderSerializer serializer, IEnumerable<IBuildHandler> handlers)
            : base(serializer)
        {
            foreach (var handler in handlers)
            {
                AddHandler(handler);
            }
        }

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