// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBuildDataDictionary.cs" company="EvePanix">
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

    /// <summary>
    /// Bundles a list of <see cref="IBuilderData"/> organized as a dictionary of strings, categorized into main categories.
    /// </summary>
    public interface IBuildDataDictionary : IEnumerable<IBuilderData>
    {
        #region Properties

        /// <summary>
        /// Gets the general category.
        /// </summary>
        IReadOnlyDictionary<string, IBuilderData> General { get; }

        /// <summary>
        /// Gets the number of <see cref="IBuilderData"/> entries across all categories.
        /// </summary>
        int EntryCount { get; }

        /// <summary>
        /// Gets the number of <see cref="IBuilderData"/> entries in the <see cref="General"/> category.
        /// </summary>
        int Count { get; }

        #endregion

        /// <summary>
        /// Adds the specified data item to the "General" topic of this list.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        void AddDataItem(string key, IBuilderData item);

        /// <summary>
        /// Adds the specified data item to the "General" topic of this list.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        /// <param name="replace">if set to <c>true</c> replaces the data if present.</param>
        void AddDataItem(string key, IBuilderData item, bool replace);

        /// <summary>
        /// Adds the specified data item to the specified category of this list.
        /// </summary>
        /// <param name="category">The category of the item.</param>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        void AddDataItem(string category, string key, IBuilderData item);

        /// <summary>
        /// Adds the specified data item to the specified category of this list.
        /// </summary>
        /// <param name="category">The category of the item.</param>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        /// <param name="replace">if set to <c>true</c> replaces the data if present.</param>
        void AddDataItem(string category, string key, IBuilderData item, bool replace);

        /// <summary>
        /// Gets the data of this instance.
        /// </summary>
        /// <returns>A read only dictionary of data.</returns>
        IBuildDataReadOnlyDictionary Data();

        /// <summary>
        /// Resets the dirty flag ( Save this instance not implemented ).
        /// </summary>
        void Save();

        /*/// <summary>
        /// Returns an enumerator that iterates through the category collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerable<KeyValuePair<string, IDictionary<string, IBuilderData>>> GetData();*/

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified
        /// key, if the key is found; otherwise, the default value for the type of the
        /// value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the this instance contains an
        /// element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">key is <c>null</c>.</exception>
        bool TryGetValue(string key, out IBuilderData value);

    }
}