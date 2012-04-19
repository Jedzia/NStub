namespace NStub.Core
{
    using System.IO;

    /// <summary>
    /// Abstract base class for a hosting system with infrastructure for filesystem access and other low level functionality.
    /// </summary>
    public abstract class BuildSystem : IBuildSystem
    {
        /*private readonly string outputDirectory;

        /// <summary>
        /// Gets the OutputDirectory.
        /// </summary>
        public string OutputDirectory
        {
            get { return outputDirectory; }
        }*/

        /// <summary>
        /// Gets the directory separator char.
        /// </summary>
        public char DirectorySeparatorChar
        {
            get 
            { 
                return Path.DirectorySeparatorChar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildSystem"/> class.
        /// </summary>
        /// <param name="outputDirectory">The OutputDirectory.</param>
        protected BuildSystem(/*string outputDirectory*/)
        {
            /*if (outputDirectory == null)
            {
                throw new ArgumentNullException("outputDirectory");
            }

            this.outputDirectory = outputDirectory;*/
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="directory">The path to test.</param>
        /// <returns>
        /// true if path refers to an existing directory; otherwise, false.
        /// </returns>
        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        /// <summary>
        /// Returns a writer that stores text in the specified path.
        /// </summary>
        /// <param name="path">The filename to write data to.</param>
        /// <param name="append">if set to <c>true</c> appends the file.</param>
        /// <returns>
        /// A writer that stores text in the specified path.
        /// </returns>
        public TextWriter GetTextWriter(string path, bool append)
        {
            return new StreamWriter(path, append);
        }

        /// <summary>
        /// Returns the file name of the specified path string without the extension.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>
        /// A System.String containing the string returned by System.IO.Path.GetFileName(System.String),
        /// minus the last period (.) and all characters following it.
        /// </returns>
        /// <exception cref="System.ArgumentException">path contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars().</exception>
        public string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Creates all directories and subdirectories as specified by path.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        /// <returns>
        /// A System.IO.DirectoryInfo as specified by path.
        /// </returns>
        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }
    }
}