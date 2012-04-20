using System;
using System.CodeDom;
//using System.IO;
using NUnit.Framework;

namespace NStub.CSharp.Tests
{
    using NStub.CSharp.MbUnit;
    using NStub.Core;
    using NStub.CSharp.ObjectGeneration;

    /// <summary>
	/// This class exercises all major functionality found in the 
	/// CSharpMbUnitCodeGenerator class.
	/// </summary>
	[TestFixture]
	public class CSharpMbUnitCodeGeneratorTest
	{
		#region Fields (Private)

		private CSharpMbUnitCodeGenerator _CSharpMbUnitCodeGenerator;
		private string _outputDirectory;
		private string _sampleNamespace = "ThisIsMySampleNamespace"; 

		#endregion Fields (Private)

		#region TestFixtureSetUp (Public)

		/// <summary>
		/// Performs the intial setup for the entire test fixture.  Sets our
		/// output directory to a valid, temporary path.
		/// </summary>
		[TestFixtureSetUp]
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
		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			/*if (Directory.Exists(_outputDirectory))
			{
				//Directory.Delete(_outputDirectory, true);
			}*/
		} 

		#endregion TestFixtureTearDown (Public)

		#region SetUp (Public)

		/// <summary>
		/// Performs the setup prior to the execution of each test.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
            //var buildSystem = new StandardBuildSystem();
            var buildSystem = new FakeBuildSystem(); 
            CodeNamespace codeNamespace = new CodeNamespace(_sampleNamespace);
            // Todo: Mock this
            var testBuilders = MemberBuilderFactory.Default;
            var configuration = new CodeGeneratorParameters(_outputDirectory);
			_CSharpMbUnitCodeGenerator =
                new CSharpMbUnitCodeGenerator(buildSystem, codeNamespace, testBuilders, configuration);
		} 

		#endregion SetUp (Public)

		#region Tests (Public)

		/// <summary>
		/// Ensures that the CodeNamespace property has been properly set
		/// on our code generator.
		/// </summary>
		[Test]
		public void CodeNamespaceTest()
		{
			Assert.AreEqual(_sampleNamespace,
				_CSharpMbUnitCodeGenerator.CodeNamespace.Name);
		}

		/// <summary>
		/// Ensures that the OutputDirectory property has been properly
		/// set on our code generator.
		/// </summary>
		[Test]
		public void OutputDirectoryTest()
		{
			Assert.AreEqual(_outputDirectory,
				_CSharpMbUnitCodeGenerator.OutputDirectory);
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
				_CSharpMbUnitCodeGenerator.GenerateCode();
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
