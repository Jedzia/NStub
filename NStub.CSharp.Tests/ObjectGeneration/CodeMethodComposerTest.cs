// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeMethodComposerTest.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.CodeDom;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using NStub.CSharp.Tests.FluentChecking;
    using NStub.CSharp.Tests.Stubs;

    public class CodeMethodComposerTest
    {
        [Test]
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
            var actualRef = actualInvoke.Method;
            Assert.IsInstanceOfType<CodeTypeReferenceExpression>(actualRef.TargetObject);
            var actualtypeRef = (CodeTypeReferenceExpression)actualRef.TargetObject;
            Assert.IsInstanceOfType<CodeTypeReference>(actualtypeRef.Type);
            var actualtype = actualtypeRef.Type;

            Assert.AreEqual("Assert", actualtype.BaseType);

            Assert.IsInstanceOfType<CodePrimitiveExpression>(actualInvoke.Parameters[0]);
            var actualPrimExpr = (CodePrimitiveExpression)actualInvoke.Parameters[0];
            Assert.AreEqual(expectedText, actualPrimExpr.Value);
        }

        [Test]
        public void CreateAndInitializeMemberFieldTest()
        {
            var type = typeof(string);
            var memberfield = "theMember";
            var actual = CodeMethodComposer.CreateAndInitializeMemberField(type, memberfield);
            Assert.IsInstanceOfType<CodeAssignStatement>(actual);

            object expectedValue = true;
            Type expectedType = expectedValue.GetType();
            actual = CodeMethodComposer.CreateAndInitializeMemberField(expectedType, memberfield);
            var assignment = (CodePrimitiveExpression)actual.Right;
            Assert.AreEqual(true, assignment.Value);
            Assert.AreEqual(expectedType, assignment.Value.GetType());

            expectedValue = typeof(CodeMethodComposerTest);
            expectedType = expectedValue.GetType();
            actual = CodeMethodComposer.CreateAndInitializeMemberField(expectedType, memberfield);
            var assignment2 = (CodeObjectCreateExpression)actual.Right;
            Assert.AreEqual(expectedType.FullName, assignment2.CreateType.BaseType);

            expectedValue = 35;
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

        [Test]
        public void CreateTestStubForMethodTest()
        {
            var method = new CodeMemberMethod();
            method.Name = "TheMethodName";
            CodeMethodComposer.CreateTestStubForMethod(method);
            var expectedComment = "TODO: Implement unit test for TheMethodName";
            AssertEx.That(
                method.ContainsComment(expectedComment),
                "Comment '{0}' not found in: {1}",
                expectedComment,
                method.ContainsCommentMsg());
            AssertEx.That(
                method.ContainsAttribute("Test"), "Attribute 'Test' not found in: {0}", method.ContainsAttributeMsg());
            AssertEx.That(method.HasReturnTypeOf(typeof(void)));
        }

        [Test]
        public void CreateTestStubForMethodWithEmptyNameThrows()
        {
            var method = new CodeMemberMethod();
            Assert.Throws<ArgumentException>(() => CodeMethodComposer.CreateTestStubForMethod(method));
        }
    }
}