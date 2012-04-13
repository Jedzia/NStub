// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberBuildContextBase.cs" company="EvePanix">
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
    /// Abstract base class for data used to create new unit tests.
    /// </summary>
    public abstract class MemberBuildContextBase : IMemberBuildContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberBuildContextBase"/> class.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="testClassDeclaration">The test class declaration.( early testObject ).</param>
        /// <param name="typeMember">The current type to create a test method for.</param>
        /// <param name="buildData">The additional build data lookup.</param>
        /// <param name="creator">The test object member field generator of the test SetUp method.</param>
        protected MemberBuildContextBase(
            CodeNamespace codeNamespace, 
            CodeTypeDeclaration testClassDeclaration, 
            CodeTypeMember typeMember,
            BuildDataCollection buildData, 
            ITestObjectBuilder creator)
        {
            Guard.NotNull(() => codeNamespace, codeNamespace);
            Guard.NotNull(() => testClassDeclaration, testClassDeclaration);
            Guard.NotNull(() => typeMember, typeMember);
            Guard.NotNull(() => buildData, buildData);
            
            // Guard.NotNull(() => creator, creator);
            this.CodeNamespace = codeNamespace;
            this.TestClassDeclaration = testClassDeclaration;
            this.TypeMember = typeMember;
            this.BuildData = buildData;
            this.TestObjectCreator = creator;

            
            
        }

        /// <summary>
        /// Gets the test method info for this test object member.
        /// </summary>
        /// <param name="typeMember">The type member that hold the Userdata of the current test object member.</param>
        /// <returns>The test method info for this test object member.</returns>
        protected abstract MethodInfo GetTestMethodInfo(CodeTypeMember typeMember);

        /// <summary>
        /// Gets the type of the object under test.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <returns>The type of the object under test.</returns>
        protected abstract Type GetTestObjectClassType(CodeTypeDeclaration testClassDeclaration);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the code namespace.
        /// </summary>
        public CodeNamespace CodeNamespace { get; private set; }

        /// <summary>
        /// Gets or sets the key associated with the test.
        /// </summary>
        /// <value>
        /// The key associated with the test.
        /// </value>
        public string TestKey { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is a property.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a property; otherwise, <c>false</c>.
        /// </value>
        public bool IsProperty
        {
            get
            {
                if (this.MemberInfo == null)
                {
                    return false;
                }
                return this.MemberInfo.Name.StartsWith("get_") || this.MemberInfo.Name.StartsWith("set_");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is an event.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is an event; otherwise, <c>false</c>.
        /// </value>
        public bool IsEvent
        {
            get
            {
                if (this.MemberInfo == null)
                {
                    return false;
                }
                return this.MemberInfo.Name.StartsWith("add_") || this.MemberInfo.Name.StartsWith("remove_");
            }
        }

        /// <summary>
        /// Contains information about the build members in a dictionary form.
        /// </summary>
        public BuildDataCollection BuildData { get; private set; }

        /// <summary>
        /// Gets test class declaration.( early testObject ).
        /// </summary>
        public CodeTypeDeclaration TestClassDeclaration { get; private set; }

        private Type testObjectType;

        /// <summary>
        /// Gets type of the object under test.
        /// </summary>
        public Type TestObjectType
        {
            get
            {
                if (testObjectType == null)
                {
                    this.testObjectType = GetTestObjectClassType(this.TestClassDeclaration);
                }
                return testObjectType;
            }
        }

        private MethodInfo memberInfo;

        /// <summary>
        /// Gets the member info about the current test method.
        /// </summary>
        public MethodInfo MemberInfo
        {
            get
            {
                if (this.memberInfo == null)
                {
                    this.memberInfo = GetTestMethodInfo(TypeMember);
                }
                return this.memberInfo;
            }
        }

        /// <summary>
        /// Gets the builder data specific to this builders key.
        /// </summary>
        /// <param name="category">Name of the category to request.</param>
        /// <returns>The builder data with the <see cref="Context.TestKey"/> or <c>null</c> if nothing is found.</returns>
        public IBuilderData GetBuilderData(string category)
        {
            IBuilderData result;
            this.BuildData.TryGetValue(category, this.TestKey, out result);
            return result;
        }

        /// <summary>
        /// Gets the test object member field initialization expression ( this.testObject = new Foo( ... ) )
        /// of the test SetUp method.
        /// </summary>
        public ITestObjectBuilder TestObjectCreator { get; private set; }

        /// <summary>
        /// Gets the current type to create a test method for.
        /// </summary>
        public CodeTypeMember TypeMember { get; private set; }

        #endregion
    }
}