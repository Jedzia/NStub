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
    using System;
    using System.Reflection;

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
        /// <param name="buildData">The additional build data lookup.</param>
        /// <param name="creator">The test object member field generator of the test SetUp method.</param>
        public MemberBuildContext(
            CodeNamespace codeNamespace, 
            CodeTypeDeclaration testClassDeclaration, 
            CodeTypeMember typeMember,
            BuildDataCollection buildData, 
            ITestObjectBuilder creator)
            : base(
                codeNamespace, 
                testClassDeclaration, 
                typeMember,
                buildData, 
                creator)
        {
        }

        #endregion

        /// <summary>
        /// Gets the type of the object under test.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <returns>
        /// The type of the object under test.
        /// </returns>
        protected override Type GetTestObjectClassType(CodeTypeDeclaration testClassDeclaration)
        {
            // Get the RuntimeType of the test class.
            return (Type)TestClassDeclaration.UserData["TestObjectClassType"];
        }

        /// <summary>
        /// Gets the test method info for this test object member.
        /// </summary>
        /// <param name="typeMember">The type member that hold the Userdata of the current test object member.</param>
        /// <returns>
        /// The test method info for this test object member.
        /// </returns>
        protected override MethodInfo GetTestMethodInfo(CodeTypeMember typeMember)
        {
            // Get the RuntimeMethodInfo of the current test method.
            return (MethodInfo)typeMember.UserData["MethodMemberInfo"];
        }

    }
}