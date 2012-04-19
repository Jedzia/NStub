// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuildSystem.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Core
{
    using System.IO;

    /// <summary>
    /// Represents a hosting system with infrastructure for file system access and other low level functionality.
    /// </summary>
    public interface IBuildSystem
    {
        #region Properties

        /// <summary>
        /// Gets the directory separator character, like the backslash "\" for Windows operation systems.
        /// </summary>
        char DirectorySeparatorChar { get; }

        #endregion

        /// <summary>
        /// Creates all directories and subdirectories as specified by path.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        /// <returns>A <see cref="System.IO.DirectoryInfo"/> as specified by path.</returns>
        DirectoryInfo CreateDirectory(string path);

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="directory">The path to test.</param>
        /// <returns><c>true</c> if path refers to an existing directory; otherwise, <c>false</c>.</returns>
        bool DirectoryExists(string directory);

        /// <summary>
        /// Returns the file name of the specified path string without the extension.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>A System.String containing the string returned by System.IO.Path.GetFileName(System.String),
        /// minus the last period (.) and all characters following it.</returns>
        /// <exception cref="System.ArgumentException">path contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars().</exception>
        string GetFileNameWithoutExtension(string path);

        /// <summary>
        /// Returns a writer that stores text in the specified path.
        /// </summary>
        /// <param name="path">The filename to write data to.</param>
        /// <param name="append">if set to <c>true</c> appends the file.</param>
        /// <returns>A writer that stores text in the specified path.</returns>
        TextWriter GetTextWriter(string path, bool append);
    }
}