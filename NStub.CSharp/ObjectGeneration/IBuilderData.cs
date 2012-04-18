// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuilderData.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System.Reflection;

    /// <summary>
    /// Stores information about a test method for an <see cref="IMemberBuilder"/>.
    /// </summary>
    public interface IBuilderData
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether the information of this instance is complete.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        bool IsComplete { get; }

        #endregion

        /// <summary>
        /// Gets the data of this instance.
        /// </summary>
        /// <returns>The stored data.</returns>
        object GetData();

        /// <summary>
        /// Determines whether this instance holds data for the specified builder type.
        /// </summary>
        /// <param name="builder">The requesting builder.</param>
        /// <returns>
        ///   <c>true</c> if this instance holds data for the specified builder type; otherwise, <c>false</c>.
        /// </returns>
        bool HasDataForType(IMemberBuilder builder);

        /// <summary>
        /// Sets the data via the specified method info.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        void SetData(MethodInfo methodInfo);
    }
}