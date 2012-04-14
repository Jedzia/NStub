using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using NStub.Core;

namespace NStub.CSharp.ObjectGeneration
{
    public static class CodeMethodComposer
    {
        public static void CreateTestStubForMethod(CodeMemberMethod codeMemberMethod)
        {
            // Clean the member name and append 'Test' to the end of it
            var origName = Utility.ScrubPathOfIllegalCharacters(codeMemberMethod.Name);
            codeMemberMethod.Name = origName;
            codeMemberMethod.Name = codeMemberMethod.Name + "Test";

            // Standard test methods accept no parameters and return void.
            codeMemberMethod.ReturnType = new CodeTypeReference(typeof(void));
            codeMemberMethod.Parameters.Clear();

            // var testAttr = new CodeAttributeDeclaration(
            // new CodeTypeReference(typeof(TestAttribute).Name));
            var testAttr = new CodeAttributeDeclaration(new CodeTypeReference("Test"));

            codeMemberMethod.CustomAttributes.Add(testAttr);

            codeMemberMethod.Statements.Add(
                new CodeCommentStatement(
                    "TODO: Implement unit test for " +
                    origName));
        }

        /// <summary>
        /// Creates a reference to a member field and initializes it with a new instance of the specified parameter type.
        /// </summary>
        /// <param name="type">Defines the type of the new object.</param>
        /// <param name="memberField">Name of the referenced member field.</param>
        /// <returns>An assignment statement for the specified member field.</returns>
        /// <remarks>Produces a statement like: 
        /// <code>this.project = new Microsoft.Build.BuildEngine.Project();</code>.</remarks>
        public static CodeAssignStatement CreateAndInitializeMemberField(Type type, string memberField)
        {
            var fieldRef1 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), memberField);
            CodeExpression assignExpr;
            if (type.IsAssignableFrom(typeof(string)))
            {
                assignExpr = new CodePrimitiveExpression("Value of " + memberField);
            }
            else if (type.IsAssignableFrom(typeof(Type)))
            {
                assignExpr = new CodeTypeOfExpression(typeof(Object));
            }
            else if (type.IsAssignableFrom(typeof(int)) || type.IsAssignableFrom(typeof(uint)) || type.IsAssignableFrom(typeof(short)))
            {
                assignExpr = new CodePrimitiveExpression("1234");
            }
            else
            {
                assignExpr = new CodeObjectCreateExpression(type.FullName, new CodeExpression[] { });
            }
            return new CodeAssignStatement(fieldRef1, assignExpr);
        }

    }
}
