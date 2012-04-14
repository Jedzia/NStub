using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NStub.CSharp.BuildContext;
using System.CodeDom;
using System.Reflection;

namespace NStub.CSharp.ObjectGeneration
{

    internal class CustomConstructorCodeMemberMethod : CodeMemberMethod
    {
    }
    /// <summary>
    /// Test method generator for constructor type members.
    /// </summary>
    internal class ConstructorBuilder : MemberBuilder
    {
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
            return context.IsConstructor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorBuilder"/> class.
        /// </summary>
        /// <param name="context">The build context of the test method member.</param>
        public ConstructorBuilder(IMemberSetupContext context)
            : base(context)
        {
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
            //var typeMember = context.TypeMember;
            var typeMemberName = originalName;// typeMember.Name;
            var propertyName = typeMemberName.Replace(".ctor", "Constructor");
            return propertyName;
        }

        protected override bool BuildMember(IMemberBuildContext context)
        {
            var bla = context.TypeMember.Name;
            var to = context.TestObjectType.Name;

            //var methodName = "ConstructWithParameters";

            var ct = context.SetUpTearDownContext as ISetupAndTearDownCreationContext;
            if (ct != null)
            {
                var objcreator = ct.TestObjectCreator;// as TestObjectBuilder;
                var assignMents = objcreator.Assignments;
                if (assignMents != null && assignMents.Count() > 1)
                {
                    foreach (var item in assignMents)
                    {
                        var usedCtor = item.UsedConstructor;
                        var methodName = BuildNameFromCtorParameters(usedCtor);
                        CodeObjectCreateExpression createExpr;
                        var cm = CreateConstructorTest(context, methodName, "testObject", out createExpr);
                        //item.AddAssignment(new ConstructorAssignment(
                        objcreator.AssignExtra(context.TestClassDeclaration, cm, createExpr, item);
                    }
                }
            }

            //CodeObjectCreateExpression crEx;
            //CreateConstructorTest(context, "ConstructWithParameters", "testObject", out crEx);

            return true;
        }

        private CodeMemberMethod CreateConstructorTest(IMemberBuildContext context, string methodName, string testObjectName, out CodeObjectCreateExpression createExpr)
        {
            var cm = new CustomConstructorCodeMemberMethod();
            cm.Name = methodName;
            cm.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            CodeMethodComposer.CreateTestStubForMethod(cm);
            createExpr = BuildTestObject(context.TestObjectType.Name, testObjectName, cm);
            context.TestClassDeclaration.Members.Add(cm);
            return cm;
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
                    paraName =  sb.ToString();
                }
                result += paraName;
            }
            return result;
        }

        protected CodeObjectCreateExpression BuildTestObject(string testObjectName, string testFieldName, CodeMemberMethod codeMemberMethod)
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
            var TestObjectMemberFieldCreateExpression = testObjectMemberFieldCreate;
            var as1 = new CodeAssignStatement(fieldRef1, testObjectMemberFieldCreate);
            //this.assignments = this.AddParametersToConstructor();
            codeMemberMethod.Statements.Add(as1);
            // Creates a statement using a code expression.
            // var expressionStatement = new CodeExpressionStatement(fieldRef1);
            //AddAssignStatement(as1);

            return testObjectMemberFieldCreate;
        }

    }
}
