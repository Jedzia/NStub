// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorConstants.cs" company="EvePanix">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NStub.CSharp.ObjectGeneration.Builders;

    /// <summary>
    /// Global constants used by the test generator.
    /// </summary>
    public static class GeneratorConstants
    { 
        /*/// <summary>
        /// The storage name for category to store <see cref="PropertyBuilderData"/> in a <see cref="BuildDataDictionary"/>.
        /// Use the test class name, that is CurrentTestClassDeclaration.Name, for the first ({0}) parameter.
        /// </summary>
        /// <remarks>Was "Property.{0}" before, but "Property" alone is specific enough, cause the key of the property data
        /// is memberBuildContext.TestKey, that is composed of CodeNamespace.Name + "." + this.CurrentTestClassDeclaration.Name 
        /// + "." + composedTestName;</remarks>
        public const string PropertyStorageCategory = "Property";*/

        /// <summary>
        /// The base class type of test classes with
        /// objects under test(OuT) that implement the <see cref="System.ComponentModel.INotifyPropertyChanged"/> interface.
        /// </summary>
        /// <remarks>The property itself is set in <see cref="BaseCSharpCodeGenerator.InitializeBuildProperties"/>.</remarks>
        public const string BaseClassOfINotifyPropertyChangedTest = "CountingPropertyChangedEventFixture";


    }
}
