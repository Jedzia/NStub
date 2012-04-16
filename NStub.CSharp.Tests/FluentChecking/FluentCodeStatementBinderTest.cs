namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.Tests.FluentChecking;
    using System.CodeDom;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;
    using NStub.CSharp.ObjectGeneration;
    using Rhino.Mocks;


    public partial class FluentCodeStatementBinderTest
    {

        private FluentCodeStatementBinder<CodeExpressionStatement> testObject;
        private CodeMemberMethod method;

        [SetUp()]
        public void SetUp()
        {
            // ToDo: Implement SetUp logic here 
            this.method = new CodeMemberMethod();
            //this.testObject 

            this.testObject = method.StatementsOf<CodeExpressionStatement>().Where();
            //.Expression<CodeMethodInvokeExpression>(Is.Named("MethodName"))
            //.Assert()
            //this.testObject = new FluentCodeStatementBinder<CodeStatement>();
        }

        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }

        [Test()]
        public void PropertyErrorNormalBehavior()
        {
            // Test read access of 'Error' Property.
            var actual = testObject.Error;
            Assert.IsNull(actual);

            method.StaticClass("Assert").Invoke("Inconclusive").With("Thisone").Commit();
            
            testObject.Expression<CodeMethodInvokeExpression>(Is.Named("MethodName"));
            var expected = "[0]By comparing System.CodeDom.CodeMethodInvokeExpression´s contained in System.CodeDom.CodeExpressionStatement elements, the value `MethodName` was not found in the checked items: [{Inconclusive}]\r\n";
            actual = testObject.Error;
            Assert.AreEqual(expected, actual);
  
            var result = testObject.Assert();
            actual = testObject.Error;
            Assert.AreEqual(expected, actual);

            result.Compile().Invoke();

            actual = testObject.Error;
            Assert.AreEqual(expected, actual);

        }

        [Test()]
        public void AssertTest()
        {
            // TODO: Implement unit test for Assert

            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [Test()]
        public void ExpressionWhenTrueTest()
        {
            var generator = method.StaticClass("Assert").Invoke("Inconclusive").With("Thisone"); //.Commit().TypeReference;
            var codeExpr = generator.TypeReference;
            generator.Commit();

            var comparer = MockRepository.GenerateStrictMock<Func<CodeMethodInvokeExpression, CompareResult>>();
            comparer.Expect((c) => c.Invoke(null))
                .Constraints(Rhino.Mocks.Constraints.Is.TypeOf<CodeMethodInvokeExpression>())
                .Do((Func<CodeMethodInvokeExpression, CompareResult>)delegate(CodeMethodInvokeExpression expr)
                {
                    Assert.AreEqual("Inconclusive", expr.Method.MethodName);
                    Assert.Contains(expr.Parameters
                        .OfType<CodePrimitiveExpression>()
                        .Select(e=>e.Value as string),
                        "Thisone");
                    return new CompareResult(true, "TheName", "TheComparer");
                })
                //.Return(new CompareResult(true, "TheName", "TheComparer")
            ;
            comparer.Replay();

            var result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            var testResult = result.Assert().Compile().Invoke();
            Assert.IsTrue(testResult);
            Assert.AreSame(testObject, result);
            comparer.VerifyAllExpectations();
        }

        [Test()]
        public void ExpressionWithSecondFalseCall()
        {
            var generator = method.StaticClass("Assert").Invoke("Inconclusive").With("Thisone"); //.Commit().TypeReference;
            var codeExpr = generator.TypeReference;
            generator.Commit();

            var comparer = MockRepository.GenerateStrictMock<Func<CodeMethodInvokeExpression, CompareResult>>();
            comparer.Expect((c) => c.Invoke(null)).IgnoreArguments()
                .Return(new CompareResult(true, "TheName", "TheComparer"));
            comparer.Expect((c) => c.Invoke(null)).IgnoreArguments()
                .Return(new CompareResult(false, "TheName2", "TheComparer2")).Repeat.Times(2, int.MaxValue);
            comparer.Replay();

            var result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            var testResult = result.Assert().Compile().Invoke();
            Assert.IsTrue(testResult);
            Assert.AreSame(testObject, result);

            result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            testResult = result.Assert().Compile().Invoke();
            Assert.IsFalse(testResult);
            Assert.AreSame(testObject, result);
            
            comparer.VerifyAllExpectations();

        }

        [Test()]
        public void ExpressionTwoMethodDeclarationsAndSecondFalseCall()
        {
            var generator = method.StaticClass("Assert").Invoke("Inconclusive").With("Thisone"); //.Commit().TypeReference;
            var generator2 = method.StaticClass("Ping").Invoke("Me").With("ToDeath"); //.Commit().TypeReference;
            generator.Commit();
            generator2.Commit();

            var comparer = MockRepository.GenerateStrictMock<Func<CodeMethodInvokeExpression, CompareResult>>();
            comparer.Expect((c) => c.Invoke(null)).IgnoreArguments()
                .Return(new CompareResult(true, "TheName", "TheComparer")).Repeat.Twice();
            comparer.Expect((c) => c.Invoke(null)).IgnoreArguments()
                .Return(new CompareResult(false, "TheName2", "TheComparer2")).Repeat.Times(2, int.MaxValue);
            comparer.Replay();

            var result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            var testResult = result.Assert().Compile().Invoke();
            Assert.IsTrue(testResult);
            Assert.AreSame(testObject, result);

            result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            testResult = result.Assert().Compile().Invoke();
            Assert.IsFalse(testResult);
            Assert.AreSame(testObject, result);

            comparer.VerifyAllExpectations();

        }

        [Test()]
        public void ExpressionNoMethodDeclarations()
        {
            // no commit = no method.
            var generator = method.StaticClass("Assert").Invoke("Inconclusive").With("Thisone"); //.Commit().TypeReference;
            var comparer = MockRepository.GenerateStrictMock<Func<CodeMethodInvokeExpression, CompareResult>>();
            comparer.Replay();

            var result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            var testResult = result.Assert().Compile().Invoke();
            Assert.IsFalse(testResult);
            Assert.AreSame(testObject, result);

            result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            testResult = result.Assert().Compile().Invoke();
            Assert.IsFalse(testResult);
            Assert.AreSame(testObject, result);
            Assert.AreEqual("[0]Expression: By comparing System.CodeDom.CodeMethodInvokeExpression´s contained in " +
                "System.CodeDom.CodeExpressionStatement elements an empty expression list always returns false\r\n" +
                "[1]Expression: By comparing System.CodeDom.CodeMethodInvokeExpression´s contained in System.CodeDo" +
                "m.CodeExpressionStatement elements an empty expression list always returns false\r\n"
                , testObject.Error);

            comparer.VerifyAllExpectations();

        }

        [Test()]
        public void ExpressionWhenFalseTest()
        {
            var generator = method.StaticClass("Assert").Invoke("Inconclusive").With("Thisone"); //.Commit().TypeReference;
            var codeExpr = generator.TypeReference;
            generator.Commit();

            CodeMethodInvokeExpression firstInvocation;
            var comparer = MockRepository.GenerateStrictMock<Func<CodeMethodInvokeExpression, CompareResult>>();
            comparer.Expect((c) => c.Invoke(null))
                .Constraints(Rhino.Mocks.Constraints.Is.TypeOf<CodeMethodInvokeExpression>())
                .Do((Func<CodeMethodInvokeExpression, CompareResult>)delegate(CodeMethodInvokeExpression expr)
                {
                    firstInvocation = expr;
                    Assert.AreEqual("Inconclusive", expr.Method.MethodName);
                    Assert.Contains(expr.Parameters
                        .OfType<CodePrimitiveExpression>()
                        .Select(e => e.Value as string),
                        "Thisone");
                    return new CompareResult(false, "TheName", "TheComparer");
                }).Repeat.Times(2, int.MaxValue);


            comparer.Replay();

            var result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            var testResult = result.Assert().Compile().Invoke();
            Assert.IsFalse(testResult);
            Assert.AreSame(testObject, result);
            comparer.VerifyAllExpectations();
        }


        [Test()]
        public void WithTest()
        {
            // TODO: Implement unit test for With

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
