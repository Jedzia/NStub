using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.IO;
using System.Linq;
using NStub.Core;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace NStub.CSharp
{
    public abstract class BaseCSharpCodeGenerator
    {

        private CodeNamespace _codeNamespace;
        private string _outputDirectory;

        /// <summary>
        /// Gets or sets the <see cref="System.CodeDom.CodeNamespace"/> object 
        /// the generator is currently working from.
        /// </summary>
        /// <value>The <see cref="System.CodeDom.CodeNamespace"/> object the 
        /// generator is currently working from.</value>
        public CodeNamespace CodeNamespace
        {
            get
            {
                return this._codeNamespace;
            }
            set
            {
                this._codeNamespace = value;
            }
        }

        /// <summary>
        /// Gets or sets the directory the new sources files will be output to.
        /// </summary>
        /// <value>The directory the new source files will be output to.</value>
        public string OutputDirectory
        {
            get
            {
                return this._outputDirectory;
            }

            set
            {
                this._outputDirectory = value;
            }
        }

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
        protected BaseCSharpCodeGenerator(CodeNamespace codeNamespace,
            string outputDirectory)
        {
            #region Validation

            // Null arguments will not be accepted
            if (codeNamespace == null)
            {
                throw new ArgumentNullException("codeNamespace",
                    Exceptions.ParameterCannotBeNull);
            }
            if (outputDirectory == null)
            {
                throw new ArgumentNullException("outputDirectory",
                    Exceptions.ParameterCannotBeNull);
            }
            // Ensure that the output directory is not empty
            if (outputDirectory.Length == 0)
            {
                throw new ArgumentException(Exceptions.StringCannotBeEmpty,
                    "outputDirectory");
            }
            // Ensure that the output directory is valid
            if (!(Directory.Exists(outputDirectory)))
            {
                throw new DirectoryNotFoundException(Exceptions.DirectoryCannotBeFound);
            }

            #endregion Validation

            _codeNamespace = codeNamespace;
            _outputDirectory = outputDirectory;
        }

        /// <summary>
        /// This methods actually performs the code generation for the
        /// file current <see cref="System.CodeDom.CodeNamespace">CodeNamespace</see>. 
        /// All classes within the namespace will have exactly one file generated for them.  
        /// </summary>
        public void GenerateCode()
        {
            // We want to write a separate file for each type
            foreach (CodeTypeDeclaration testClassDeclaration in CodeNamespace.Types)
            {
                // Create a namespace for the Type in order to put it in scope
                CodeNamespace codeNamespace =
                    new CodeNamespace((CodeNamespace.Name));

                // add using imports.
                codeNamespace.Imports.AddRange(RetrieveNamespaceImports().ToArray());
                var indexcodeNs = testClassDeclaration.Name.LastIndexOf('.');
                if (indexcodeNs > 0)
                {
                    // try to import the namespace for the object under test.
                    var codeNs = testClassDeclaration.Name.Substring(0, indexcodeNs);
                    codeNamespace.Imports.Add(new CodeNamespaceImport(codeNs));
                }

                // Clean the type name
                testClassDeclaration.Name =
                    Utility.ScrubPathOfIllegalCharacters(testClassDeclaration.Name);

                var testObjectName = Utility.GetUnqualifiedTypeName(testClassDeclaration.Name);
                
                // Add testObject field
                CodeMemberField testObjectMemberField = AddTestMemberField(testClassDeclaration, testObjectName, "testObject");

                // Give it a default public constructor
                var codeConstructor = new CodeConstructor();
                codeConstructor.Attributes = MemberAttributes.Public;
                testClassDeclaration.Members.Add(codeConstructor);

                // Set out member names correctly
                foreach (CodeTypeMember typeMember in testClassDeclaration.Members)
                {
                    GenerateCodeTypeMember(typeMember);
                }

                // Setup and TearDown
                GenerateSetupAndTearDown(codeNamespace,testClassDeclaration, testObjectName, testObjectMemberField);

                // Create our test type
                testClassDeclaration.Name = testObjectName + "Test";
                testClassDeclaration.IsPartial = true;

                // Add test class to the CodeNamespace.
                codeNamespace.Types.Add(testClassDeclaration);

                RemoveDuplicatedMembers(testClassDeclaration);
                SortMembers(testClassDeclaration);
                WriteClassFile(testClassDeclaration.Name, codeNamespace);
            }
        }

        private void GenerateSetupAndTearDown(
            CodeNamespace codeNamespace,
            CodeTypeDeclaration codeTypeDeclaration, 
            string testObjectName, 
            CodeMemberField testObjectMemberField)
        {
            var setUpMethod = CreateCustomCodeMemberMethodWithSameNameAsAttribute("Setup");
            var testObjectMemberFieldCreate = ComposeTestSetupMethod(setUpMethod, testObjectMemberField, testObjectName);
            var assignedMockObjects = ComposeTestSetupMockery(codeTypeDeclaration, setUpMethod, testObjectMemberField, testObjectName);
            if (assignedMockObjects.Count() > 0)
            {
                foreach (var mockObject in assignedMockObjects)
                {
                    testObjectMemberFieldCreate.Parameters.Add(mockObject.Left);
                }

                // Todo: move this to the implementing class.
                var rhinoImport = "Rhino.Mocks";
                codeNamespace.Imports.Add(new CodeNamespaceImport(rhinoImport));
            }
            codeTypeDeclaration.Members.Add(setUpMethod);
            var tearDownMethod = CreateCustomCodeMemberMethodWithSameNameAsAttribute("TearDown");
            ComposeTestTearDownMethod(tearDownMethod, testObjectMemberField, testObjectName);
            codeTypeDeclaration.Members.Add(tearDownMethod);
        }

        /// <summary>
        /// Compose additional items of the test setup method.
        /// </summary>
        /// <param name="testClassDeclaration">The test class declaration.( early testObject ).</param>
        /// <param name="setUpMethod">The test setup method.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <returns>The list of assigned mock objects.</returns>
        /// <remarks>
        /// Till the execution of the <see cref="RemoveDuplicatedMembers"/> method, the
        /// <paramref name="testClassDeclaration"/> parameter is identical to the test object.
        /// </remarks>
        protected virtual IEnumerable<CodeAssignStatement> ComposeTestSetupMockery(
            CodeTypeDeclaration testClassDeclaration, 
            CodeMemberMethod setUpMethod, 
            CodeMemberField testObjectMemberField, 
            string testObjectName)
        {
            return new CodeAssignStatement[0];
        }

        /// <summary>
        /// Compose additional items of the test setup method.
        /// </summary>
        /// <param name="setUpMethod">A reference to the test setup method.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <returns>The initialization expression of the object under test.
        /// Is <c>null</c>, when none is created.</returns>
        protected virtual CodeObjectCreateExpression ComposeTestSetupMethod(CodeMemberMethod setUpMethod, CodeMemberField testObjectMemberField, string testObjectName)
        {
            return null;
        }

        /// <summary>
        /// Compose additional items of the test teardown method.
        /// </summary>
        /// <param name="teardownMethod">A reference to the teardown method of the test.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        protected virtual void ComposeTestTearDownMethod(CodeMemberMethod teardownMethod, CodeMemberField testObjectMemberField, string testObjectName)
        { 
        }

        /*private Dictionary<string, CodeMemberField> testMemberFieldLookup =
            new Dictionary<string, CodeMemberField>();

        protected IDictionary<string, CodeMemberField> TestMemberFieldLookup
        {
            get
            {
                return this.testMemberFieldLookup;
            }
        }*/

        /// <summary>
        /// Add a member field to the test method.
        /// </summary>
        /// <param name="codeTypeDeclaration">The code type declaration of the test class.</param>
        /// <param name="testObjectType">Type of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <returns></returns>
        protected CodeMemberField AddTestMemberField(
            CodeTypeDeclaration codeTypeDeclaration,
            string testObjectType, 
            string testObjectName)
        {
            var memberField = new CodeMemberField(
                testObjectType, testObjectName);
            memberField.Attributes = MemberAttributes.Private;
            //typeMember.Statements.Add(variableDeclaration);
            codeTypeDeclaration.Members.Add(memberField);
            //testMemberFieldLookup.Add(testObjectName, memberField);
            return memberField;
        }

        /// <summary>
        /// Processes the code type members of the compilation unit.
        /// </summary>
        /// <param name="typeMember">The type member to process.</param>
        private void GenerateCodeTypeMember(CodeTypeMember typeMember)
        {
            var typeMemberName = typeMember.Name;
            if (typeMember is CodeMemberMethod)
            {
                // We don't generate default constructors
                if (!(typeMember is CodeConstructor))
                {
                    if (typeMember is CodeMemberProperty)
                    {
                        // before stub has created.
                        //PreComputeCodeMemberProperty(typeMember);
                    }

                    CreateStubForCodeMemberMethod(typeMember as CodeMemberMethod);

                    if (typeMember is CodeMemberMethod && (typeMember.Name.Contains("get_") || typeMember.Name.Contains("set_")))
                    {
                        var propertyName = typeMemberName.Replace("get_", "").Replace("set_", "");
                        // hmm Generate to generate new and compute to process existing !?!
                        ComputeCodeMemberProperty(typeMember as CodeMemberMethod, propertyName);
                    }
                    else if (typeMember is CodeMemberMethod && (typeMember.Name.Contains("add_") || typeMember.Name.Contains("remove_")))
                    {
                        var eventName = typeMemberName.Replace("add_", "").Replace("remove_", "");
                        // hmm Generate to generate new and compute to process existing !?!
                        ComputeCodeMemberEvent(typeMember as CodeMemberMethod, eventName);
                    }
                }
            }
        }

        /// <summary>
        /// Add namespace imports to the main compilation unit.
        /// </summary>
        /// <returns>A list of code name spaces, to be added to the compilation unit.</returns>
        protected abstract IEnumerable<CodeNamespaceImport> RetrieveNamespaceImports();

        /// <summary>
        /// Handle property related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected abstract void ComputeCodeMemberProperty(CodeMemberMethod typeMember, string propertyName);

        /// <summary>
        /// Handle event related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="eventName">Name of the event.</param>
        protected abstract void ComputeCodeMemberEvent(CodeMemberMethod typeMember, string eventName);

        //protected abstract void CreateStubForCodeMemberMethod(CodeMemberMethod codeMemberMethod);

        /// <summary>
        /// Creates the stub for the code member method.  This method actually
        /// implements the method body for the test method.
        /// </summary>
        /// <param name="codeMemberMethod">The code member method.</param>
        protected virtual void CreateStubForCodeMemberMethod(CodeMemberMethod codeMemberMethod)
        {
            // Clean the member name and append 'Test' to the end of it
            codeMemberMethod.Name = Utility.ScrubPathOfIllegalCharacters(codeMemberMethod.Name);
            codeMemberMethod.Name = codeMemberMethod.Name + "Test";

            // Standard test methods accept no parameters and return void.
            codeMemberMethod.ReturnType = new CodeTypeReference(typeof(void));
            codeMemberMethod.Parameters.Clear();

            //var testAttr = new CodeAttributeDeclaration(
            //            new CodeTypeReference(typeof(TestAttribute).Name));
            var testAttr = new CodeAttributeDeclaration(new CodeTypeReference("Test"));

            codeMemberMethod.CustomAttributes.Add(testAttr);
            //codeMemberMethod.CustomAttributes.Add(
            //		new CodeAttributeDeclaration(
            //		new CodeTypeReference(typeof(IgnoreAttribute))));
            codeMemberMethod.Statements.Add(
                new CodeCommentStatement("TODO: Implement unit test for " +
                                         codeMemberMethod.Name));
        }

        private class CustomCodeMemberMethod : CodeMemberMethod
        { 
        }

        /// <summary>
        /// Creates a custom member method with an attribute of the same name.
        /// </summary>
        /// <param name="methodName">Name of the method and attribute.</param>
        /// <returns>The new code member method with the specified method.</returns>
        private CodeMemberMethod CreateCustomCodeMemberMethodWithSameNameAsAttribute(string methodName)
        {
            var codeMemberMethod = new CustomCodeMemberMethod();
            codeMemberMethod.Attributes = MemberAttributes.Public;

            // Clean the member name and append 'Test' to the end of it
            codeMemberMethod.Name = methodName;

            // Standard test methods accept no parameters and return void.
            codeMemberMethod.ReturnType = new CodeTypeReference(typeof(void));
            codeMemberMethod.Parameters.Clear();

            var setupAttr = new CodeAttributeDeclaration(new CodeTypeReference(methodName));

            codeMemberMethod.CustomAttributes.Add(setupAttr);
            codeMemberMethod.Statements.Add(
                new CodeCommentStatement("ToDo: Implement " + methodName + " logic here "));
            return codeMemberMethod;
        }

        private static void SortMembers(CodeTypeDeclaration codeTypeDeclaration)
        {
            var members = codeTypeDeclaration.Members.OfType<CodeTypeMember>();
            //var ordered = members.OrderBy(e => e, new CodeTypeDeclarationComparer());
            //var grp = members.OrderBy(e => e, new CodeTypeDeclarationComparer()).GroupBy(e => e.GetType().FullName);
            var grp = members.GroupBy(e => e.GetType().FullName).Reverse();
            //var result = ordered.ToArray();
            //foreach (var item in grp)
            {
                //item.Select(e=>e.
            }
            var result = grp.SelectMany(
                (e, k) => e.Select(s => s).OrderBy(o => o, new CodeTypeDeclarationComparer())).ToArray();
            codeTypeDeclaration.Members.Clear();
            codeTypeDeclaration.Members.AddRange(result);
        }

        private class CodeTypeDeclarationComparer : IComparer<CodeTypeMember>
        {
            public int Compare(CodeTypeMember x, CodeTypeMember y)
            {
                int diff = 0;
                CompareNameStart(-2, "Event", x, y, ref diff);
                CompareNameStart(-1, "Property", x, y, ref diff);

                CompareForName(12, "EqualsTest", x, y, ref diff);
                CompareForName(13, "GetHashCodeTest", x, y, ref diff);
                CompareForName(14, "GetTypeTest", x, y, ref diff);
                CompareForName(15, "ToStringTest", x, y, ref diff);


                return x.Name.CompareTo(y.Name) + diff;
                /*if (y.Name == "Setup" && !(x.Name == "Setup"))
                {
                    return x.GetType().FullName.CompareTo("!!" + y.GetType().FullName);
                    //return int.MinValue;
                }
                else if (y.Name == "TearDown" && !(x.Name == "TearDown"))
                {
                    return x.GetType().FullName.CompareTo("!" + y.GetType().FullName);
                    //return int.MinValue + 1;
                }
                return (x.GetType().FullName.CompareTo(y.GetType().FullName) * 16) - (x.Name.CompareTo(y.Name));
                return x.Name.CompareTo(y.Name);*/
            }

            private static void CompareForName(int rank, string name, CodeTypeMember x, CodeTypeMember y, ref int diff)
            {
                if (x.Name == name)
                {
                    diff += rank;
                }
                if (y.Name == name)
                {
                    diff -= rank;
                }
            }
            
            private static void CompareNamePart(int rank, string name, CodeTypeMember x, CodeTypeMember y, ref int diff)
            {
                if (x.Name.Contains(name))
                {
                    diff += rank;
                }
                if (y.Name.Contains(name))
                {
                    diff -= rank;
                }
            }

            private static void CompareNameStart(int rank, string name, CodeTypeMember x, CodeTypeMember y, ref int diff)
            {
                if (x.Name.StartsWith(name))
                {
                    diff += rank;
                }
                if (y.Name.StartsWith(name))
                {
                    diff -= rank;
                }
            }

        }
        /// <summary>
        /// Since types can contain multiple overloads of the same method, once
        /// we remove the parameters from every method our type may have the 
        /// many duplicates of the same method.  This method removes those
        /// duplicates.
        /// </summary>
        /// <param name="codeTypeDeclaration">The <see cref="CodeTypeDeclaration"/>
        /// from which to remove the duplicates.</param>
        private static void RemoveDuplicatedMembers(CodeTypeDeclaration codeTypeDeclaration)
        {
            for (int i = 0; i < codeTypeDeclaration.Members.Count; ++i)
            {
                int occurrences = 0;
                for (int j = 0; j < codeTypeDeclaration.Members.Count; ++j)
                {
                    // Compare each CodeTypeMember to all other CodeTypeMembers
                    // in its type.  If more than one match is found (the 
                    // codeTypeMember matching itself) then we remove that match.
                    if (codeTypeDeclaration.Members[i].Name.Equals(
                        codeTypeDeclaration.Members[j].Name))
                    {
                        occurrences++;
                        if (occurrences > 1)
                        {
                            codeTypeDeclaration.Members.Remove(codeTypeDeclaration.Members[j]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Writes the class file.  This method actually creates the physical
        /// class file and populates it accordingly.
        /// </summary>
        /// <param name="className">Name of the class file to be written.</param>
        /// <param name="codeNamespace">The CodeNamespace which represents the
        /// file to be written.</param>
        private void WriteClassFile(string className, CodeNamespace codeNamespace)
        {
            CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
            string sourceFile = OutputDirectory + Path.DirectorySeparatorChar +
                className + "." + cSharpCodeProvider.FileExtension;
            sourceFile = Utility.ScrubPathOfIllegalCharacters(sourceFile);
            IndentedTextWriter indentedTextWriter =
                new IndentedTextWriter(new StreamWriter(sourceFile, false), "  ");
            CodeGeneratorOptions codeGenerationOptions = new CodeGeneratorOptions();
            codeGenerationOptions.BracingStyle = "C";
            cSharpCodeProvider.GenerateCodeFromNamespace(codeNamespace, indentedTextWriter,
                codeGenerationOptions);
            indentedTextWriter.Flush();
            indentedTextWriter.Close();
        }

        /// <summary>
        /// Replaces the ending name of the test ("Test") with a specified string.
        /// </summary>
        /// <param name="typeMember">The type member that name gets modified.</param>
        /// <param name="replacement">The replacement string.</param>
        protected static void ReplaceTestInTestName(CodeTypeMember typeMember, string replacement)
        {
            typeMember.Name = typeMember.Name.Replace("Test", replacement);
        }


    }
}
