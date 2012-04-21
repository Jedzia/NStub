// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuilderSerializer.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration.Builders
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides serialization support for <see cref="BuildParametersBase{T}"/> data classes.
    /// </summary>
    public interface IBuilderSerializer
    {
        /// <summary>
        /// Set the parameters in the properties storage from a specified xml representation of the data.
        /// </summary>
        /// <param name="xml">The xml representation of the data.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <param name="handlers">The list of existing handlers to investigate.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="InvalidCastException"><c>InvalidCastException</c> Problem building from serialization data.</exception>
        IEnumerable<IMemberBuildParameters> DeserializeAllSetupData(
            string xml, 
            IBuildDataDictionary properties, 
            IEnumerable<IBuildHandler> handlers);

        /// <summary>
        /// Determines the matching member builder by the first child's name from a XML fragment.
        /// </summary>
        /// <param name="xml">The xml fragment.</param>
        /// <param name="handlers">The list of existing handlers to investigate.</param>
        /// <returns>The matching handler or <c>null</c> if none is found.</returns>
        IBuildHandler DetermineIMemberBuilderFromXmlFragment(string xml, IEnumerable<IBuildHandler> handlers);

        /// <summary>
        /// Get the parameters for the specified builder type, possibly creating it, if there
        /// is not yet one in the build data collection.
        /// </summary>
        /// <param name="builderType">Type of the builder to request a set of sample data for.</param>
        /// <param name="paraType">Type of the parameter class.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="KeyNotFoundException">The given <paramref name="builderType"/> was not present in the lookup.</exception>
        IMemberBuildParameters GetParameters(Type builderType, Type paraType, IBuildDataDictionary properties);

        /// <summary>
        /// Gets the sample setup xml data for a specified <see cref="IMemberBuilder"/>.
        /// </summary>
        /// <param name="builderType">Type of the builder to request a set of sample data for.</param>
        /// <param name="paraType">Type of the parameter class.</param>
        /// <returns>
        /// A string containing the sample data for the specified <paramref name="builderType"/> as xml string.
        /// </returns>
        string GetSampleSetupData(Type builderType, Type paraType);

        /// <summary>
        /// Gets the xml data representation of all registered <see cref="IBuildHandler"/>s.
        /// </summary>
        /// <param name="properties">The properties storage which stores the <see cref="IMemberBuildParameters"/> data to serialize.</param>
        /// <param name="handlers">The handlers with the links to the <see cref="IMemberBuilder"/> types.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        string SerializeAllHandlers(IBuildDataDictionary properties, IEnumerable<IBuildHandler> handlers);

        /// <summary>
        /// Gets the xml data representation of a single registered <see cref="IMemberBuilder"/>s parameters.
        /// </summary>
        /// <param name="builderType">Type of the builder to request data for.</param>
        /// <param name="parameters">The parameters with the data to serialize.</param>
        /// <returns>
        /// The serialized data of the specified <paramref name="builderType"/>.
        /// </returns>
        string SerializeParametersForBuilderType(Type builderType, IMemberBuildParameters parameters);

        /// <summary>
        /// Set the parameters in the properties storage from a specified xml representation of the data.
        /// </summary>
        /// <param name="xml">The xml representation of the data.</param>
        /// <param name="properties">The global properties storage.</param>
        /// <param name="handler">The handler that holds the builder- to parameter-type relation.</param>
        /// <returns>
        /// A new instance of a matching parameter data set for the specified builder.
        /// </returns>
        /// <exception cref="InvalidCastException"><c>InvalidCastException</c> Problem building from serialization data.</exception>
        IMemberBuildParameters SetParameters(string xml, IBuildDataDictionary properties, IBuildHandler handler);
    }
}