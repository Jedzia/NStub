using System.Collections.Generic;
using System.Text;

namespace NStub.Core
{
    /// <summary>
    /// Represents a Configuration for a <see cref="ICodeGenerator"/>.
    /// </summary>
    public interface ICodeGeneratorParameters : ICodeGeneratorSetup
    {
        /// <summary>
        /// Gets the output directory.
        /// </summary>
        string OutputDirectory { get; }

    }
}
