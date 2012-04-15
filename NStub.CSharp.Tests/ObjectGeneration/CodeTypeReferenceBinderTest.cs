namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using System.CodeDom;
    using NStub.CSharp.Tests.FluentChecking;
    
    
    public partial class CodeTypeReferenceBinderTest
    {

        private CodeMemberMethod method;
        private CodeTypeReferenceBinder testObject;
        
        [SetUp()]
        public void SetUp()
        {
            //var ctdecl = new CodeTypeDeclaration("MyClass");
            this.method = new CodeMemberMethod();
            this.testObject = this.method.StaticClass("ReferencedClass");
        }

        [Test()]
        public void ConstructedFromStaticClass()
        {
            Assert.IsNotNull(testObject.TypeReference);
            Assert.IsInstanceOfType<CodeTypeReferenceExpression>(testObject.TypeReference);
            Assert.IsEmpty(method.Statements);
            // cm.StaticClass("Assert").Invoke("Inconclusive").With("Thisone").Commit();
        }

        [Test()]
        public void PropertyTypeReferenceNormalBehavior()
        {
            // Test read access of 'TypeReference' Property.
            var expected = "ReferencedClass";
            var actual = testObject.TypeReference.Type.BaseType;
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void CommitTest()
        {
            var result = testObject.Invoke("MethodName").With("Thisone").Commit();
            //testObject.Invoke("Arsch").With("Thisone").Commit();
            //testObject.Invoke("Maulaff").With("Thisone").Commit();
            //testObject.Invoke("Nase").With("Thisone").Commit();
            Assert.AreSame(method, result);
            Assert.IsNotEmpty(method.Statements);
            Assert.AreEqual(1, method.Statements.Count);

            //AssertEx.That(method.ContainsStatement<CodeExpressionStatement>(), "Attribute 'Test' not found in: {0}", method.ContainsAttributeMsg());
            //AssertEx.That(method.ContainsStatement<CodeMethodInvokeExpression>());
            //AssertEx.That(method.ContainsStatement<CodeExpressionStatement>().Where().Commit());
            //AssertEx.That(method.StatementsOf<CodeExpressionStatement>().Where().Expressions<CodeMethodInvokeExpression>("bla").Commit());
            
            AssertEx.That(method
                .StatementsOf<CodeExpressionStatement>()
                .Where().Expression<CodeMethodInvokeExpression>(Is.Named("MethodName"))
                .Assert());
             
            //method.StatementsOf<CodeExpressionStatement>().Where();
            // Assert.AreSame(testObject.TypeReference, actualCodeExpression.Expression);
        }
        
        [Test()]
        public void InvokeTest()
        {
            var result = testObject.Invoke("MethodName");
            Assert.AreSame(testObject, result);
            Assert.IsEmpty(method.Statements);
        }

        [Test()]
        public void WithWithoutPreceedingInvokeShouldThrow()
        {
            Assert.Throws<CodeTypeReferenceException>(() => testObject.With("Thisone"));
            Assert.IsEmpty(method.Statements);
        }

        [Test()]
        public void WithTest()
        {
            var result = testObject.Invoke("MethodName").With("Thisone");
            Assert.AreSame(testObject, result);
            Assert.IsEmpty(method.Statements);
        }
    }
}
