// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpTestProjectBuilder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp
{
    using System;
    using System.CodeDom;
    using NStub.Core;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.ObjectGeneration.Builders;

    /// <summary>
    /// Builds the test files with user provided build properties.
    /// </summary>
    public class CSharpTestProjectBuilder : TestProjectBuilder
    {
        #region Fields

        private readonly
            Func<IBuildSystem, IBuildDataDictionary, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator> createGeneratorCallback;

        private readonly BuildDataDictionary properties;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpTestProjectBuilder"/> class.
        /// </summary>
        /// <param name="sbs">The system wide build system.</param>
        /// <param name="buildData">The build data properties.</param>
        /// <param name="projectGenerator">The project generator.</param>
        /// <param name="createGeneratorCallback">The callback to create new code
        /// generators per test class <see cref="CodeNamespace"/>.</param>
        /// <param name="logger">The logging method.</param>
        public CSharpTestProjectBuilder(
            IBuildSystem sbs,
            IBuildDataDictionary buildData,
            IProjectGenerator projectGenerator,
            Func<IBuildSystem, IBuildDataDictionary, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator> createGeneratorCallback,
            Action<string> logger)
            : base(sbs, projectGenerator, OldOnCreateCodeGenerator, logger)
        {
            Guard.NotNull(() => buildData, buildData);
            Guard.NotNull(() => createGeneratorCallback, createGeneratorCallback);
            this.createGeneratorCallback = createGeneratorCallback;

            // properties = SetUpBuildProperties();
            this.properties = SetUpBuildProperties(buildData);
        }

        #endregion

        /// <summary>
        /// Called when the <c>TestProjectBuilder</c> is about to create the code generator.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="buildSystem">The build system.</param>
        /// <returns>
        /// An with the parameters initialized code generator.
        /// </returns>
        protected override ICodeGenerator OnCreateCodeGenerator(
            CodeNamespace codeNamespace, ICodeGeneratorParameters configuration, IBuildSystem buildSystem)
        {
            var codeGenerator = this.createGeneratorCallback(buildSystem, this.properties, configuration, codeNamespace);
            return codeGenerator;
        }

        private static ICodeGenerator OldOnCreateCodeGenerator(
            IBuildSystem buildSystem, ICodeGeneratorParameters configuration, CodeNamespace codeNamespace)
        {
            return null;
        }

        /// <summary>
        /// SetUp the default build properties.
        /// </summary>
        /// <param name="buildData">The incoming build data.</param>
        /// <returns>A new <see cref="BuildDataDictionary"/>, merged with the input <paramref name="buildData"/>.</returns>
        private static BuildDataDictionary SetUpBuildProperties(IBuildDataDictionary buildData)
        {
            var props = new BuildDataDictionary();

            // ReadOnlyCollection
            foreach (var category in buildData.Data())
            {
                foreach (var item in category.Value)
                {
                    props.AddDataItem(category.Key, item.Key, item.Value);
                }

                // props.AddDataItem(item.Key, item,);
            }

            props.AddDataItem("DasGuuuut", new EmptyBuildParameters());

            // Todo: !!!
            return props;
        }
    }
}