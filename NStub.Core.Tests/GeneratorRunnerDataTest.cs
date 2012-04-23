namespace NStub.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core;
using System.Reflection;
    
    
    [TestFixture()]
    public partial class GeneratorRunnerDataTest
    {
        
        private System.Type generatorType;
        private string inputAssemblyPath;
        private string outputFolder;
        private IList<TestNode> mainNodes;
        private IList<AssemblyName> referencedAssemblies;
        private GeneratorRunnerData testObject;
        
        [SetUp()]
        public void SetUp()
        {
            // ToDo: Implement SetUp logic here 
            this.outputFolder = "Value of outputFolder";
            this.generatorType = typeof(object);
            this.inputAssemblyPath = "Value of inputAssemblyPath";
            this.mainNodes = new[] { new TestNode("A", TestNodeType.Class, null), new TestNode("B", TestNodeType.Method, null) };
            this.referencedAssemblies = typeof(GeneratorRunnerDataTest).Assembly.GetReferencedAssemblies();
            this.testObject = new GeneratorRunnerData(
                this.outputFolder, this.generatorType, this.inputAssemblyPath, this.mainNodes, this.referencedAssemblies);
        }
        
        [TearDown()]
        public void TearDown()
        {
            // ToDo: Implement TearDown logic here 
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersOutputFolderGeneratorTypeInputAssemblyPathMainNodesReferencedAssembliesTest()
        {
            // TODO: Implement unit test for ConstructWithParametersOutputFolderGeneratorTypeInputAssemblyPathMainNodesReferencedAssemblies
            this.outputFolder = "Value of outputFolder";
            this.generatorType = typeof(object);
            this.inputAssemblyPath = "Value of inputAssemblyPath";
            this.mainNodes = new[] { new TestNode("A", TestNodeType.Class, null), new TestNode("B", TestNodeType.Method, null) };
            this.referencedAssemblies = typeof(GeneratorRunnerDataTest).Assembly.GetReferencedAssemblies();
            this.testObject = new GeneratorRunnerData(
                this.outputFolder, this.generatorType, this.inputAssemblyPath, this.mainNodes, this.referencedAssemblies);

            Assert.Throws<ArgumentNullException>(() => new GeneratorRunnerData(
                null, this.generatorType, this.inputAssemblyPath, this.mainNodes, this.referencedAssemblies));
            Assert.Throws<ArgumentException>(() => new GeneratorRunnerData(
                string.Empty, this.generatorType, this.inputAssemblyPath, this.mainNodes, this.referencedAssemblies));
            Assert.Throws<ArgumentNullException>(() => new GeneratorRunnerData(
                this.outputFolder, null, this.inputAssemblyPath, this.mainNodes, this.referencedAssemblies));
            Assert.Throws<ArgumentNullException>(() => new GeneratorRunnerData(
                this.outputFolder, this.generatorType, null, this.mainNodes, this.referencedAssemblies));
            Assert.Throws<ArgumentException>(() => new GeneratorRunnerData(
                this.outputFolder, this.generatorType, string.Empty, this.mainNodes, this.referencedAssemblies));
            Assert.Throws<ArgumentNullException>(() => new GeneratorRunnerData(
                this.outputFolder, this.generatorType, this.inputAssemblyPath, null, this.referencedAssemblies));
            Assert.Throws<ArgumentNullException>(() => new GeneratorRunnerData(
                this.outputFolder, this.generatorType, this.inputAssemblyPath, this.mainNodes, null));
        }
        
        [Test()]
        public void PropertyGeneratorTypeNormalBehavior()
        {
            // Test read access of 'GeneratorType' Property.
            var expected = typeof(object);
            var actual = testObject.GeneratorType;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyInputAssemblyPathNormalBehavior()
        {
            // Test read access of 'InputAssemblyPath' Property.
            var expected = "Value of inputAssemblyPath";
            var actual = testObject.InputAssemblyPath;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyOutputFolderNormalBehavior()
        {
            // Test read access of 'OutputFolder' Property.
            var expected = "Value of outputFolder";
            var actual = testObject.OutputFolder;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyReferencedAssembliesNormalBehavior()
        {
            // Test read access of 'ReferencedAssemblies' Property.
            var expected = this.referencedAssemblies;
            var actual = testObject.ReferencedAssemblies;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyRootNodesNormalBehavior()
        {
            // Test read access of 'RootNodes' Property.
            var expected = this.mainNodes;
            var actual = testObject.RootNodes;
            Assert.AreEqual(expected, actual);
        }
    }
}
