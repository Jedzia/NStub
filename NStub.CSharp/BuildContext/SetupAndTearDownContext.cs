// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupAndTearDownContext.cs" company="EvePanix">
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
    /// Implementation of a data class used by SetUp and TearDown test-method generation.
    /// </summary>
    public class SetupAndTearDownContext : SetupAndTearDownContextBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupAndTearDownContext"/> class.
        /// </summary>
        /// <param name="buildData">The build data dictionary.</param>
        /// <param name="codeNamespace">The code namespace of the test.</param>
        /// <param name="testClassDeclaration">The test class declaration.( early testObject ).</param>
        /// <param name="setUpMethod">A reference to the test setup method.</param>
        /// <param name="tearDownMethod">The tear down method.</param>
        /// <param name="creator">The test object member field generator of the test SetUp method.</param>
        public SetupAndTearDownContext(
            BuildDataDictionary buildData,
            CodeNamespace codeNamespace, 
            CodeTypeDeclaration testClassDeclaration, 
            CodeMemberMethod setUpMethod, 
            CodeMemberMethod tearDownMethod, 
            ITestObjectComposer creator)
            : base(
                buildData, 
                codeNamespace, 
                testClassDeclaration, 
                setUpMethod, 
                tearDownMethod, 
                creator)
        {
        }

        #endregion
    }
}