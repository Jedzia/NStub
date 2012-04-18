// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeTypeSetup.cs" company="EvePanix">
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
    using System.Linq;
    using NStub.Core;

    /// <summary>
    /// Setup Helper for <see cref="CodeTypeDeclaration"/>'s.
    /// </summary>
    internal class CodeTypeSetup
    {
        #region Fields

        private readonly NamespaceDetector namespaceDetector;
        private readonly CodeTypeDeclaration testClassDeclaration;
        private bool setUpCodeNamespaceCalled;
        private bool setUpTestnameCalled;

        #endregion

        #region Constructors

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

        #endregion

        /// <summary>
        /// Create and initialize a <see cref="CodeNamespace"/> with the correct test-namespace and namespace imports.
        /// </summary>
        /// <param name="rootNamespace">The root namespace of the test Project.</param>
        /// <param name="namespaceImports">The namespace imports.</param>
        /// <returns>A <see cref="CodeNamespace"/> initialized with the specified namespace and namespace imports.</returns>
        /// <exception cref="InvalidOperationException"><see cref="CodeTypeSetup.SetUpCodeNamespace"/>(...) was called the second time.</exception>
        public CodeNamespace SetUpCodeNamespace(
            string rootNamespace, 
            IEnumerable<string> namespaceImports)
        {
            if (this.setUpCodeNamespaceCalled)
            {
                throw new InvalidOperationException("CodeTypeSetup.SetUpCodeNamespace(...) was called the second time.");
            }

            // Create a namespace for the Type in order to put it in scope
            var ndiff = this.namespaceDetector.GetDifferingTypeFullname(this.testClassDeclaration, ".Tests");
            var codeNamespace = new CodeNamespace(rootNamespace + ndiff);

            // add using imports.
            codeNamespace.Imports.AddRange(this.namespaceDetector.PrepareNamespaceImports(namespaceImports).ToArray());

            var indexcodeNs = this.testClassDeclaration.Name.LastIndexOf('.');
            if (indexcodeNs > 0)
            {
                // try to import the namespace for the object under test.
                var codeNs = this.testClassDeclaration.Name.Substring(0, indexcodeNs);
                codeNamespace.Imports.Add(new CodeNamespaceImport(codeNs));
            }

            this.setUpCodeNamespaceCalled = true;
            return codeNamespace;
        }

        /// <summary>
        /// Gets the name of the test for the object under test associated with this instance.
        /// </summary>
        /// <returns>The name of the test class.</returns>
        /// <exception cref="InvalidOperationException"><see cref="CodeTypeSetup.SetUpTestname"/>(...) was called the second time.</exception>
        public string SetUpTestname()
        {
            if (this.setUpTestnameCalled)
            {
                throw new InvalidOperationException("CodeTypeSetup.SetUpTestname() was called the second time.");
            }

            // Clean the type name
            this.testClassDeclaration.Name =
                Utility.ScrubPathOfIllegalCharacters(this.testClassDeclaration.Name);
            this.testClassDeclaration.Name =
                this.namespaceDetector.InsertAfterShortestNamespace(this.testClassDeclaration, ".Tests");

            var testObjectName = Utility.GetUnqualifiedTypeName(this.testClassDeclaration.Name);

            // Add "Test" to the name.
            this.testClassDeclaration.Name = testObjectName + "Test";
            this.setUpTestnameCalled = true;
            return testObjectName;
        }
    }
}