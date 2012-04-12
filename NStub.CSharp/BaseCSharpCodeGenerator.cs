﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCSharpCodeGenerator.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.CSharp;
    using NStub.Core;

    /// <summary>
    /// The <see cref="BaseCSharpCodeGenerator"/> is responsible for the generation of the individual
    /// class files with <see cref="MbUnit"/> support which will make up the actual test project.  For information
    /// regarding the generation of the project file, see 
    /// <see cref="CSharpProjectGenerator"></see>.
    /// </summary>
    public abstract class BaseCSharpCodeGenerator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCSharpCodeGenerator"/> class
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
        protected BaseCSharpCodeGenerator(
            CodeNamespace codeNamespace,
            string outputDirectory)
        {
            // Null arguments will not be accepted
            if (codeNamespace == null)
            {
                throw new ArgumentNullException(
                    "codeNamespace",
                    Exceptions.ParameterCannotBeNull);
            }

            if (outputDirectory == null)
            {
                throw new ArgumentNullException(
                    "outputDirectory",
                    Exceptions.ParameterCannotBeNull);
            }

            // Ensure that the output directory is not empty
            if (outputDirectory.Length == 0)
            {
                throw new ArgumentException(
                    Exceptions.StringCannotBeEmpty,
                    "outputDirectory");
            }

            // Ensure that the output directory is valid
            if (!Directory.Exists(outputDirectory))
            {
                throw new DirectoryNotFoundException(Exceptions.DirectoryCannotBeFound);
            }

            this.CodeNamespace = codeNamespace;
            this.OutputDirectory = outputDirectory;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="System.CodeDom.CodeNamespace"/> object 
        /// the generator is currently working from.
        /// </summary>
        /// <value>The <see cref="System.CodeDom.CodeNamespace"/> object the 
        /// generator is currently working from.</value>
        public CodeNamespace CodeNamespace { get; set; }

        /// <summary>
        /// Gets or sets the directory the new sources files will be output to.
        /// </summary>
        /// <value>The directory the new source files will be output to.</value>
        public string OutputDirectory { get; set; }

        protected CodeTypeDeclaration CurrentTestClassDeclaration { get; private set; }
        #endregion

        /// <summary>
        /// This methods actually performs the code generation for the
        /// file current <see cref="System.CodeDom.CodeNamespace"><see cref="CodeNamespace"/></see>. 
        /// All classes within the namespace will have exactly one file generated for them.  
        /// </summary>
        public void GenerateCode()
        {
            // We want to write a separate file for each type
            foreach (CodeTypeDeclaration testClassDeclaration in this.CodeNamespace.Types)
            {
                CurrentTestClassDeclaration = testClassDeclaration;

                // Create a namespace for the Type in order to put it in scope
                var codeNamespace = new CodeNamespace(this.CodeNamespace.Name);

                // add using imports.
                codeNamespace.Imports.AddRange(this.RetrieveNamespaceImports().ToArray());
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
                var testObjectMemberField = AddTestMemberField(
                    testClassDeclaration, testObjectName, "testObject");

                // Give it a default public constructor
                var codeConstructor = new CodeConstructor { Attributes = MemberAttributes.Public };
                testClassDeclaration.Members.Add(codeConstructor);

                // Set out member names correctly
                foreach (CodeTypeMember typeMember in testClassDeclaration.Members)
                {
                    this.GenerateCodeTypeMember(typeMember);
                }

                // Setup and TearDown
                CodeMemberMethod setUpMethod;
                CodeMemberMethod tearDownMethod;
                CodeObjectCreateExpression testObjectMemberFieldCreate;
                this.GenerateSetupAndTearDown(codeNamespace, testClassDeclaration, testObjectName, testObjectMemberField, out  setUpMethod, out  tearDownMethod,out testObjectMemberFieldCreate);

                this.GenerateAdditional(codeNamespace, testClassDeclaration, testObjectName, testObjectMemberField, setUpMethod, tearDownMethod, testObjectMemberFieldCreate);

                // Create our test type
                testClassDeclaration.Name = testObjectName + "Test";
                testClassDeclaration.IsPartial = true;

                // Add test class to the CodeNamespace.
                codeNamespace.Types.Add(testClassDeclaration);

                RemoveDuplicatedMembers(testClassDeclaration);
                SortMembers(testClassDeclaration);
                this.WriteClassFile(testClassDeclaration.Name, codeNamespace);
            }
        }

        /// <summary>
        /// Generates additional members of the test class.
        /// </summary>
        /// <param name="codeNamespace">The code namespace of the test class.</param>
        /// <param name="testClassDeclaration">The test class declaration.( early testObject ).</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        protected virtual void GenerateAdditional(
            CodeNamespace codeNamespace,
            CodeTypeDeclaration testClassDeclaration,
            string testObjectName,
            CodeMemberField testObjectMemberField,
            CodeMemberMethod setUpMethod,
            CodeMemberMethod tearDownMethod,
            CodeObjectCreateExpression testObjectMemberFieldCreate)
        {

        }

        /// <summary>
        /// Generates the setup and tear down methods.
        /// </summary>
        /// <param name="codeNamespace">The code namespace of the test class.</param>
        /// <param name="testClassDeclaration">The test class declaration.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="setUpMethod">A reference to the test setup method.</param>
        /// <param name="tearDownMethod">The tear down method.</param>
        /// <param name="testObjectMemberFieldCreate">The test object member field create.</param>
        private void GenerateSetupAndTearDown(
            CodeNamespace codeNamespace,
            CodeTypeDeclaration testClassDeclaration,
            string testObjectName,
            CodeMemberField testObjectMemberField, 
            out CodeMemberMethod setUpMethod, 
            out CodeMemberMethod tearDownMethod,
            out CodeObjectCreateExpression testObjectMemberFieldCreate)
        {
            setUpMethod = CreateCustomCodeMemberMethodWithSameNameAsAttribute("SetUp");
            var testObjectType = testClassDeclaration.UserData["TestObjectClassType"] as Type;

            testObjectMemberFieldCreate = this.ComposeTestSetupMethod(
                setUpMethod, testObjectMemberField, testObjectName, testObjectType);

            testClassDeclaration.Members.Add(setUpMethod);
            tearDownMethod = CreateCustomCodeMemberMethodWithSameNameAsAttribute("TearDown");
            this.ComposeTestTearDownMethod(tearDownMethod, testObjectMemberField, testObjectName);
            testClassDeclaration.Members.Add(tearDownMethod);


            /*var assignedMockObjects = this.ComposeTestSetupMockery(
                codeTypeDeclaration, setUpMethod, testObjectMemberField, testObjectName);
            if (assignedMockObjects.Count() > 0)
            {
                foreach (var mockObject in assignedMockObjects)
                {
                    testObjectMemberFieldCreate.Parameters.Add(mockObject.Left);
                }

                string rhinoImport = typeof(MockRepository).Namespace;
                codeNamespace.Imports.Add(new CodeNamespaceImport(rhinoImport));
            }*/

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

        /// <summary>
        /// Add a member field to the test method.
        /// </summary>
        /// <param name="codeTypeDeclaration">The code type declaration of the test class.</param>
        /// <param name="testObjectType">Type of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <returns>A reference to the created member field of the test.</returns>
        public static CodeMemberField AddTestMemberField(
            CodeTypeDeclaration codeTypeDeclaration,
            string testObjectType,
            string testObjectName)
        {
            var memberField = CreateMemberField(testObjectType, testObjectName);
            codeTypeDeclaration.Members.Add(memberField);

            return memberField;
        }

        /// <summary>
        /// Create a private member field declaration.
        /// </summary>
        /// <param name="memberFieldType">Type of the field.</param>
        /// <param name="memberFieldName">The name of the field.</param>
        /// <returns>
        /// A new member field declaration.
        /// </returns>
        public static CodeMemberField CreateMemberField(string memberFieldType, string memberFieldName)
        {
            var memberField = new CodeMemberField(
                memberFieldType, memberFieldName)
            {
                Attributes = MemberAttributes.Private
            };
            return memberField;
        }

        /// <summary>
        /// Compose additional items of the test setup method.
        /// </summary>
        /// <param name="setUpMethod">A reference to the test setup method.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        /// <param name="testObjectType">Type of the test object.</param>
        /// <returns>
        /// The initialization expression of the object under test.
        /// Is <c>null</c>, when none is created.
        /// </returns>
        protected virtual CodeObjectCreateExpression ComposeTestSetupMethod(
            CodeMemberMethod setUpMethod, CodeMemberField testObjectMemberField, string testObjectName, Type testObjectType)
        {
            return null;
        }

        /// <summary>
        /// Compose additional items of the test TearDown method.
        /// </summary>
        /// <param name="teardownMethod">A reference to the TearDown method of the test.</param>
        /// <param name="testObjectMemberField">The member field of the object under test.</param>
        /// <param name="testObjectName">The name of the object under test.</param>
        protected virtual void ComposeTestTearDownMethod(
            CodeMemberMethod teardownMethod,
            CodeMemberField testObjectMemberField,
            string testObjectName)
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
        /// Handle event related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="eventName">Name of the event.</param>
        protected abstract void ComputeCodeMemberEvent(CodeMemberMethod typeMember, string eventName);

        /// <summary>
        /// Handle property related stuff before type generation.
        /// </summary>
        /// <param name="typeMember">The type member.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected abstract void ComputeCodeMemberProperty(CodeMemberMethod typeMember, string propertyName);

        /// <summary>
        /// Creates a custom member method with an attribute of the same name.
        /// </summary>
        /// <param name="methodName">Name of the method and attribute.</param>
        /// <returns>The new code member method with the specified method.</returns>
        protected CodeMemberMethod CreateCustomCodeMemberMethodWithSameNameAsAttribute(string methodName)
        {
            var codeMemberMethod = new CustomCodeMemberMethod
                                       {
                                           Attributes = MemberAttributes.Public,
                                           Name = methodName,
                                           ReturnType = new CodeTypeReference(typeof(void))
                                       };

            // Clean the member name and append 'Test' to the end of it

            // Standard test methods accept no parameters and return void.
            codeMemberMethod.Parameters.Clear();

            var setupAttr = new CodeAttributeDeclaration(new CodeTypeReference(methodName));

            codeMemberMethod.CustomAttributes.Add(setupAttr);
            codeMemberMethod.Statements.Add(
                new CodeCommentStatement("ToDo: Implement " + methodName + " logic here "));
            return codeMemberMethod;
        }

        // protected abstract void CreateStubForCodeMemberMethod(CodeMemberMethod codeMemberMethod);

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

            // var testAttr = new CodeAttributeDeclaration(
            // new CodeTypeReference(typeof(TestAttribute).Name));
            var testAttr = new CodeAttributeDeclaration(new CodeTypeReference("Test"));

            codeMemberMethod.CustomAttributes.Add(testAttr);

            codeMemberMethod.Statements.Add(
                new CodeCommentStatement(
                    "TODO: Implement unit test for " +
                    codeMemberMethod.Name));
        }

        /// <summary>
        /// Add namespace imports to the main compilation unit.
        /// </summary>
        /// <returns>A list of code name spaces, to be added to the compilation unit.</returns>
        protected abstract IEnumerable<CodeNamespaceImport> RetrieveNamespaceImports();

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

        private static void SortMembers(CodeTypeDeclaration codeTypeDeclaration)
        {
            var members = codeTypeDeclaration.Members.OfType<CodeTypeMember>();

            // var ordered = members.OrderBy(e => e, new CodeTypeDeclarationComparer());
            // var grp = members.OrderBy(e => e, new CodeTypeDeclarationComparer()).GroupBy(e => e.GetType().FullName);
            var grp = members.GroupBy(e => e.GetType().FullName).Reverse();
            {
                // var result = ordered.ToArray();
                // foreach (var item in grp)
                // item.Select(e=>e.
            }

            var result = grp.SelectMany(
                (e, k) => e.Select(s => s).OrderBy(o => o, new CodeTypeDeclarationComparer())).ToArray();
            codeTypeDeclaration.Members.Clear();
            codeTypeDeclaration.Members.AddRange(result);
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
                    /*if (typeMember is CodeMemberProperty)
                    {
                        // before stub has created.
                        // PreComputeCodeMemberProperty(typeMember);
                    }*/
                    this.CreateStubForCodeMemberMethod(typeMember as CodeMemberMethod);

                    if (typeMember.Name.Contains("get_") || typeMember.Name.Contains("set_"))
                    {
                        var propertyName = typeMemberName.Replace("get_", string.Empty).Replace("set_", string.Empty);

                        // hmm Generate to generate new and compute to process existing !?!
                        this.ComputeCodeMemberProperty(typeMember as CodeMemberMethod, propertyName);
                    }
                    else if (typeMember.Name.Contains("add_") || typeMember.Name.Contains("remove_"))
                    {
                        var eventName = typeMemberName.Replace("add_", string.Empty).Replace("remove_", string.Empty);

                        // hmm Generate to generate new and compute to process existing !?!
                        this.ComputeCodeMemberEvent(typeMember as CodeMemberMethod, eventName);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the class file.  This method actually creates the physical
        /// class file and populates it accordingly.
        /// </summary>
        /// <param name="className">Name of the class file to be written.</param>
        /// <param name="codeNamespace">The <see cref="CodeNamespace"/> which represents the
        /// file to be written.</param>
        private void WriteClassFile(string className, CodeNamespace codeNamespace)
        {
            var csharpCodeProvider = new CSharpCodeProvider();
            string sourceFile = this.OutputDirectory + Path.DirectorySeparatorChar +
                                className + "." + csharpCodeProvider.FileExtension;
            sourceFile = Utility.ScrubPathOfIllegalCharacters(sourceFile);
            var indentedTextWriter =
                new IndentedTextWriter(new StreamWriter(sourceFile, false), "  ");
            var codeGenerationOptions = new CodeGeneratorOptions { BracingStyle = "C" };
            csharpCodeProvider.GenerateCodeFromNamespace(
                codeNamespace,
                indentedTextWriter,
                codeGenerationOptions);
            indentedTextWriter.Flush();
            indentedTextWriter.Close();
        }

        #region Nested type: CodeTypeDeclarationComparer

        /// <summary>
        /// Comparer for test member sorting.
        /// </summary>
        private class CodeTypeDeclarationComparer : IComparer<CodeTypeMember>
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Value
            /// Condition
            /// Less than zero
            /// <paramref name="x"/> is less than <paramref name="y"/>.
            /// Zero
            /// <paramref name="x"/> equals <paramref name="y"/>.
            /// Greater than zero
            /// <paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
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

            private static void CompareNameStart(
                int rank, string name, CodeTypeMember x, CodeTypeMember y, ref int diff)
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

        #endregion

        #region Nested type: CustomCodeMemberMethod

        /// <summary>
        /// Represents a Custom Code Member Method. Used for sorting the test members.
        /// </summary>
        private class CustomCodeMemberMethod : CodeMemberMethod
        {
        }

        #endregion
    }
}