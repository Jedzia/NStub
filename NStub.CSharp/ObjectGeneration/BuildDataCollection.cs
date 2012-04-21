// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildDataCollection.cs" company="EvePanix">
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
    using System.Collections.ObjectModel;

    public interface IReadOnlyDictionary<TKey, TValue> : IEnumerable
    {
        bool ContainsKey(TKey key);
        ICollection<TKey> Keys { get; }
        ICollection<TValue> Values { get; }
        int Count { get; }
        bool TryGetValue(TKey key, out TValue value);
        TValue this[TKey key] { get; }
        bool Contains(KeyValuePair<TKey, TValue> item);
        void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex);
        IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();
    }

    public class ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        readonly IDictionary<TKey, TValue> _dictionary;
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }
        public bool ContainsKey(TKey key) { return _dictionary.ContainsKey(key); }
        public ICollection<TKey> Keys { get { return _dictionary.Keys; } }
        public bool TryGetValue(TKey key, out TValue value) { return _dictionary.TryGetValue(key, out value); }
        public ICollection<TValue> Values { get { return _dictionary.Values; } }
        public TValue this[TKey key] { get { return _dictionary[key]; } }
        public bool Contains(KeyValuePair<TKey, TValue> item) { return _dictionary.Contains(item); }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) { _dictionary.CopyTo(array, arrayIndex); }
        public int Count { get { return _dictionary.Count; } }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() { return _dictionary.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _dictionary.GetEnumerator(); }
    }

    public interface IBuildDataReadOnlyCollection : IReadOnlyDictionary<string, IReadOnlyDictionary<string, IBuilderData>>
    {
    }

    public class BuildDataReadOnlyCollection : ReadOnlyDictionary<string, IReadOnlyDictionary<string, IBuilderData>>, IBuildDataReadOnlyCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:FCKW"/> class.
        /// </summary>
        public BuildDataReadOnlyCollection(IDictionary<string, IReadOnlyDictionary<string, IBuilderData>> root)
            : base(root)
        {
        }
    }


    //public class FJAJKSDAK : IReadOnlyDictionary<string, IReadOnlyDictionary<string, IBuilderData>>
    //{
    //}

    /// <summary>
    /// Bundles a list of <see cref="IBuilderData"/> organized as a dictionary of strings, categorized into main categories.
    /// </summary>
    public interface IBuildDataCollection : IEnumerable<IBuilderData>
    {
        IReadOnlyDictionary<string, IBuilderData> General { get; }

        /// <summary>
        /// Adds the specified data item to the "General" topic of this list.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        void AddDataItem(string key, IBuilderData item);

        /// <summary>
        /// Adds the specified data item to the specified category of this list.
        /// </summary>
        /// <param name="category">The category of the item.</param>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        void AddDataItem(string category, string key, IBuilderData item);

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

        /// <summary>
        /// Returns an enumerator that iterates through the category collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerable<KeyValuePair<string, IDictionary<string, IBuilderData>>> GetData();

        IBuildDataReadOnlyCollection Data();
    }

    /// <summary>
    /// List of <see cref="IBuilderData"/> organized as a dictionary of strings, categorized into main categories.
    /// </summary>
    public class BuildDataCollection : IBuildDataCollection
    {
        #region Fields

        private readonly Dictionary<string, IDictionary<string, IBuilderData>> data =
            new Dictionary<string, IDictionary<string, IBuilderData>>();

        private readonly Dictionary<string, IBuilderData> generalData;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildDataCollection"/> class.
        /// </summary>
        public BuildDataCollection()
        {
            this.generalData = new Dictionary<string, IBuilderData>();
            this.data.Add("General", this.generalData);
        }

        #endregion

        #region Properties

        public IReadOnlyDictionary<string, IBuilderData> General
        {
            get
            {
                return new ReadOnlyDictionary<string, IBuilderData>(this.generalData);
            }
        }

        /// <summary>
        /// Gets the number of data items.
        /// </summary>
        public int Count
        {
            get
            {
                return this.data.Count;
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
        }

        /// <summary>
        /// Adds the specified data item to the specified category of this list.
        /// </summary>
        /// <param name="category">The category of the item.</param>
        /// <param name="key">The key of the item.</param>
        /// <param name="item">The data item to add.</param>
        public void AddDataItem(string category, string key, IBuilderData item)
        {
            IDictionary<string, IBuilderData> catLookup;
            var found = this.TryGetCategory(category, out catLookup);
            if (!found)
            {
                catLookup = new Dictionary<string, IBuilderData>();
                this.data.Add(category, catLookup);
            }

            catLookup.Add(key, item);
        }

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

        /// <summary>
        /// Returns an enumerator that iterates through the category collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerable<KeyValuePair<string, IDictionary<string, IBuilderData>>> GetData()
        {
            return this.data;
        }

        //public abstract class mapCollection<T,K> : Gallio.Common.Collections.ReadOnlyDictionary<T,K>, ICollection<T> //where T : new()
        //{
        //}

        public IEnumerable<KeyValuePair<string, IDictionary<string, IBuilderData>>> TestGetData()
        {
            //public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
            //public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
            //IEnumerable<KeyValuePair<string, IDictionary<string, IBuilderData>>>
            //ICollection<KeyValuePair<string, IDictionary<string, IBuilderData>>>
            //this.data.

            //IDictionary<string, IBuilderData> moese = this.data["adf"];
            IEnumerable<KeyValuePair<string, IBuilderData>> moese = this.data["adf"];



            //IEnumerable<KeyValuePair<string, ICollection<KeyValuePair<string, IBuilderData>>>> aaaaa = this.data;

            //var moese =                this.data["adf"];




            return this.data;
        }

        //public IReadOnlyDictionary<string, IReadOnlyDictionary<string, IBuilderData>> Moep()
        public IBuildDataReadOnlyCollection Data()
        {

            var rlist = new Dictionary<string, IReadOnlyDictionary<string, IBuilderData>>();
            foreach (var item in this.data)
            {
                var sub = new ReadOnlyDictionary<string, IBuilderData>(item.Value);
                rlist.Add(item.Key, sub);
            }
            var xxxx = new BuildDataReadOnlyCollection(rlist);
            // var ro = new ReadOnlyDictionary<string, IReadOnlyDictionary<string, IBuilderData>>(rlist);
            return xxxx;
        }

        /// <summary>
        /// Gets the category associated with the specified key.
        /// </summary>
        /// <param name="category">The key of the value to get.</param>
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
        /// Gets the value associated with the specified key.
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