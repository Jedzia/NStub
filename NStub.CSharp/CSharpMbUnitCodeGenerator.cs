using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using NStub.Core;
using NStub.CSharp;
using MbUnit.Framework;
using System.Collections.Generic;

namespace NStub.CSharp
{

    public class ImplicitVariableDeclarationStatement : CodeVariableDeclarationStatement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitVariableDeclarationStatement"/> class
        /// using the specified data type, variable name, and initialization expression.
        /// </summary>
        // /// <param name="type">The name of the data type of the variable.</param>
        /// <param name="name">The name of the variable.</param>
        /// <param name="initExpression">A <see cref="System.CodeDom.CodeExpression"/> that indicates
        /// the initialization expression for the variable.</param>
        public ImplicitVariableDeclarationStatement(string name, CodeExpression initExpression)
            : base("var", name, initExpression)
        {

        }
    }
    /// <summary>
    /// The CSharpCodeGenerator is responsible for the generation of the individual
    /// class files which will make up the actual test project.  For information
    /// regarding the generation of the project file, see 
    /// <see cref="CSharpProjectGenerator">CSharpProjectGenerator</see>.
    /// </summary>
    public class CSharpMbUnitCodeGenerator : BaseCSharpCodeGenerator, Core.ICodeGenerator
    {
        #region Fields (Private)

        //private CodeNamespace UnitCodeNamespace;
        //private string UnitOutputDirectory;

        #endregion Fields (Private)

        #region Constructor (Public)

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpCodeGenerator"/> class
        /// based the given CodeNamespace which will output to the given directory.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="outputDirectory">The output directory.</param>
        /// <exception cref="System.ArgumentNullException">codeNamepsace or
        /// outputDirectory is null.</exception>
        /// <exception cref="System.ArgumentException">outputDirectory is an
        /// empty string.</exception>
        /// <exception cref="DirectoryNotFoundException">outputDirectory
        /// cannot be found.</exception>
        public CSharpMbUnitCodeGenerator(CodeNamespace codeNamespace, string outputDirectory)
            : base(codeNamespace, outputDirectory)
        {
        }

        #endregion Constructor (Public)

        #region Properties (Public)


        #endregion Properties (Public)

        #region Methods (Public)

        /// <summary>
        /// Handle property related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ComputeCodeMemberProperty(CodeMemberMethod typeMember, string propertyName)
        {

            typeMember.Name = "Property" + propertyName + "NormalBehavior";
            /*if (typeMember.Name.Contains("get_"))
            {
                typeMember.Name = typeMember.Name.Replace("get_", "Property");
            }
            else if (typeMember.Name.Contains("set_"))
            {
                typeMember.Name = typeMember.Name.Replace("set_", "Property");
            }*/
            //typeMember.Name += "NormalBehavior";
            //ReplaceTestInTestName(typeMember, "NormalBehavior");


            var variableDeclaration = new CodeVariableDeclarationStatement(
                // Type of the variable to declare.
                //typeof(string),
                "var",
                // Name of the variable to declare.
                "expected",
                // Optional initExpression parameter initializes the variable.
                new CodePrimitiveExpression("Testing"));

            // A C# code generator produces the following source code for the preceeding example code:

            // string TestString = "Testing";
            typeMember.Statements.Add(variableDeclaration);

            variableDeclaration = new ImplicitVariableDeclarationStatement(
                "actual", new CodePrimitiveExpression("Testing"));
            typeMember.Statements.Add(variableDeclaration);

            // Creates a code expression for a CodeExpressionStatement to contain.
            var invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                //new CodePrimitiveExpression("expected")
                new CodeVariableReferenceExpression("expected")
                , new CodeVariableReferenceExpression("actual"));

            // Creates a statement using a code expression.
            var expressionStatement = new CodeExpressionStatement(invokeExpression);

            // A C# code generator produces the following source code for the preceeding example code:

            // Console.Write( "Example string" );
            typeMember.Statements.Add(invokeExpression);
        }

        /// <summary>
        /// Handle event related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="eventName">Name of the event.</param>
        protected override void ComputeCodeMemberEvent(CodeMemberMethod typeMember, string eventName)
        {
            if (typeMember.Name.Contains("add_"))
            {
                typeMember.Name = typeMember.Name.Replace("add_", "Event");
            }
            else if (typeMember.Name.Contains("remove_"))
            {
                typeMember.Name = typeMember.Name.Replace("remove_", "Event");
            }
            ReplaceTestInTestName(typeMember, "AddAndRemove");
        }

        #endregion Methods (Public)

        #region Helper Methods (Private)

        /// <summary>
        /// Add namespace imports to the main compilation unit.
        /// </summary>
        /// <returns>
        /// A list of code name spaces, to be added to the compilation unit.
        /// </returns>
        protected override IEnumerable<CodeNamespaceImport> RetrieveNamespaceImports()
        {
            return new[] { new CodeNamespaceImport(typeof(TestAttribute).Namespace) };
        }

        #endregion Helper Methods (Private)
    }
}
