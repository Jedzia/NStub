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

    /// <summary>
    /// Implementation of a hosting system with infrastructure for filesystem access and other low level functionality.
    /// </summary>
    public class StandardBuildSystem : BuildSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardBuildSystem"/> class.
        /// </summary>
        public StandardBuildSystem(/*string outputDirectory*/)
            : base(/*outputDirectory*/)
        {

        }
    }

}
