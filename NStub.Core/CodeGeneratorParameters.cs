namespace NStub.Core
{
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