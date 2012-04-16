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
    using System.Linq;
    using System.CodeDom;
    using NStub.CSharp.BuildContext;
    using System.Reflection;
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;
    using System.Collections.Generic;

    /// <summary>
    /// Test method generator for property type members.
    /// </summary>
    public class StaticMethodBuilder : MemberBuilder
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticConstructorBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public StaticMethodBuilder(IMemberSetupContext context)
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
            if (context.MemberInfo != null)
            {

            }
            if (context.TestKey != null)
            {
                if (context.TestObjectType.Name.Contains("AllIwantToTest"))
                {
                    if (context.TestKey.Contains("Static"))
                    {
                        var abc = context.TestKey;
                        var older = context.TestObjectType.Name;
                        var mstatic = context.MemberInfo;
                    }
                }
            }
            return !context.IsConstructor && !context.IsProperty && !context.IsEvent && context.MemberInfo != null && context.MemberInfo.IsStatic;
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
            /*
            // Todo: just placeholders. Use this in the TestConstructor builder.
            var typeMember = context.TypeMember as CodeMemberMethod;
            var testClass = context.TestClassDeclaration.Name;
            var test = context.TestKey;
            var suMethod = context.SetUpTearDownContext.SetUpMethod;
            var setupData = context.BuildData["Setup"];
            if (setupData != null)
            {
                IBuilderData bdata;
                var found = setupData.TryGetValue("bla", out bdata);
                foreach (var item in setupData)
                {
                    var testField = item.Key;
                    var testDaa = item.Value.GetData() as CodeMemberField;
                    typeMember.Statements.Add(new CodeSnippetStatement(""));
                    typeMember.Statements.Add(new CodeCommentStatement("... have a this." + testField + " link."));
                }
            }
            else
            {
                
            }*/

            Guard.NotNull(() => context, context);
            var typeMember = context.TypeMember as CodeMemberMethod;

            var abc = context.TestKey;
            var older = context.TestObjectType.Name;
            var mstatic = context.MemberInfo;

            /*if (!(typeMember is CodeMemberMethod))
            {
                throw new ArgumentOutOfRangeException("context", "The supplied build context is not for a CodeMemberMethod type.");
            }*/

            var typeMemberName = typeMember.Name;
            var propertyName = typeMemberName;
            //var propertyName = typeMemberName.Replace("get_", string.Empty).Replace("set_", string.Empty);
            //BaseCSharpCodeGenerator.ReplaceTestInTestName(typeMember, "XX_Norm_XX");
            var propertyData = context.GetBuilderData("Property");
            //var testName = DetermineTestName(context);
            // hmm Generate to generate new and compute to process existing !?!
            /*var propertyData = builderData as PropertyBuilderData;

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
            typeMember.Statements.Add(invokeExpression);*/
            //BaseCSharpCodeGenerator.ReplaceTestInTestName(typeMember, "NormalBehavior");
            //typeMember.AddBlankLine();
            //CodeExpression ctorAssignmentRight = new CodePrimitiveExpression("This is from Static Method Builder");
            //var expectedAsign = new CodeVariableDeclarationStatement("var", "expected",
            //                                                         ctorAssignmentRight);

            //typeMember.StaticClass(context.TestObjectType.Name).Invoke(context.TestKey).WithReference("expected").AssignLocal("actual", true);
            //.Invoke(context.TestKey).WithReference("expected").AssignLocal("actual", true);
            typeMember.AddBlankLine();
            var returnType = string.Empty;
            if (context.MemberInfo.ReturnType != null)
            {
                returnType = context.MemberInfo.ReturnType.FullName;
            }
            typeMember.AddComment("Type is " + returnType);
            typeMember.AddBlankLine();

            var parameterNames = new List<string>();
            foreach (var item in context.MemberInfo.GetParameters())
            {
                bool activatorAdded = false;
                var parameterType = item.ParameterType;
                typeMember.AddComment("Parameter '" + item.Name + "' is of type " + parameterType.Name);
                //var def = default(parameterType);
                object cct = 55;
                try
                {
                    cct = Activator.CreateInstance(parameterType);
                    if (parameterType.FullName.StartsWith("System.") && 
                        parameterType.FullName.ToCharArray().Count(e=>e == '.') < 2)
                    {
                        typeMember.Var(item.Name, true).Assign().With(cct).Commit();
                        activatorAdded = true;
                    }
                }
                catch (Exception ex)
                {
                }
                
                if (!activatorAdded)
                {
                    typeMember.Var(item.Name, true).Assign().With("Insert initialization of parameter ´" + item.Name + "´ here").Commit();
                }
                // Todo: impl var assignValue = context.SetUpTearDownContext.TryGetAcceptableParameterValue(item.Name);
                parameterNames.Add(item.Name);
            }

            //typeMember.Var("expected", true).StaticClass(context.TestObjectType.Name).Invoke(context.TestKey).With("12:03:05").Commit();
            typeMember.AddBlankLine();
            typeMember.Var("expected", true).Assign().With("Dreck").Commit();
            //typeMember.Var("expected", false).StaticClass("DateTime").Invoke("Now").Commit(); 
            
            //typeMember.Statements.Add(expectedAsign);
            typeMember.StaticClass(context.TestObjectType.Name).Invoke(context.TestKey)
                .WithReference(parameterNames.ToArray()).AssignLocal("actual", true);
            typeMember.StaticClass("Assert").Invoke("AreEqual").WithReference("expected").WithReference("actual").Commit();
            return true;
        }

    }
}