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
    [TestFixture]
    public class FluentCodeMethodCheckingTest
    {
        [Test]
        public void HasCommentTest()
        {
            var cm = new CodeMemberMethod();
            Assert.IsFalse(cm.HasComment("Bla").Compile().Invoke());
            cm.AddComment("You Foo");
            Assert.IsTrue(cm.HasComment("You Foo").Compile().Invoke());

            cm.AddComment(" ABC");
            cm.AddComment(" CDE ");
            cm.AddComment("E F G");

            Assert.IsTrue(cm.HasComment("E F G").Compile().Invoke());
            Assert.IsTrue(cm.HasComment(" CDE ").Compile().Invoke());
            Assert.IsTrue(cm.HasComment(" ABC").Compile().Invoke());
            Assert.IsTrue(cm.HasComment("You Foo").Compile().Invoke());

            Assert.IsFalse(cm.HasComment(" aBC").Compile().Invoke());
        }
    }
}
