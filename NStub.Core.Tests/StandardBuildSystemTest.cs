namespace NStub.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MbUnit.Framework;
    using NStub.Core;
    using System.IO;

    [TestFixture()]
    public partial class StandardBuildSystemTest
    {
        private StandardBuildSystem testObject;
        private IBuildSystem fakeObject;
        
        [SetUp()]
        public void SetUp()
        {
            this.testObject = new StandardBuildSystem();
            this.fakeObject = new FakeBuildSystem();
        }
        
        [TearDown()]
        public void TearDown()
        {
            this.testObject = null;
        }
        
        [Test()]
        public void ConstructWithParametersTest()
        {
            this.testObject = new StandardBuildSystem();
        }
        
        [Test()]
        public void PropertyDirectorySeparatorCharNormalBehavior()
        {
            // Test read access of 'DirectorySeparatorChar' Property.
            var expected = Path.DirectorySeparatorChar;
            var actual = testObject.DirectorySeparatorChar;
            Assert.AreEqual(expected, actual);
        }
        
        [Test()]
        public void CreateDirectoryTest()
        {
            fakeObject.CreateDirectory("Bla");
        }
        
        [Test()]
        public void DirectoryExistsTest()
        {
            Assert.IsFalse(testObject.DirectoryExists("Bla45FDF%&$"));
        }
        
        [Test()]
        public void GetFileNameWithoutExtensionTest()
        {
            Assert.AreEqual("Bla", testObject.GetFileNameWithoutExtension("Bla.txt"));
        }
        
        [Test()]
        public void GetTextWriterTest()
        {
            Assert.IsNotNull(fakeObject.GetTextWriter("Bla", false));
        }
    }
}
