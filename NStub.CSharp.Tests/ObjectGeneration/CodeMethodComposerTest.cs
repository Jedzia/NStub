namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using System.CodeDom;
    using NStub.CSharp.Tests.FluentChecking;
using System.Linq.Expressions;
    using NStub.CSharp.Tests.Stubs;
    
    
    public partial class CodeMethodComposerTest
    {
        [Test()]
        public void AppendAssertInconclusiveTest()
        {
            var method = new CodeMemberMethod();
            var expectedText = "The assert text";
            CodeMethodComposer.AppendAssertInconclusive(method, expectedText);
            var actualMethodStatements = method.Statements;
            Assert.AreEqual(2, actualMethodStatements.Count);
            Assert.IsInstanceOfType<CodeExpressionStatement>(actualMethodStatements[1]);
            var actualExpr = (CodeExpressionStatement)method.Statements[1];
            Assert.IsInstanceOfType<CodeMethodInvokeExpression>(actualExpr.Expression);
            var actualInvoke = (CodeMethodInvokeExpression)actualExpr.Expression;
            Assert.IsInstanceOfType<CodeMethodReferenceExpression>(actualInvoke.Method);
            var actualRef = (CodeMethodReferenceExpression)actualInvoke.Method;
            Assert.IsInstanceOfType<CodeTypeReferenceExpression>(actualRef.TargetObject);
            var actualtypeRef = (CodeTypeReferenceExpression)actualRef.TargetObject;
            Assert.IsInstanceOfType<CodeTypeReference>(actualtypeRef.Type);
            var actualtype = (CodeTypeReference)actualtypeRef.Type;

            Assert.AreEqual("Assert", actualtype.BaseType);

            Assert.IsInstanceOfType<CodePrimitiveExpression>(actualInvoke.Parameters[0]);
            var actualPrimExpr = (CodePrimitiveExpression)actualInvoke.Parameters[0];
            Assert.AreEqual(expectedText, actualPrimExpr.Value);
        }
        
        [Test()]
        public void CreateAndInitializeMemberFieldTest()
        {
            var type = typeof(string);
            var memberfield = "theMember";
            var actual = CodeMethodComposer.CreateAndInitializeMemberField(type, memberfield);
            Assert.IsInstanceOfType<CodeAssignStatement>(actual);

            object expectedValue = (bool)true;
            Type expectedType = expectedValue.GetType();
            actual = CodeMethodComposer.CreateAndInitializeMemberField(expectedType, memberfield);
            var assignment = (CodePrimitiveExpression)actual.Right;
            Assert.AreEqual(true, assignment.Value);
            Assert.AreEqual(expectedType, assignment.Value.GetType());

            expectedValue = (Type)typeof(CodeMethodComposerTest);
            expectedType = expectedValue.GetType();
            actual = CodeMethodComposer.CreateAndInitializeMemberField(expectedType, memberfield);
            var assignment2 = (CodeObjectCreateExpression)actual.Right;
            Assert.AreEqual(expectedType.FullName, assignment2.CreateType.BaseType);

            expectedValue = (int)35;
            expectedType = expectedValue.GetType();
            actual = CodeMethodComposer.CreateAndInitializeMemberField(expectedType, memberfield);
            assignment = (CodePrimitiveExpression)actual.Right;
            Assert.AreEqual(1234, assignment.Value);
            Assert.AreEqual(expectedType, assignment.Value.GetType());

            expectedValue = new InfoApe();
            expectedType = expectedValue.GetType();
            actual = CodeMethodComposer.CreateAndInitializeMemberField(expectedType, memberfield);
            assignment2 = (CodeObjectCreateExpression)actual.Right;
            Assert.AreEqual(expectedType.FullName, assignment2.CreateType.BaseType);
            // Todo: implement all CLR-Types.
        }

        [Test()]
        public void CreateTestStubForMethodWithEmptyNameThrows()
        {
            var method = new CodeMemberMethod();
            Assert.Throws<ArgumentException>(() => CodeMethodComposer.CreateTestStubForMethod(method));
        }

        [Test()]
        public void CreateTestStubForMethodTest()
        {
            var method = new CodeMemberMethod();
            method.Name = "TheMethodName";
            CodeMethodComposer.CreateTestStubForMethod(method);
            var expectedComment = "TODO: Implement unit test for TheMethodName";
            AssertEx.That(method.ContainsComment(expectedComment), "Comment '{0}' not found in: {1}", expectedComment, method.ContainsCommentMsg());
            AssertEx.That(method.ContainsAttribute("Test"), "Attribute 'Test' not found in: {0}", method.ContainsAttributeMsg());
            AssertEx.That(method.HasReturnTypeOf(typeof(void)));
        }
    }
}
