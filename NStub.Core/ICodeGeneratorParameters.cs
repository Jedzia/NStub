using System.Collections.Generic;
using System.Text;
using System;

namespace NStub.Core
{
    /// <summary>
    /// Type member visibility.
    /// </summary>
    [Flags]
    public enum MemberVisibility
    {
        /// <summary>
        /// All public types.
        /// </summary>
        Public = 1 << 0,
        
        /// <summary>
        /// All internal types.
        /// </summary>
        Internal = 1 << 1,
        
        /// <summary>
        /// All private types.
        /// </summary>
        Private = 1 << 2,
    }

    /// <summary>
    /// Represents the setup data for a <see cref="ICodeGenerator"/>.
    /// </summary>
    public interface ICodeGeneratorSetup : /*ICodeGeneratorSetupBase,*/ ICloneable
    {
    /*}

    public interface ICodeGeneratorSetupBase 
    {*/
        /// <summary>
        /// Gets or sets a value indicating whether to produce setup and tear down methods.
        /// </summary>
        /// <value>
        /// <c>true</c> if [use setup and tear down]; otherwise, <c>false</c>.
        /// </value>
        bool UseSetupAndTearDown { get; set; }

        /// <summary>
        /// Gets or sets the method generators level of detail .
        /// </summary>
        /// <value>
        /// The method generators level of detail.
        /// </value>
        MemberVisibility MethodGeneratorLevelOfDetail { get; set; }
    }

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
