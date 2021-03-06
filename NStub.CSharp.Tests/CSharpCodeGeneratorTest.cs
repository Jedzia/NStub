using System;
using System.CodeDom;
//using System.IO;
using global::MbUnit.Framework;
using NStub.Core;
using NStub.CSharp.ObjectGeneration;
using Rhino.Mocks;

namespace NStub.CSharp.Tests
{
	/// <summary>
	/// This class exercises all major functionality found in the 
	/// CSharpCodeGenerator class.
	/// </summary>
	[TestFixture]
	public class CSharpCodeGeneratorTest
	{
		#region Fields (Private)

		private CSharpCodeGenerator _cSharpCodeGenerator;
		private string _outputDirectory;
		private string _sampleNamespace = "ThisIsMySampleNamespace"; 

		#endregion Fields (Private)

		#region TestFixtureSetUp (Public)

		/// <summary>
		/// Performs the intial setup for the entire test fixture.  Sets our
		/// output directory to a valid, temporary path.
		/// </summary>
		[FixtureSetUp]
		public void TestFixtureSetUp()
		{
			_outputDirectory = System.IO.Path.GetTempPath();
		} 

		#endregion TestFixtureSetUp (Public)

		#region TestFixtureTearDown (Public)

		/// <summary>
		/// Performs the final tear down after all tests in the test fixture
		/// have executed.  Removes our output directory from the system.
		/// </summary>
		[FixtureTearDown]
		public void TestFixtureTearDown()
		{
			//if (Directory.Exists(_outputDirectory))
			{
				//Directory.Delete(_outputDirectory, true);
			}
		} 

		#endregion TestFixtureTearDown (Public)

		#region SetUp (Public)

		/// <summary>
		/// Performs the setup prior to the execution of each test.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			var codeNamespace = new CodeNamespace(_sampleNamespace);
            var configuration = new CodeGeneratorParameters(_outputDirectory);
            // Todo: Mock this.
            var testBuilders = MemberBuilderFactory.Default;
            var buildSystem = new FakeBuildSystem();
			_cSharpCodeGenerator =
                new CSharpCodeGenerator(buildSystem, codeNamespace, testBuilders, configuration);
		} 

		#endregion SetUp (Public)

		#region Tests (Public)

        [Test]
        public void Construct()
        {
            var buildSystem = new FakeBuildSystem();
            var codeNamespace = new CodeNamespace(_sampleNamespace);
            ICodeGeneratorParameters configuration = new CodeGeneratorParameters(_outputDirectory);
            var testBuilders = MemberBuilderFactory.Default;

            Assert.Throws<ArgumentNullException>(() => new CSharpCodeGenerator(null, codeNamespace, testBuilders, configuration));
            Assert.Throws<ArgumentNullException>(() => new CSharpCodeGenerator(buildSystem, null, testBuilders, configuration));
            new CSharpCodeGenerator(buildSystem, codeNamespace, null, configuration);
            Assert.Throws<ArgumentException>(() => new CSharpCodeGenerator(buildSystem, codeNamespace, testBuilders, null));


            configuration = MockRepository.GenerateStub<ICodeGeneratorParameters>();
            configuration.Expect((e)=> e.OutputDirectory).Return(null);
            Assert.Throws<ArgumentNullException>(() => new CSharpCodeGenerator(buildSystem, codeNamespace, testBuilders, configuration));
            configuration.Expect((e) => e.OutputDirectory).Return(string.Empty);
            Assert.Throws<ArgumentException>(() => new CSharpCodeGenerator(buildSystem, codeNamespace, testBuilders, configuration));

            configuration = new CodeGeneratorParameters(_outputDirectory);
            buildSystem = new FakeBuildSystem() { FakeDirectoryExists = false };
            Assert.Throws<ApplicationException>(() => new CSharpCodeGenerator(buildSystem, codeNamespace, testBuilders, configuration));
        }

		/// <summary>
		/// Ensures that the CodeNamespace property has been properly set
		/// on our code generator.
		/// </summary>
		[Test]
		public void CodeNamespaceTest()
		{
			Assert.AreEqual(_sampleNamespace,
				_cSharpCodeGenerator.CodeNamespace.Name);
		}

		/// <summary>
		/// Ensures that the OutputDirectory property has been properly
		/// set on our code generator.
		/// </summary>
		[Test]
		public void OutputDirectoryTest()
		{
			Assert.AreEqual(_outputDirectory,
				_cSharpCodeGenerator.OutputDirectory);
		}

		/// <summary>
		/// Ensures that the code generator can perform its code generation
		/// without failure.
		/// </summary>
		[Test]
		public void GenerateCodeTest()
		{
			try
			{
				_cSharpCodeGenerator.GenerateCode();
			}
			catch (Exception exception)
			{
				Assert.Fail("Any exception will be considered a failure: {0}",
					exception.Message);
			}
		} 

		#endregion Tests (Public)
	}
}
