// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpMbUnitCodeGenerator.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp
{
    using System.CodeDom;
    using System.Collections.Generic;
    using System.IO;
    using MbUnit.Framework;
    using NStub.Core;

    /// <summary>
    /// The <see cref="CSharpMbUnitCodeGenerator"/> is responsible for the generation of the individual
    /// class files with <see cref="MbUnit"/> support which will make up the actual test project.  For information
    /// regarding the generation of the project file, see 
    /// <see cref="CSharpProjectGenerator"></see>.
    /// </summary>
    public class CSharpMbUnitCodeGenerator : BaseCSharpCodeGenerator, ICodeGenerator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpMbUnitCodeGenerator"/> class
        /// based the given <see cref="CodeNamespace"/> which will output to the given directory.
        /// </summary>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="outputDirectory">The output directory.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="codeNamespace"/> or
        /// <paramref name="outputDirectory"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="outputDirectory"/> is an
        /// empty string.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="outputDirectory"/>
        /// cannot be found.</exception>
        public CSharpMbUnitCodeGenerator(CodeNamespace codeNamespace, string outputDirectory)
            : base(codeNamespace, outputDirectory)
        {
        }

        #endregion

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

        /// <summary>
        /// Handle property related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ComputeCodeMemberProperty(CodeMemberMethod typeMember, string propertyName)
        {
            // var methodMemberInfo = typeMember.UserData["MethodMemberInfo"];
            typeMember.Name = "Property" + propertyName + "NormalBehavior";

            /*if (typeMember.Name.Contains("get_"))
                        {
                            typeMember.Name = typeMember.Name.Replace("get_", "Property");
                        }
                        else if (typeMember.Name.Contains("set_"))
                        {
                            typeMember.Name = typeMember.Name.Replace("set_", "Property");
                        }*/

            // typeMember.Name += "NormalBehavior";
            // ReplaceTestInTestName(typeMember, "NormalBehavior");
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
        }

        /// <summary>
        /// Add namespace imports to the main compilation unit.
        /// </summary>
        /// <returns>
        /// A list of code name spaces, to be added to the compilation unit.
        /// </returns>
        protected override IEnumerable<CodeNamespaceImport> RetrieveNamespaceImports()
        {
            return new[]
                       {
                           new CodeNamespaceImport("System"), 
                           new CodeNamespaceImport("System.Collections.Generic"), 
                           new CodeNamespaceImport("System.Linq"), 
                           new CodeNamespaceImport(typeof(TestAttribute).Namespace), 
                       };
        }
    }
}