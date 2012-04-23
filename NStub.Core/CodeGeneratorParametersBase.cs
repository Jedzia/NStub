// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGeneratorParametersBase.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Core
{
    /// <summary>
    /// Abstract base class for the Configuration of an <see cref="ICodeGenerator"/>.
    /// </summary>
    public abstract class CodeGeneratorParametersBase : ICodeGeneratorParameters
    {
        #region Fields

        private readonly string outputDirectory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGeneratorParametersBase"/> class.
        /// </summary>
        /// <param name="outputDirectory">The output directory.</param>
        protected CodeGeneratorParametersBase(string outputDirectory)
        {
            Guard.NotNullOrEmpty(() => outputDirectory, outputDirectory);
            this.outputDirectory = outputDirectory;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the output directory.
        /// </summary>
        public string OutputDirectory
        {
            get
            {
                return this.outputDirectory;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use setup and tear down.
        /// </summary>
        /// <value>
        /// <c>true</c> if [use setup and tear down]; otherwise, <c>false</c>.
        /// </value>
        public bool UseSetupAndTearDown { get; set; }

        #endregion
    }
}