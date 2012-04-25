namespace NStub.CSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp;
    using Rhino.Mocks;
    using NStub.Core;
    using System.CodeDom;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks.Constraints;
    using System.Reflection;
    
    
    [TestFixture()]
    public partial class CSharpTestProjectBuilderTest
    {

        private MockRepository mocks;
        private NStub.CSharp.ObjectGeneration.IBuildDataDictionary buildData;
        private NStub.Core.IProjectGenerator projectGenerator;
        private NStub.Core.IBuildSystem sbs;
        private Func<IBuildSystem, IBuildDataDictionary, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator> createGeneratorCallback;
        private Action<string> logger;
        private TestProjectBuilder testObject;
        private string logText;
        private IBuilderData buildDataItem;

        [SetUp()]
        public void SetUp()
        {
            logText = string.Empty;
            this.mocks = new MockRepository();
            this.buildData = new BuildDataDictionary();
            this.buildDataItem = new BuilderData<string>("Whuut? ...Data?");
            this.buildData.AddDataItem("MyKey", this.buildDataItem);
            this.sbs = this.mocks.StrictMock<NStub.Core.IBuildSystem>();
            this.projectGenerator = this.mocks.StrictMock<NStub.Core.IProjectGenerator>();
            this.createGeneratorCallback = this.mocks.StrictMock<Func<IBuildSystem, IBuildDataDictionary, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator>>();
            this.logger = this.mocks.StrictMock<Action<string>>();
            // this.logger = text => { this.logText += text; };
            this.testObject = new CSharpTestProjectBuilder(this.sbs, this.buildData, this.projectGenerator, createGeneratorCallback, logger);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
            this.mocks = null;
        }
        
        [Test()]
        public void ConstructWithParametersSbsBuildDataProjectGeneratorCreateGeneratorCallbackLoggerTest()
        {
            Assert.Throws<ArgumentNullException>(() => new CSharpTestProjectBuilder(null, this.buildData, this.projectGenerator, createGeneratorCallback, logger));
            Assert.Throws<ArgumentNullException>(() => new CSharpTestProjectBuilder(this.sbs, null, this.projectGenerator, createGeneratorCallback, logger));
            Assert.Throws<ArgumentNullException>(() => new CSharpTestProjectBuilder(this.sbs, this.buildData, null, createGeneratorCallback, logger));
            Assert.Throws<ArgumentNullException>(() => new CSharpTestProjectBuilder(this.sbs, this.buildData, this.projectGenerator, null, logger));

            this.testObject = new CSharpTestProjectBuilder(this.sbs, this.buildData, this.projectGenerator, createGeneratorCallback, null);
        }

        public int FakeMethod(string parameter1, int parameter2)
        {
            return 42;
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

            var referencedAssemblies = typeof(CSharpTestProjectBuilderTest).Assembly.GetReferencedAssemblies();
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
            IBuildDataDictionary generatorBuildData = null;

            Expect.Call(createGeneratorCallback(sbs, null, null, null))
                //.Constraints(Is.Same(sbs), Is.TypeOf<CodeGeneratorParameters>(), Is.TypeOf<CodeNamespace>())
                .Constraints(
                Is.Same(sbs),
                Is.TypeOf<IBuildDataDictionary>() && Is.NotNull(),
                Is.TypeOf<CodeGeneratorParameters>() && Property.Value("OutputDirectory", expectedDirectory),
                Is.TypeOf<CodeNamespace>())
                //.Return(codeGenerator)
                .Do((Func<IBuildSystem,IBuildDataDictionary, ICodeGeneratorParameters, CodeNamespace, ICodeGenerator>)
                delegate(IBuildSystem buildSys,IBuildDataDictionary data, ICodeGeneratorParameters cgp, CodeNamespace cn)
                {
                    Assert.AreEqual(expectedDirectory, cgp.OutputDirectory);
                    Assert.IsFalse(cgp.UseSetupAndTearDown);

                    generatorBuildData = data;
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

            Assert.IsNotEmpty(generatorBuildData);
            Assert.Count(1, generatorBuildData);
            Assert.IsTrue(generatorBuildData.Contains(this.buildDataItem));
            
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

            // Todo: More checks on the generated types.
            mocks.VerifyAll();
        }
    }
}
