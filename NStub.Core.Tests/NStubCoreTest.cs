using System;
using System.CodeDom;
//using System.IO;
using global::MbUnit.Framework;
using Rhino.Mocks;

namespace NStub.Core.Tests
{

    internal class FakeBuildSystem : IBuildSystem
    {
        public bool FakeDirectoryExists = true;

        #region IBuildSystem Members

        public char DirectorySeparatorChar
        {
            get { return System.IO.Path.DirectorySeparatorChar; }
        }

        public bool DirectoryExists(string directory)
        {
            return FakeDirectoryExists;
        }

        public System.IO.TextWriter GetTextWriter(string path, bool append)
        {
            var ms = new System.IO.MemoryStream();
            return new System.IO.StreamWriter(ms);
        }

        #endregion

        #region IBuildSystem Members


        public string GetFileNameWithoutExtension(string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }

        public System.IO.DirectoryInfo CreateDirectory(string path)
        {
            return new System.IO.DirectoryInfo(@"C:\Tmp");
        }

        #endregion
    }

	/// <summary>
	/// This class exercises all major methods of NStubCore.  Mock objects are
	/// used where possible to enhance granularity.
	/// </summary>
	[TestFixture]
	public class NStubCoreTest
	{
		#region Fields (Private)

		private string _outputDirectory;
		private string _sampleNamespace = "ThisIsMySampleNamespace";
		private ICodeGenerator _mockCodeGenerator;
		private NStubCore _nStubCore;

		#endregion Fields (Private)

		#region TestFixtureSetUp (Public)

		/// <summary>
		/// Performs the initial setup for the entire test suite.  We create
		/// an temporary output directory for code generation.  This directory
		/// will be deleted during cleanup.
		/// </summary>
		[FixtureSetUp]
		public void TestFixtureSetUp()
		{
			_outputDirectory = System.IO.Path.GetTempPath();
		} 

		#endregion TestFixtureSetUp (Public)

		/// <summary>
		/// Performs clean up after the entire test suite has completed.
		/// The scratch ouput directory created for testing will be 
		/// completely removed.
		/// </summary>
		[FixtureTearDown]
		public void TestFixtureTearDown()
		{
			/*if (Directory.Exists(_outputDirectory))
			{
				Directory.Delete(_outputDirectory, true);
			}*/
		}

		#region SetUp (Public)

		/// <summary>
		/// Performs the setup for each individual test.  This method
		/// refreshes the instances of both MockCodeGenerator and
		/// NStubCore.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			MockRepository mockRepository = new MockRepository();
            _mockCodeGenerator = mockRepository.Stub<ICodeGenerator>();

			CodeNamespace codeNamespace = new CodeNamespace(_sampleNamespace);

			_nStubCore =
				new NStubCore(new FakeBuildSystem(), codeNamespace, _outputDirectory, _mockCodeGenerator);
		} 

		#endregion SetUp (Public)

		#region Tests (Public)

        [Test()]
        public void ConstructWithParametersBuildSystemCodeNamespaceOutputDirectoryCodeGeneratorTest()
        {
            var codeNamespace = new System.CodeDom.CodeNamespace();
            var outputDirectory = "Value of outputDirectory";
            var testObject = new NStubCore(new FakeBuildSystem(), codeNamespace, outputDirectory, _mockCodeGenerator);

            Assert.Throws<ArgumentNullException>(() => new NStubCore(null, codeNamespace, outputDirectory, _mockCodeGenerator));
            Assert.Throws<ArgumentNullException>(() => new NStubCore(new FakeBuildSystem(), null, outputDirectory, _mockCodeGenerator));
            Assert.Throws<ArgumentNullException>(() => new NStubCore(new FakeBuildSystem(), codeNamespace, null, _mockCodeGenerator));
            Assert.Throws<ArgumentException>(() => new NStubCore(new FakeBuildSystem(), codeNamespace, string.Empty, _mockCodeGenerator));
            Assert.Throws<ArgumentNullException>(() => new NStubCore(new FakeBuildSystem(), codeNamespace, outputDirectory, null));
            Assert.Throws<System.IO.DirectoryNotFoundException>(() => 
                new NStubCore(new FakeBuildSystem() { FakeDirectoryExists = false }, codeNamespace, outputDirectory, _mockCodeGenerator));
        }

		/// <summary>
		/// This test ensures that a properly initialized instance of NStubCore can successfully
		/// generate code without throwing an exception.
		/// </summary>
		[Test]
		public void GenerateCodeTest()
		{
			try
			{
				_nStubCore.GenerateCode();
			}
			catch (Exception exception)
			{
				Assert.Fail("This operation should be able to complete with no errors: {0}",
					exception.Message);
			}
		}

		/// <summary>
		/// This test ensures that the code namespace set on NStubCore's constructor
		/// is set correctly.
		/// </summary>
		[Test]
		public void CodeNamespaceTest()
		{
			Assert.AreEqual(_sampleNamespace, _nStubCore.CodeNamespace.Name);

            // Test write access of 'CodeGenerator' Property.
            var expected = new CodeNamespace(_sampleNamespace); ;
            _nStubCore.CodeNamespace = expected;
            var actual = _nStubCore.CodeNamespace;
            Assert.AreSame(expected, actual);
        }

		/// <summary>
		/// This test ensures that the code generator set on NStubCore's constructor
		/// is set correctly.
		/// </summary>
		[Test]
		public void CodeGeneratorTest()
		{
			Assert.AreEqual(_mockCodeGenerator, _nStubCore.CodeGenerator);

            // Test write access of 'CodeGenerator' Property.
            MockRepository mockRepository = new MockRepository();
            _mockCodeGenerator = mockRepository.Stub<ICodeGenerator>();
            var expected = _mockCodeGenerator;
            _nStubCore.CodeGenerator = _mockCodeGenerator;
            var actual = _nStubCore.CodeGenerator;
            Assert.AreEqual(expected, actual);
        }

		/// <summary>
		/// This test ensures that the output directory set on NStubCore's constructor
		/// is set correctly.
		/// </summary>
		[Test]
		public void OutputDirectoryTest()
		{
            // Test read access of 'OutputDirectory' Property.
            var expected = _outputDirectory;
            var actual = _nStubCore.OutputDirectory;
            Assert.AreEqual(expected, actual);

            // Test write access of 'OutputDirectory' Property.
            expected = "Insert setter object here";
            _nStubCore.OutputDirectory = expected;
            actual = _nStubCore.OutputDirectory;
            Assert.AreEqual(expected, actual);

		} 

		#endregion Tests (Public)
	}
}
