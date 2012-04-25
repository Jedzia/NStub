namespace NStub.CSharp.Tests.ObjectGeneration.FluentCodeBuild
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;
    using System.CodeDom;
    
    
    [TestFixture()]
    public partial class StaticClassTest
    {
        
        [Test()]
        public void OfTest()
        {
            var className = "MyClass";

            var expected = className;
            var actual = StaticClass.Of(className);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.Type.BaseType);
        }
        
        [Test()]
        public void PropertyTest()
        {
            var actual = StaticClass.Property(() => DateTime.Now);
            Assert.AreEqual("Now", actual.PropertyName);
            Assert.IsInstanceOfType<CodeTypeReferenceExpression>(actual.TargetObject);
            Assert.AreEqual(typeof(DateTime).FullName, ((CodeTypeReferenceExpression)actual.TargetObject).Type.BaseType);
        }
    }
}
