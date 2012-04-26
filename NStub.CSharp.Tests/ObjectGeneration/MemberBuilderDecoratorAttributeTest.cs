namespace NStub.CSharp.Tests.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    
    
    [TestFixture()]
    public partial class MemberBuilderDecoratorAttributeTest
    {
        
        private System.Type decoratedType;
        private MemberBuilderDecoratorAttribute testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.decoratedType = typeof(NStub.CSharp.ObjectGeneration.Builders.StaticMethodBuilder);
            this.testObject = new MemberBuilderDecoratorAttribute(this.decoratedType);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersDecoratedTypeTest()
        {
            this.decoratedType = typeof(NStub.CSharp.ObjectGeneration.Builders.RenamingBuilder);
            this.testObject = new MemberBuilderDecoratorAttribute(this.decoratedType);
            Assert.Throws<ArgumentException>(() => new MemberBuilderDecoratorAttribute(null));
            Assert.Throws<ArgumentException>(() => new MemberBuilderDecoratorAttribute(typeof(object)));
        }
        
        [Test()]
        public void PropertyDecoratedTypeNormalBehavior()
        {
            // Test read access of 'DecoratedType' Property.
            var expected = this.decoratedType;
            var actual = testObject.DecoratedType;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyTypeIdNormalBehavior()
        {
            // Test read access of 'TypeId' Property.
            var expected = testObject.GetType();
            var actual = testObject.TypeId;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void IsDefaultAttributeTest()
        {
            var actual = testObject.IsDefaultAttribute();
            Assert.IsFalse(actual);
        }
        
        [Test()]
        public void MatchTest()
        {
            Assert.IsFalse(testObject.Match(this));
            Assert.IsTrue(testObject.Match(testObject));
        }
    }
}
