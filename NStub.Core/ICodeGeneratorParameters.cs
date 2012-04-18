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
}
