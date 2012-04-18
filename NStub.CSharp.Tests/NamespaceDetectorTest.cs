namespace NStub.CSharp.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp;
    using System.CodeDom;


    public partial class NamespaceDetectorTest
    {

        private NamespaceDetector testObject;
        private System.CodeDom.CodeTypeDeclarationCollection typeDeclarations;

        [SetUp()]
        public void SetUp()
        {
            this.typeDeclarations = new System.CodeDom.CodeTypeDeclarationCollection();
            typeDeclarations.Add(new CodeTypeDeclaration("Jedzia.Loves.Testing.TheClassToTest"));
            this.testObject = new NamespaceDetector(this.typeDeclarations);
        }

        [Test()]
        public void ConstructWithParametersTypeDeclarationsTest()
        {
            this.typeDeclarations = new System.CodeDom.CodeTypeDeclarationCollection();
            this.testObject = new NamespaceDetector(this.typeDeclarations);
            Assert.Throws<ArgumentNullException>(() => new NamespaceDetector(null));
        }

        [Test()]
        public void PropertyShortestNamespaceNormalBehavior()
        {
            // Test read access of 'ShortestNamespace' Property.
            var expected = "Jedzia.Loves.Testing";
            var actual = testObject.ShortestNamespace;
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PropertyTypeDeclarationsNormalBehavior()
        {
            // Test read access of 'TypeDeclarations' Property.
            var expected = this.typeDeclarations;
            var actual = testObject.TypeDeclarations;
            Assert.AreSame(expected, actual);
        }

        [Test()]
        public void CombineWithShortestNamespaceTest()
        {
            //typeDeclarations.Add(new CodeTypeDeclaration("Jedzia.Loves.Testing.TheClassToTest"));
            var type = new CodeTypeDeclaration("Jedzia.Loves.Testing.TheClassToTest");

            var expected = "Jedzia.Loves.Testing.Tests.TheClassToTest";
            var actual = testObject.CombineWithShortestNamespace(type, ".Tests");
            Assert.AreEqual(expected, actual);

        }

        [Test()]
        public void EqualsTest()
        {
            // TODO: Implement unit test for Equals

            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [Test()]
        public void GetHashCodeTest()
        {
            // TODO: Implement unit test for GetHashCode

            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [Test()]
        public void GetTypeTest()
        {
            // TODO: Implement unit test for GetType

            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [Test()]
        public void ToStringTest()
        {
            // TODO: Implement unit test for ToString

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
