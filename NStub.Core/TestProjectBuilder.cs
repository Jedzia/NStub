// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestProjectBuilder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Core
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Builds the test files.
    /// </summary>
    public class TestProjectBuilder
    {
        #region Fields

        private readonly Func<IBuildSystem, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator>
            createGeneratorCallback;

        private readonly IProjectGenerator csharpProjectGenerator;

        private readonly Action<string> logger;
        private readonly IBuildSystem sbs;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestProjectBuilder"/> class.
        /// </summary>
        /// <param name="sbs">The build system.</param>
        /// <param name="projectGenerator">The project generator.</param>
        /// <param name="createGeneratorCallback">The callback to create new code 
        /// generators per test class <see cref="CodeNamespace"/>.</param>
        /// <param name="logger">The logging method.</param>
        public TestProjectBuilder(
            IBuildSystem sbs,
            IProjectGenerator projectGenerator,
            Func<IBuildSystem, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator> createGeneratorCallback,
            Action<string> logger)
        {
            Guard.NotNull(() => sbs, sbs);
            Guard.NotNull(() => projectGenerator, projectGenerator);
            Guard.NotNull(() => createGeneratorCallback, createGeneratorCallback);

            // Guard.NotNull(() => logger, logger);
            this.sbs = sbs;
            this.logger = logger;
            this.csharpProjectGenerator = projectGenerator;
            this.createGeneratorCallback = createGeneratorCallback;
        }

        #endregion

        /// <summary>
        /// Generates the tests.
        /// </summary>
        /// <param name="data">The data that is necessary for test building.</param>
        public void GenerateTests(GeneratorRunnerData data)
        {
            Guard.NotNull(() => data, data);
            this.GenerateTests(
                data.OutputFolder, data.InputAssemblyPath, data.RootNodes, data.ReferencedAssemblies);
        }

        /// <summary>
        /// Creates a list of the methods
        /// for which the user wishes to generate test cases for and instantiates an
        /// instance of <c>NStub</c> around these methods.  The sources are files are then
        /// generated.
        /// </summary>
        /// <param name="outputFolder">The output folder.</param>
        /// <param name="inputAssemblyPath">The input assembly path.</param>
        /// <param name="mainNodes">The main nodes.</param>
        /// <param name="referencedAssemblies">The list of referenced assemblies.</param>
        public void GenerateTests(
            string outputFolder,
            // Type generatorType,
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
            foreach (var mainnode in mainNodes)
            {
                string outputDirectory = outputFolder +
                                         this.sbs.DirectorySeparatorChar +
                                         this.sbs.GetFileNameWithoutExtension(mainnode.Text) +
                                         ".Tests";
                this.sbs.CreateDirectory(outputDirectory);

                // Create our project generator

                // Add our referenced assemblies to the project generator so we
                // can build the resulting project
                foreach (AssemblyName assemblyName in referencedAssemblies)
                {
                    this.csharpProjectGenerator.ReferencedAssemblies.Add(assemblyName);
                }

                // Generate the new test namespace
                // At the assembly level
                foreach (var rootsubnode in mainnode.Nodes)
                {
                    if (!rootsubnode.Checked)
                    {
                        continue;
                    }

                    // Create the namespace and initial inputs
                    var codeNamespace = new CodeNamespace();

                    // At the type level
                    foreach (var rootsubsubnode in rootsubnode.Nodes)
                    {
                        // TODO: This namespace isn't being set correctly.  
                        // Also one namespace per run probably won't work, we may 
                        // need to break this up more.
                        codeNamespace.Name =
                            Utility.GetNamespaceFromFullyQualifiedTypeName(rootsubsubnode.Text);

                        if (!rootsubsubnode.Checked)
                        {
                            continue;
                        }
                            
                        // Create the class
                        var testClass = new CodeTypeDeclaration(rootsubsubnode.Text);
                        codeNamespace.Types.Add(testClass);
                        var testObjectClassType = rootsubsubnode.ClrType;
                        testClass.UserData.Add("TestObjectClassType", testObjectClassType);

                        // At the method level
                        // Create a test method for each method in this type
                        foreach (var rootsubsubsubnode in rootsubsubnode.Nodes)
                        {
                            try
                            {
                                if (rootsubsubsubnode.Checked)
                                {
                                    try
                                    {
                                        // Retrieve the MethodInfo object from this TreeNode's tag
                                        // var memberInfo = (MethodInfo)rootsubsubsubnode.Tag;
                                        var memberInfo = rootsubsubsubnode.MethodInfo;
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
                                            this.logger(
                                                ex.Message + Environment.NewLine + ex + Environment.NewLine);
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

                    this.WriteTestFile(outputDirectory, codeNamespace);
                }

                // Now write the project file and add all of the test files to it
                this.csharpProjectGenerator.GenerateProjectFile();
            }
        }

        private CodeMemberMethod CreateMethod(string methodName, MethodInfo methodInfo)
        {
            // Create the method
            var codeMemberMethod = new CodeMemberMethod
            {
                Attributes = (MemberAttributes)methodInfo.Attributes,
                Name = methodName,
                ReturnType = new CodeTypeReference(methodInfo.ReturnType)
            };

            // Set the return type for the method

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

        /*
                private CodeNamespace CreateNamespace(TestNode treeNode)
                {
                    return null;
                }
        */

        private void WriteTestFile(
            string outputDirectory, CodeNamespace codeNamespace)
        {
            // Now write the test file 
            // NStubCore nStub =
            // new NStubCore(codeNamespace, outputDirectory,
            // new CSharpCodeGenerator(codeNamespace, outputDirectory));
            // NStubCore nStub =
            // new NStubCore(codeNamespace, outputDirectory,
            // new CSharpMbUnitCodeGenerator(codeNamespace, outputDirectory));
            // var nStub =
            // new NStubCore(
            // codeNamespace,
            // outputDirectory,
            // new CSharpMbUnitRhinoMocksCodeGenerator(codeNamespace, outputDirectory));

            // var testBuilders = new TestBuilderFactory(new PropertyBuilder(), new EventBuilder(), new MethodBuilder());
            var configuration = new CodeGeneratorParameters(outputDirectory);

            // var testBuilders = new TestBuilderFactory();
            // var codeGenerator = (ICodeGenerator)Activator.CreateInstance(generatorType, new object[]
            // {
            // sbs, codeNamespace, testBuilders, configuration
            // });
            var codeGenerator = this.createGeneratorCallback(this.sbs, configuration, codeNamespace);

            // codeNamespace.Dump(3);
            var nstub = new NStubCore(codeNamespace, outputDirectory, codeGenerator);
            nstub.GenerateCode();

            // Add all of our classes to the project
            foreach (CodeTypeDeclaration codeType in nstub.CodeNamespace.Types)
            {
                string fileName = codeType.Name;
                fileName = fileName.Remove(0, fileName.LastIndexOf(".") + 1);
                fileName += ".cs";
                this.csharpProjectGenerator.ClassFiles.Add(fileName);
                if (this.logger != null)
                {
                    this.logger("Writing '" + fileName + "'");
                }
            }
        }
    }
}