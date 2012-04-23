namespace NStub.CoreNStub.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core;
    
    
    [TestFixture()]
    public partial class CodeGeneratorParametersTest
    {
        
        private string outputDirectory;
        private CodeGeneratorParameters testObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.outputDirectory = "Value of outputDirectory";
            this.testObject = new CodeGeneratorParameters(this.outputDirectory);
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersOutputDirectoryTest()
        {
            this.outputDirectory = "Value of outputDirectory";
            this.testObject = new CodeGeneratorParameters(this.outputDirectory);
            Assert.Throws<ArgumentException>(() => new CodeGeneratorParameters(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new CodeGeneratorParameters(null));
        }
        
        [Test()]
        public void PropertyOutputDirectoryNormalBehavior()
        {
            // Test read access of 'OutputDirectory' Property.
            var expected = this.outputDirectory;
            var actual = testObject.OutputDirectory;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void PropertyUseSetupAndTearDownNormalBehavior()
        {
            // Test read access of 'UseSetupAndTearDown' Property.
            var expected = false;
            var actual = testObject.UseSetupAndTearDown;
            Assert.AreEqual(expected, actual);

            // Test write access of 'UseSetupAndTearDown' Property.
            expected = true;
            testObject.UseSetupAndTearDown = expected;
            actual = testObject.UseSetupAndTearDown;
            Assert.AreEqual(expected, actual);
        }
    }
}
