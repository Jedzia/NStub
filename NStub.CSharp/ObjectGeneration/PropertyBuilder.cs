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
    using System.Reflection;

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
        public PropertyBuilder(IMemberSetupContext context)
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
            //context.
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

            var typeMember = context.TypeMember;
            var typeMemberName = typeMember.Name;
            var propertyName = typeMemberName;
            //var propertyName = typeMemberName.Replace("get_", string.Empty).Replace("set_", string.Empty);
            //BaseCSharpCodeGenerator.ReplaceTestInTestName(typeMember, "XX_Norm_XX");
            var propertyData = context.GetBuilderData("Property");
            //var testName = DetermineTestName(context);
            // hmm Generate to generate new and compute to process existing !?!
            this.ComputeCodeMemberProperty(typeMember as CodeMemberMethod, propertyData, propertyName);
            return true;
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
            var typeMember = context.TypeMember;
            var typeMemberName = originalName;// typeMember.Name;
            var propertyName = typeMemberName.Replace("get_", string.Empty).Replace("set_", string.Empty);
            // var methodMemberInfo = typeMember.UserData["MethodMemberInfo"];
            //typeMember.Name = "Property" + propertyName + "NormalBehavior";
            var result = "Property" + propertyName;

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
        /// <param name="builderData">The builder data.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void ComputeCodeMemberProperty(CodeMemberMethod typeMember, IBuilderData builderData, string propertyName)
        {
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
            BaseCSharpCodeGenerator.ReplaceTestInTestName(typeMember, "NormalBehavior");
        }

    }


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

            var co = context.SetUpTearDownContext as ISetupAndTearDownCreationContext;
            var creator = co.TestObjectCreator as TestObjectBuilder;
            creator.TryFindConstructorAssignment(propertyName);

            this.ComputeCodeMemberProperty(typeMember as CodeMemberMethod, propertyData, testObjectName, propertyName);
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

                var propName = getAccessor.Name.Replace("get_", "");

                typeMember.Statements.Add(new CodeSnippetStatement(""));
                typeMember.Statements.Add(new CodeCommentStatement("Test read access of '" + propName + "' Property."));

                var expectedAsign = new CodeVariableDeclarationStatement("var", "expected",
                    new CodePrimitiveExpression("Insert expected object here"));
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


            return;


            var variableDeclaration = new CodeVariableDeclarationStatement(
                "var",
                "expected",
                new CodePrimitiveExpression("From the GET Builder"));

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


    /// <summary>
    /// Test method generator for property type members.
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
            //return context.TypeMember.Name.StartsWith("set_");
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
            //var testObjectName = context.TestObjectName;
            var testObjectName = "testObject";
            this.ComputeCodeMemberProperty(typeMember as CodeMemberMethod, propertyData, testObjectName, propertyName);
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

            var setAccessor = propertyData.SetAccessor;
            var getAccessor = propertyData.GetAccessor;

            if (setAccessor != null)
            {
                if (getAccessor == null)
                {
                    // create the actual and expected var's here.
                    // actualRef
                    // expectedRef
                }

                var propName = setAccessor.Name.Replace("set_","");

                typeMember.Statements.Add(new CodeSnippetStatement(""));
                typeMember.Statements.Add(new CodeCommentStatement("Test write access of '" + propName + "' Property."));

                var expectedRef = new CodeVariableReferenceExpression("expected");
                var expectedAsign = new CodeAssignStatement(expectedRef,
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


            return;
            var variableDeclaration = new CodeVariableDeclarationStatement(
                "var",
                "expected",
                new CodePrimitiveExpression("From the SET Builder"));

            typeMember.Statements.Add(variableDeclaration);

            variableDeclaration = new ImplicitVariableDeclarationStatement(
                "actual", new CodePrimitiveExpression("Testing"));
            typeMember.Statements.Add(variableDeclaration);

            // Creates a code expression for a CodeExpressionStatement to contain.
            // Todo: better return the Statements and add it to the typeMember in the generator.
            BaseCSharpCodeGenerator.ReplaceTestInTestName(typeMember, "NormalBehavior");
        }

    }


}