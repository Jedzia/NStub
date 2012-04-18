// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpProjectGenerator.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    using NStub.Core;

    /// <summary>
    /// The CSharpProjectGenerator class is responsible for writing the XML which
    /// will create the project file.  This class ensures that all necessary
    /// resources as well as all necessary references are properly included.
    /// </summary>
    public class CSharpProjectGenerator : IProjectGenerator
    {
        #region Fields

        private readonly IBuildSystem buildSystem;
        private string prjOutputDirectory;
        private string projectName;
        private IList<AssemblyName> referencedAssemblies;
        private XmlWriter xmlWriter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpProjectGenerator"/> class
        /// within the given <paramref name="projectName"/> which will output to the given
        /// <paramref name="outputDirectory"/>.
        /// </summary>
        /// <param name="buildSystem">The build system.</param>
        /// <param name="projectName">The name of the project.</param>
        /// <param name="outputDirectory">The directory where the project
        /// will be output.</param>
        /// <exception cref="System.ArgumentNullException">Either <paramref name="projectName"/> or
        /// <paramref name="outputDirectory"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">Either <paramref name="projectName"/> or
        /// <paramref name="outputDirectory"/> is empty.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"><paramref name="outputDirectory"/>
        /// cannot be found.</exception>
        /// <exception cref="ApplicationException"><c>ApplicationException</c> Directory Cannot Be Found.</exception>
        public CSharpProjectGenerator(IBuildSystem buildSystem, string projectName, string outputDirectory)
        {
            Guard.NotNull(() => buildSystem, buildSystem);
            this.buildSystem = buildSystem;

            // Null arguments will not be accepted
            if (projectName == null)
            {
                throw new ArgumentNullException(
                    "projectName",
                    Exceptions.ParameterCannotBeNull);
            }

            if (outputDirectory == null)
            {
                throw new ArgumentNullException(
                    "outputDirectory",
                    Exceptions.ParameterCannotBeNull);
            }

            // Empty arguments will not be accepted
            if (projectName.Length == 0)
            {
                throw new ArgumentException(
                    Exceptions.StringCannotBeEmpty,
                    "projectName");
            }

            if (outputDirectory.Length == 0)
            {
                throw new ArgumentException(
                    Exceptions.StringCannotBeEmpty,
                    "outputDirectory");
            }

            // Ensure that the output directory is valid
            if (!this.buildSystem.DirectoryExists(outputDirectory))
            {
                throw new ApplicationException(Exceptions.DirectoryCannotBeFound);
            }

            // Set our member variables
            this.prjOutputDirectory = outputDirectory;
            this.projectName = projectName;

            // Set our collection member variables
            this.ClassFiles = new List<string>();

            // We know that we'll need a reference to the NUnit framework, so
            // let's go ahead and add it
            this.referencedAssemblies = new List<AssemblyName> { new AssemblyName("NUnit.Framework") };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the class files which will be included in the project.
        /// </summary>
        /// <value>The class files which will be included in the project.</value>
        public IList<string> ClassFiles { get; set; }

        /// <summary>
        /// Gets or sets the directory where the project will be output to.
        /// </summary>
        /// <value>The directory where the project will be output to.</value>
        public string OutputDirectory
        {
            get
            {
                return this.prjOutputDirectory;
            }

            set
            {
                this.prjOutputDirectory = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        public string ProjectName
        {
            get
            {
                return this.projectName;
            }

            set
            {
                this.projectName = value;
            }
        }

        /// <summary>
        /// Gets or sets the assemblies which will be referenced in the project.
        /// Any duplicate references found in this list will be removed at generation time.
        /// </summary>
        /// <value>The assemblies which will be referenced by the project.</value>
        public IList<AssemblyName> ReferencedAssemblies
        {
            get
            {
                return this.referencedAssemblies;
            }

            set
            {
                this.referencedAssemblies = value;
            }
        }

        #endregion

        /// <summary>
        /// Generates the project file.  This method is responsible for actually
        /// generating the XML which will represent the project as well including
        /// all necessary resources and references.
        /// </summary>
        public void GenerateProjectFile()
        {
            // Create our XmlWriter according to our specified settings
            var xmlWriterSettings = new XmlWriterSettings { OmitXmlDeclaration = true };

            this.xmlWriter = XmlWriter.Create(
                this.prjOutputDirectory +
                this.buildSystem.DirectorySeparatorChar + this.projectName + ".csproj",
                xmlWriterSettings);

            // Specify the scheams we will be using 
            this.xmlWriter.WriteStartElement("Project", @"http://schemas.microsoft.com/developer/msbuild/2003");
            this.xmlWriter.WriteAttributeString("DefaultTargets", "Build");
            this.xmlWriter.WriteAttributeString("xmlns", @"http://schemas.microsoft.com/developer/msbuild/2003");

            // Write our configuration elements
            this.WritePropertyGroupElement();
            this.WritePropertyGroupElement("Debug", "AnyCPU");
            this.WritePropertyGroupElement("Release", "AnyCPU");

            // Write the items to our project
            this.AddReferencedAssemblies();
            this.AddClassFiles();
            this.AddDefaultTarget();

            this.xmlWriter.Close();
        }

        /// <summary>
        /// Adds an ItemGroup to the project file which includes the class files which will be
        /// part of the project.
        /// </summary>
        private void AddClassFiles()
        {
            // Add our class files
            this.xmlWriter.WriteStartElement("ItemGroup");
            foreach (string classFile in this.ClassFiles)
            {
                string scrubbedClassFile =
                    Utility.ScrubPathOfIllegalCharacters(classFile);
                this.xmlWriter.WriteStartElement("Compile");
                this.xmlWriter.WriteAttributeString("Include", scrubbedClassFile);
                this.xmlWriter.WriteEndElement(); // Compile
            }

            this.xmlWriter.WriteEndElement(); // ItemGroup
        }

        /// <summary>
        /// Writes the default target to the project file.
        /// </summary>
        private void AddDefaultTarget()
        {
            // Add a default target
            this.xmlWriter.WriteStartElement("Import");
            this.xmlWriter.WriteAttributeString("Project", @"$(MSBuildBinPath)\Microsoft.CSharp.targets");
            this.xmlWriter.WriteEndElement();

            this.xmlWriter.WriteEndElement(); // Project
        }

        /// <summary>
        /// Adds an ItemGroup to the project file which includes the list of assemblies
        /// which will be referenced as part of the project.
        /// </summary>
        private void AddReferencedAssemblies()
        {
            // This list will keep track of assemblies we've already referenced so as not
            // to add a duplicate
            var assembliesAlreadyReferenced =
                new List<AssemblyName>(this.referencedAssemblies.Count);

            // Add our referenced assemblies
            this.xmlWriter.WriteStartElement("ItemGroup");
            foreach (AssemblyName referencedAssembly in this.referencedAssemblies)
            {
                // Only add this assembly to the References group if we haven't added
                // it already
                if (!assembliesAlreadyReferenced.Contains(referencedAssembly))
                {
                    this.xmlWriter.WriteStartElement("Reference");
                    this.xmlWriter.WriteAttributeString(
                        "Include",
                        referencedAssembly.FullName);
                    this.xmlWriter.WriteEndElement(); // Reference

                    assembliesAlreadyReferenced.Add(referencedAssembly);
                }
            }

            this.xmlWriter.WriteEndElement(); // ItemGroup
        }

        /// <summary>
        /// Writes a Configuration element to the project file specifying the Debug
        /// configuration.
        /// </summary>
        private void WriteDebugConfiguration()
        {
            // Debug configuration
            this.xmlWriter.WriteStartElement("PropertyGroup");
            this.xmlWriter.WriteAttributeString("Condition", " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ");
            this.xmlWriter.WriteElementString("DebugSymbols", "true");
            this.xmlWriter.WriteElementString("DebugType", "full");
            this.xmlWriter.WriteElementString("Optimize", "false");
            this.xmlWriter.WriteElementString("OutputPath", @"bin\Debug\");
            this.xmlWriter.WriteElementString("DefineConstants", "DEBUG;TRACE");
            this.xmlWriter.WriteElementString("ErrorReport", "prompt");
            this.xmlWriter.WriteElementString("WarningLevel", "4");
            this.xmlWriter.WriteEndElement(); // PropertyGroup
        }

        /// <summary>
        /// Writes a Configuration element to the project file specifying the default
        /// configuration.
        /// </summary>
        private void WriteDefaultConfiguration()
        {
            // Default configuration (debug)
            this.xmlWriter.WriteStartElement("PropertyGroup");

            this.xmlWriter.WriteStartElement("Configuration");
            this.xmlWriter.WriteAttributeString(
                "Condition",
                " '$(Configuration)' == '' ");
            this.xmlWriter.WriteValue("Debug");
            this.xmlWriter.WriteEndElement(); // Configuration

            this.xmlWriter.WriteStartElement("ProductVersion");
            this.xmlWriter.WriteEndElement(); // ProductVersion

            this.xmlWriter.WriteStartElement("SchemaVersion");
            this.xmlWriter.WriteEndElement(); // SchemaVersion

            this.xmlWriter.WriteStartElement("ProjectGuid");
            this.xmlWriter.WriteEndElement(); // ProjectGuid

            this.xmlWriter.WriteStartElement("OutputType");
            this.xmlWriter.WriteValue("Library");
            this.xmlWriter.WriteEndElement(); // OutputType

            this.xmlWriter.WriteStartElement("AppDesignerFolder");
            this.xmlWriter.WriteEndElement(); // AppDesignerFolder

            this.xmlWriter.WriteStartElement("RootNamespace");
            this.xmlWriter.WriteEndElement(); // RootNamespace

            this.xmlWriter.WriteStartElement("AssemblyName");
            this.xmlWriter.WriteValue(this.projectName);
            this.xmlWriter.WriteEndElement(); // AssemblyName

            this.xmlWriter.WriteEndElement(); // PropertyGroup
        }

        /// <summary>
        /// Writes an empty property group element to the project file.
        /// </summary>
        private void WritePropertyGroupElement()
        {
            this.WritePropertyGroupElement(string.Empty, string.Empty);
        }

        /// <summary>
        /// Writes a property group element with the given configuration and
        /// platform to the project file.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="platform">The platform.</param>
        private void WritePropertyGroupElement(string configuration, string platform)
        {
            this.xmlWriter.WriteWhitespace(Environment.NewLine);

            if ((configuration.Length == 0) && (platform.Length == 0))
            {
                this.xmlWriter.WriteWhitespace(Environment.NewLine);
                this.WriteDefaultConfiguration();
            }
            else if (string.Equals(configuration, "Debug", StringComparison.InvariantCultureIgnoreCase) &&
                     string.Equals(platform, "AnyCPU", StringComparison.InvariantCultureIgnoreCase))
            {
                this.WriteDebugConfiguration();
            }
            else if (string.Equals(configuration, "Release", StringComparison.InvariantCultureIgnoreCase) &&
                     string.Equals(platform, "AnyCPU", StringComparison.InvariantCultureIgnoreCase))
            {
                this.WriteReleaseConfiguration();
            }

            this.xmlWriter.WriteWhitespace(Environment.NewLine);
        }

        /// <summary>
        /// Writes a Configuration element to the project file specifying the Release
        /// configuration.
        /// </summary>
        private void WriteReleaseConfiguration()
        {
            // Release configuration
            this.xmlWriter.WriteStartElement("PropertyGroup");
            this.xmlWriter.WriteAttributeString("Condition", " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ");
            this.xmlWriter.WriteElementString("DebugType", "pdbOnly");
            this.xmlWriter.WriteElementString("Optimize", "true");
            this.xmlWriter.WriteElementString("OutputPath", @"bin\Release\");
            this.xmlWriter.WriteElementString("DefineConstants", "TRACE");
            this.xmlWriter.WriteElementString("ErrorReport", "prompt");
            this.xmlWriter.WriteElementString("WarningLevel", "4");
            this.xmlWriter.WriteEndElement(); // PropertyGroup
        }
    }
}