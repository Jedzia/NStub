using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.Reflection;
using NStub.CSharp;

namespace NStub.Core
{
    public struct TestNode
    {
        public object Tag;
        public string Text;
        public IList<TestNode> Nodes;
        public bool Checked;
    }

    /// <summary>
    /// Helds data for running the <see cref="TestBuilder"/>.
    /// </summary>
    public class GeneratorRunnerData
    {
        /// <summary>
        /// Gets the output folder.
        /// </summary>
        public string OutputFolder { get; private set; }

        /// <summary>
        /// Gets the type of the generator.
        /// </summary>
        /// <value>
        /// The type of the generator.
        /// </value>
        public Type GeneratorType { get; private set; }

        /// <summary>
        /// Gets the input assembly path.
        /// </summary>
        public string InputAssemblyPath { get; private set; }

        /// <summary>
        /// Gets the root nodes.
        /// </summary>
        public IList<TestNode> RootNodes { get; private set; }

        /// <summary>
        /// Gets the list of referenced assemblies.
        /// </summary>
        public IList<AssemblyName> ReferencedAssemblies { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorRunnerData"/> class.
        /// </summary>
        /// <param name="outputFolder">The output folder.</param>
        /// <param name="generatorType">Type of the generator.</param>
        /// <param name="inputAssemblyPath">The input assembly path.</param>
        /// <param name="mainNodes">The main nodes.</param>
        /// <param name="referencedAssemblies">The list of referenced assemblies.</param>
        public GeneratorRunnerData(
            string outputFolder,
            Type generatorType,
            string inputAssemblyPath,
            IList<TestNode> mainNodes,
            IList<AssemblyName> referencedAssemblies)
        {
            Guard.NotNullOrEmpty(() => outputFolder, outputFolder);
            this.OutputFolder = outputFolder;
            Guard.NotNull(() => generatorType, generatorType);
            this.GeneratorType = generatorType;
            Guard.NotNullOrEmpty(() => inputAssemblyPath, inputAssemblyPath);
            this.InputAssemblyPath = inputAssemblyPath;
            Guard.NotNull(() => mainNodes, mainNodes);
            this.RootNodes = mainNodes;
            Guard.NotNull(() => referencedAssemblies, referencedAssemblies);
            this.ReferencedAssemblies = referencedAssemblies;
        }
    }

    /// <summary>
    /// Builds the test files.
    /// </summary>
    public class TestProjectBuilder
    {
        private readonly IBuildSystem sbs;
        private readonly Action<string> logger;
        private readonly IProjectGenerator cSharpProjectGenerator;
        private readonly Func<IBuildSystem, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator> getGenerator;
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyGraphBuilder"/> class.
        /// </summary>
        /// <param name="sbs">The build system.</param>
        /// <param name="logger">The logging method.</param>
        public TestProjectBuilder(
            IBuildSystem sbs, 
            IProjectGenerator projectGenerator,
            Func<IBuildSystem, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator> getGenerator, 
            Action<string> logger)
        {
            Guard.NotNull(() => sbs, sbs);
            Guard.NotNull(() => projectGenerator, projectGenerator);
            Guard.NotNull(() => getGenerator, getGenerator);
            this.sbs = sbs;
            //Guard.NotNull(() => logger, logger);
            this.logger = logger;
            this.cSharpProjectGenerator = projectGenerator;
            this.getGenerator = getGenerator;

    /*new CSharpProjectGenerator(
        sbs,
        Path.GetFileNameWithoutExtension(inputAssemblyPath) + ".Tests",
        outputDirectory);*/

        }

        /// <summary>
        /// Generates the tests.
        /// </summary>
        /// <param name="data">The data that is neccessary for test building.</param>
        public void GenerateTests(GeneratorRunnerData data)
        {
            Guard.NotNull(() => data, data);
            this.GenerateTests(data.OutputFolder, data.GeneratorType, data.InputAssemblyPath, data.RootNodes, data.ReferencedAssemblies);
        }

        /// <summary>
        /// Creates a list of the methods
        /// for which the user wishes to generate test cases for and instantiates an
        /// instance of NStub around these methods.  The sources are files are then
        /// generated.
        /// </summary>
        /// <param name="outputFolder">The output folder.</param>
        /// <param name="generatorType">Type of the generator.</param>
        /// <param name="inputAssemblyPath">The input assembly path.</param>
        /// <param name="mainNodes">The main nodes.</param>
        /// <param name="referencedAssemblies">The list of referenced assemblies.</param>
        public void GenerateTests(
     string outputFolder,
     Type generatorType,
     string inputAssemblyPath,
     IList<TestNode> mainNodes,
     IList<AssemblyName> referencedAssemblies)
        {
            // string outputFolder = this._outputDirectoryTextBox.Text;
            // Type generatorType = (Type)cbGenerators.SelectedItem;
            // string inputAssemblyPath = this._inputAssemblyTextBox.Text;
            // //TreeNodeCollection mainNodes = this._assemblyGraphTreeView.Nodes;
            // IList<TreeNode> mainNodes = this._assemblyGraphTreeView.Nodes.Cast<TreeNode>().ToList();
            // IList<AssemblyName> referencedAssemblies = this._referencedAssemblies;

            // Create a new directory for each assembly
            for (int h = 0; h < mainNodes.Count; h++)
            {
                var mainnode = mainNodes[h];
                string outputDirectory = outputFolder +
                                         sbs.DirectorySeparatorChar +
                                         sbs.GetFileNameWithoutExtension(mainnode.Text) +
                                         ".Tests";
                sbs.CreateDirectory(outputDirectory);

                // Create our project generator

                // Add our referenced assemblies to the project generator so we
                // can build the resulting project
                foreach (AssemblyName assemblyName in referencedAssemblies)
                {
                    cSharpProjectGenerator.ReferencedAssemblies.Add(assemblyName);
                }

                // Generate the new test namespace
                // At the assembly level
                for (int i = 0; i < mainnode.Nodes.Count; i++)
                {
                    var rootsubnode = mainnode.Nodes[i];

                    if (rootsubnode.Checked)
                    {
                        // Create the namespace and initial inputs
                        CodeNamespace codeNamespace = new CodeNamespace();

                        // At the type level
                        for (int j = 0; j < rootsubnode.Nodes.Count; j++)
                        {
                            var rootsubsubnode = rootsubnode.Nodes[j];
                            // TODO: This namespace isn't being set correctly.  
                            // Also one namespace per run probably won't work, we may 
                            // need to break this up more.
                            codeNamespace.Name =
                                Utility.GetNamespaceFromFullyQualifiedTypeName(rootsubsubnode.Text);

                            if (rootsubsubnode.Checked)
                            {
                                // Create the class
                                CodeTypeDeclaration testClass = new CodeTypeDeclaration(rootsubsubnode.Text);
                                codeNamespace.Types.Add(testClass);
                                var testObjectClassType = (Type)rootsubsubnode.Tag;
                                testClass.UserData.Add("TestObjectClassType", testObjectClassType);

                                // At the method level
                                // Create a test method for each method in this type
                                for (int k = 0;
                                     k < rootsubsubnode.Nodes.Count;
                                     k++)
                                {
                                    var rootsubsubsubnode = rootsubsubnode.Nodes[k];
                                    try
                                    {
                                        if (rootsubsubsubnode.Checked)
                                        {
                                            try
                                            {
                                                // Retrieve the MethodInfo object from this TreeNode's tag
                                                var memberInfo =
                                                    (MethodInfo)
                                                    rootsubsubsubnode.Tag;
                                                CodeMemberMethod codeMemberMethod =
                                                    this.CreateMethod(rootsubsubsubnode.Text, memberInfo);
                                                codeMemberMethod.UserData.Add("MethodMemberInfo", memberInfo);
                                                testClass.Members.Add(codeMemberMethod);
                                            }
                                            catch (Exception ex)
                                            {
                                                // MessageBox.Show(ex.Message + Environment.NewLine + ex.ToString());
                                                if (this.logger != null)
                                                {
                                                    this.logger(ex.Message + Environment.NewLine + ex + Environment.NewLine);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        return;
                                    }
                                }
                            }
                        }

                        WriteTestFile(generatorType, outputDirectory, cSharpProjectGenerator, codeNamespace);
                    }
                }

                // Now write the project file and add all of the test files to it
                cSharpProjectGenerator.GenerateProjectFile();
            }
        }


        private void WriteTestFile(Type generatorType, string outputDirectory, IProjectGenerator cSharpProjectGenerator, CodeNamespace codeNamespace)
        {
            // Now write the test file 
            // NStubCore nStub =
            // new NStubCore(codeNamespace, outputDirectory,
            // new CSharpCodeGenerator(codeNamespace, outputDirectory));
            // NStubCore nStub =
            // new NStubCore(codeNamespace, outputDirectory,
            // new CSharpMbUnitCodeGenerator(codeNamespace, outputDirectory));
            //var nStub =
            //   new NStubCore(
            //       codeNamespace,
            //      outputDirectory,
            //       new CSharpMbUnitRhinoMocksCodeGenerator(codeNamespace, outputDirectory));

            //var testBuilders = new TestBuilderFactory(new PropertyBuilder(), new EventBuilder(), new MethodBuilder());
            var configuration = new CodeGeneratorParameters(outputDirectory);
            //var testBuilders = new TestBuilderFactory();
            //var codeGenerator = (ICodeGenerator)Activator.CreateInstance(generatorType, new object[]
              //           {
                //             sbs, codeNamespace, testBuilders, configuration
                  //       });
            var codeGenerator = getGenerator(sbs, configuration, codeNamespace);

            //codeNamespace.Dump(3);
            var nStub = new NStubCore(codeNamespace, outputDirectory, codeGenerator);
            nStub.GenerateCode();

            // Todo: some of this shit should be done by the core itself.

            // Add all of our classes to the project
            foreach (CodeTypeDeclaration codeType in nStub.CodeNamespace.Types)
            {
                string fileName = codeType.Name;
                fileName = fileName.Remove(0, fileName.LastIndexOf(".") + 1);
                fileName += ".cs";
                cSharpProjectGenerator.ClassFiles.Add(fileName);
                if (this.logger != null)
                {
                    this.logger("Writing '" + fileName + "'");
                }
            }

        }

        private CodeMemberMethod CreateMethod(string methodName, MethodInfo methodInfo)
        {
            // Create the method
            CodeMemberMethod codeMemberMethod = new CodeMemberMethod();

            codeMemberMethod.Attributes = (MemberAttributes)methodInfo.Attributes;
            codeMemberMethod.Name = methodName;

            // Set the return type for the method
            codeMemberMethod.ReturnType = new CodeTypeReference(methodInfo.ReturnType);

            // Setup and add the parameters
            ParameterInfo[] methodParameters = methodInfo.GetParameters();
            foreach (ParameterInfo parameter in methodParameters)
            {
                codeMemberMethod.Parameters.Add(
                    new CodeParameterDeclarationExpression(
                        parameter.ParameterType,
                        parameter.Name));
            }

            return codeMemberMethod;
        }

        private CodeNamespace CreateNamespace(TestNode treeNode)
        {
            return null;
        }

    }

}
