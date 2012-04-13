// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberBuildContext.cs" company="EvePanix">
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
    /// Implementation of a data class used to create new unit tests.
    /// </summary>
    public class MemberBuildContext : MemberBuildContextBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberBuildContext"/> class.
        /// </summary>
        /// <param name="codeNamespace">The code namespace of the test.</param>
        /// <param name="testClassDeclaration">The test class declaration.( early testObject ).</param>
        /// <param name="typeMember">The current type to create a test method for.</param>
        /// <param name="tearDownMethod">The tear down method.</param>
        /// <param name="creator">The test object member field generator of the test SetUp method.</param>
        public MemberBuildContext(
            CodeNamespace codeNamespace, 
            CodeTypeDeclaration testClassDeclaration, 
            CodeTypeMember typeMember, 
            CodeMemberMethod tearDownMethod, 
            ITestObjectBuilder creator)
            : base(
                codeNamespace, 
                testClassDeclaration, 
                typeMember, 
                tearDownMethod, 
                creator)
        {
        }

        #endregion
    }
}