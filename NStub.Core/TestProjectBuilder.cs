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
    using System.Threading;

    /// <summary>
    /// Builds the test files.
    /// </summary>
    public class TestProjectBuilder
    {
        #region Fields

        private readonly Func<IBuildSystem, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator> createGeneratorCallback;

        private readonly IProjectGenerator csharpProjectGenerator;

        private readonly Action<string> logger;
        private readonly IBuildSystem sbs;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestProjectBuilder"/> class.
        /// </summary>
        /// <param name="sbs">The system wide build system.</param>
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
        /// Gets or sets the user provided code generator parameters.
        /// </summary>
        public ICodeGeneratorSetup CustomGeneratorParameters
        {
            get;
            set;
        }

        /// <summary>
        /// Generates the tests.
        /// </summary>
        /// <param name="data">The data that is necessary for test building.</param>
        public void GenerateTests(GeneratorRunnerData data)
        {
            Guard.NotNull(() => data, data);
            this.GenerateTests(
                data.OutputFolder, data.GeneratorType, data.InputAssemblyPath, data.RootNodes, data.ReferencedAssemblies);
        }

        /// <summary>
        /// Creates a list of the methods
        /// for which the user wishes to generate test cases for and instantiates an
        /// instance of <c>NStub</c> around these methods.  The sources are files are then
        /// generated.
        /// </summary>
        /// <param name="outputFolder">The output folder.</param>
        /// <param name="generatorType">Type of the code generator to use.</param>
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
            if (this.logger != null)
            {
                this.logger("-----------------------------------------------------------------------------");
                this.logger("Generating Test Project '" + this.csharpProjectGenerator.ProjectName + "'");
                this.logger("-----------------------------------------------------------------------------");
            }

            // Create a new directory for each assembly
            foreach (var assemblyNode in mainNodes)
            {
                string calculatedOutputDirectory = outputFolder +
                                         this.sbs.DirectorySeparatorChar +
                                         this.sbs.GetFileNameWithoutExtension(assemblyNode.Text) +
                                         ".Tests";
                this.sbs.CreateDirectory(calculatedOutputDirectory);

                // Create our project generator

                // Add our referenced assemblies to the project generator so we
                // can build the resulting project
                foreach (AssemblyName assemblyName in referencedAssemblies)
                {
                    this.csharpProjectGenerator.ReferencedAssemblies.Add(assemblyName);
                }

                // Generate the new test namespace
                // At the assembly level
                foreach (var moduleNode in assemblyNode.Nodes)
                {
                    if (!moduleNode.Checked)
                    {
                        // continue;
                    }

                    // Create the namespace and initial inputs
                    var codeNamespace = new CodeNamespace();

                    // At the type level
                    foreach (var classNode in moduleNode.Nodes)
                    {
                        // TODO: This namespace isn't being set correctly.  
                        // Also one namespace per run probably won't work, we may 
                        // need to break this up more.
                        codeNamespace.Name =
                            Utility.GetNamespaceFromFullyQualifiedTypeName(classNode.Text);

                        if (!classNode.Checked)
                        {
                            continue;
                        }

                        Thread.Sleep(1);
                        if (this.logger != null)
                        {
                            this.logger("Building '" + classNode.Text + "'");
                        }

                        // Create the class
                        var testClass = new CodeTypeDeclaration(classNode.Text);
                        codeNamespace.Types.Add(testClass);
                        var testObjectClassType = classNode.ClrType;
                        testClass.UserData.Add(NStubConstants.UserDataClassTypeKey, testObjectClassType);

                        // At the method level
                        // Create a test method for each method in this type
                        foreach (var classmemberNode in classNode.Nodes)
                        {
                            if (!classmemberNode.Checked)
                            {
                                continue;
                            }

                            try
                            {
                                // Retrieve the MethodInfo object from this TreeNode's tag
                                // var memberInfo = (MethodInfo)rootsubsubsubnode.Tag;
                                var memberInfo = classmemberNode.MethodInfo;
                                CodeMemberMethod codeMemberMethod =
                                    this.CreateMethod(classmemberNode.Text, memberInfo);
                                codeMemberMethod.UserData.Add(NStubConstants.TestMemberMethodInfoKey, memberInfo);
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

                    this.WriteTestFile(calculatedOutputDirectory, codeNamespace);
                }

                // Now write the project file and add all of the test files to it
                this.csharpProjectGenerator.GenerateProjectFile();
            }

            if (this.logger != null)
            {
                this.logger("-----------------------------------------------------------------------------");
                this.logger("Finished generating Project '" + this.csharpProjectGenerator.ProjectName + "'");
                this.logger("-----------------------------------------------------------------------------");
            }
        }

        /// <summary>
        /// Called when the <c>TestProjectBuilder</c> is about to create the code generator.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="buildSystem">The build system.</param>
        /// <returns>An with the parameters initialized code generator.</returns>
        /// <remarks>Override this method to instantiate others than the standard generators.
        /// <example>The default implementation is:
        /// <code><![CDATA[
        /// protected virtual ICodeGenerator OnCreateCodeGenerator(CodeNamespace codeNamespace, CodeGeneratorParameters configuration, IBuildSystem buildSystem)
        /// {
        ///     var codeGenerator = this.createGeneratorCallback(buildSystem, configuration, codeNamespace);
        ///     return codeGenerator;
        /// }
        /// ]]></code></example>
        /// </remarks>
        protected virtual ICodeGenerator OnCreateCodeGenerator(
            CodeNamespace codeNamespace, ICodeGeneratorParameters configuration, IBuildSystem buildSystem)
        {
            var codeGenerator = this.createGeneratorCallback(buildSystem, configuration, codeNamespace);
            return codeGenerator;
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

        /// <summary>
        /// Writes the test file.
        /// </summary>
        /// <param name="calculatedOutputDirectory">The calculated output directory.</param>
        /// <param name="codeNamespace">The code namespace.</param>
        private void WriteTestFile(
            string calculatedOutputDirectory, CodeNamespace codeNamespace)
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

            ICodeGeneratorParameters configuration = null;
            if (this.CustomGeneratorParameters == null)
            {
                configuration = new CodeGeneratorParameters(calculatedOutputDirectory);
            }
            else
            {
                configuration = new CodeGeneratorParameters(this.CustomGeneratorParameters, calculatedOutputDirectory);
            }
            

            // var testBuilders = new TestBuilderFactory();
            // var codeGenerator = (ICodeGenerator)Activator.CreateInstance(generatorType, new object[]
            // {
            // sbs, codeNamespace, testBuilders, configuration
            // });
            var buildSystem = this.sbs;
            var codeGenerator = this.OnCreateCodeGenerator(codeNamespace, configuration, buildSystem);

            // codeNamespace.Dump(3);
            var nstub = new NStubCore(buildSystem, codeNamespace, calculatedOutputDirectory, codeGenerator);
            nstub.GenerateCode();

            // Add all of our classes to the project
            foreach (CodeTypeDeclaration codeType in nstub.CodeNamespace.Types)
            {
                string fileName = codeType.Name;
                fileName = fileName.Remove(0, fileName.LastIndexOf(".") + 1);
                fileName += ".cs";
                this.csharpProjectGenerator.ClassFiles.Add(fileName);
                Thread.Sleep(1);
                if (this.logger != null)
                {
                    this.logger("Writing '" + fileName + "'");
                }
            }
        }
    }
}