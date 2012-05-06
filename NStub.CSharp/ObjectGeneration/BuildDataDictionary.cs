// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildDataDictionary.cs" company="EvePanix">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NStub.Core;
    using NStub.CSharp.ObjectGeneration.Builders;

    /// <summary>
    /// Lookup of <see cref="IBuilderData"/> organized as a dictionary of strings, categorized into main categories.
    /// </summary>
    public class BuildDataDictionary : IBuildDataDictionary
    {
        #region Fields

        private readonly Dictionary<string, IDictionary<string, IBuilderData>> data =
            new Dictionary<string, IDictionary<string, IBuilderData>>();

        private readonly Dictionary<string, IBuilderData> generalData;
        private bool isDirty;

        /// <summary>
        /// The category key for the global 'General' category.
        /// </summary>
        public const string GeneralCategory = "General";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildDataDictionary"/> class.
        /// </summary>
        public BuildDataDictionary()
        {
            this.generalData = new Dictionary<string, IBuilderData>();
            this.data.Add(GeneralCategory, this.generalData);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance has uncomitted changes.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has uncomitted changes; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirty
        {
            get
            {
                return this.isDirty;
            }
        }

        /// <summary>
        /// Gets the number of <see cref="IBuilderData"/> entries in the <see cref="General"/> category.
        /// </summary>
        public int Count
        {
            get
            {
                return this.data.Count;
            }
        }

        /// <summary>
        /// Gets the number of <see cref="IBuilderData"/> entries across all categories.
        /// </summary>
        public int EntryCount
        {
            get
            {
                return this.data.Select(e => e.Value.Count).Aggregate((e, a) => a + e);
            }
        }

        /// <summary>
        /// Gets the general category.
        /// </summary>
        public IReadOnlyDictionary<string, IBuilderData> General
        {
            get
            {
                return new ReadOnlyDictionary<string, IBuilderData>(this.generalData);
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the <see cref="IBuilderData"/> lookup of the specified category.
        /// </summary>
        /// <param name="category">The category of the requested data items.</param>
        public IDictionary<string, IBuilderData> this[string category]
        {
            get
            {
                IDictionary<string, IBuilderData> catLookup;
                this.TryGetCategory(category, out catLookup);
                return catLookup;
            }

            /*set
                                    {
                                        this.data[category] = value;
                                    }*/
        }

        #endregion

        /*/// <summary>
        /// Gets the number of parameters with standard arguments. ( Currently all but interfaces ).
        /// </summary>
        public int CountOfStandardTypes
        {
            get
            {
                return this.parameters.Count(e => !e.ParameterType.IsInterface);
            }
        }*/

        /*/// <summary>
        /// Gets a value indicating whether this instance has parameters of a interface type stored.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has interfaces in the parameter types; otherwise, <c>false</c>.
        /// </value>
        public bool HasInterfaces { get; private set; }*/

        /*/// <summary>
        /// Gets the parameters where the parameter-type isn't an interface.
        /// </summary>
        public IEnumerable<ParameterInfo> StandardTypes
        {
            get
            {
                return this.parameters.Where(e => !e.ParameterType.IsInterface);
            }
        }*/

        /// <summary>
        /// Adds the specified data item to the "General" topic of this list.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        public void AddDataItem(string key, IBuilderData item)
        {
            this.generalData.Add(key, item);
            isDirty = true;
        }

        /// <summary>
        /// Adds the specified data item to the "General" topic of this list.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        /// <param name="replace">if set to <c>true</c> replaces the data if present.</param>
        public void AddDataItem(string key, IBuilderData item, bool replace)
        {
            this.AddDataItem(GeneralCategory, key, item, replace);
        }


        /// <summary>
        /// Adds the specified data item to the specified category of this list.
        /// </summary>
        /// <param name="category">The category of the item.</param>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        public void AddDataItem(string category, string key, IBuilderData item)
        {
            this.AddDataItem(category, key, item, false);
        }

        /// <summary>
        /// Adds the specified data item to the specified category of this list.
        /// </summary>
        /// <param name="category">The category of the item.</param>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        /// <param name="replace">if set to <c>true</c> replaces the data if present.</param>
        /// <remarks>If the <paramref name="item"/> is already present, then the add operation throws an exception.</remarks>
        public void AddDataItem(string category, string key, IBuilderData item, bool replace)
        {
            AddDataItem(category, key, item, replace, false);
        }

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
        public void AddDataItem(string category, string key, IBuilderData item, bool replace, bool skip)
        {
            IDictionary<string, IBuilderData> catLookup;
            var found = this.TryGetCategory(category, out catLookup);
            if (!found)
            {
                catLookup = new Dictionary<string, IBuilderData>();
                this.data.Add(category, catLookup);
                isDirty = true;
            }

            if (!replace)
            {
                // Todo: this changes the behavior .. already present items are skipped. reflect this in the unit test.
                if (skip && catLookup.ContainsKey(key))
                    return;
                catLookup.Add(key, item);
                isDirty = true;
            }
            else
            {
                if (catLookup.ContainsKey(key))
                {
                    if (catLookup[key] != item)
                    {
                        isDirty = true;
                    }
                    catLookup[key] = item;
                }
                else
                {
                    catLookup.Add(key, item);
                    isDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets the data of this instance.
        /// </summary>
        /// <returns>
        /// A read only dictionary of data.
        /// </returns>
        public IBuildDataReadOnlyDictionary Data()
        {
            var rlist = new Dictionary<string, IReadOnlyDictionary<string, IBuilderData>>();
            foreach (var item in this.data)
            {
                var sub = new ReadOnlyDictionary<string, IBuilderData>(item.Value);
                rlist.Add(item.Key, sub);
            }

            var xxxx = new BuildDataReadOnlyDictionary(rlist);

            // var ro = new ReadOnlyDictionary<string, IReadOnlyDictionary<string, IBuilderData>>(rlist);
            return xxxx;
        }

        /// <summary>
        /// Resets the dirty flag ( Save this instance not implemented ).
        /// </summary>
        public void Save()
        {
            this.isDirty = false;
        }

        /*/// <summary>
        /// Returns an enumerator that iterates through the category collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerable<KeyValuePair<string, IDictionary<string, IBuilderData>>> GetData()
        {
            return this.data;
        }*/

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IBuilderData> GetEnumerator()
        {
            return this.generalData.Values.GetEnumerator();
        }

        /*public Dictionary<string, IBuilderData>.Enumerator GetEnumerator()
        {
            return this.generalData.GetEnumerator();
        }*/

        // public abstract class mapCollection<T,K> : Gallio.Common.Collections.ReadOnlyDictionary<T,K>, ICollection<T> //where T : new()
        // {
        // }

        /*/// <summary>
        /// DeleteME only for devel.
        /// </summary>
        /// <returns>DeleteME only for devel</returns>
        public IEnumerable<KeyValuePair<string, IDictionary<string, IBuilderData>>> TestGetData()
        {
            // public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
            // public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
            // IEnumerable<KeyValuePair<string, IDictionary<string, IBuilderData>>>
            // ICollection<KeyValuePair<string, IDictionary<string, IBuilderData>>>
            // this.data.

            // IDictionary<string, IBuilderData> moese = this.data["adf"];
            // IEnumerable<KeyValuePair<string, IBuilderData>> moese = this.data["adf"];

            // IEnumerable<KeyValuePair<string, ICollection<KeyValuePair<string, IBuilderData>>>> aaaaa = this.data;

            // var xxxx =                this.data["adf"];
            return this.data;
        }*/

        // public IReadOnlyDictionary<string, IReadOnlyDictionary<string, IBuilderData>> Moep()

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
        public bool TryGetCategory(string category, out IDictionary<string, IBuilderData> value)
        {
            return this.data.TryGetValue(category, out value);
        }

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
        public bool TryGetValue(string key, out IBuilderData value)
        {
            return this.generalData.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the <see cref="NStub.CSharp.ObjectGeneration.Builders.StringConstantBuildParameter.Value"/> out 
        /// of the general data set associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>
        /// The requested string from the category 'General' and with the specified <paramref name="key"/> constant.
        /// </returns>
        public string GeneralString(string key)
        {
            IBuilderData value;
            this.TryGetValue(key, out value);
            var str = (StringConstantBuildParameter)value;
            return str.Value;
        }

        /// <summary>
        /// Gets the <see cref="NStub.CSharp.ObjectGeneration.Builders.StringConstantBuildParameter.Value"/> out of 
        /// the general data set associated with the specified key.
        /// </summary>
        /// <param name="category">The category of the value to get.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>
        /// The requested string from the category 'General' and with the specified <paramref name="key"/> constant.
        /// </returns>
        public string GeneralString(string category, string key)
        {
            IBuilderData value;
            this.TryGetValue(category, key, out value);
            var str = (StringConstantBuildParameter)value;
            return str.Value;
        }

        /// <summary>
        /// Adds a <see cref="NStub.CSharp.ObjectGeneration.Builders.StringConstantBuildParameter.Value"/> to the
        /// general category data set associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="value">The string value of the item.</param>
        /// <remarks>
        /// Already present items are skipped (not replaced) and no exception is thrown.
        /// </remarks>
        public void AddGeneralString(string key, string value)
        {
            this.AddDataItem(GeneralCategory, key, new StringConstantBuildParameter(value), false, true);
        }

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
        public void AddGeneralString(string category, string key, string value)
        {
            this.AddDataItem(category, key, new StringConstantBuildParameter(value), false, true);
        }

        /// <summary>
        /// Gets the value associated with the specified category and key.
        /// </summary>
        /// <param name="category">The requested category.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified
        /// key, if the key is found; otherwise, the default value for the type of the
        /// value parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        ///   <c>true</c> if the this instance contains an
        /// element with the specified category and key; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">key is <c>null</c>.</exception>
        public bool TryGetValue(string category, string key, out IBuilderData value)
        {
            IDictionary<string, IBuilderData> catLookup;
            var found = this.TryGetCategory(category, out catLookup);
            if (found)
            {
                return catLookup.TryGetValue(key, out value);
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.Values.GetEnumerator();
        }
    }
}