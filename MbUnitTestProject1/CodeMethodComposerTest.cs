using NStub.CSharp.ObjectGeneration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.CodeDom;

namespace MbUnitTestProject1
{
    
    
    /// <summary>
    ///This is a test class for CodeMethodComposerTest and is intended
    ///to contain all CodeMethodComposerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CodeMethodComposerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CreateTestStubForMethod
        ///</summary>
        [TestMethod()]
        public void CreateTestStubForMethodTest()
        {
            CodeMemberMethod codeMemberMethod = null; // TODO: Initialize to an appropriate value
            CodeMethodComposer.CreateTestStubForMethod(codeMemberMethod);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateAndInitializeMemberField
        ///</summary>
        [TestMethod()]
        public void CreateAndInitializeMemberFieldTest()
        {
            Type type = null; // TODO: Initialize to an appropriate value
            string memberField = string.Empty; // TODO: Initialize to an appropriate value
            CodeAssignStatement expected = null; // TODO: Initialize to an appropriate value
            CodeAssignStatement actual;
            actual = CodeMethodComposer.CreateAndInitializeMemberField(type, memberField);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
