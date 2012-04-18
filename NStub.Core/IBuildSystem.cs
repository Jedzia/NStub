using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NStub.Core
{
    /// <summary>
    /// Represents a hosting system with infrastructure for filesystem access and other low level functionality.
    /// </summary>
    public interface IBuildSystem
    {
        /*/// <summary>
        /// Gets the OutputDirectory.
        /// </summary>
        string OutputDirectory { get; }*/

        char DirectorySeparatorChar { get; }

        bool DirectoryExists(string directory);

        TextWriter GetTextWriter(string path, bool append);
    }
}
