using System;
using System.Collections.Generic;
using System.Text;

namespace NStub.Core
{

    /// <summary>
    /// Represents a Configuration for a <see cref="ICodeGenerator"/>.
    /// </summary>
    public interface ICodeGeneratorParameters
    {
        /// <summary>
        /// Gets the output directory.
        /// </summary>
        string OutputDirectory { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to produce setup and tear down methods.
        /// </summary>
        /// <value>
        /// <c>true</c> if [use setup and tear down]; otherwise, <c>false</c>.
        /// </value>
        bool UseSetupAndTearDown { get; set; }
    }
    
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

    /// <summary>
    /// Implementation of a Configuration for an <see cref="ICodeGenerator"/>.
    /// </summary>
    public class CodeGeneratorParameters : CodeGeneratorParametersBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGeneratorParameters"/> class.
        /// </summary>
        public CodeGeneratorParameters(string outputDirectory)
            :base(outputDirectory)
        {
            
        }
    }


}
