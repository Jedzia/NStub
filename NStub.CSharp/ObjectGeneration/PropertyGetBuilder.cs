namespace NStub.CSharp.ObjectGeneration
{
    using System.CodeDom;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Test method generator for property type members.
    /// </summary>
    public class PropertyGetBuilder : MemberBuilder
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGetBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public PropertyGetBuilder(IMemberSetupContext context)
            : base(context)
        {
        }

        #endregion

        /// <summary>
        /// Determines whether this instance can handle a specified build context.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// <c>true</c> if this instance can handle the specified context; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanHandleContext(IMemberBuildContext context)
        {
            return context.IsProperty;
        }

        /// <summary>
        /// Builds the specified context.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// <c>true</c> on success.
        /// </returns>
        protected override bool BuildMember(IMemberBuildContext context)
        {
            var typeMember = context.TypeMember;
            var typeMemberName = typeMember.Name;
            var propertyName = typeMemberName;
            //var propertyName = typeMemberName.Replace("get_", string.Empty).Replace("set_", string.Empty);
            //BaseCSharpCodeGenerator.ReplaceTestInTestName(typeMember, "XX_Norm_XX");
            var propertyData = context.GetBuilderData("Property");
            //var testName = DetermineTestName(context);
            // hmm Generate to generate new and compute to process existing !?!
            var testObjectName = "testObject";


            this.ComputeCodeMemberProperty(context, typeMember as CodeMemberMethod, propertyData, testObjectName, propertyName);
            return true;
        }

        /// <summary>
        /// Handle property related stuff before type generation.
        /// </summary>
        /// <param name="context">The context. Todo: remove it with specialized parameters after devel.</param>
        /// <param name="typeMember">The type member.</param>
        /// <param name="builderData">The builder data.</param>
        /// <param name="testObjectName">Name of the test object member field.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void ComputeCodeMemberProperty(
            IMemberBuildContext context,
            CodeMemberMethod typeMember,
            IBuilderData builderData,
            string testObjectName,
            string propertyName)
        {

            var propertyData = builderData as PropertyBuilderData;

            var setAccessor = propertyData.SetAccessor;
            var getAccessor = propertyData.GetAccessor;

            if (getAccessor != null)
            {
                if (setAccessor == null)
                {
                    // create the actual and expected var's here.
                    // actualRef
                    // expectedRef
                }

                var propName = propertyData.PropertyName;

                CodeExpression ctorAssignmentRight = new CodePrimitiveExpression("Insert expected object here");
                {
                    // devel: how to create a string initializer from possible constructor setups of the 'SetUp' method.
                    var co = context.SetUpTearDownContext as ISetupAndTearDownCreationContext;
                    var creator = co.TestObjectCreator as TestObjectBuilder;
                    ConstructorAssignment ctorAssignment;
                    var found = creator.TryFindConstructorAssignment(propName, out ctorAssignment, false);
                    if (found)
                    {
                        ctorAssignmentRight = ctorAssignment.AssignStatement.Right;
                    }

                }

                typeMember.Statements.Add(new CodeSnippetStatement(""));
                typeMember.Statements.Add(new CodeCommentStatement("Test read access of '" + propName + "' Property."));

                var expectedAsign = new CodeVariableDeclarationStatement("var", "expected",
                                                                         ctorAssignmentRight);
                typeMember.Statements.Add(expectedAsign);


                var testObjRef = new CodeTypeReferenceExpression(testObjectName);
                var testPropRef = new CodePropertyReferenceExpression(testObjRef, propName);
                var actualAsign = new CodeVariableDeclarationStatement("var", "actual", testPropRef);
                typeMember.Statements.Add(actualAsign);


                var assertExpr = new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression("Assert"),
                    "AreEqual",
                    new CodeVariableReferenceExpression("expected"),
                    new CodeVariableReferenceExpression("actual"));
                typeMember.Statements.Add(assertExpr);
            }
        }
    }
}