// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorRunnerData.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Core
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Holds data for running a <see cref="TestProjectBuilder"/>.
    /// </summary>
    public class GeneratorRunnerData
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorRunnerData"/> class.
        /// </summary>
        /// <param name="outputFolder">The output folder.</param>
        /// <param name="inputAssemblyPath">The input assembly path.</param>
        /// <param name="mainNodes">The main nodes.</param>
        /// <param name="referencedAssemblies">The list of referenced assemblies.</param>
        public GeneratorRunnerData(
            string outputFolder,
            string inputAssemblyPath,
            IList<TestNode> mainNodes,
            IList<AssemblyName> referencedAssemblies)
        {
            Guard.NotNullOrEmpty(() => outputFolder, outputFolder);

            // Guard.NotNull(() => generatorType, generatorType);
            Guard.NotNullOrEmpty(() => inputAssemblyPath, inputAssemblyPath);
            Guard.NotNull(() => mainNodes, mainNodes);
            Guard.NotNull(() => referencedAssemblies, referencedAssemblies);

            this.OutputFolder = outputFolder;

            // this.GeneratorType = generatorType;
            this.InputAssemblyPath = inputAssemblyPath;
            this.RootNodes = mainNodes;
            this.ReferencedAssemblies = referencedAssemblies;
        }

        #endregion

        // /// <summary>
        // /// Gets the type of the generator.
        // /// </summary>
        // /// <value>
        // /// The type of the generator.
        // /// </value>
        // public Type GeneratorType { get; private set; }
        #region Properties

        /// <summary>
        /// Gets the path to the input assembly.
        /// </summary>
        public string InputAssemblyPath { get; private set; }

        /// <summary>
        /// Gets the output folder.
        /// </summary>
        public string OutputFolder { get; private set; }

        /// <summary>
        /// Gets the list of referenced assemblies.
        /// </summary>
        public IList<AssemblyName> ReferencedAssemblies { get; private set; }

        /// <summary>
        /// Gets the root nodes containing hierarchical data with structural information 
        /// about the assembly to generate tests of.
        /// </summary>
        public IList<TestNode> RootNodes { get; private set; }

        #endregion
    }
}