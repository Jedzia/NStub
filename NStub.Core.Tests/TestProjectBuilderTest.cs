namespace NStub.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core;
    using Rhino.Mocks;
    using System.CodeDom;
    using System.Reflection;
    using Rhino.Mocks.Constraints;


    [TestFixture()]
    public partial class TestProjectBuilderTest
    {

        private MockRepository mocks;
        private NStub.Core.IProjectGenerator projectGenerator;
        private NStub.Core.IBuildSystem sbs;
        private Func<IBuildSystem, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator> createGeneratorCallback;
        private Action<string> logger;
        private TestProjectBuilder testObject;

        private string logText;

        public int FakeMethod(string parameter1, int parameter2)
        {
            return 42;
        }

        [SetUp()]
        public void SetUp()
        {
            logText = string.Empty;
            this.mocks = new MockRepository();
            this.sbs = this.mocks.StrictMock<NStub.Core.IBuildSystem>();
            this.projectGenerator = this.mocks.StrictMock<NStub.Core.IProjectGenerator>();
            this.createGeneratorCallback = this.mocks.StrictMock<Func<IBuildSystem, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator>>();
            this.logger = this.mocks.StrictMock<Action<string>>();
            // this.logger = text => { this.logText += text; };
            this.testObject = new TestProjectBuilder(this.sbs, this.projectGenerator, createGeneratorCallback, logger);
        }

        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }

        [Test()]
        public void ConstructWithParametersSbsProjectGeneratorCreateGeneratorCallbackLoggerTest()
        {
            // TODO: Implement unit test for ConstructWithParametersSbsProjectGeneratorCreateGeneratorCallbackLogger
            this.testObject = new TestProjectBuilder(this.sbs, this.projectGenerator, createGeneratorCallback, logger);
        }

        [Test()]
        public void GenerateWithTwoEmptyAssemblies()
        {
            //expected = false;
            var outputFolder = "Value of outputFolder";
            var generatorType = typeof(object);
            var inputAssemblyPath = "Value of inputAssemblyPath";
            char directorySeparator = '\\';
            var firstAssemblyName = "TheTestNode";
            var secondAssemblyName = "AnotherModule";
            var isGetFileNameUsed = "amIused";

            var mainNodes = new[] 
            { 
                new TestNode(firstAssemblyName + ".dll", TestNodeType.Module, null),
                new TestNode(secondAssemblyName + ".dll", TestNodeType.Module, null),
                //new TestNode("C", TestNodeType.Method, null),
            };
            var referencedAssemblies = typeof(GeneratorRunnerDataTest).Assembly.GetReferencedAssemblies();
            var runnerData = new GeneratorRunnerData(
                outputFolder, generatorType, inputAssemblyPath, mainNodes, referencedAssemblies);

            Expect.Call(sbs.DirectorySeparatorChar).Return(directorySeparator).Repeat.AtLeastOnce();
            // The return value of GetFileNameWithoutExtension: Add isGetFileNameUsed as indicator, that the return value is
            // used in further processing.
            Expect.Call(sbs.GetFileNameWithoutExtension(firstAssemblyName + ".dll")).Return(firstAssemblyName + isGetFileNameUsed);
            Expect.Call(sbs.GetFileNameWithoutExtension(secondAssemblyName + ".dll")).Return(secondAssemblyName + isGetFileNameUsed);
            // Well, here it should be used, when requesting to create a directory.
            Expect.Call(sbs.CreateDirectory(outputFolder + directorySeparator + firstAssemblyName + isGetFileNameUsed + ".Tests")).Return(null);
            Expect.Call(sbs.CreateDirectory(outputFolder + directorySeparator + secondAssemblyName + isGetFileNameUsed + ".Tests")).Return(null);

            //var prjReferencedAssemblies = referencedAssemblies.ToList();
            var prjReferencedAssemblies = new List<AssemblyName>();
            Expect.Call(projectGenerator.ReferencedAssemblies).Return(prjReferencedAssemblies).Repeat.Any();
            Expect.Call(projectGenerator.GenerateProjectFile).Repeat.Twice();

            // logging
            Expect.Call(delegate { this.logger(null); }).Constraints(Is.NotNull()).Repeat.AtLeastOnce();
            Expect.Call(projectGenerator.ProjectName).Return(firstAssemblyName);
            Expect.Call(projectGenerator.ProjectName).Return(secondAssemblyName);

            mocks.ReplayAll();

            testObject.GenerateTests(runnerData);
            //Assert.AreEqual(expected, actual);
            Assert.IsNotEmpty(prjReferencedAssemblies);

            // Todo: where is the test assembly in the project? seems like the project generator has his problems with assembly refs.
            // Assert.Contains(prjReferencedAssemblies.Select(e => e.Name), firstModuleName + ".dll");
            // Assert.Contains(prjReferencedAssemblies.Select(e => e.Name), secondModuleName + ".dll");


            mocks.VerifyAll();
        }

        [Test()]
        public void GenerateOnMissingDirectoryThrows()
        {
            //expected = false;
            var outputFolder = "Value ? of ou$tputFol*der";
            var generatorType = typeof(object);
            var inputAssemblyPath = "Value of inputAssemblyPath";
            char directorySeparator = '\\';
            var firstAssemblyName = "TheTestNode";
            var moduleName = "The.Namespace.VeryPrettyFactory";

            var assemblyNode = new TestNode(firstAssemblyName + ".dll", TestNodeType.Module, null);
            var moduleNode = new TestNode(moduleName, TestNodeType.Class, null);
            assemblyNode.Nodes.Add(moduleNode);
            var mainNodes = new[] { assemblyNode };

            var referencedAssemblies = typeof(GeneratorRunnerDataTest).Assembly.GetReferencedAssemblies();
            var runnerData = new GeneratorRunnerData(
                outputFolder, generatorType, inputAssemblyPath, mainNodes, referencedAssemblies);

            Expect.Call(sbs.DirectorySeparatorChar).Return(directorySeparator).Repeat.Any();
            Expect.Call(sbs.GetFileNameWithoutExtension(firstAssemblyName + ".dll")).Return(firstAssemblyName).Repeat.Any();
            Expect.Call(sbs.CreateDirectory(outputFolder + directorySeparator + firstAssemblyName + ".Tests")).Return(null);
            
            // return false = Directory non existant.
            Expect.Call(sbs.DirectoryExists(outputFolder + directorySeparator + firstAssemblyName + ".Tests")).Return(false);

            var prjReferencedAssemblies = new List<AssemblyName>();
            Expect.Call(projectGenerator.ReferencedAssemblies).Return(prjReferencedAssemblies).Repeat.Any();
            Expect.Call(projectGenerator.GenerateProjectFile).Repeat.Any();

            var codeGenerator = mocks.StrictMock<ICodeGenerator>();
            Expect.Call(createGeneratorCallback(sbs, null, null)).IgnoreArguments().Return(codeGenerator);

            // logging
            Expect.Call(delegate { this.logger(null); }).Constraints(Is.NotNull()).Repeat.AtLeastOnce();
            Expect.Call(projectGenerator.ProjectName).Return(firstAssemblyName);
            mocks.ReplayAll();

            Assert.Throws<System.IO.DirectoryNotFoundException>(() => testObject.GenerateTests(runnerData));

            mocks.VerifyAll();
        }

        [Test()]
        public void GenerateWithAssemblyAndModule()
        {
            var outputFolder = "Value ? of ou$tputFol*der";
            var generatorType = typeof(object);
            var inputAssemblyPath = "Value of inputAssemblyPath";
            char directorySeparator = '\\';
            var firstModuleName = "TheTestNode";
            var firstModuleTag = "FirstModule";

            var assemblyNode = new TestNode(firstModuleName + ".dll", TestNodeType.Assembly, null) { Checked = true };
            var moduleNode = new TestNode(firstModuleName + ".dll", TestNodeType.Module, firstModuleTag) { Checked = true };
            assemblyNode.Nodes.Add(moduleNode);
            var mainNodes = new[] { assemblyNode };

            var referencedAssemblies = typeof(GeneratorRunnerDataTest).Assembly.GetReferencedAssemblies();
            var runnerData = new GeneratorRunnerData(
                outputFolder, generatorType, inputAssemblyPath, mainNodes, referencedAssemblies);

            Expect.Call(sbs.DirectorySeparatorChar).Return(directorySeparator).Repeat.Any();
            Expect.Call(sbs.GetFileNameWithoutExtension(firstModuleName + ".dll")).Return(firstModuleName);
            var expectedDirectory = outputFolder + directorySeparator + firstModuleName + ".Tests";
            Expect.Call(sbs.CreateDirectory(expectedDirectory)).Return(null);
            Expect.Call(sbs.DirectoryExists(expectedDirectory)).Return(true);

            var prjReferencedAssemblies = new List<AssemblyName>();
            Expect.Call(projectGenerator.ReferencedAssemblies).Return(prjReferencedAssemblies).Repeat.Any();
            Expect.Call(projectGenerator.GenerateProjectFile).Repeat.Once();

            var codeGenerator = mocks.StrictMock<ICodeGenerator>();

            Expect.Call(createGeneratorCallback(sbs, null, null))
                //.Constraints(Is.Same(sbs), Is.TypeOf<CodeGeneratorParameters>(), Is.TypeOf<CodeNamespace>())
                .Constraints(
                Is.Same(sbs),
                Is.TypeOf<CodeGeneratorParameters>() && Property.Value("OutputDirectory", expectedDirectory), 
                Is.TypeOf<CodeNamespace>())
                .Return(codeGenerator);

            Expect.Call(codeGenerator.CodeNamespace = null).PropertyBehavior();
            Expect.Call(codeGenerator.OutputDirectory = expectedDirectory);
            Expect.Call(codeGenerator.GenerateCode);

            // logging
            Expect.Call(delegate { this.logger(null); }).Constraints(Is.NotNull()).Repeat.AtLeastOnce();
            Expect.Call(projectGenerator.ProjectName).Return(firstModuleName).Repeat.Any();

            mocks.ReplayAll();

            testObject.GenerateTests(runnerData);
            //Assert.AreEqual(expected, actual);
            Assert.IsNotEmpty(prjReferencedAssemblies);

            mocks.VerifyAll();
        }

        [Test()]
        public void GenerateWithAssemblyAndModuleAndClass()
        {
            //expected = false;
            var outputFolder = "Value ? of ou$tputFol*der";
            var generatorType = typeof(object);
            var inputAssemblyPath = "Value of inputAssemblyPath";
            char directorySeparator = '\\';
            var firstModuleName = "TheTestNode";
            var firstModuleTag = "FirstModule";
            var className1 = "The.Namespace.VeryPrettyFactory";
            var classTag = "ClassTag";

            var assemblyNode = new TestNode(firstModuleName + ".dll", TestNodeType.Assembly, null) { Checked = true };
            var moduleNode = new TestNode(firstModuleName + ".dll", TestNodeType.Module, firstModuleTag) { Checked = true };
            var classNode = new TestNode(className1, TestNodeType.Class, classTag) { Checked = true };
            assemblyNode.Nodes.Add(moduleNode);
            moduleNode.Nodes.Add(classNode);
            var mainNodes = new[] { assemblyNode };

            var referencedAssemblies = typeof(GeneratorRunnerDataTest).Assembly.GetReferencedAssemblies();
            var runnerData = new GeneratorRunnerData(
                outputFolder, generatorType, inputAssemblyPath, mainNodes, referencedAssemblies);

            Expect.Call(sbs.DirectorySeparatorChar).Return(directorySeparator).Repeat.Any();
            Expect.Call(sbs.GetFileNameWithoutExtension(firstModuleName + ".dll")).Return(firstModuleName);
            var expectedDirectory = outputFolder + directorySeparator + firstModuleName + ".Tests";
            Expect.Call(sbs.CreateDirectory(expectedDirectory)).Return(null);
            Expect.Call(sbs.DirectoryExists(expectedDirectory)).Return(true);

            var prjReferencedAssemblies = new List<AssemblyName>();
            Expect.Call(projectGenerator.ReferencedAssemblies).Return(prjReferencedAssemblies).Repeat.Any();
            Expect.Call(projectGenerator.GenerateProjectFile).Repeat.Once();
            var classFilesList = new List<string>();
            Expect.Call(projectGenerator.ClassFiles).Return(classFilesList).Repeat.Once();

            var codeGenerator = mocks.StrictMock<ICodeGenerator>();

            Expect.Call(createGeneratorCallback(sbs, null, null))
                //.Constraints(Is.Same(sbs), Is.TypeOf<CodeGeneratorParameters>(), Is.TypeOf<CodeNamespace>())
                .Constraints(
                Is.Same(sbs),
                Is.TypeOf<CodeGeneratorParameters>() && Property.Value("OutputDirectory", expectedDirectory),
                Is.TypeOf<CodeNamespace>())
                .Return(codeGenerator);

            Expect.Call(codeGenerator.CodeNamespace = null).PropertyBehavior();
            Expect.Call(codeGenerator.OutputDirectory = expectedDirectory);
            Expect.Call(codeGenerator.GenerateCode);

            // logging
            Expect.Call(delegate { this.logger(null); }).Constraints(Is.NotNull()).Repeat.AtLeastOnce();
            Expect.Call(projectGenerator.ProjectName).Return(firstModuleName).Repeat.Any();

            mocks.ReplayAll();

            testObject.GenerateTests(runnerData);
            //Assert.AreEqual(expected, actual);
            Assert.IsNotEmpty(prjReferencedAssemblies);
            Assert.IsNotEmpty(classFilesList);

            mocks.VerifyAll();
        }

        [Test()]
        public void GenerateWithAssemblyAndModuleAndClassNotChecked()
        {
            //expected = false;
            var outputFolder = "Value ? of ou$tputFol*der";
            var generatorType = typeof(object);
            var inputAssemblyPath = "Value of inputAssemblyPath";
            char directorySeparator = '\\';
            var firstModuleName = "TheTestNode";
            var firstModuleTag = "FirstModule";
            var className1Namespace = "The.Namespace";
            var className1 = "VeryPrettyFactory";
            var className1FullName = className1Namespace + "." + className1;
            var classTag = this.GetType();
            var methodName = "MyMethod";
            var methodTag = this.GetType().GetMethod("FakeMethod");

            var assemblyNode = new TestNode(firstModuleName + ".dll", TestNodeType.Assembly, null) { Checked = true };
            var moduleNode = new TestNode(firstModuleName + ".dll", TestNodeType.Module, firstModuleTag) { Checked = true };
            var classNode = new TestNode(className1FullName, TestNodeType.Class, classTag) { Checked = false };
            var methodNode = new TestNode(methodName, TestNodeType.Method, methodTag) { Checked = true };
            assemblyNode.Nodes.Add(moduleNode);
            moduleNode.Nodes.Add(classNode);
            classNode.Nodes.Add(methodNode);
            var mainNodes = new[] { assemblyNode };

            var referencedAssemblies = typeof(GeneratorRunnerDataTest).Assembly.GetReferencedAssemblies();
            var runnerData = new GeneratorRunnerData(
                outputFolder, generatorType, inputAssemblyPath, mainNodes, referencedAssemblies);

            Expect.Call(sbs.DirectorySeparatorChar).Return(directorySeparator).Repeat.Any();
            Expect.Call(sbs.GetFileNameWithoutExtension(firstModuleName + ".dll")).Return(firstModuleName);
            var expectedDirectory = outputFolder + directorySeparator + firstModuleName + ".Tests";
            Expect.Call(sbs.CreateDirectory(expectedDirectory)).Return(null);
            Expect.Call(sbs.DirectoryExists(expectedDirectory)).Return(true);

            var prjReferencedAssemblies = new List<AssemblyName>();
            Expect.Call(projectGenerator.ReferencedAssemblies).Return(prjReferencedAssemblies).Repeat.Any();
            Expect.Call(projectGenerator.GenerateProjectFile).Repeat.Once();

            var codeGenerator = mocks.StrictMock<ICodeGenerator>();
            CodeNamespace generatorCodeNamespace = null;

            Expect.Call(createGeneratorCallback(sbs, null, null))
                .Constraints(
                Is.Same(sbs),
                Is.TypeOf<CodeGeneratorParameters>() && Property.Value("OutputDirectory", expectedDirectory),
                Is.TypeOf<CodeNamespace>())
                .Do((Func<IBuildSystem, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator>)
                delegate(IBuildSystem buildSys, ICodeGeneratorParameters cgp, CodeNamespace cn)
                {
                    Assert.AreEqual(expectedDirectory, cgp.OutputDirectory);
                    Assert.IsFalse(cgp.UseSetupAndTearDown);

                    generatorCodeNamespace = cn;
                    return codeGenerator;
                });

            Expect.Call(codeGenerator.CodeNamespace = null).PropertyBehavior();
            Expect.Call(codeGenerator.OutputDirectory = expectedDirectory);
            Expect.Call(codeGenerator.GenerateCode);

            // logging
            Expect.Call(delegate { this.logger(null); }).Constraints(Is.NotNull()).Repeat.AtLeastOnce();
            Expect.Call(projectGenerator.ProjectName).Return(firstModuleName).Repeat.Any();

            mocks.ReplayAll();

            testObject.GenerateTests(runnerData);
            //Assert.AreEqual(expected, actual);
            Assert.IsNotEmpty(prjReferencedAssemblies);

            Assert.IsEmpty(generatorCodeNamespace.Comments);
            Assert.IsEmpty(generatorCodeNamespace.Imports);
            Assert.AreEqual(className1Namespace, generatorCodeNamespace.Name);
            Assert.IsEmpty(generatorCodeNamespace.Types);
            Assert.IsEmpty(generatorCodeNamespace.UserData);

            mocks.VerifyAll();
        }

        [Test()]
        public void GenerateWithAssemblyAndModuleAndClassAndMethodNotChecked()
        {
            //expected = false;
            var outputFolder = "Value ? of ou$tputFol*der";
            var generatorType = typeof(object);
            var inputAssemblyPath = "Value of inputAssemblyPath";
            char directorySeparator = '\\';
            var firstModuleName = "TheTestNode";
            var firstModuleTag = "FirstModule";
            var className1Namespace = "The.Namespace";
            var className1 = "VeryPrettyFactory";
            var className1FullName = className1Namespace + "." + className1;
            var classTag = this.GetType();
            var methodName = "MyMethod";
            var methodTag = this.GetType().GetMethod("FakeMethod");

            var assemblyNode = new TestNode(firstModuleName + ".dll", TestNodeType.Assembly, null) { Checked = true };
            var moduleNode = new TestNode(firstModuleName + ".dll", TestNodeType.Module, firstModuleTag) { Checked = true };
            var classNode = new TestNode(className1FullName, TestNodeType.Class, classTag) { Checked = true };
            var methodNode = new TestNode(methodName, TestNodeType.Method, methodTag) { Checked = false };
            assemblyNode.Nodes.Add(moduleNode);
            moduleNode.Nodes.Add(classNode);
            classNode.Nodes.Add(methodNode);
            var mainNodes = new[] { assemblyNode };

            var referencedAssemblies = typeof(GeneratorRunnerDataTest).Assembly.GetReferencedAssemblies();
            var runnerData = new GeneratorRunnerData(
                outputFolder, generatorType, inputAssemblyPath, mainNodes, referencedAssemblies);

            Expect.Call(sbs.DirectorySeparatorChar).Return(directorySeparator).Repeat.Any();
            Expect.Call(sbs.GetFileNameWithoutExtension(firstModuleName + ".dll")).Return(firstModuleName);
            var expectedDirectory = outputFolder + directorySeparator + firstModuleName + ".Tests";
            Expect.Call(sbs.CreateDirectory(expectedDirectory)).Return(null);
            Expect.Call(sbs.DirectoryExists(expectedDirectory)).Return(true);

            var prjReferencedAssemblies = new List<AssemblyName>();
            Expect.Call(projectGenerator.ReferencedAssemblies).Return(prjReferencedAssemblies).Repeat.Any();
            Expect.Call(projectGenerator.GenerateProjectFile).Repeat.Once();
            var classFilesList = new List<string>();
            Expect.Call(projectGenerator.ClassFiles).Return(classFilesList).Repeat.Once();

            var codeGenerator = mocks.StrictMock<ICodeGenerator>();
            CodeNamespace generatorCodeNamespace = null;

            Expect.Call(createGeneratorCallback(sbs, null, null))
                //.Constraints(Is.Same(sbs), Is.TypeOf<CodeGeneratorParameters>(), Is.TypeOf<CodeNamespace>())
                .Constraints(
                Is.Same(sbs),
                Is.TypeOf<CodeGeneratorParameters>() && Property.Value("OutputDirectory", expectedDirectory),
                Is.TypeOf<CodeNamespace>())
                //.Return(codeGenerator)
                .Do((Func<IBuildSystem, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator>)
                delegate(IBuildSystem buildSys, ICodeGeneratorParameters cgp, CodeNamespace cn)
                {
                    Assert.AreEqual(expectedDirectory, cgp.OutputDirectory);
                    Assert.IsFalse(cgp.UseSetupAndTearDown);

                    generatorCodeNamespace = cn;
                    return codeGenerator;
                });

            Expect.Call(codeGenerator.CodeNamespace = null).PropertyBehavior();
            Expect.Call(codeGenerator.OutputDirectory = expectedDirectory);
            Expect.Call(codeGenerator.GenerateCode);

            // logging
            Expect.Call(delegate { this.logger(null); }).Constraints(Is.NotNull()).Repeat.AtLeastOnce();
            Expect.Call(projectGenerator.ProjectName).Return(firstModuleName).Repeat.Any();

            mocks.ReplayAll();

            testObject.GenerateTests(runnerData);
            //Assert.AreEqual(expected, actual);
            Assert.IsNotEmpty(prjReferencedAssemblies);
            Assert.IsNotEmpty(classFilesList);

            Assert.IsEmpty(generatorCodeNamespace.Comments);
            Assert.IsEmpty(generatorCodeNamespace.Imports);
            Assert.AreEqual(className1Namespace, generatorCodeNamespace.Name);
            Assert.IsNotEmpty(generatorCodeNamespace.Types);
            Assert.IsEmpty(generatorCodeNamespace.UserData);

            var classDefinition1 = generatorCodeNamespace.Types[0];
            Assert.AreEqual(MemberAttributes.Private | MemberAttributes.Final, classDefinition1.Attributes);
            Assert.IsEmpty(classDefinition1.BaseTypes);
            Assert.IsEmpty(classDefinition1.Comments);
            Assert.IsEmpty(classDefinition1.CustomAttributes);

            Assert.AreEqual(className1FullName, classDefinition1.Name);
            Assert.IsTrue(classDefinition1.IsClass);
            Assert.IsEmpty(classDefinition1.StartDirectives);
            Assert.IsEmpty(classDefinition1.TypeParameters);

            Assert.IsNotEmpty(classDefinition1.UserData);
            Assert.Count(1, classDefinition1.UserData);
            Assert.AreEqual(classTag, classDefinition1.UserData[NStubConstants.UserDataClassTypeKey]);

            Assert.IsEmpty(classDefinition1.Members);

            mocks.VerifyAll();
        }

        [Test()]
        public void GenerateWithAssemblyAndModuleAndClassesAndMethod()
        {
            //expected = false;
            var outputFolder = "Value ? of ou$tputFol*der";
            var generatorType = typeof(object);
            var inputAssemblyPath = "Value of inputAssemblyPath";
            char directorySeparator = '\\';
            var firstModuleName = "TheTestNode";
            var firstModuleTag = "FirstModule";
            var className1Namespace = "The.Namespace";
            var className1 = "VeryPrettyFactory";
            var className1FullName = className1Namespace + "." + className1;
            var classTag = this.GetType();
            var methodName = "MyMethod";
            var methodTag = this.GetType().GetMethod("FakeMethod");

            var assemblyNode = new TestNode(firstModuleName + ".dll", TestNodeType.Assembly, null) { Checked = true };
            var moduleNode = new TestNode(firstModuleName + ".dll", TestNodeType.Module, firstModuleTag) { Checked = true };
            var classNode = new TestNode(className1FullName, TestNodeType.Class, classTag) { Checked = true };
            var methodNode = new TestNode(methodName, TestNodeType.Method, methodTag) { Checked = true };
            assemblyNode.Nodes.Add(moduleNode);
            moduleNode.Nodes.Add(classNode);
            classNode.Nodes.Add(methodNode);
            var mainNodes = new[] { assemblyNode };

            var referencedAssemblies = typeof(GeneratorRunnerDataTest).Assembly.GetReferencedAssemblies();
            var runnerData = new GeneratorRunnerData(
                outputFolder, generatorType, inputAssemblyPath, mainNodes, referencedAssemblies);

            Expect.Call(sbs.DirectorySeparatorChar).Return(directorySeparator).Repeat.Any();
            Expect.Call(sbs.GetFileNameWithoutExtension(firstModuleName + ".dll")).Return(firstModuleName);
            var expectedDirectory = outputFolder + directorySeparator + firstModuleName + ".Tests";
            Expect.Call(sbs.CreateDirectory(expectedDirectory)).Return(null);
            Expect.Call(sbs.DirectoryExists(expectedDirectory)).Return(true);

            var prjReferencedAssemblies = new List<AssemblyName>();
            Expect.Call(projectGenerator.ReferencedAssemblies).Return(prjReferencedAssemblies).Repeat.Any();
            Expect.Call(projectGenerator.GenerateProjectFile).Repeat.Once();
            var classFilesList = new List<string>();
            Expect.Call(projectGenerator.ClassFiles).Return(classFilesList).Repeat.Once();

            var codeGenerator = mocks.StrictMock<ICodeGenerator>();
            CodeNamespace generatorCodeNamespace = null;

            Expect.Call(createGeneratorCallback(sbs, null, null))
                //.Constraints(Is.Same(sbs), Is.TypeOf<CodeGeneratorParameters>(), Is.TypeOf<CodeNamespace>())
                .Constraints(
                Is.Same(sbs),
                Is.TypeOf<CodeGeneratorParameters>() && Property.Value("OutputDirectory", expectedDirectory),
                Is.TypeOf<CodeNamespace>())
                //.Return(codeGenerator)
                .Do((Func<IBuildSystem, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator>)
                delegate(IBuildSystem buildSys, ICodeGeneratorParameters cgp, CodeNamespace cn) 
                {
                    Assert.AreEqual(expectedDirectory, cgp.OutputDirectory);
                    Assert.IsFalse(cgp.UseSetupAndTearDown);

                    generatorCodeNamespace = cn;
                    return codeGenerator; 
                });

            Expect.Call(codeGenerator.CodeNamespace = null).PropertyBehavior();
            Expect.Call(codeGenerator.OutputDirectory = expectedDirectory);
            Expect.Call(codeGenerator.GenerateCode);

            // logging
            Expect.Call(delegate { this.logger(null); }).Constraints(Is.NotNull()).Repeat.AtLeastOnce();
            Expect.Call(projectGenerator.ProjectName).Return(firstModuleName).Repeat.Any();
            
            mocks.ReplayAll();

            testObject.GenerateTests(runnerData);
            //Assert.AreEqual(expected, actual);
            Assert.IsNotEmpty(prjReferencedAssemblies);
            Assert.IsNotEmpty(classFilesList);

            Assert.IsEmpty(generatorCodeNamespace.Comments);
            Assert.IsEmpty(generatorCodeNamespace.Imports);
            Assert.AreEqual(className1Namespace, generatorCodeNamespace.Name);
            Assert.IsNotEmpty(generatorCodeNamespace.Types);
            Assert.IsEmpty(generatorCodeNamespace.UserData);

            var classDefinition1 = generatorCodeNamespace.Types[0];
            Assert.AreEqual(MemberAttributes.Private | MemberAttributes.Final, classDefinition1.Attributes);
            Assert.IsEmpty(classDefinition1.BaseTypes);
            Assert.IsEmpty(classDefinition1.Comments);
            Assert.IsEmpty(classDefinition1.CustomAttributes);

            Assert.AreEqual(className1FullName, classDefinition1.Name);
            Assert.IsTrue(classDefinition1.IsClass);
            Assert.IsEmpty(classDefinition1.StartDirectives);
            Assert.IsEmpty(classDefinition1.TypeParameters);
            
            Assert.IsNotEmpty(classDefinition1.UserData);
            Assert.Count(1, classDefinition1.UserData);
            Assert.AreEqual(classTag, classDefinition1.UserData[NStubConstants.UserDataClassTypeKey]);

            Assert.IsNotEmpty(classDefinition1.Members);
            var class1Members = classDefinition1.Members;

            mocks.VerifyAll();
        }


        // Todo: last test should be a full test WithNoLogger
    }
}
