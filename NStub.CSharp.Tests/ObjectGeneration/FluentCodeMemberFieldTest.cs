namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using System.CodeDom;
    
    
    public partial class FluentCodeMemberFieldTest
    {
        [Test()]
        public void Create()
        {
            var actual = FluentCodeMemberField.Create("myField", typeof(string).Name);
            Assert.AreEqual("myField", actual.Name);
            Assert.AreEqual(MemberAttributes.Private, actual.Attributes);
            Assert.AreEqual("string", actual.Type.BaseType);
        }

        [Test()]
        public void CreateGeneric()
        {
            var actual = FluentCodeMemberField.Create<string>("myField");
            Assert.AreEqual("myField", actual.Name);
            Assert.AreEqual(MemberAttributes.Private, actual.Attributes);
            Assert.AreEqual("System.String", actual.Type.BaseType);
        }

        [Test()]
        public void CreateWithType()
        {
            var actual = FluentCodeMemberField.Create("myField", typeof(string));
            Assert.AreEqual("myField", actual.Name);
            Assert.AreEqual(MemberAttributes.Private, actual.Attributes);
            Assert.AreEqual("System.String", actual.Type.BaseType);
        }

    }
}
