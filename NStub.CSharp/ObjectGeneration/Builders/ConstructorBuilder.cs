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
    using NStub.CSharp.ObjectGeneration.FluentCodeBuild;
    using System;
    using Microsoft.CSharp;
    using System.IO;
    using System.CodeDom.Compiler;

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
                foreach (var assignmentInfos in assignMents)
                {
                    var usedCtor = assignmentInfos.UsedConstructor;
                    if (usedCtor == null)
                    {
                        throw new InvalidOperationException("ConstructorBuilder was called with an AssignmentInfoCollection.UsedConstructor that was null.");
                    }

                    var methodName = BuildNameFromCtorParameters(usedCtor);
                    
                    // Create a new instance of the used constructor.
                    CodeObjectCreateExpression createExpr;
                    var codeMemberMethod = this.CreateConstructorTest(context, methodName, "testObject", out createExpr);
                    objcreator.AssignExtra(context.TestClassDeclaration, codeMemberMethod, createExpr, assignmentInfos);

                    // create the null parameter checks for the current constructor.
                    CreateNullParameterAssertions(context, objcreator, assignmentInfos, createExpr, codeMemberMethod);
                }
            }

            return true;
        }

        /// <summary>
        /// Builds a lambda throw assertion arround a CodeObjectCreateExpression.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="objcreator">The constructor of the objcreator.</param>
        /// <param name="assignmentInfos">The assignment information about common known parameters(OuT specific).</param>
        /// <param name="createExpr">The constructor create expression of the object under tst.</param>
        /// <param name="codeMemberMethod">The code member method definition of the current test method.</param>
        private void CreateNullParameterAssertions(
            IMemberBuildContext context, 
            ITestObjectComposer objcreator, 
            AssignmentInfoCollection assignmentInfos, 
            CodeObjectCreateExpression createExpr,
            CodeMemberMethod codeMemberMethod)
        {
            codeMemberMethod.AddBlankLine();

            for (int i = 0; i < createExpr.Parameters.Count; i++)
            {
                var cref = (CodeFieldReferenceExpression)createExpr.Parameters[i];
                var paraName = cref.FieldName;
                var inf = assignmentInfos[paraName];
                var paraType = inf.MemberField.Type.BaseType;
                CodeExpression paraAssert = new CodePrimitiveExpression(null);
                var expectedException = typeof(ArgumentNullException);
                CreateAssertThrowWithExceptionType(context, objcreator, assignmentInfos, codeMemberMethod, i, paraAssert, expectedException);
                if (paraType == typeof(string).FullName)
                {
                    expectedException = typeof(ArgumentException);
                    //paraAssert = StaticClass<string>.Property("Empty");
                    paraAssert = StaticClass.Property(() => string.Empty);
                    CreateAssertThrowWithExceptionType(context, objcreator, assignmentInfos, codeMemberMethod, i, paraAssert, expectedException);
                }

            }
        }

        private void CreateAssertThrowWithExceptionType(IMemberBuildContext context, ITestObjectComposer objcreator, AssignmentInfoCollection assignmentInfos, CodeMemberMethod codeMemberMethod, int i, CodeExpression paraAssert, Type expectedException)
        {
            var subcreator = BuildTestObject(context.TestObjectType.Name, "notneeded", codeMemberMethod);

            // The upper method created a field assignment statement an initialized it with "= new <Object Under Test>()".
            // We only need the CreateExpression of it, so remove it from the statements of the Constructor test method.
            codeMemberMethod.Statements.RemoveAt(codeMemberMethod.Statements.Count - 1);

            // assign well known constructor arguments
            objcreator.AssignOnly(context.TestClassDeclaration, codeMemberMethod, subcreator, assignmentInfos);

            //var cref = (CodeFieldReferenceExpression)subcreator.Parameters[i];
            //var paraName = cref.FieldName;
            //var inf = assignmentInfos[paraName];
            //var paraType = inf.MemberField.Type.BaseType;


            // for each assert statement null a parameter of the constructor.
            if (subcreator.Parameters.Count > i)
            {
                subcreator.Parameters[i] = paraAssert;
            }
            var ancreate = BuildLambdaThrowAssertion(expectedException, codeMemberMethod, subcreator);
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
            // create a constructor invocation. this.testobject = new <Object Under Test>( );
            var fieldRef1 =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), testFieldName);
            var testObjectMemberFieldCreate = new CodeObjectCreateExpression(testObjectName, new CodeExpression[] { });
            var assignment = new CodeAssignStatement(fieldRef1, testObjectMemberFieldCreate);
            codeMemberMethod.Statements.Add(assignment);

            return testObjectMemberFieldCreate;
        }

        /// <summary>
        /// Builds a lambda throw assertion arround a specified <see cref="CodeObjectCreateExpression"/>.
        /// </summary>
        /// <param name="expectedExceptionType">For <b>Assert.Throw</b>: Expected type of the exception.</param>
        /// <param name="codeMemberMethod">The code member method.</param>
        /// <param name="creator">The constructor creating expression.</param>
        /// <returns>
        /// A a reference to the lambda expression statement creator.
        /// </returns>
        protected CodeExpression BuildLambdaThrowAssertion(Type expectedExceptionType, CodeMemberMethod codeMemberMethod, CodeObjectCreateExpression creator)
        {

            // Todo: test for having "using System;" namespace import and then use Name or Fullname of the exception.

            // wrap the new OuT(a,b,c, ...) into a lambda = (() => new OuT(a,b,c, ...)).
            var lambdaExpression = WrapIntoLambdaExpression(creator);

            // Build 'Assert.Throws<ArgumentNullException>( () => new OuT(...) );'
            codeMemberMethod.StaticClass("Assert").Invoke("Throws", expectedExceptionType.Name)
                .With(lambdaExpression).Commit();

            return lambdaExpression;
        }

        private CodeExpression WrapIntoAnonymousDelegateExpression(CodeStatement statementToWrap)
        {
            CodeExpression delegateExpression;
            CSharpCodeProvider csc = new CSharpCodeProvider();
            StringWriter sw = new StringWriter();
            csc.GenerateCodeFromStatement(statementToWrap, sw, new CodeGeneratorOptions());
            delegateExpression = new CodeSnippetExpression("delegate {" + sw.ToString()
            + "}");
            return delegateExpression;
        }

        /// <summary>
        /// Wraps the specified statement into a lambda expression. 'statement' -> '() => statement'. 
        /// </summary>
        /// <param name="statementToWrap">The statement to wrap.</param>
        /// <returns>The code expression with the wrapped <paramref name="statementToWrap"/> statement.</returns>
        private CodeExpression WrapIntoLambdaExpression(CodeStatement statementToWrap)
        {
            CodeExpression delegateExpression;
            CSharpCodeProvider csc = new CSharpCodeProvider();
            StringWriter sw = new StringWriter();
            csc.GenerateCodeFromStatement(statementToWrap, sw, new CodeGeneratorOptions());
            delegateExpression = new CodeSnippetExpression("()=>" + sw.ToString() + "");
            return delegateExpression;
        }

        /// <summary>
        /// Wraps the specified code expression into a lambda expression. 'expression' -> '() => expression'. 
        /// </summary>
        /// <param name="statementToWrap">The statement to wrap.</param>
        /// <returns>The code expression with the wrapped <paramref name="expressionToWrap"/> code expression.</returns>
        private CodeExpression WrapIntoLambdaExpression(CodeExpression expressionToWrap)
        {
            CodeExpression delegateExpression;
            CSharpCodeProvider csc = new CSharpCodeProvider();
            StringWriter sw = new StringWriter();
            csc.GenerateCodeFromExpression(expressionToWrap, sw, new CodeGeneratorOptions());
            delegateExpression = new CodeSnippetExpression("() => " + sw.ToString() + "");
            return delegateExpression;
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