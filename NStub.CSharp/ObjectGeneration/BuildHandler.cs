// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildHandler.cs" company="EvePanix">
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
    using System.ComponentModel;
    using System.Linq;
    using NStub.Core;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Checks if a registered <see cref="IMemberBuilder"/> can handle a <see cref="IMemberBuildContext"/> request.
    /// </summary>
    public class BuildHandler : IBuildHandler
    {
        #region Fields

        private readonly string description;
        private readonly Func<IMemberBuildContext, bool> handler;
        private readonly Type parameterDataType;
        private readonly Type type;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildHandler"/> class.
        /// </summary>
        /// <param name="type">The type of the <see cref="IMemberBuilder"/> worker.</param>
        /// <param name="handler">The handler that can determine if the <paramref name="type"/> can
        /// accept an <see cref="IMemberBuildContext"/> order.</param>
        /// <param name="parameterDataType">Type of the associated parameter data.</param>
        public BuildHandler(Type type, Func<IMemberBuildContext, bool> handler, Type parameterDataType)
        {
            Guard.CanBeAssigned(() => type, type, typeof(IMemberBuilder));
            Guard.NotNull(() => handler, handler);

            // Guard.NotNull(() => parameterDataType, parameterDataType);
            Guard.CanBeAssigned(() => parameterDataType, parameterDataType, typeof(IMemberBuildParameters));

            this.type = type;
            this.handler = handler;
            this.parameterDataType = parameterDataType;

            this.description = CheckForAttributes(type, parameterDataType);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the handler, that checks, if the associated <see cref="Type"/> can process an <see cref="IMemberBuildContext"/> assignment.
        /// </summary>
        public Func<IMemberBuildContext, bool> CanHandle
        {
            get
            {
                return this.handler;
            }
        }

        /// <summary>
        /// Gets the description of the builder.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Gets the type of the parameter data.
        /// </summary>
        /// <value>
        /// The type of the parameter data.
        /// </value>
        public Type ParameterDataType
        {
            get
            {
                return this.parameterDataType;
            }
        }

        /// <summary>
        /// Gets the associated <see cref="IMemberBuilder"/> type that can handle the request specified in the <see cref="CanHandle"/> method.
        /// </summary>
        public Type Type
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a multi builder type.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a multi builder type; otherwise, <c>false</c>.
        /// </value>
        public bool IsMultiBuilder
        {
            get
            {
                return typeof(IMultiBuilder).IsAssignableFrom(this.type);
            }
        }

        #endregion

        /// <summary>
        /// Creates a new instance of the <see cref="IMemberBuilder"/> specified in the <see cref="Type"/> property with 
        /// the specified context data.
        /// </summary>
        /// <param name="context">The context of the current test object.</param>
        /// <returns>
        /// A test member builder that can handle the request specified in the <see cref="CanHandle"/> method.
        /// </returns>
        public IMemberBuilder CreateInstance(IMemberBuildContext context)
        {
            var parameters = new object[] { context };
            IMemberBuilder memberBuilder = null;
            //try
            {
                var inst = Activator.CreateInstance(this.Type, parameters);
                memberBuilder = (IMemberBuilder)inst;
            }
            //catch (Exception ex)
            //{
            //}
            return memberBuilder;
        }

        private static string CheckForAttributes(Type builderType, Type parameterDataType)
        {
            var descriptionAttrs =
                (DescriptionAttribute[])parameterDataType.GetCustomAttributes(typeof(DescriptionAttribute), false);
            var paraDescr = descriptionAttrs.Select(e => e.Description).FirstOrDefault();

            descriptionAttrs =
                (DescriptionAttribute[])builderType.GetCustomAttributes(typeof(DescriptionAttribute), false);
            var builderDescr = descriptionAttrs.Select(e => e.Description).FirstOrDefault();
            string line = "--------------------------------------------------------------------------------------------" + Environment.NewLine;
            var descr = "[Builder]: " + builderDescr + Environment.NewLine + line + "[Parameters]: " + paraDescr + Environment.NewLine + line;
            return descr;
        }
    }
}