// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupAndTearDownContextBase.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.BuildContext
{
    using System.CodeDom;
    using NStub.CSharp.ObjectGeneration;

    /// <summary>
    /// Abstract base class for data used in SetUp and TearDown test-method generation.
    /// </summary>
    public abstract class SetupAndTearDownContextBase : ISetupAndTearDownContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupAndTearDownContextBase"/> class.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="testClassDeclaration">The test class declaration.( early testObject ).</param>
        /// <param name="setUpMethod">A reference to the test setup method.</param>
        /// <param name="tearDownMethod">The tear down method.</param>
        /// <param name="creator">The test object member field generator of the test SetUp method.</param>
        protected SetupAndTearDownContextBase(
            CodeNamespace codeNamespace, 
            CodeTypeDeclaration testClassDeclaration,
            CodeMemberMethod setUpMethod, 
            CodeMemberMethod tearDownMethod, 
            ITestObjectBuilder creator)
        {
            Guard.NotNull(() => codeNamespace, codeNamespace);
            Guard.NotNull(() => testClassDeclaration, testClassDeclaration);
            Guard.NotNull(() => setUpMethod, setUpMethod);
            Guard.NotNull(() => tearDownMethod, tearDownMethod);
            Guard.NotNull(() => creator, creator);

            this.CodeNamespace = codeNamespace;
            TestClassDeclaration = testClassDeclaration;
            this.SetUpMethod = setUpMethod;
            this.TearDownMethod = tearDownMethod;
            this.TestObjectCreator = creator;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the code namespace.
        /// </summary>
        public CodeNamespace CodeNamespace { get; private set; }

        /// <summary>
        /// Gets test class declaration.( early testObject ).
        /// </summary>
        public CodeTypeDeclaration TestClassDeclaration { get; private set; }

        /// <summary>
        /// Gets a reference to the test SetUp method.
        /// </summary>
        public CodeMemberMethod SetUpMethod { get; private set; }

        /// <summary>
        /// Gets a reference to the test TearDown method.
        /// </summary>
        public CodeMemberMethod TearDownMethod { get; private set; }

        /// <summary>
        /// Gets the test object member field initialization expression ( this.testObject = new Foo( ... ) )
        /// of the test SetUp method.
        /// </summary>
        public ITestObjectBuilder TestObjectCreator { get; private set; }

        #endregion
    }
}