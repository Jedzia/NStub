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

        /// <summary>
        /// Gets the directory separator char.
        /// </summary>
        char DirectorySeparatorChar { get; }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="directory">The path to test.</param>
        /// <returns>true if path refers to an existing directory; otherwise, false.</returns>
        bool DirectoryExists(string directory);

        /// <summary>
        /// Returns a writer that stores text in the specified path.
        /// </summary>
        /// <param name="path">The filename to write data to.</param>
        /// <param name="append">if set to <c>true</c> appends the file.</param>
        /// <returns>A writer that stores text in the specified path.</returns>
        TextWriter GetTextWriter(string path, bool append);

        /// <summary>
        /// Returns the file name of the specified path string without the extension.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>A System.String containing the string returned by System.IO.Path.GetFileName(System.String),
        /// minus the last period (.) and all characters following it.</returns>
        /// <exception cref="System.ArgumentException">path contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars().</exception>
        string GetFileNameWithoutExtension(string path);

        /// <summary>
        /// Creates all directories and subdirectories as specified by path.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        /// <returns>A System.IO.DirectoryInfo as specified by path.</returns>
        DirectoryInfo CreateDirectory(string path);
    }
}
