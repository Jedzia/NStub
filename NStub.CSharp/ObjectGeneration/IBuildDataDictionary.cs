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
    using NStub.Core;

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
        /// <remarks>If the <paramref name="item"/> is already present, then the add operation throws an exception.</remarks>
        void AddDataItem(string category, string key, IBuilderData item, bool replace);

        /// <summary>
        /// Adds the specified data item to the specified category of this list.
        /// </summary>
        /// <param name="category">The category of the item.</param>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        /// <param name="replace">if set to <c>true</c> replaces the data if present.</param>
        /// <param name="skip">if set to <c>true</c> skip already present items.</param>
        /// <remarks>
        /// If <paramref name="replace"/> is <c>true</c>, <paramref name="skip"/> is <c>true</c> 
        /// and the <paramref name="item"/> is already present, then the add operation is skipped.
        /// if <paramref name="skip"/> is <c>false</c> an exception is thrown.
        /// </remarks>
        void AddDataItem(string category, string key, IBuilderData item, bool replace, bool skip);

        /// <summary>
        /// Adds a <see cref="NStub.CSharp.ObjectGeneration.Builders.StringConstantBuildParameter.Value"/> to the
        /// general category data set associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="value">The string value of the item.</param>
        /// <remarks>
        /// Already present items are skipped (not replaced) and no exception is thrown.
        /// </remarks>
        void AddGeneralString(string key, string value);

        /// <summary>
        /// Adds a <see cref="NStub.CSharp.ObjectGeneration.Builders.StringConstantBuildParameter.Value"/> to the
        /// specified category data set associated with the specified key.
        /// </summary>
        /// <param name="category">The category of the item.</param>
        /// <param name="key">The key of the item.</param>
        /// <param name="value">The string value of the item.</param>
        /// <remarks>
        /// Already present items are skipped (not replaced) and no exception is thrown.
        /// </remarks>
        void AddGeneralString(string category, string key, string value);

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
        /// Gets the value out of the specified category, associated with the specified key.
        /// </summary>
        /// <param name="category">The category of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified
        /// key, if the key is found; otherwise, the default value for the type of the
        /// value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the this instance contains an
        /// element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">key is <c>null</c>.</exception>
        bool TryGetCategory(string category, out IDictionary<string, IBuilderData> value);

        /// <summary>
        /// Gets the value out of the general data set associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified
        /// key, if the key is found; otherwise, the default value for the type of the
        /// value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the this instance contains an
        /// element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">key is <c>null</c>.</exception>
        bool TryGetValue(string key, out IBuilderData value);

        /// <summary>
        /// Gets the <see cref="NStub.CSharp.ObjectGeneration.Builders.StringConstantBuildParameter.Value"/> out 
        /// of the general data set associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>
        /// The requested string from the category 'General' and with the specified <paramref name="key"/> constant.
        /// </returns>
        string GeneralString(string key);

        /// <summary>
        /// Gets the <see cref="NStub.CSharp.ObjectGeneration.Builders.StringConstantBuildParameter.Value"/> out of 
        /// the general data set associated with the specified key.
        /// </summary>
        /// <param name="category">The category of the value to get.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>
        /// The requested string from the category 'General' and with the specified <paramref name="key"/> constant.
        /// </returns>
        string GeneralString(string category, string key);

    }
}