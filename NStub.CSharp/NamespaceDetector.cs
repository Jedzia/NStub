using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Collections.Specialized;

namespace NStub.CSharp
{
    /// <summary>
    /// Determines the namespace to use, based on a supplied list of type declarations.
    /// </summary>
    public class NamespaceDetector
    {
        /// <summary>
        /// The list of global type declarations.
        /// </summary>
        private CodeTypeDeclarationCollection typeDeclarations;

        /// <summary>
        /// Gets the list of global type declarations.
        /// </summary>
        /// <value>The summary.</value>
        public CodeTypeDeclarationCollection TypeDeclarations
        {
            get
            {
                return this.typeDeclarations;
            }
            /*
            set
            {
                this.typeDeclarations = value;
            }*/
        }

        /// <summary>
        /// Gets the shortest common namespace part.
        /// </summary>
        public string ShortestNamespace
        {
            get;
            private set;
        }

        //private readonly List<CodeTypeDeclaration> storedDeclarations;
        private readonly CodeTypeDeclaration[] storedDeclarations;
        private readonly string[] namespaces;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceDetector"/> class.
        /// </summary>
        /// <param name="typeDeclarations">The type declarations to investigate.</param>
        public NamespaceDetector(CodeTypeDeclarationCollection typeDeclarations)
        {
            Guard.NotNull(() => typeDeclarations, typeDeclarations);
            this.typeDeclarations = typeDeclarations;
            //storedDeclarations = typeDeclarations.OfType<CodeTypeDeclaration>().ToList();
            storedDeclarations = typeDeclarations.OfType<CodeTypeDeclaration>().ToArray();
            
            IEnumerable<string> distinctNamespaces;
            ShortestNamespace = DetermineShortestNamespace(storedDeclarations, out distinctNamespaces);
            namespaces = distinctNamespaces.ToArray();
        }

        /// <summary>
        /// Normalizes the namespace of a specified type for use as the test class namespace.
        /// </summary>
        /// <param name="inputType">The type with the input namespace.</param>
        /// <param name="testNamespacePart">The additional namespace part of the test project, e.g. ".Test".</param>
        /// <returns>A normalized namespace for the test unit.</returns>
        /// <remarks>
        /// <example>
        /// If the fullname of the input type is "NStub.CSharp.BuildContext.ISetupAndTearDownContext"
        /// and the shortest (the root namespace of the library to test) namespace is "NStub.CSharp", then the output
        /// of this method is ".Tests.BuildContext". To build a namespace for the unit test, you simply sum the result
        /// in the following way:
        /// <code><![CDATA[
        /// var testNamespace = detector.ShortestNamespace + GetDifferingNamespace(inputType);
        /// ]]></code>
        /// </example></remarks>
        public string GetDifferingNamespace(CodeTypeDeclaration inputType, string testNamespacePart)
        {
            string codeNs = inputType.Name;
            var indexcodeNs = inputType.Name.LastIndexOf('.');
            if (indexcodeNs > 0)
            {
                codeNs = inputType.Name.Substring(0, indexcodeNs);
            }

            var combined = codeNs;
            var splitter = codeNs.Split(new[] { this.ShortestNamespace }, StringSplitOptions.RemoveEmptyEntries);
            if (splitter.Length == 1)
            {
                combined = testNamespacePart + splitter[0];
            }

            return combined;
        }


        /// <summary>
        /// Combines the with shortest namespace.
        /// </summary>
        /// <param name="inputType">The type with the input namespace.</param>
        /// <param name="testNamespacePart">The additional namespace part of the test project, e.g. ".Test".</param>
        /// <returns>
        /// A normalized namespace for the test unit.
        /// </returns>
        /// <remarks>
        /// If the fullname of the input type is "NStub.CSharp.BuildContext.SetupAndTearDownContext"
        /// and the shortest (the root namespace of the library to test) namespace is "NStub.CSharp", then the output
        /// of this method is "NStub.CSharp.Tests.BuildContext.SetupAndTearDownContext", inserting the <paramref name="testNamespacePart"/>
        /// between the <see cref="ShortestNamespace"/> and the rest of the fullname of <paramref name="inputType"/>. Simply add "Test" at the end
        /// to it an you have the fullname to your test class.
        /// </remarks>
        public string CombineWithShortestNamespace(CodeTypeDeclaration inputType, string testNamespacePart)
        {
            var combined = inputType.Name;
            var splitter = inputType.Name.Split(new[] {this.ShortestNamespace}, StringSplitOptions.RemoveEmptyEntries);
            if (splitter.Length == 1)
            {
                combined = this.ShortestNamespace + testNamespacePart + splitter[0];
            }

            return combined;
        }

        /// <summary>
        /// Prepares the specified namespace imports.
        /// </summary>
        /// <param name="imports">The namespace imports.</param>
        /// <returns>
        /// A normalized list of <see cref="CodeNamespaceImport"/>s.
        /// </returns>
        /// <remarks>
        /// This method detects problematic namespaces like in the example below and prefixes
        /// them with global:: to prevent namespace collisions in the test unit files.
        /// <para>
        /// <example>
        /// The namespace of the object to test is 
        /// <code><![CDATA[
        /// namespace NStub.CSharp.MbUnit
        /// {
        ///     public class CodeLocalVariableBinderTest
        ///     {
        ///     }
        /// }
        /// ]]></code>
        /// so the unit test becomes:
        /// <code><![CDATA[
        /// namespace NStub.CSharp.Tests.MbUnit
        /// {
        ///     using System;
        ///     using System.Collections.Generic;
        ///     using System.Linq;
        ///     using MbUnit.Framework;
        ///     using NStub.CSharp.MbUnit;
        ///     
        ///     
        ///     public partial class CSharpMbUnitCodeGeneratorTest
        ///     {
        ///        [Test()]
        ///        public void ConstructorTest()
        ///        {
        ///     ...
        /// 
        /// ]]></code>
        /// </example>
        /// </para>
        /// <para>
        /// Take a look at the 'using MbUnit.Framework' import. In that way it conflicts with the 'NStub.CSharp.MbUnit' import
        /// and circumvents the name resolution of the <b>TestAttribute</b>. This method detects such concerns and puts a
        /// <b>global::</b> prefix in front of the 'MbUnit.Framework' namespace import.
        /// </para>
        /// </remarks>
        public IEnumerable<CodeNamespaceImport> PrepareNamespaceImports(IEnumerable<string> imports)
        {
            var result = new List<CodeNamespaceImport>();
            foreach (var ns in imports)
            {
                var corectedns = PrepareNamespace(ns);
                result.Add(new CodeNamespaceImport(corectedns));
            }
            return result;
        }
        private StringDictionary correctedNamespaces = new StringDictionary();

        /// <summary>
        /// Prepares and caches a namespace.
        /// </summary>
        /// <param name="ns">The input namespace.</param>
        /// <returns>A normalized namespace.</returns>
        private string PrepareNamespace(string ns)
        {
            var sel = correctedNamespaces[ns];
            if (sel != null)
            {
                return sel;
            }
            string result = ns;
            foreach (var ctdecl in this.namespaces)
            {
                string combined = ctdecl;
                var splitter = ctdecl.Split(new[] { this.ShortestNamespace }, StringSplitOptions.RemoveEmptyEntries);
                if (splitter.Length == 1)
                {
                    combined = splitter[0];
                    if (combined.StartsWith("."))
                    {
                        combined = combined.Substring(1, combined.Length - 1);
                        if (ns.StartsWith(combined + "."))
                        {
                            result = "global::" + result;
                            break;
                        }
                    }
                }
            }

            correctedNamespaces[ns] = result;
            return result;
        }


        /// <summary>
        /// Determines the shortest namespace from a list of type declarations.
        /// </summary>
        /// <param name="typeDeclarations">The type declarations.</param>
        /// <param name="distinctNamespaces">The distinct namespaces.</param>
        /// <returns></returns>
        private static string DetermineShortestNamespace(IEnumerable<CodeTypeDeclaration> typeDeclarations, out IEnumerable<string> distinctNamespaces)
        {
            var typeNamespace = new List<string>();
            foreach (CodeTypeDeclaration typedecl in typeDeclarations)
            {
                var typeName = typedecl.Name;
                var indexcodeNs = typeName.LastIndexOf('.');
                if (indexcodeNs > 0)
                {
                    var codeNs = typeName.Substring(0, indexcodeNs);
                    typeNamespace.Add(codeNs);
                }
            }

            var typeNamespaceDistinct = typeNamespace.Distinct();
            distinctNamespaces = typeNamespaceDistinct;
            var shortestNamespace = typeNamespaceDistinct.Min();
            return shortestNamespace;
        }
    }
}
