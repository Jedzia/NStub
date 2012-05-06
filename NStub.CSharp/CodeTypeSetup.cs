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
    using System.Globalization;
    using System.ComponentModel;
    using NStub.CSharp.ObjectGeneration;

    /// <summary>
    /// Setup Helper for <see cref="CodeTypeDeclaration"/>'s.
    /// </summary>
    internal class CodeTypeSetup
    {
        #region Fields

        private readonly NamespaceDetector namespaceDetector;
        private readonly CodeTypeDeclaration testClassDeclaration;
        private readonly IBuildDataDictionary buildData;
        private bool setUpCodeNamespaceCalled;
        private bool setUpTestnameCalled;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTypeSetup"/> class.
        /// </summary>
        /// <param name="namespaceDetector">The namespace detector.</param>
        /// <param name="buildData">The build data properties.</param>
        /// <param name="testClassDeclaration">The class declaration of the object under test.</param>
        public CodeTypeSetup(NamespaceDetector namespaceDetector, IBuildDataDictionary buildData, CodeTypeDeclaration testClassDeclaration)
        {
            Guard.NotNull(() => namespaceDetector, namespaceDetector);
            this.namespaceDetector = namespaceDetector;
            Guard.NotNull(() => buildData, buildData);
            this.buildData = buildData;
            Guard.NotNull(() => testClassDeclaration, testClassDeclaration);
            this.testClassDeclaration = testClassDeclaration;
            //result = SetUp();
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
        private string genericPart;

        private bool currentIsGeneric;

        /// <summary>
        /// Adjusts the test class declaration name and provides the name for the OuT (object under test).
        /// </summary>
        /// <returns>The name for the OuT (object under test)</returns>
        /// <exception cref="InvalidOperationException"><see cref="CodeTypeSetup.SetUpTestname"/>(...) was called the second time.</exception>
        public string SetUpTestname()
        {
            //return result;
            return SetUp();
        }
        
        private string SetUp()
        {
            if (this.setUpTestnameCalled)
            {
                throw new InvalidOperationException("CodeTypeSetup.SetUpTestname() was called the second time.");
            }
            genericPart = string.Empty;
            currentIsGeneric = false;

            // Adjust OuT with generic types parameters. 
            var testObjectType = testClassDeclaration.UserData[NStubConstants.UserDataClassTypeKey] as Type;
            if (testObjectType == null)
            {
                var message = string.Format(CultureInfo.CurrentCulture, "The UserData of the type '{0}' is not correct initialized." +
                    "Provide the System.Type info about the object under test with the {1} UserData key."
                    , testClassDeclaration.Name, NStubConstants.UserDataClassTypeKey);
                throw new KeyNotFoundException(message);
            }

            var baseObjectFullname = this.testClassDeclaration.Name;
            baseKey = baseObjectFullname;
            if (testObjectType.IsGenericType)
            {
                var genIndexLast = this.testClassDeclaration.Name.LastIndexOf('`');
                baseObjectFullname = this.testClassDeclaration.Name.Substring(0, genIndexLast);
                //var newName = newNameFirst + "Generic";
                var newName = baseObjectFullname + "Generic";// +"<string>";
                genericPart = "<string>";
                currentIsGeneric = true;
                this.testClassDeclaration.Name = newName;
            }

            if (testObjectType.IsAssignableFrom(typeof(INotifyPropertyChanged)))
            {
            }
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(testObjectType))
            {
                var bd = buildData.GeneralString(BuilderConstants.PropertyBaseClassOfINotifyPropertyChangedTest);
                testClassDeclaration.BaseTypes.Add(new CodeTypeReference(bd));
                //testClassDeclaration.BaseTypes.Add(new CodeTypeReference("CountingPropertyChangedEventFixture"));
            }

            // expand the test class namespace by ".Test"
            //this.testClassDeclaration.Name =
            //    this.namespaceDetector.InsertAfterShortestNamespace(this.testClassDeclaration, ".Tests");

            // Add "Test" to the test classes name.
            this.testClassDeclaration.Name = Utility.GetUnqualifiedTypeName(this.testClassDeclaration.Name) + "Test";


            var objectUnderTestName = Utility.GetUnqualifiedTypeName(baseObjectFullname);
            if (testObjectType.IsGenericType)
            {
                objectUnderTestName = objectUnderTestName + genericPart;
            }

            this.setUpTestnameCalled = true;
            return objectUnderTestName;
        }

        private string baseKey;

        public string BaseKey
        {
            get { return baseKey; }
            // set { baseKey = value; }
        }

        public string FixForWriteFile(string name)
        {
            var testFilename = Utility.ScrubPathOfIllegalCharacters(name);
            if (currentIsGeneric)
            {
                // testFilename += "Generic";
            }
            // var testFilename = this.testClassDeclaration.Name;
            // return testFilename = testFilename.Replace("<string>", "Generic");
            return testFilename;
        }
    }
}