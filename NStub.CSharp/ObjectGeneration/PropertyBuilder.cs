// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBuilder.cs" company="EvePanix">
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
    using NStub.CSharp.BuildContext;

    /// <summary>
    /// Test method generator for property type members.
    /// </summary>
    public class PropertyBuilder : MemberBuilder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public PropertyBuilder(IMemberBuildContext context)
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

            // return context.TypeMember.Name.StartsWith("get_") || context.TypeMember.Name.StartsWith("set_");
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

            // hmm Generate to generate new and compute to process existing !?!
            this.ComputeCodeMemberProperty(typeMember as CodeMemberMethod, propertyName);
            return true;
        }

        /// <summary>
        /// Determines the name of the test method.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        /// <returns>
        /// The name of the test method.
        /// </returns>
        protected override string DetermineTestName(IMemberBuildContext context)
        {
            var typeMember = context.TypeMember;
            var typeMemberName = typeMember.Name;
            var propertyName = typeMemberName.Replace("get_", string.Empty).Replace("set_", string.Empty);
            // var methodMemberInfo = typeMember.UserData["MethodMemberInfo"];
            //typeMember.Name = "Property" + propertyName + "NormalBehavior";
            var result = context.TypeMember.Name = "Property" + propertyName;

            /*if (typeMember.Name.Contains("get_"))
                        {
                            typeMember.Name = typeMember.Name.Replace("get_", "Property");
                        }
                        else if (typeMember.Name.Contains("set_"))
                        {
                            typeMember.Name = typeMember.Name.Replace("set_", "Property");
                        }*/

            // typeMember.Name += "NormalBehavior";
            //result = result.Replace("Test", "NormalBehavior");
            return result;

            // Todo: return a Test name and a test description ... for the todo.
        }

        /// <summary>
        /// Handle property related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void ComputeCodeMemberProperty(CodeMemberMethod typeMember, string propertyName)
        {

            var variableDeclaration = new CodeVariableDeclarationStatement(
                "var",
                "expected",
                new CodePrimitiveExpression("Testing"));

            typeMember.Statements.Add(variableDeclaration);

            variableDeclaration = new ImplicitVariableDeclarationStatement(
                "actual", new CodePrimitiveExpression("Testing"));
            typeMember.Statements.Add(variableDeclaration);

            // Creates a code expression for a CodeExpressionStatement to contain.
            var invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                new CodeVariableReferenceExpression("expected"),
                new CodeVariableReferenceExpression("actual"));

            // Creates a statement using a code expression.
            // var expressionStatement = new CodeExpressionStatement(invokeExpression);

            // A C# code generator produces the following source code for the preceeding example code:

            // Console.Write( "Example string" );
            typeMember.Statements.Add(invokeExpression);
            BaseCSharpCodeGenerator.ReplaceTestInTestName(typeMember, "NormalBehavior");
        }

    }
}