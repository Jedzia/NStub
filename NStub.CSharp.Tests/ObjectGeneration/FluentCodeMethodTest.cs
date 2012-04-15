namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration;
    using System.CodeDom;
    using NStub.CSharp.Tests.FluentChecking;
    
    
    public partial class FluentCodeMethodTest
    {

        private CodeMemberMethod method;

        [SetUp()]
        public void SetUp()
        {
            this.method = new CodeMemberMethod();
        }

        [Test()]
        public void AddBlankLineTest()
        {
            var result = method.AddBlankLine();
            Assert.AreSame(method, result);
            Assert.IsInstanceOfType<CodeSnippetStatement>(result.Statements[0]);
            var snippet = (CodeSnippetStatement)result.Statements[0];
            Assert.AreEqual(string.Empty, snippet.Value);
        }
        
        [Test()]
        public void AddCommentTest()
        {
            var expectedComment = "My Comment is very cute.";
            var result = method.AddComment(expectedComment);
            Assert.AreSame(method, result);
            AssertEx.That(method.HasComment(expectedComment), "Comment '{0}' not found in: {1}", expectedComment, method.HasCommentMsg());
        }
        
        [Test()]
        public void AddMethodAttributeTest()
        {
            method.AddMethodAttribute("Test");
            AssertEx.That(method.HasAttribute("Test"), "Attribute 'Test' not found in: {0}", method.HasAttributeMsg());
        }
        
        [Test()]
        public void AssignTest()
        {
            var expected = "memberField";
            var result = method.Assign(expected);
            Assert.IsInstanceOfType<CodeFieldReferenceBinder>(result);
            Assert.AreEqual(expected, result.FieldReference.FieldName);
        }
        
        [Test()]
        public void ClearParametersTest()
        {
            Assert.AreEqual(0, method.Parameters.Count);
            var result = method.ClearParameters();
            Assert.AreSame(method, result);
            Assert.AreEqual(0, method.Parameters.Count);

            // Todo: fluent .AddParameter(string type, string name) .AddParameter<T>(string name)
            method.Parameters.Add(new CodeParameterDeclarationExpression("MyType", "parameterName"));
            method.Parameters.Add(new CodeParameterDeclarationExpression("MyOtherType", "parameterName2"));
            Assert.AreEqual(2, method.Parameters.Count);
            result = method.ClearParameters();
            Assert.AreSame(method, result);
            Assert.AreEqual(0, method.Parameters.Count);
        }
        
        [Test()]
        public void SetNameTest()
        {
            Assert.AreEqual(string.Empty, method.Name);
            
            var expected = "Purzel";
            var result = method.SetName(expected);
            Assert.AreSame(method, result);
            var actual = method.Name;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void StaticClassTest()
        {
            var expected = "ReferencedClass";
            var result = method.StaticClass(expected);
            Assert.IsInstanceOfType<CodeTypeReferenceBinder>(result);
            Assert.AreEqual(expected, result.TypeReference.Type.BaseType);
        }
        
        [Test()]
        public void WithReturnTypeTest()
        {
            Assert.AreEqual(typeof(void).ToString(), method.ReturnType.BaseType);

            var expected = typeof(string);
            var result = method.WithReturnType(expected);
            Assert.AreSame(method, result);
            var actual = method.ReturnType.BaseType;
            Assert.AreEqual(expected.ToString(), actual);

            expected = typeof(FluentCodeMethodTest);
            result = method.WithReturnType<FluentCodeMethodTest>();
            Assert.AreSame(method, result);
            actual = method.ReturnType.BaseType;
            Assert.AreEqual(expected.ToString(), actual);
        }
    }
}
