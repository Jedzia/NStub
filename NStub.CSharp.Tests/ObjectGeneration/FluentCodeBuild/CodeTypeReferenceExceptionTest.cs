namespace NStub.CSharp.ObjectGeneration.FluentCodeBuild
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;
    using System.CodeDom;
    
    
    public partial class CodeTypeReferenceExceptionTest
    {
        
        private NStub.CSharp.ObjectGeneration.FluentCodeBuild.CodeTypeReferenceBinder binder;
        private System.Exception inner;
        private string message;
        private CodeTypeReferenceException testObject;
        
        public CodeTypeReferenceExceptionTest()
        {
        }
        
        [SetUp()]
        public void SetUp()
        {
            // ToDo: Implement SetUp logic here
            var method = new CodeMemberMethod();
            this.binder = method.StaticClass("ReferencedClass");
            this.message = "Value of message";
            this.inner = new System.Exception();
            this.testObject = new CodeTypeReferenceException(this.binder, this.message, this.inner);
        }
        
        [Test()]
        public void ConstructWithParametersBinderMessageInnerTest()
        {
            this.testObject = new CodeTypeReferenceException(this.binder, this.message, this.inner);
            this.testObject = new CodeTypeReferenceException(this.binder, null, this.inner);
            var expectedMsg = "Exception of type \'NStub.CSharp.ObjectGeneration.FluentCodeBuild.CodeTypeReferenceException\' was thrown.";
            Assert.AreEqual(expectedMsg, testObject.Message);
            this.testObject = new CodeTypeReferenceException(this.binder, null, null);
            Assert.AreEqual(expectedMsg, testObject.Message);
            Assert.Throws<ArgumentNullException>(() => new CodeTypeReferenceException(null, this.message, this.inner));
        }
        
        [Test()]
        public void ConstructWithParametersBinderMessageTest()
        {
            this.message = "Value of message";
            this.testObject = new CodeTypeReferenceException(this.binder, this.message);
            Assert.IsNull(testObject.InnerException);
            Assert.AreEqual(this.message, testObject.Message);

            this.testObject = new CodeTypeReferenceException(this.binder, null);
            var expectedMsg = "Exception of type \'NStub.CSharp.ObjectGeneration.FluentCodeBuild.CodeTypeReferenceException\' was thrown.";
            Assert.AreEqual(expectedMsg, testObject.Message);
            Assert.Throws<ArgumentNullException>(() => new CodeTypeReferenceException(null, this.message));
        }
        
        [Test()]
        public void ConstructWithParametersBinderTest()
        {
            this.testObject = new CodeTypeReferenceException(this.binder);
            Assert.IsNull(testObject.InnerException);
            var expectedMsg = "Exception of type \'NStub.CSharp.ObjectGeneration.FluentCodeBuild.CodeTypeReferenceException\' was thrown.";
            Assert.AreEqual(expectedMsg,testObject.Message);
            
            Assert.Throws<ArgumentNullException>(() => new CodeTypeReferenceException(null));
        }
        
        [Test()]
        public void PropertyInnerExceptionNormalBehavior()
        {
            // Test read access of 'InnerException' Property.
            var expected = this.inner;
            var actual = testObject.InnerException;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyMessageNormalBehavior()
        {
            // Test read access of 'Message' Property.
            var expected = "Value of message";
            var actual = testObject.Message;
            Assert.AreEqual(expected, actual);
        }
    }
}
