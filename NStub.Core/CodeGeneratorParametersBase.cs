namespace NStub.Core
{
    using System;

    /// <summary>
    /// Abstract base class for the Configuration of an <see cref="ICodeGenerator"/>.
    /// </summary>
    public abstract class CodeGeneratorParametersBase : ICodeGeneratorParameters
    {
        private readonly string outputDirectory;

        /// <summary>
        /// Gets the output directory.
        /// </summary>
        public string OutputDirectory
        {
            get { return outputDirectory; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use setup and tear down.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [use setup and tear down]; otherwise, <c>false</c>.
        /// </value>
        public bool UseSetupAndTearDown
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGeneratorParametersBase"/> class.
        /// </summary>
        /// <param name="outputDirectory">The output directory.</param>
        protected CodeGeneratorParametersBase(string outputDirectory)
        {
            if (outputDirectory == null)
            {
                throw new ArgumentNullException("outputDirectory");
            }

            this.outputDirectory = outputDirectory;
        }
    }
}