namespace NStub.CSharp.BuildContext
{
    using NStub.CSharp.ObjectGeneration;

    /// <summary>
    /// Represents the data used in SetUp and TearDown test-method generation.
    /// </summary>
    public interface ISetupAndTearDownCreationContext : ISetupAndTearDownContext
    {
        /// <summary>
        /// Gets the build data dictionary that stores generation wide category/key/value properties.
        /// </summary>
        BuildDataCollection BuildData { get; }

        /// <summary>
        /// Gets the test object member field creator.
        /// </summary>
        /// <remarks>
        /// Contains the test object member field initialization expression ( this.testObject = new Foo( ... ) )
        /// of the test SetUp method.
        /// </remarks>
        ITestObjectBuilder TestObjectCreator { get; }
    }
}