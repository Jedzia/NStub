namespace NStub.CoreNStub.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core;
    using NStub.Core.Util.Dumper;
    using System.Reflection;
    
    
    [TestFixture()]
    public partial class AssemblyFetcherTest
    {

        private string assemblyName;
        private List<string> inputAssemblies;
        private MemberVisibility methodVisibility;
        private AssemblyFetcher testObject;


        [FixtureSetUp]
        public void FixtureSetUp()
        {
            Server.Default.LambdaFormatter = global::Gallio.Framework.TestLog.Warnings;
        }
        
        [SetUp()]
        public void SetUp()
        {
            var assemblies = new[] 
            { 
                typeof(AssemblyFetcher).Assembly,
                typeof(AssemblyFetcherTest).Assembly, 
                typeof(Rhino.Mocks.MockRepository).Assembly,
            }.ToList();

            this.methodVisibility = MemberVisibility.Public;
            this.assemblyName = typeof(AssemblyFetcherTest).Assembly.Location;
            this.inputAssemblies = assemblies.Select(e => e.Location).ToList();
            this.testObject = new AssemblyFetcher(this.methodVisibility, this.assemblyName, this.inputAssemblies);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersAssemblyNameInputAssembliesTest()
        {
            this.testObject = new AssemblyFetcher(this.methodVisibility, "Some other root", this.inputAssemblies);

            Assert.Throws<ArgumentNullException>(() => new AssemblyFetcher(this.methodVisibility, null, this.inputAssemblies));
            Assert.Throws<ArgumentException>(() => new AssemblyFetcher(this.methodVisibility, string.Empty, this.inputAssemblies));
            Assert.Throws<ArgumentNullException>(() => new AssemblyFetcher(this.methodVisibility, this.assemblyName, null));
        }

        [Test()]
        public void LoadAssemblyWithDifferentDescription()
        {
            var expectedRootName = "Different Name";
            this.testObject = new AssemblyFetcher(this.methodVisibility, expectedRootName, this.inputAssemblies);
            var result = testObject.LoadAssembly();
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Nodes);
            Assert.AreEqual(expectedRootName, result.Text);
        }

        [Test()]
        public void LoadAssemblyWithNullInInputAssembliesThrows()
        {
            this.inputAssemblies[1] = null;
            this.testObject = new AssemblyFetcher(this.methodVisibility, "HelloTest", this.inputAssemblies);
            Assert.Throws<ArgumentNullException>(() => testObject.LoadAssembly());
        }

        [Test()]
        public void LoadAssemblyWithNonValidFilenameInInputAssembliesThrows()
        {
            this.inputAssemblies[1] = "A assembly that can't be a valid one.dll";
            this.testObject = new AssemblyFetcher(this.methodVisibility, "HelloTest", this.inputAssemblies);
            Assert.Throws<ArgumentException>(() => testObject.LoadAssembly());
        }

        [Test()]
        public void LoadAssemblySecondTimeReturnsCached()
        {
            var first = testObject.LoadAssembly();
            var second = testObject.LoadAssembly();
            Assert.AreSame(first, second);
        }

        [Test()]
        public void LoadAssemblyTest()
        {
            // Maybe use an injected object that performs the real assembly loading, for instance 'Assembly.LoadFile(...), etc.
            var result = testObject.LoadAssembly();
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Nodes);
            
            var nodes = result.Nodes.SelectMany(e => e.Nodes).ToArray();
            Assert.Count(3, result.Nodes);
            Assert.AreEqual("NStub.Core.dll", nodes[0].Text);
            Assert.AreEqual("NStub.Core.Tests.dll", nodes[1].Text);
            Assert.AreEqual("Rhino.Mocks.dll", nodes[2].Text);

            nodes.Dump(1,10);

            //Assert.Contains(nodes, new TestNode("Bla", TestNodeType.Class, null));

        }
    }
}
