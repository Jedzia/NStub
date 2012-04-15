using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using NStub.Core;

namespace NStub.CSharp.ObjectGeneration
{
    /// <summary>
    /// Provides helper methods for composition of <see cref="CodeMemberMethod"/> method members.
    /// </summary>
    public static class CodeMethodComposer
    {
        /// <summary>
        /// Creates the stub for the specified code member method.  This method actually
        /// implements the method body for the test method.
        /// </summary>
        /// <param name="codeMemberMethod">The code member method.</param>
        public static void CreateTestStubForMethod(CodeMemberMethod codeMemberMethod)
        {
            // Clean the member name and append 'Test' to the end of it
            var origName = Utility.ScrubPathOfIllegalCharacters(codeMemberMethod.Name);
            // Standard test methods accept no parameters and return void.
            codeMemberMethod
                .SetName(origName + "Test")
                .WithReturnType(typeof(void))
                .ClearParameters()
                .AddMethodAttribute("Test")
                .AddComment("TODO: Implement unit test for " + origName);
        }

        /// <summary>
        /// Appends a 'Assert.Inconclusive("Text")' statement to the specified method.
        /// </summary>
        /// <param name="codeMemberMethod">The code member method.</param>
        public static void AppendAssertInconclusive(CodeMemberMethod codeMemberMethod, string inconclusiveText)
        {

            // add a blank line and Assert.Inconclusive(" specified text ..."); to the method body.
            codeMemberMethod
                .AddBlankLine()
                .StaticClass("Assert").Invoke("Inconclusive").With(inconclusiveText).Commit();
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
            else if (type.IsAssignableFrom(typeof(bool)))
            {
                assignExpr = new CodePrimitiveExpression(true);
            }
            else if (type.IsAssignableFrom(typeof(Type)))
            {
                assignExpr = new CodeTypeOfExpression(typeof(Object));
            }
            else if (type.IsAssignableFrom(typeof(int)) || type.IsAssignableFrom(typeof(uint)) || type.IsAssignableFrom(typeof(short)))
            {
                assignExpr = new CodePrimitiveExpression(1234);
            }
            else
            {
                assignExpr = new CodeObjectCreateExpression(type.FullName, new CodeExpression[] { });
            }
            return new CodeAssignStatement(fieldRef1, assignExpr);
        }

    }
}
