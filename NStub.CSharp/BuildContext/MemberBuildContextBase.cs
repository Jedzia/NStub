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
        /// <param name="tearDownMethod">The tear down method.</param>
        /// <param name="creator">The test object member field generator of the test SetUp method.</param>
        protected MemberBuildContextBase(
            CodeNamespace codeNamespace, 
            CodeTypeDeclaration testClassDeclaration, 
            CodeTypeMember typeMember, 
            CodeMemberMethod tearDownMethod, 
            ITestObjectBuilder creator)
        {
            Guard.NotNull(() => codeNamespace, codeNamespace);
            Guard.NotNull(() => testClassDeclaration, testClassDeclaration);
            Guard.NotNull(() => typeMember, typeMember);

            // Guard.NotNull(() => tearDownMethod, tearDownMethod);
            // Guard.NotNull(() => creator, creator);
            this.CodeNamespace = codeNamespace;
            this.TestClassDeclaration = testClassDeclaration;
            this.TypeMember = typeMember;
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
        /// Gets a value indicating whether this instance is a property.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a property; otherwise, <c>false</c>.
        /// </value>
        public bool IsProperty
        {
            get
            {
                return this.TypeMember.Name.StartsWith("get_") || this.TypeMember.Name.StartsWith("set_");
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
                return this.TypeMember.Name.StartsWith("add_") || this.TypeMember.Name.StartsWith("remove_");
            }
        }

        /// <summary>
        /// Gets a reference to the test TearDown method.
        /// </summary>
        public CodeMemberMethod TearDownMethod { get; private set; }

        /// <summary>
        /// Gets test class declaration.( early testObject ).
        /// </summary>
        public CodeTypeDeclaration TestClassDeclaration { get; private set; }

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