using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using NStub.Core;
using NStub.CSharp;
using NUnit.Framework;
using NStub.CSharp.ObjectGeneration;

namespace NStub.CSharp
{
	/// <summary>
	/// The CSharpCodeGenerator is responsible for the generation of the individual
	/// class files which will make up the actual test project.  For information
	/// regarding the generation of the project file, see 
	/// <see cref="CSharpProjectGenerator">CSharpProjectGenerator</see>.
	/// </summary>
	public class CSharpCodeGenerator : Core.ICodeGenerator
	{
		#region Fields (Private)

		private CodeNamespace _codeNamespace;
		private string _outputDirectory;

		#endregion Fields (Private)

		#region Constructor (Public)
        private readonly IBuildSystem buildSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpCodeGenerator"/> class
        /// based the given CodeNamespace which will output to the given directory.
        /// </summary>
        /// <param name="buildSystem">The build system.</param>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="testBuilders">The test builders.</param>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="System.ArgumentNullException">codeNamepsace or
        /// outputDirectory is null.</exception>
        ///   
        /// <exception cref="System.ArgumentException">outputDirectory is an
        /// empty string.</exception>
        ///   
        /// <exception cref="System.IO.DirectoryNotFoundException">outputDirectory
        /// cannot be found.</exception>
        public CSharpCodeGenerator(
            IBuildSystem buildSystem,
            CodeNamespace codeNamespace, 
            ITestBuilderFactory testBuilders,
            ICodeGeneratorParameters configuration)
		{
			#region Validation

            Guard.NotNull(() => buildSystem, buildSystem);
            this.buildSystem = buildSystem;
            Guard.NotNull(() => configuration, configuration);
            //this.configuration = configuration;
            string outputDirectory = configuration.OutputDirectory;

			// Null arguments will not be accepted
			if (codeNamespace == null)
			{
				throw new ArgumentNullException("codeNamespace",
					Exceptions.ParameterCannotBeNull);
			}
			if (outputDirectory == null)
			{
				throw new ArgumentNullException("outputDirectory",
					Exceptions.ParameterCannotBeNull);
			}
			// Ensure that the output directory is not empty
			if (outputDirectory.Length == 0)
			{
				throw new ArgumentException(Exceptions.StringCannotBeEmpty,
					"outputDirectory");
			}
			// Ensure that the output directory is valid
            if (!this.buildSystem.DirectoryExists(outputDirectory))
            {
                throw new ApplicationException(Exceptions.DirectoryCannotBeFound);
            }

			#endregion Validation

			_codeNamespace = codeNamespace;
			_outputDirectory = outputDirectory;
		}

		#endregion Constructor (Public)

		#region Properties (Public)

		/// <summary>
		/// Gets or sets the directory the new sources files will be output to.
		/// </summary>
		/// <value>The directory the new source files will be output to.</value>
		public string OutputDirectory
		{
			get
			{
				return _outputDirectory;
			}
			set
			{
				_outputDirectory = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="System.CodeDom.CodeNamespace"/> object 
		/// the generator is currently working from.
		/// </summary>
		/// <value>The <see cref="System.CodeDom.CodeNamespace"/> object the 
		/// generator is currently working from.</value>
		public CodeNamespace CodeNamespace
		{
			get
			{
				return _codeNamespace;
			}
			set
			{
				_codeNamespace = value;
			}
		}

		#endregion Properties (Public)

		#region Methods (Public)

		/// <summary>
		/// This methods actually performs the code generation for the
		/// file current <see cref="System.CodeDom.CodeNamespace">CodeNamespace</see>. 
		/// All classes within the namespace will have exactly one file generated for them.  
		/// </summary>
		public void GenerateCode()
		{
			// We want to write a separate file for each type
			foreach (CodeTypeDeclaration codeTypeDeclaration in _codeNamespace.Types)
			{
				// Create a namespace for the Type in order to put it in scope
				CodeNamespace codeNamespace =
					new CodeNamespace((_codeNamespace.Name));

				// Clean the type name
				codeTypeDeclaration.Name =
					Utility.ScrubPathOfIllegalCharacters(codeTypeDeclaration.Name);

				// Create our test type
				codeTypeDeclaration.Name =
					Utility.GetUnqualifiedTypeName(codeTypeDeclaration.Name) + "Test";
				codeTypeDeclaration.IsPartial = true;

				// Give it a default public constructor
				CodeConstructor codeConstructor = new CodeConstructor();
				codeConstructor.Attributes = MemberAttributes.Public;
				codeTypeDeclaration.Members.Add(codeConstructor);

				// Set out member names correctly
				foreach (CodeTypeMember typeMember in codeTypeDeclaration.Members)
				{
					if (typeMember is CodeMemberMethod)
					{
						// We don't generate default constructors
						if (!(typeMember is CodeConstructor))
						{
							CreateStubForCodeMemberMethod(typeMember as CodeMemberMethod);
						}
					}
				}
				codeNamespace.Types.Add(codeTypeDeclaration);

				RemoveDuplicatedMembers(codeTypeDeclaration);

				WriteClassFile(codeTypeDeclaration.Name, codeNamespace);
			}
		}

		#endregion Methods (Public)

		#region Helper Methods (Private)

		/// <summary>
		/// Since types can contain multiple overloads of the same method, once
		/// we remove the parameters from every method our type may have the 
		/// many duplicates of the same method.  This method removes those
		/// duplicates.
		/// </summary>
		/// <param name="codeTypeDeclaration">The <see cref="CodeTypeDeclaration"/>
		/// from which to remove the duplicates.</param>
		private static void RemoveDuplicatedMembers(CodeTypeDeclaration codeTypeDeclaration)
		{
			for (int i = 0; i < codeTypeDeclaration.Members.Count; ++i)
			{
				int occurrences = 0;
				for (int j = 0; j < codeTypeDeclaration.Members.Count; ++j)
				{
					// Compare each CodeTypeMember to all other CodeTypeMembers
					// in its type.  If more than one match is found (the 
					// codeTypeMember matching itself) then we remove that match.
					if (codeTypeDeclaration.Members[i].Name.Equals(
						codeTypeDeclaration.Members[j].Name))
					{
						occurrences++;
						if (occurrences > 1)
						{
							codeTypeDeclaration.Members.Remove(codeTypeDeclaration.Members[j]);
						}
					}
				}
			}
		}

		/// <summary>
		/// Creates the stub for the code member method.  This method actually
		/// implements the method body for the test method.
		/// </summary>
		/// <param name="codeMemberMethod">The code member method.</param>
		private static void CreateStubForCodeMemberMethod(CodeMemberMethod codeMemberMethod)
		{
			// Clean the member name and append 'Test' to the end of it
			codeMemberMethod.Name = Utility.ScrubPathOfIllegalCharacters(codeMemberMethod.Name);
			codeMemberMethod.Name = codeMemberMethod.Name + "Test";

			// Standard test methods accept no parameters and return void.
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(void));
			codeMemberMethod.Parameters.Clear();

			codeMemberMethod.CustomAttributes.Add(
					new CodeAttributeDeclaration(
						new CodeTypeReference(typeof(TestAttribute))));
			codeMemberMethod.CustomAttributes.Add(
					new CodeAttributeDeclaration(
					new CodeTypeReference(typeof(IgnoreAttribute))));
			codeMemberMethod.Statements.Add(
				new CodeCommentStatement("TODO: Implement unit test for " +
										 codeMemberMethod.Name));
		}

		/// <summary>
		/// Writes the class file.  This method actually creates the physical
		/// class file and populates it accordingly.
		/// </summary>
		/// <param name="className">Name of the class file to be written.</param>
		/// <param name="codeNamespace">The CodeNamespace which represents the
		/// file to be written.</param>
		private void WriteClassFile(string className, CodeNamespace codeNamespace)
		{
            var csharpCodeProvider = new CSharpCodeProvider();
            string sourceFile = this.OutputDirectory + this.buildSystem.DirectorySeparatorChar +
                                className + "." + csharpCodeProvider.FileExtension;
            sourceFile = Utility.ScrubPathOfIllegalCharacters(sourceFile);
            var indentedTextWriter =
                new IndentedTextWriter(this.buildSystem.GetTextWriter(sourceFile, false), "  ");
            var codeGenerationOptions = new CodeGeneratorOptions { BracingStyle = "C" };
            csharpCodeProvider.GenerateCodeFromNamespace(
                codeNamespace,
                indentedTextWriter,
                codeGenerationOptions);
            indentedTextWriter.Flush();
            indentedTextWriter.Close();
        }

		#endregion Helper Methods (Private)
	}
}
