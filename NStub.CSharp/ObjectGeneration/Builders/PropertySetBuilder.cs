// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertySetBuilder.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    using System.CodeDom;
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Test method generator for the 'set' part of property type members.
    /// </summary>
    public class PropertySetBuilder : MemberBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySetBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public PropertySetBuilder(IMemberSetupContext context)
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
            // return context.TypeMember.Name.StartsWith("set_");
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

            // var propertyName = typeMemberName.Replace("get_", string.Empty).Replace("set_", string.Empty);
            // BaseCSharpCodeGenerator.ReplaceTestInTestName(typeMember, "XX_Norm_XX");
            var propertyData = context.GetBuilderData("Property");

            // var testName = DetermineTestName(context);
            // hmm Generate to generate new and compute to process existing !?!
            // var testObjectName = context.TestObjectName;
            const string TestObjectName = "testObject";
            this.ComputeCodeMemberProperty(typeMember as CodeMemberMethod, propertyData, TestObjectName, propertyName);
            return true;
        }

        /// <summary>
        /// Handle property related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="builderData">The builder data.</param>
        /// <param name="testObjectName">Name of the test object member field.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void ComputeCodeMemberProperty(
            CodeMemberMethod typeMember,
            IBuilderData builderData,
            string testObjectName,
            string propertyName)
        {
            var propertyData = builderData as PropertyBuilderData;

            if (propertyData == null)
            {
                return;
            }

            var setAccessor = propertyData.SetAccessor;
            var getAccessor = propertyData.GetAccessor;

            if (setAccessor == null)
            {
                return;
            }

            if (getAccessor == null)
            {
                // create the actual and expected var's here.
                // actualRef
                // expectedRef
            }

            var propName = setAccessor.Name.Replace("set_", string.Empty);

            typeMember.Statements.Add(new CodeSnippetStatement(string.Empty));
            typeMember.Statements.Add(new CodeCommentStatement("Test write access of '" + propName + "' Property."));

            var expectedRef = new CodeVariableReferenceExpression("expected");
            var expectedAsign = new CodeAssignStatement(
                expectedRef,
                new CodePrimitiveExpression("Insert setter object here"));
            typeMember.Statements.Add(expectedAsign);

            var actualRef = new CodeVariableReferenceExpression("actual");
            var testObjRef = new CodeTypeReferenceExpression(testObjectName);
            var testPropRef = new CodePropertyReferenceExpression(testObjRef, propName);
            var invokeSetProp = new CodeAssignStatement(testPropRef, expectedRef);
            typeMember.Statements.Add(invokeSetProp);

            var invoke = new CodeAssignStatement(actualRef, testPropRef);
            typeMember.Statements.Add(invoke);

            var assertExpr = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                new CodeVariableReferenceExpression("expected"),
                new CodeVariableReferenceExpression("actual"));
            typeMember.Statements.Add(assertExpr);
        }
    }
}