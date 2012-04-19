// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamespaceDetector.cs" company="EvePanix">
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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using global::MbUnit.Framework;
    using NStub.Core;

    /// <summary>
    /// Determines the namespace to use, based on a supplied list of type declarations.
    /// </summary>
    public class NamespaceDetector
    {
        #region Fields

        private readonly StringDictionary correctedNamespaces = new StringDictionary();
        private readonly string[] namespaces;
        private readonly CodeTypeDeclaration[] storedDeclarations;

        /// <summary>
        /// The list of global type declarations.
        /// </summary>
        private readonly CodeTypeDeclarationCollection typeDeclarations;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceDetector"/> class.
        /// </summary>
        /// <param name="typeDeclarations">The type declarations to investigate.</param>
        public NamespaceDetector(CodeTypeDeclarationCollection typeDeclarations)
        {
            Guard.NotNull(() => typeDeclarations, typeDeclarations);
            this.typeDeclarations = typeDeclarations;

// storedDeclarations = typeDeclarations.OfType<CodeTypeDeclaration>().ToList();
            this.storedDeclarations = typeDeclarations.OfType<CodeTypeDeclaration>().ToArray();

            IEnumerable<string> distinctNamespaces;
            this.ShortestNamespace = DetermineShortestNamespace(this.storedDeclarations, out distinctNamespaces);
            this.namespaces = distinctNamespaces.ToArray();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the shortest common namespace part.
        /// </summary>
        public string ShortestNamespace { get; private set; }

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

        #endregion

        /// <summary>
        /// Get the differing namespace part between object to test and unit test module,
        /// seen relative from the <see cref="ShortestNamespace"/>, prefixed with the specified string.
        /// </summary>
        /// <param name="inputType">The type with the input namespace.</param>
        /// <param name="testNamespacePart">The additional namespace part of the test project, e.g. ".Test".</param>
        /// <returns>A normalized namespace for the test unit.</returns>
        /// <remarks>
        /// <example>
        /// If the full qualified name of the input type is "NStub.CSharp.BuildContext.ISetupAndTearDownContext"
        /// and the shortest (the root namespace of the library to test) namespace is "NStub.CSharp", then the output
        /// of this method is ".Tests.BuildContext". To build a namespace for the unit test, you simply sum the result
        /// in the following way:
        /// <code><![CDATA[
        /// var testNamespace = detector.ShortestNamespace + GetDifferingNamespace(inputType);
        /// ]]></code>
        /// </example></remarks>
        public string GetDifferingTypeFullname(CodeTypeDeclaration inputType, string testNamespacePart)
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
        /// Insert the specified extra string into the namespace of a specified type, relative to the 
        /// <see cref="ShortestNamespace"/>.
        /// </summary>
        /// <param name="inputType">The type with the input namespace.</param>
        /// <param name="testNamespacePart">The additional namespace part of the test project, e.g. ".Test".</param>
        /// <returns>
        /// A normalized namespace for the test unit.
        /// </returns>
        /// <remarks>
        /// If the full qualified name of the input type is "NStub.CSharp.BuildContext.SetupAndTearDownContext"
        /// and the shortest (the root namespace of the library to test) namespace is "NStub.CSharp", then the output
        /// of this method is "NStub.CSharp.Tests.BuildContext.SetupAndTearDownContext", inserting the <paramref name="testNamespacePart"/>
        /// between the <see cref="ShortestNamespace"/> and the rest of the full qualified name of <paramref name="inputType"/>. Simply add "Test" at the end
        /// to it an you have the full qualified name to your test class.
        /// </remarks>
        public string InsertAfterShortestNamespace(CodeTypeDeclaration inputType, string testNamespacePart)
        {
            var combined = inputType.Name;
            var splitter = inputType.Name.Split(new[] { this.ShortestNamespace }, StringSplitOptions.RemoveEmptyEntries);
            if (splitter.Length == 1)
            {
                combined = this.ShortestNamespace + testNamespacePart + splitter[0];
            }

            return combined;
        }

        /// <summary>
        /// Prepares the specified namespace imports to prevent collisions with the types under test.
        /// </summary>
        /// <param name="imports">The namespace imports.</param>
        /// <returns>
        /// A normalized list of <see cref="CodeNamespaceImport"/>s.
        /// </returns>
        /// <remarks>
        /// This method detects problematic <c>namespaces</c> like in the example below and prefixes
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
        ///     public partial class CSharpMbUnitCodeGeneratorTest
        ///     {
        ///        [Test()]
        ///        public void ConstructorTest()
        ///        {
        ///     ...
        /// ]]></code>
        /// </example>
        /// </para>
        /// <para>
        /// Take a look at the 'using MbUnit.Framework' import. In that way it conflicts with the 'NStub.CSharp.MbUnit' import
        /// and circumvents the name resolution of the <b><see cref="TestAttribute"/></b>. This method detects such concerns and puts a
        /// <b>global::</b> prefix in front of the 'MbUnit.Framework' namespace import.
        /// </para>
        /// </remarks>
        public IEnumerable<CodeNamespaceImport> PrepareNamespaceImports(IEnumerable<string> imports)
        {
            var result = new List<CodeNamespaceImport>();
            foreach(var ns in imports)
            {
                var corectedns = this.PrepareNamespace(ns);
                result.Add(new CodeNamespaceImport(corectedns));
            }

            return result;
        }

        /// <summary>
        /// Determines the shortest namespace from a list of type declarations.
        /// </summary>
        /// <param name="typeDeclarations">The type declarations.</param>
        /// <param name="distinctNamespaces">The list of unique <c>namespaces</c>.</param>
        /// <returns>The possible library namespace to use.</returns>
        private static string DetermineShortestNamespace(
            IEnumerable<CodeTypeDeclaration> typeDeclarations, out IEnumerable<string> distinctNamespaces)
        {
            var typeNamespace = new List<string>();
            foreach(CodeTypeDeclaration typedecl in typeDeclarations)
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

        /// <summary>
        /// Prepares and caches a namespace.
        /// </summary>
        /// <param name="ns">The input namespace.</param>
        /// <returns>A normalized namespace.</returns>
        private string PrepareNamespace(string ns)
        {
            var sel = this.correctedNamespaces[ns];
            if (sel != null)
            {
                return sel;
            }

            string result = ns;
            foreach(var ctdecl in this.namespaces)
            {
                var splitter = ctdecl.Split(new[] { this.ShortestNamespace }, StringSplitOptions.RemoveEmptyEntries);
                if (splitter.Length == 1)
                {
                    string combined = splitter[0];
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

            this.correctedNamespaces[ns] = result;
            return result;
        }
    }
}