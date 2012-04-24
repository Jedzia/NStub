// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorBuilder.cs" company="EvePanix">
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
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using NStub.Core;
    using NStub.CSharp.BuildContext;
    using System;

    /// <summary>
    /// Test method generator for constructor type members.
    /// </summary>
    internal class ConstructorBuilder : MemberBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public ConstructorBuilder(IMemberSetupContext context)
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
            return context.IsConstructor;
        }

        /// <summary>
        /// Builds the test method member with the specified context.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// <c>true</c> on success.
        /// </returns>
        protected override bool BuildMember(IMemberBuildContext context)
        {
            Guard.NotNull(() => context, context);

            // var bla = context.TypeMember.Name;
            // var to = context.TestObjectType.Name;

            // var methodName = "ConstructWithParameters";
            var ct = context.SetUpTearDownContext as ISetupAndTearDownCreationContext;
            Guard.NotNull(() => ct, ct);
            var objcreator = ct.TestObjectCreator; // as TestObjectBuilder;
            var assignMents = objcreator.Assignments;
            if (assignMents != null && assignMents.Count() > 1)
            {
                foreach (var item in assignMents)
                {
                    var usedCtor = item.UsedConstructor;
                    if (usedCtor == null)
                    {
                        throw new InvalidOperationException("ConstructorBuilder was called with an AssignmentInfoCollection.UsedConstructor that was null.");
                    }

                    var methodName = BuildNameFromCtorParameters(usedCtor);
                    CodeObjectCreateExpression createExpr;
                    var cm = this.CreateConstructorTest(context, methodName, "testObject", out createExpr);

                    // item.AddAssignment(new ConstructorAssignment(
                    objcreator.AssignExtra(context.TestClassDeclaration, cm, createExpr, item);
                }
            }

            // CodeObjectCreateExpression crEx;
            // CreateConstructorTest(context, "ConstructWithParameters", "testObject", out crEx);
            return true;
        }

        /// <summary>
        /// Builds the test object.
        /// </summary>
        /// <param name="testObjectName">Name of the test object.</param>
        /// <param name="testFieldName">Name of the test field.</param>
        /// <param name="codeMemberMethod">The code member method.</param>
        /// <returns>A <see cref="CodeObjectCreateExpression"/> that is ready to form the test object.</returns>
        protected CodeObjectCreateExpression BuildTestObject(
            string testObjectName, string testFieldName, CodeMemberMethod codeMemberMethod)
        {
            /*var invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                //new CodePrimitiveExpression("expected")
                new CodeFieldReferenceExpression(testObjectMemberField, "bla")
                , new CodeVariableReferenceExpression("actual"));*/
            var fieldRef1 =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), testFieldName);

            var testObjectMemberFieldCreate = new CodeObjectCreateExpression(testObjectName, new CodeExpression[] { });

            // var TestObjectMemberFieldCreateExpression = testObjectMemberFieldCreate;
            var as1 = new CodeAssignStatement(fieldRef1, testObjectMemberFieldCreate);

            // this.assignments = this.AddParametersToConstructor();
            codeMemberMethod.Statements.Add(as1);

            // Creates a statement using a code expression.
            // var expressionStatement = new CodeExpressionStatement(fieldRef1);
            // AddAssignStatement(as1);
            return testObjectMemberFieldCreate;
        }

        /// <summary>
        /// Determines the name of the test method.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <param name="originalName">The initial name of the test method member.</param>
        /// <returns>
        /// The name of the test method.
        /// </returns>
        protected override string DetermineTestName(IMemberSetupContext context, string originalName)
        {
            // var typeMember = context.TypeMember;
            var typeMemberName = originalName; // typeMember.Name;
            var propertyName = typeMemberName.Replace(".ctor", "Constructor");
            return propertyName;
        }

        private static string BuildNameFromCtorParameters(ConstructorInfo usedCtor)
        {
            string result = "ConstructWithParameters";
            foreach (var paraInfo in usedCtor.GetParameters())
            {
                var paraName = paraInfo.Name;
                if (paraName.Length > 0)
                {
                    var paraNameArray = paraName.ToCharArray();
                    paraNameArray[0] = char.ToUpper(paraName[0]);
                    var sb = new StringBuilder();
                    sb.Append(paraNameArray);
                    paraName = sb.ToString();
                }

                result += paraName;
            }

            return result;
        }

        /// <summary>
        /// Creates a constructor test method.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="testObjectName">Name of the test object.</param>
        /// <param name="createExpr">A <see cref="CodeObjectCreateExpression"/> building the test object.</param>
        /// <returns>A new test method, forming the constructor test.</returns>
        private CodeMemberMethod CreateConstructorTest(
            IMemberBuildContext context,
            string methodName,
            string testObjectName,
            out CodeObjectCreateExpression createExpr)
        {
            var cm = new CustomConstructorCodeMemberMethod
                         {
                             Name = methodName,
                             Attributes = MemberAttributes.Public | MemberAttributes.Final
                         };

            CodeMethodComposer.CreateTestStubForMethod(cm);
            createExpr = this.BuildTestObject(context.TestObjectType.Name, testObjectName, cm);
            context.TestClassDeclaration.Members.Add(cm);
            context.BuildResult.ClassMethodsToAdd.Add(cm);
            return cm;
        }
    }
}