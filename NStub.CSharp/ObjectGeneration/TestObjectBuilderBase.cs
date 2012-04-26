// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestObjectBuilderBase.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using NStub.Core;

    /// <summary>
    /// Base class for a <see cref="ITestObjectComposer"/> test-class member field generator.
    /// </summary>
    public abstract class TestObjectComposerBase : ITestObjectComposer
    {
        // Todo: this assignments field should be refactered into a protected property and null checks for each usage should be added.
        // hint: this.assignments = this.AddParametersToConstructor(); in BuildTestObject() creates it.
        #region Fields

        private ConstructorAssignmentCollection assignments;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestObjectComposerBase"/> class.
        /// </summary>
        /// <param name="buildData">The build data dictionary.</param>
        /// <param name="setUpMethod">The method where the test object is initialized</param>
        /// <param name="testObjectMemberField">The member field creation expression for the object under test.</param>
        /// <param name="testObjectName">The name of the test object.</param>
        /// <param name="testObjectType">Type of the test object.</param>
        protected TestObjectComposerBase(
            BuildDataDictionary buildData,
            CodeMemberMethod setUpMethod,
            CodeMemberField testObjectMemberField,
            string testObjectName,
            Type testObjectType)
        {
            Guard.NotNull(() => buildData, buildData);
            Guard.NotNull(() => setUpMethod, setUpMethod);
            Guard.NotNull(() => testObjectMemberField, testObjectMemberField);
            Guard.NotNull(() => testObjectName, testObjectName);
            Guard.NotNull(() => testObjectType, testObjectType);

            this.BuildData = buildData;
            this.SetUpMethod = setUpMethod;
            this.TestObjectMemberField = testObjectMemberField;
            this.TestObjectName = testObjectName;
            this.TestObjectType = testObjectType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the assignments related to this instance.
        /// </summary>
        public IEnumerable<AssignmentInfoCollection> Assignments
        {
            get
            {
                return this.assignments;
            }
        }

        /// <summary>
        /// Gets the build data dictionary that stores generation wide category/key/value properties.
        /// </summary>
        public BuildDataDictionary BuildData { get; private set; }

        /// <summary>
        /// Gets the method where the test object is initialized.
        /// </summary>
        /// <value>The method where the test object is initialized.</value>
        public CodeMemberMethod SetUpMethod { get; private set; }

        /// <summary>
        /// Gets the member field associated with the test object.
        /// </summary>
        /// <value>The member field associated with the test object.</value>
        public CodeMemberField TestObjectMemberField { get; private set; }

        /// <summary>
        /// Gets or sets the member field creation expression for the object under test.
        /// </summary>
        public CodeObjectCreateExpression TestObjectMemberFieldCreateExpression { get; protected set; }

        /// <summary>
        /// Gets the name of the test object.
        /// </summary>
        /// <value>The name of the test object.</value>
        public string TestObjectName { get; private set; }

        /// <summary>
        /// Gets the type of the test object.
        /// </summary>
        /// <value>
        /// The type of the test object.
        /// </value>
        public Type TestObjectType { get; private set; }

        /// <summary>
        /// Gets or sets the constructor assignments of the builder.
        /// </summary>
        /// <value>
        /// The constructor assignments.
        /// </value>
        internal ConstructorAssignmentCollection CtorAssignments
        {
            get
            {
                return this.assignments;
            }

            set
            {
                this.assignments = value;
            }
        }

        #endregion

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to an explicitly specified constructor
        /// create expression to a specified method and creates field initialization expressions for each parameter.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="testMethod">The test method, to add the assign-statements to.</param>
        /// <param name="testObjectConstructor">The object constructor to create the parameter initializers for.</param>
        /// <param name="ctorAssignments">The list of constructor assignments that specify the parameter to add.</param>
        public abstract void AssignExtra(
            CodeTypeDeclaration testClassDeclaration,
            CodeMemberMethod testMethod,
            CodeObjectCreateExpression testObjectConstructor,
            AssignmentInfoCollection ctorAssignments);

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to an explicitly specified constructor
        /// create expression without initializing extra fields like in <see cref="AssignExtra"/>.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="testMethod">The test method, to add the assign-statements to.</param>
        /// <param name="testObjectConstructor">The object constructor to create the parameter initializers for.</param>
        /// <param name="ctorAssignments">The list of constructor assignments that specify the parameter to add.</param>
        public abstract void AssignOnly(   
            CodeTypeDeclaration testClassDeclaration,
            CodeMemberMethod testMethod,
            CodeObjectCreateExpression testObjectConstructor,
            AssignmentInfoCollection ctorAssignments);

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to the constructor create
        /// expression stored in <see cref="TestObjectMemberFieldCreateExpression"/>.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <remarks>Simply means, to add the calculated fields to the specified <paramref name="testClassDeclaration"/>.</remarks>
        public void AssignParameters(CodeTypeDeclaration testClassDeclaration)
        {
            this.AssignParameters(testClassDeclaration, this.TestObjectMemberFieldCreateExpression);
        }

        /// <summary>
        /// Assigns the parameters detected with <see cref="BuildTestObject"/> to the specified constructor create
        /// expression.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="testObjectConstructor">The object constructor to create the parameter initializers for.</param>
        public abstract void AssignParameters(
            CodeTypeDeclaration testClassDeclaration, CodeObjectCreateExpression testObjectConstructor);

        /// <summary>
        /// Creates a code generation expression for an object to test with a member field and initialization
        /// in the previous specified <see cref="SetUpMethod"/> method.
        /// </summary>
        /// <param name="visibility">The visibility level of the objects to parse. Default should be public.</param>
        /// <returns>
        /// The initialization expression of the object under test.
        /// </returns>
        public abstract CodeObjectCreateExpression BuildTestObject(MemberVisibility visibility);

        /// <summary>
        /// Add a assign statement to the test object initializer method.
        /// </summary>
        /// <param name="assignStatement">The code assign statement.</param>
        protected void AddAssignStatement(CodeAssignStatement assignStatement)
        {
            this.SetUpMethod.Statements.Add(assignStatement);
        }
    }
}