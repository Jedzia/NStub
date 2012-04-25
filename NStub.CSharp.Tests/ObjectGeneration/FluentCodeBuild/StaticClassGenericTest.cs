namespace NStub.CSharp.Tests.ObjectGeneration.FluentCodeBuild
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;
    using System.CodeDom;
    
    
    [TestFixture()]
    public partial class StaticClassGenericTest
    {
        
        [Test()]
        public void PropertyTest()
        {
            // Parameter 'propertyName' is of type String
            var propertyName = "Empty";

            var expected = propertyName;
            var actual = StaticClass<string>.Property(propertyName);
            Assert.AreEqual(expected, actual.PropertyName);
            Assert.IsInstanceOfType<CodeTypeReferenceExpression>(actual.TargetObject);
            Assert.AreEqual(typeof(string).FullName, ((CodeTypeReferenceExpression)actual.TargetObject).Type.BaseType);
        }
    }
}
