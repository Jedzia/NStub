using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.CodeDom;
using NStub.CSharp.ObjectGeneration;

namespace NStub.CSharp.Tests.FluentChecking
{
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;
    using Gallio.Framework.Assertions;
    using NStub.CSharp.Tests.Stubs;

    [TestFixture]
    public class FluentCodeMethodCheckingTest
    {
        [Test]
        public void ContainsCommentOnEmptyCommentsShouldThrow()
        {
            var cm = new CodeMemberMethod();
            Assert.Throws<AssertionException>(() => cm.ContainsComment("Bla").Compile().Invoke());
        }

        [Test]
        public void ContainsCommentTest()
        {
            var cm = new CodeMemberMethod();
            cm.AddComment("You Foo");
            Assert.IsTrue(cm.ContainsComment("You Foo").Compile().Invoke());

            cm.AddComment(" ABC");
            cm.AddComment(" CDE ");
            cm.AddComment("E F G");

            Assert.IsTrue(cm.ContainsComment("E F G").Compile().Invoke());
            Assert.IsTrue(cm.ContainsComment(" CDE ").Compile().Invoke());
            Assert.IsTrue(cm.ContainsComment(" ABC").Compile().Invoke());
            Assert.IsTrue(cm.ContainsComment("You Foo").Compile().Invoke());

            Assert.IsFalse(cm.ContainsComment(" aBC").Compile().Invoke());
        }

        [Test]
        public void ContainsCommentMessageTest()
        {
            var cm = new CodeMemberMethod();
            var expected = string.Empty;
            var actual = cm.ContainsCommentMsg();
            Assert.AreEqual(expected, actual);

            cm.AddComment("You Foo");
            expected = "[{You Foo}]";
            actual = cm.ContainsCommentMsg();
            Assert.AreEqual(expected, actual);

            cm.AddComment(" ABC");
            cm.AddComment(" CDE ");
            cm.AddComment("E F G");
            expected = "[{You Foo}, { ABC}, { CDE }, {E F G}]";
            actual = cm.ContainsCommentMsg();
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void ContainsAttributeOnEmptyCommentsShouldThrow()
        {
            var cm = new CodeMemberMethod();
            Assert.Throws<AssertionException>(() => cm.ContainsAttribute("Bla").Compile().Invoke());
        }

        [Test]
        public void ContainsAttributeTest()
        {
            var cm = new CodeMemberMethod();
            cm.AddMethodAttribute("You Foo");
            Assert.IsTrue(cm.ContainsAttribute("You Foo").Compile().Invoke());

            cm.AddMethodAttribute(" ABC");
            cm.AddMethodAttribute(" CDE ");
            cm.AddMethodAttribute("E F G");

            Assert.IsTrue(cm.ContainsAttribute("E F G").Compile().Invoke());
            Assert.IsTrue(cm.ContainsAttribute(" CDE ").Compile().Invoke());
            Assert.IsTrue(cm.ContainsAttribute(" ABC").Compile().Invoke());
            Assert.IsTrue(cm.ContainsAttribute("You Foo").Compile().Invoke());

            Assert.IsFalse(cm.ContainsAttribute(" aBC").Compile().Invoke());
        }

        [Test]
        public void ContainsAttributeMessageTest()
        {
            var cm = new CodeMemberMethod();
            var expected = string.Empty;
            var actual = cm.ContainsAttributeMsg();
            Assert.AreEqual(expected, actual);

            cm.AddMethodAttribute("You Foo");
            expected = "[{You Foo}]";
            actual = cm.ContainsAttributeMsg();
            Assert.AreEqual(expected, actual);

            cm.AddMethodAttribute(" ABC");
            cm.AddMethodAttribute(" CDE ");
            cm.AddMethodAttribute("E F G");
            expected = "[{You Foo}, { ABC}, { CDE }, {E F G}]";
            actual = cm.ContainsAttributeMsg();
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void HasReturnTypeTest()
        {
            var cm = new CodeMemberMethod();
            Assert.IsFalse(cm.HasReturnType<string>().Compile().Invoke());

            cm.WithReturnType<int>();
            Assert.IsFalse(cm.HasReturnType<string>().Compile().Invoke());
            Assert.IsTrue(cm.HasReturnType<int>().Compile().Invoke());
        }

        [Test]
        public void HasReturnTypeOfTest()
        {
            var cm = new CodeMemberMethod();
            Assert.IsFalse(cm.HasReturnTypeOf(typeof(bool)).Compile().Invoke());

            cm.WithReturnType<InfoApe>();
            Assert.IsFalse(cm.HasReturnTypeOf(typeof(uint)).Compile().Invoke());
            Assert.IsTrue(cm.HasReturnTypeOf(typeof(InfoApe)).Compile().Invoke());
        }

        [Test]
        public void ContainsStatementOnEmptyStatementShouldThrow()
        {
            var cm = new CodeMemberMethod();
            Assert.Throws<AssertionException>(() => cm.ContainsStatement(typeof(bool)).Compile().Invoke());
        }

        [Test]
        public void ContainsStatementTest()
        {
            var cm = new CodeMemberMethod();
            cm.AddBlankLine();
            Assert.IsFalse(cm.ContainsStatement(typeof(bool)).Compile().Invoke());

            cm.WithReturnType<InfoApe>();
            Assert.IsFalse(cm.ContainsStatement(typeof(uint)).Compile().Invoke());
            Assert.IsTrue(cm.ContainsStatement(typeof(CodeSnippetStatement)).Compile().Invoke());
            Assert.IsFalse(cm.ContainsStatement<CodeStatement>().Compile().Invoke());
            Assert.IsTrue(cm.ContainsStatement<CodeSnippetStatement>().Compile().Invoke());
        }

    }
}
