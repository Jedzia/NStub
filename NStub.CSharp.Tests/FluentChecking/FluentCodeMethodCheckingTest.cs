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

    [TestFixture]
    public class FluentCodeMethodCheckingTest
    {
        [Test]
        public void HasCommentTest()
        {
            var cm = new CodeMemberMethod();
            Assert.IsFalse(cm.ContainsComment("Bla").Compile().Invoke());
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
    }
}
