// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuilderConstants.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NStub.CSharp.ObjectGeneration.Builders;

    /// <summary>
    /// Global constants related to build data.
    /// </summary>
    public static class BuilderConstants
    { 
        /// <summary>
        /// Xml-Element identifier for the collection of <see cref="NStub.CSharp.ObjectGeneration.IMemberBuildParameters"/>.
        /// </summary>
        public const string BuildParametersXmlId = "BuildParameters";

        /// <summary>
        /// The storage name for category to store <see cref="PropertyBuilderData"/> in a <see cref="BuildDataDictionary"/>.
        /// Use the test class name, that is CurrentTestClassDeclaration.Name, for the first ({0}) parameter.
        /// </summary>
        /// <remarks>Was "Property.{0}" before, but "Property" alone is specific enough, cause the key of the property data
        /// is memberBuildContext.TestKey, that is composed of CodeNamespace.Name + "." + this.CurrentTestClassDeclaration.Name 
        /// + "." + composedTestName;</remarks>
        public const string PropertyStorageCategory = "Property";

    }
}
