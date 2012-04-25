namespace NStub.CSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp;
    using System.CodeDom;
    
    
    [TestFixture()]
    public partial class CodeTypeSetupTest
    {
        
        private NStub.CSharp.NamespaceDetector namespaceDetector;
        
        private System.CodeDom.CodeTypeDeclaration testClassDeclaration;

        private CodeTypeSetup testObject;
        private CodeTypeDeclarationCollection typeDeclarations;
        
        public CodeTypeSetupTest()
        {
        }
        
        [SetUp()]
        public void SetUp()
        {
            this.typeDeclarations = new CodeTypeDeclarationCollection();
            this.testClassDeclaration = new System.CodeDom.CodeTypeDeclaration();
            typeDeclarations.Add(this.testClassDeclaration);

            this.namespaceDetector = new NStub.CSharp.NamespaceDetector(typeDeclarations);
            this.testObject = new CodeTypeSetup(this.namespaceDetector, this.testClassDeclaration);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersNamespaceDetectorTestClassDeclarationTest()
        {
            this.testObject = new CodeTypeSetup(this.namespaceDetector, this.testClassDeclaration);

            Assert.Throws<ArgumentNullException>(() => new CodeTypeSetup(null, this.testClassDeclaration));
            Assert.Throws<ArgumentNullException>(() => new CodeTypeSetup(this.namespaceDetector, null));
        }
        
        [Test()]
        public void SetUpCodeNamespaceTest()
        {
            // TODO: Implement unit test for SetUpCodeNamespace
            // testObject.SetUpCodeNamespace(
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        [Test()]
        public void SetUpTestnameTest()
        {
            // TODO: Implement unit test for SetUpTestname

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
