namespace NStub.CSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp;
    using System.CodeDom;
    using NStub.Core;
    
    
    [TestFixture()]
    public partial class CodeTypeSetupTest
    {
        
        private NStub.CSharp.NamespaceDetector namespaceDetector;
        private System.CodeDom.CodeTypeDeclaration testClassDeclaration;
        private CodeTypeSetup testObject;
        private CodeTypeDeclarationCollection typeDeclarations;
        
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
            var actual = testObject.SetUpCodeNamespace("Jedzia.Loves.Testing", new[] { "System.Fuck", "Rhino.Mocks" });
            Assert.Count(2, actual.Imports);
            Assert.AreEqual("Jedzia.Loves.Testing.Tests", actual.Name);
        }

        [Test()]
        public void SetUpTestnameThrowsWithoutInit()
        {
            Assert.Throws<KeyNotFoundException>(() => testObject.SetUpTestname());
        }

        [Test()]
        public void SetUpTestnameThrowsWithoutClassDeclarationName()
        {
            testClassDeclaration.UserData[NStubConstants.UserDataClassTypeKey] = typeof(Guid);
            Assert.Throws<ArgumentException>(() => testObject.SetUpTestname());
        }

        [Test()]
        public void SetUpTestnameTest()
        {
            /*this.typeDeclarations = new CodeTypeDeclarationCollection();
            this.testClassDeclaration = new System.CodeDom.CodeTypeDeclaration();
            typeDeclarations.Add(this.testClassDeclaration);*/

            testClassDeclaration.UserData[NStubConstants.UserDataClassTypeKey] = typeof(NStub.CSharp.Tests.Stubs.InfoApe);
            testClassDeclaration.Name = "NStub.CSharp.TopRootClass";
            typeDeclarations.Add(this.testClassDeclaration);

            testClassDeclaration.UserData[NStubConstants.UserDataClassTypeKey] = typeof(NStub.CSharp.Tests.Stubs.InfoApe);
            testClassDeclaration.Name = "NStub.CSharp.BlaFasel.MyWorkClass";

            this.namespaceDetector = new NStub.CSharp.NamespaceDetector(typeDeclarations);
            this.testObject = new CodeTypeSetup(this.namespaceDetector, this.testClassDeclaration);
            // testObject.SetUpCodeNamespace("NStub.CSharp", new[] { "System.Fuck", "Rhino.Mocks" });

            var expected = "MyWorkClass";
            var actual = testObject.SetUpTestname();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual("NStub.CSharp.BlaFasel.MyWorkClass", testObject.BaseKey);
            Assert.AreEqual("MyWorkClassTest", testClassDeclaration.Name);

            // testObject.SetUpCodeNamespace("Jedzia.Loves.Testing", new[] { "System.Fuck", "Rhino.Mocks" });
        }
    }
}
