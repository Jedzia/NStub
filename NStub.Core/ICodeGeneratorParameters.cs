using System.Collections.Generic;
using System.Text;
using System;

namespace NStub.Core
{
    [Flags]
    public enum MemberVisibility
    {
        Public = 1 << 0,
        Internal = 1 << 1,
        Private = 1 << 2,
    }

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
