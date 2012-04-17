namespace NStub.CSharp.ObjectGeneration.FluentCodeBuild
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using System.CodeDom;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;
    using NStub.CSharp.Tests.FluentChecking;

    
    public partial class CodeLocalVariableBinderTest
    {

        private CodeMemberMethod methodVarCreate;
        private CodeMemberMethod methodVarNoCreate;
        private CodeLocalVariableBinder testVarCreate;
        private CodeLocalVariableBinder testVarNoCreate;
        
        [SetUp()]
        public void SetUp()
        {
            this.methodVarCreate = new CodeMemberMethod();
            this.methodVarNoCreate = new CodeMemberMethod();
            this.testVarCreate = this.methodVarCreate.Var("variableNameCreate", true);
            this.testVarNoCreate = this.methodVarNoCreate.Var("variableNameNoCreate", false);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testVarCreate = null;
            this.testVarNoCreate = null;
            this.methodVarCreate = null;
            this.methodVarNoCreate = null;
        }
        
        [Test()]
        public void PropertyLocalVariableDeclarationNormalBehavior()
        {
            // Test read access of 'LocalVariableDeclaration' Property.
            var actual = testVarCreate.LocalVariableDeclaration;
            Assert.IsNotNull(actual);
            Assert.IsNull(testVarCreate.LocalVariableDeclaration.InitExpression);
            
            actual = testVarNoCreate.LocalVariableDeclaration;
            Assert.IsNull(actual);
        }

        [Test()]
        public void PropertyLocalVariableReferenceNormalBehavior()
        {
            // Test read access of 'LocalVariableReference' Property.
            var actual = testVarCreate.LocalVariableReference;
            Assert.IsNull(actual);
            
            actual = testVarNoCreate.LocalVariableReference;
            Assert.IsNotNull(actual);
        }

        [Test()]
        public void AssignTest()
        {
            var result = testVarCreate.Assign();
            Assert.AreSame(testVarCreate, result);
            result = testVarNoCreate.Assign();
            Assert.AreSame(testVarNoCreate, result);
        }
        
        [Test()]
        public void CommitTest()
        {
            // TODO: Implement unit test for Commit

            // nach commit StaticClassTest.
            //Assert.IsNotEmpty(methodVarCreate.StatementsOfType<CodeVariableDeclarationStatement>().Where(e => e.Name == "variableNameCreate"));
            //AssertEx.That(methodVarCreate.StatementsOfType<CodeAssignStatement>()
            // .Where().ExpressionLeft<CodeVariableReferenceExpression>(Is.VarRefNamed("localVar"))
            //.Assert());
            
            // initialization happened
            //AssertEx.That(methodVarCreate.StatementsOfType<CodeVariableDeclarationStatement>()
           //  .Where().Expression<CodePrimitiveExpression>(Is.Primitve("localVar")).WasNotFound()
           //  .Assert());
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [Test()]
        public void CommitStaticClass()
        {
            var expected = "DateTime";
            var binder = testVarCreate.StaticClass(expected);
            var result = binder.Commit();
            Assert.AreSame(this.methodVarCreate, result);

            Assert.IsNotEmpty(methodVarCreate.StatementsOfType<CodeVariableDeclarationStatement>().Where(e => e.Name == "variableNameCreate"));
            // no .With(...) -> no initialization happened  ... Todo: should i block this?
            Assert.IsEmpty(methodVarCreate.StatementsOfType<CodeVariableDeclarationStatement>().Where(e => e.InitExpression != null));

            // no create
            binder = testVarNoCreate.StaticClass(expected);
            result = binder.Commit();
            Assert.AreSame(this.methodVarNoCreate, result);

            AssertEx.That(methodVarNoCreate.StatementsOfType<CodeAssignStatement>()
             .Where().ExpressionLeft<CodeVariableReferenceExpression>(Is.VarRefNamed("variableNameNoCreate"))
            .Assert());

            // Todo: create something like
            // .Where().IsNull().ExpressionLeft<CodeVariableReferenceExpression>(Is.VarRefNamed("variableNameNoCreate"))
            // to test that ExpressionLeft<CodeVariableReferenceExpression> has no set .Left expressions at all.

            //Assert.IsNotEmpty(methodVarNoCreate.StatementsOfType<CodeAssignStatement>().Where(e => e. == "variableNameNoCreate"));
            //Assert.IsEmpty(methodVarNoCreate.StatementsOfType<CodeVariableDeclarationStatement>().Where(e => e.InitExpression != null));

        }

        [Test()]
        public void StaticClassTest()
        {
            // testVarCreate
            var expected = "DateTime";
            var result = testVarCreate.StaticClass(expected);
            Assert.IsInstanceOfType<CodeTypeReferenceBinder>(result);
            Assert.AreSame(result.LocalVar, testVarCreate);
            
            Assert.AreEqual(expected, result.TypeReference.Type.BaseType);
            Assert.IsInstanceOfType<CodeTypeReferenceExpression>(result.Expression);
            Assert.AreSame(result.TypeReference.Type, ((CodeTypeReferenceExpression)result.Expression).Type);

            Assert.IsNull(result.Invoker);
            Assert.IsTrue(result.IsTypeReference);

            Assert.IsEmpty(methodVarCreate.Statements);

            // testVarNoCreate
            result = testVarNoCreate.StaticClass(expected);
            Assert.IsInstanceOfType<CodeTypeReferenceBinder>(result);
            Assert.AreSame(result.LocalVar, testVarNoCreate);

            Assert.AreEqual(expected, result.TypeReference.Type.BaseType);
            Assert.IsInstanceOfType<CodeTypeReferenceExpression>(result.Expression);
            Assert.AreSame(result.TypeReference.Type, ((CodeTypeReferenceExpression)result.Expression).Type);

            Assert.IsNull(result.Invoker);
            Assert.IsTrue(result.IsTypeReference);

            Assert.IsEmpty(methodVarNoCreate.Statements);
        }
        
        [Test()]
        public void WithTest()
        {
            var expected = "primitive string";
            var result = testVarCreate.With(expected);
            Assert.AreSame(result, testVarCreate);
            Assert.AreSame(result.LocalVariableDeclaration, result.AssignStatement);
            Assert.IsInstanceOfType<CodePrimitiveExpression>(result.LocalVariableDeclaration.InitExpression);
            Assert.AreEqual(expected, ((CodePrimitiveExpression)result.LocalVariableDeclaration.InitExpression).Value);

            result = testVarNoCreate.With(expected);
            Assert.AreSame(result, testVarNoCreate);
            Assert.IsInstanceOfType<CodeAssignStatement>(result.AssignStatement);
            Assert.AreSame(result.LocalVariableReference, ((CodeAssignStatement)result.AssignStatement).Left);
            Assert.IsInstanceOfType<CodePrimitiveExpression>(((CodeAssignStatement)result.AssignStatement).Right);
            Assert.AreSame(expected, ((CodePrimitiveExpression)((CodeAssignStatement)result.AssignStatement).Right).Value);
        }
    }
}
