namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.BuildContext;
    using Rhino.Mocks;
    
    
    public partial class SetupAndTearDownContextTest
    {
        
        private NStub.CSharp.ObjectGeneration.BuildDataCollection buildData;
        
        private System.CodeDom.CodeNamespace codeNamespace;
        
        private NStub.CSharp.ObjectGeneration.ITestObjectBuilder creator;
        
        private MockRepository mocks;
        
        private System.CodeDom.CodeMemberMethod setUpMethod;
        
        private System.CodeDom.CodeMemberMethod tearDownMethod;
        
        private System.CodeDom.CodeTypeDeclaration testClassDeclaration;
        
        private SetupAndTearDownContext testObject;
        
        public SetupAndTearDownContextTest()
        {
        }
        
        [SetUp()]
        public void SetUp()
        {
            this.buildData = new NStub.CSharp.ObjectGeneration.BuildDataCollection();
            this.codeNamespace = new System.CodeDom.CodeNamespace();
            this.testClassDeclaration = new System.CodeDom.CodeTypeDeclaration();
            this.setUpMethod = new System.CodeDom.CodeMemberMethod();
            this.tearDownMethod = new System.CodeDom.CodeMemberMethod();
            this.mocks = new MockRepository();
            this.creator = this.mocks.StrictMock<NStub.CSharp.ObjectGeneration.ITestObjectBuilder>();
            this.testObject = new SetupAndTearDownContext(this.buildData, this.codeNamespace, this.testClassDeclaration, this.setUpMethod, this.tearDownMethod, this.creator);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersBuildDataCodeNamespaceTestClassDeclarationSetUpMethodTearDownMethodCreatorTest()
        {
            // TODO: Implement unit test for ConstructWithParametersBuildDataCodeNamespaceTestClassDeclarationSetUpMethodTearDownMethodCreator
            this.buildData = new NStub.CSharp.ObjectGeneration.BuildDataCollection();
            this.codeNamespace = new System.CodeDom.CodeNamespace();
            this.testClassDeclaration = new System.CodeDom.CodeTypeDeclaration();
            this.setUpMethod = new System.CodeDom.CodeMemberMethod();
            this.tearDownMethod = new System.CodeDom.CodeMemberMethod();
            this.testObject = new SetupAndTearDownContext(this.buildData, this.codeNamespace, this.testClassDeclaration, this.setUpMethod, this.tearDownMethod, creator);
        }

        [Test()]
        public void PropertyBuildDataNormalBehavior()
        {
            // Test read access of 'BuildData' Property.
            mocks.ReplayAll();
            var expected = buildData;
            var actual = testObject.BuildData;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }

        [Test()]
        public void PropertyCodeNamespaceNormalBehavior()
        {
            // Test read access of 'CodeNamespace' Property.
            mocks.ReplayAll();
            var expected = codeNamespace;
            var actual = testObject.CodeNamespace;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }

        [Test()]
        public void PropertyTestClassDeclarationNormalBehavior()
        {
            // Test read access of 'TestClassDeclaration' Property.
            mocks.ReplayAll();
            var expected = testClassDeclaration;
            var actual = testObject.TestClassDeclaration;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }

        [Test()]
        public void PropertyNormalBehaviorObjectCreatorNormalBehavior()
        {
            // Test read access of 'TestObjectCreator' Property.
            mocks.ReplayAll();
            var expected = creator;
            var actual = testObject.TestObjectCreator;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }

        [Test()]
        public void PropertySetUpMethodNormalBehavior()
        {
            // Test read access of 'SetUpMethod' Property.
            mocks.ReplayAll();
            var expected = setUpMethod;
            var actual = testObject.SetUpMethod;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }

        [Test()]
        public void PropertyTearDownMethodNormalBehavior()
        {
            // Test read access of 'TearDownMethod' Property.
            mocks.ReplayAll();
            var expected = tearDownMethod;
            var actual = testObject.TearDownMethod;
            Assert.AreEqual(expected, actual);
            mocks.VerifyAll();
        }

    }
}
