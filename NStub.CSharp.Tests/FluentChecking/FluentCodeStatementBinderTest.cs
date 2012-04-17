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
    using Gallio.Framework.Assertions;


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

            this.testObject = method.StatementsOfType<CodeExpressionStatement>().Where();
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
            
            testObject.Expression<CodeMethodInvokeExpression>(Is.MethodNamed("MethodName"));
            var expected = "[0]By comparing System.CodeDom.CodeMethodInvokeExpression큦 contained "+
                "in System.CodeDom.CodeExpressionStatement  elements, the value `MethodName` was not "+
                "found in the checked items: [{Inconclusive}]\r\n";
            actual = testObject.Error;
            Assert.AreEqual(expected, actual);

            expected = "[0]By comparing System.CodeDom.CodeMethodInvokeExpression큦 contained in System.CodeDom" +
                ".CodeExpressionStatement  elements, the value `MethodName` was not found in the checked items: " +
                "[{Inconclusive}]\r\n[1]WasFound(> 0) on 1 total items with 0 found items.\r\n[2]Auto WasFound() called.\r\n";
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
                }).Repeat.Times(3)
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
        public void ExpressionAutoAssertWithSecondFalseCall()
        {
            var generator = method.StaticClass("Assert").Invoke("Inconclusive").With("Thisone"); //.Commit().TypeReference;
            var codeExpr = generator.TypeReference;
            generator.Commit();

            var comparer1 = MockRepository.GenerateStrictMock<Func<CodeMethodInvokeExpression, CompareResult>>();
            comparer1.Expect((c) => c.Invoke(null)).IgnoreArguments()
                .Return(new CompareResult(true, "TheName", "TheComparer")).Repeat.Times(1, int.MaxValue);
            var comparer2 = MockRepository.GenerateStrictMock<Func<CodeMethodInvokeExpression, CompareResult>>();
            comparer2.Expect((c) => c.Invoke(null)).IgnoreArguments()
                .Return(new CompareResult(false, "TheName2", "TheComparer2")).Repeat.Times(2, int.MaxValue);
            comparer1.Replay();
            comparer2.Replay();

            var result = testObject.Expression<CodeMethodInvokeExpression>(comparer1);
            var testResult = result.Assert().Compile().Invoke();
            Assert.IsTrue(testResult);
            Assert.AreSame(testObject, result);

            result = testObject.Expression<CodeMethodInvokeExpression>(comparer2);
            testResult = result.Assert().Compile().Invoke();
            Assert.IsFalse(testResult);
            Assert.AreSame(testObject, result);

            comparer1.VerifyAllExpectations();
            comparer2.VerifyAllExpectations();
            //Assert.AreEqual(3, result.Result);

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
                .Return(new CompareResult(true, "TheName", "TheComparer")).Repeat.Any();
            //comparer.AssertWasNotCalled((c) => c.Invoke(null));
            comparer.Replay();

            var result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            var testResult = result.Assert().Compile().Invoke();
            Assert.IsTrue(testResult);
            Assert.AreSame(testObject, result);

            result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            testResult = result.Assert().Compile().Invoke();
            // Only if all comparer runs return false, the method is not found.
            Assert.IsTrue(testResult);
            Assert.AreSame(testObject, result);

            comparer.VerifyAllExpectations();

        }

        [Test()]
        public void ExpressionTwoMethodDeclarationsAndAllFalseCall()
        {
            var generator = method.StaticClass("Assert").Invoke("Inconclusive").With("Thisone"); //.Commit().TypeReference;
            var generator2 = method.StaticClass("Ping").Invoke("Me").With("ToDeath"); //.Commit().TypeReference;
            generator.Commit();
            generator2.Commit();

            var comparer = MockRepository.GenerateStrictMock<Func<CodeMethodInvokeExpression, CompareResult>>();
            comparer.Expect((c) => c.Invoke(null)).IgnoreArguments()
                .Return(new CompareResult(false, "TheName", "TheComparer")).Repeat.Twice();
            comparer.Expect((c) => c.Invoke(null)).IgnoreArguments()
                .Return(new CompareResult(false, "TheName2", "TheComparer2")).Repeat.Times(2, int.MaxValue);
            comparer.Replay();

            var result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            var testResult = result.Assert().Compile().Invoke();
            Assert.IsFalse(testResult);
            Assert.AreSame(testObject, result);

            result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            testResult = result.Assert().Compile().Invoke();
            // Only if all comparer runs return false, the method is not found.
            Assert.IsFalse(testResult);
            Assert.AreSame(testObject, result);

            comparer.VerifyAllExpectations();
            //Assert.AreEqual(3, result.Result);

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
            Assert.AreEqual("[0]Expression: By comparing System.CodeDom.CodeMethodInvokeExpression큦 contained in "+
                "System.CodeDom.CodeExpressionStatement elements an empty expression list always returns false\r\n[1]" +
                "WasFound(> 0) on 0 total items with -1 found items.\r\n[2]Auto WasFound() called.\r\n[3]Expression: " +
                "By comparing System.CodeDom.CodeMethodInvokeExpression큦 contained in System.CodeDom.CodeExpressionSta" +
                "tement elements an empty expression list always returns false\r\n[4]WasFound(> 0) on 0 total items with" +
                " -1 found items.\r\n[5]Auto WasFound() called.\r\n"
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
        public void ExpressionThrowsOnWrongStatementType()
        {
            Assert.Throws<AssertionException>(() => method.StatementsOfType<CodeAssignStatement>()
                .Where().Expression<CodeFieldReferenceExpression>(Is.FieldNamed("xMethodName"))
                .Assert().Compile().Invoke());
        }

        [Test()]
        public void ExpressionLeftThrowsOnWrongStatementType()
        {
            Assert.Throws<AssertionException>(()=>method.StatementsOfType<CodeExpressionStatement>()
                .Where().ExpressionLeft<CodeFieldReferenceExpression>(Is.FieldNamed("xMethodName"))
                .Assert().Compile().Invoke());
        }

        [Test()]
        public void ExpressionResult()
        {
            var generator = method.StaticClass("Assert").Invoke("Inconclusive").With("Thisone"); //.Commit().TypeReference;
            var codeExpr = generator.TypeReference;
            generator.Commit();

            var comparer = MockRepository.GenerateStrictMock<Func<CodeMethodInvokeExpression, CompareResult>>();
            var comparer2 = MockRepository.GenerateStrictMock<Func<CodeFieldReferenceExpression, CompareResult>>();
            //comparer.Expect((c) => c.Invoke(null)).IgnoreArguments()
              //  .Return(new CompareResult(false, "TheName", "TheComparer"));
            comparer.Expect((c) => c.Invoke(null)).IgnoreArguments()
                .Return(new CompareResult(true, "TheName2", "TheComparer2")).Repeat.Any();// Times(2, int.MaxValue);
            comparer2.Expect((c) => c.Invoke(null)).IgnoreArguments()
                .Return(new CompareResult(false, "TheName2", "TheComparer2")).Repeat.Any();// Times(2, int.MaxValue);
            comparer.Replay();
            comparer2.Replay();

            var result = testObject.Expression<CodeMethodInvokeExpression>(comparer);
            var testResult = result.Assert().Compile().Invoke();
            //Assert.IsFalse(testResult);

            //this.testObject = method.StatementsOfType<CodeExpressionStatement>().Where();

            //result = testObject.Expression<CodeMethodInvokeExpression>(comparer)
            //    .Expression<CodeMethodInvokeExpression>(comparer).IsNotEmpty().AndX<CodeExpressionStatement>(
            //    testObject.Expression<CodeMethodInvokeExpression>(comparer))
            //    ;
            //Assert.
             AssertEx.That(method.StatementsOfType<CodeExpressionStatement>()
                .Where().Expression<CodeMethodInvokeExpression>(Is.MethodNamed("OtherMethodName")).WasNotFound()
                .Assert());
            result = testObject.Expression<CodeMethodInvokeExpression>(comparer)
                .Expression<CodeMethodInvokeExpression>(comparer).WasFound()
                .Expression<CodeFieldReferenceExpression>(comparer2).WasNotFound()
                ;
            testResult = result.Assert().Compile().Invoke();
            Assert.IsTrue(testResult, "MSG: {0}", testObject.Error);

            comparer.VerifyAllExpectations();
            //Assert.AreEqual(13, result.Result);

        }




        [Test()]
        public void WithTest()
        {
            // TODO: Implement unit test for With

            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
