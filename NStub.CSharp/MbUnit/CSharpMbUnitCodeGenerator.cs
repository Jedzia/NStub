// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpMbUnitCodeGenerator.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.MbUnit
{
    using System.CodeDom;
    using System.Collections.Generic;
    using global::MbUnit.Framework;
    using NStub.Core;
    using NStub.CSharp.BuildContext;
    using NStub.CSharp.ObjectGeneration;

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
        /// <param name="buildSystem">The build system.</param>
        /// <param name="codeNamespace">The code namespace.</param>
        /// <param name="testBuilders">The test builder repository.</param>
        /// <param name="configuration">The configuration of the generator.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="codeNamespace"/> or
        ///   <paramref name="outputDirectory"/> is <c>null</c>.</exception>
        ///   
        /// <exception cref="System.ArgumentException"><paramref name="outputDirectory"/> is an
        /// empty string.</exception>
        ///   
        /// <exception cref="System.ApplicationException"><paramref name="outputDirectory"/>
        /// cannot be found.</exception>
        public CSharpMbUnitCodeGenerator(
            IBuildSystem buildSystem, 
            CodeNamespace codeNamespace, 
            ITestBuilderFactory testBuilders,
            ICodeGeneratorParameters configuration)
            : base(buildSystem, codeNamespace, testBuilders, configuration)
        {
        }

        #endregion

        /// <summary>
        /// Compose additional items of the test TearDown method.
        /// </summary>
        /// <param name="teardownMethod">A reference to the TearDown method of the test.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        protected override void ComposeTestTearDownMethod(
            CodeMemberMethod teardownMethod, 
            CodeMemberField testObjectMemberField, 
            string testObjectName)
        {
            /*var invokeExpression = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Assert"),
                "AreEqual",
                //new CodePrimitiveExpression("expected")
                new CodeFieldReferenceExpression(testObjectMemberField, "bla")
                , new CodeVariableReferenceExpression("actual"));*/
            var fieldRef1 =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), testObjectMemberField.Name);

            // var objectCreate1 = new CodeObjectCreateExpression(testObjectName, new CodeExpression[] { });
            var as1 =
                new CodeAssignStatement(fieldRef1, new CodePrimitiveExpression(null));

            // new CodeAssignStatement(fieldRef1, objectCreate1);

            // Creates a statement using a code expression.
            // var expressionStatement = new CodeExpressionStatement(fieldRef1);
            teardownMethod.Statements.Add(as1);
        }

        /*/// <summary>
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
        }*/

        /// <summary>
        /// Generates additional members of the test class.
        /// </summary>
        /// <param name="context">Contains data specific to SetUp and TearDown test-method generation.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        protected override void GenerateAdditional(
            ISetupAndTearDownCreationContext context, 
            string testObjectName, 
            CodeMemberField testObjectMemberField)
        {
            this.GenerateSetupAndTearDownAdditional(context, testObjectName, testObjectMemberField);
        }

        /// <summary>
        /// Generates the setup and tear down additional members and fields.
        /// </summary>
        /// <param name="context">Contains data specific to SetUp and TearDown test-method generation.</param>
        /// <param name="testObjectName">Name of the test object.</param>
        /// <param name="testObjectMemberField">The test object member field.</param>
        /// <remarks>Override this to customize the behavior of the generator.</remarks>
        protected virtual void GenerateSetupAndTearDownAdditional(ISetupAndTearDownCreationContext context, string testObjectName, CodeMemberField testObjectMemberField)
        {
        }

        /// <summary>
        /// Add namespace imports to the main compilation unit.
        /// </summary>
        /// <returns>
        /// A list of code name spaces, to be added to the compilation unit.
        /// </returns>
        protected override IEnumerable<string> RetrieveNamespaceImports()
        {
            return new[]
                       {
                           "System", 
                           "System.Collections.Generic", 
                           "System.Linq",
                           typeof(TestAttribute).Namespace, 
                       };
        }
    }
}