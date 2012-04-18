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

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public TextWriter GetTextWriter(string path, bool append)
        {
            return new StreamWriter(path, append);
        }
    }
}