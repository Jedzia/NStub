using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NStub.Core;
using System.CodeDom;

namespace NStub.CSharp
{
    /// <summary>
    /// Setup Helper for <see cref="CodeTypeDeclaration"/>'s.
    /// </summary>
    internal class CodeTypeSetup
    {

        private readonly NamespaceDetector namespaceDetector;
        private readonly CodeTypeDeclaration testClassDeclaration;
        private bool setUpTestnameCalled;
        private bool setUpCodeNamespaceCalled;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTypeSetup"/> class.
        /// </summary>
        /// <param name="namespaceDetector">The namespace detector.</param>
        /// <param name="testClassDeclaration">The class declaration of the object under test.</param>
        public CodeTypeSetup(NamespaceDetector namespaceDetector, CodeTypeDeclaration testClassDeclaration)
        {
            Guard.NotNull(() => namespaceDetector, namespaceDetector);
            this.namespaceDetector = namespaceDetector;
            Guard.NotNull(() => testClassDeclaration, testClassDeclaration);
            this.testClassDeclaration = testClassDeclaration;
        }

        /// <summary>
        /// Gets the name of the test for the object under test associated with this instance.
        /// </summary>
        /// <returns>The name of the test class.</returns>
        public string SetUpTestname()
        {
            if (setUpTestnameCalled)
            {
                throw new InvalidOperationException("CodeTypeSetup.SetUpTestname() was called the second time.");
            }

            // Clean the type name
            testClassDeclaration.Name =
                Utility.ScrubPathOfIllegalCharacters(testClassDeclaration.Name);
            testClassDeclaration.Name = namespaceDetector.InsertAfterShortestNamespace(testClassDeclaration, ".Tests");

            var testObjectName = Utility.GetUnqualifiedTypeName(testClassDeclaration.Name);

            // Add "Test" to the name.
            testClassDeclaration.Name = testObjectName + "Test";
            setUpTestnameCalled = true;
            return testObjectName;
        }

        /// <summary>
        /// Create and initialize a <see cref="CodeNamespace"/> with the correct test-namespace and namespace imports.
        /// </summary>
        /// <param name="rootNamespace">The root namespace of the test Project.</param>
        /// <param name="namespaceImports">The namespace imports.</param>
        /// <returns>A <see cref="CodeNamespace"/> initialized with the specified namespace and namespace imports.</returns>
        public CodeNamespace SetUpCodeNamespace(
            string rootNamespace,
            IEnumerable<string> namespaceImports)
        {
            if (setUpCodeNamespaceCalled)
            {
                throw new InvalidOperationException("CodeTypeSetup.SetUpCodeNamespace(...) was called the second time.");
            }

            // Create a namespace for the Type in order to put it in scope
            var ndiff = namespaceDetector.GetDifferingTypeFullname(testClassDeclaration, ".Tests");
            var codeNamespace = new CodeNamespace(rootNamespace + ndiff);

            // add using imports.
            codeNamespace.Imports.AddRange(namespaceDetector.PrepareNamespaceImports(namespaceImports).ToArray());

            var indexcodeNs = testClassDeclaration.Name.LastIndexOf('.');
            if (indexcodeNs > 0)
            {
                // try to import the namespace for the object under test.
                var codeNs = testClassDeclaration.Name.Substring(0, indexcodeNs);
                codeNamespace.Imports.Add(new CodeNamespaceImport(codeNs));
            }
            setUpCodeNamespaceCalled = true;
            return codeNamespace;
        }
    }
}
