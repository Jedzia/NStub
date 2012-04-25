// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuilderData.cs" company="EvePanix">
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
    using NStub.Core;

    /// <summary>
    /// Stores information for an <see cref="IMemberBuilder"/>. 
    /// </summary>
    /// <typeparam name="T">The type of the stored data.</typeparam>
    public class BuilderData<T> : /*Nullable<T>,*/ IBuilderData
    {
        #region Fields

        private T data;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BuilderData&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        public BuilderData(T dataObject)
        {
            Guard.NotNull(() => dataObject, dataObject);
            this.data = dataObject;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the data of this instance.
        /// </summary>
        public T Data
        {
            get
            {
                return this.data;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the information of this instance is complete. 
        /// For this generic type, this means, that a data object is present.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplete
        {
            get
            {
                return data != null;
            }
        }

        #endregion

        /// <summary>
        /// Gets the data of this instance.
        /// </summary>
        /// <returns>
        /// The stored data.
        /// </returns>
        public object GetData()
        {
            return this.data;
        }

        /// <summary>
        /// Determines whether this instance holds data for the specified builder type. Is always true. (see. Todo below)
        /// </summary>
        /// <param name="builder">The requesting builder.</param>
        /// <returns>
        ///   <c>true</c> if this instance holds data for the specified builder type; otherwise, <c>false</c>.
        /// </returns>
        public bool HasDataForType(IMemberBuilder builder)
        {
            // Todo: maybe request the builder for the use of this type of data.
            return true;
        }

        /// <summary>
        /// Sets the data via the specified method info.
        /// </summary>
        /// <param name="data">The data to store.</param>
        public void SetData(object data)
        {
            Guard.CanBeAssignedTo<T>(() => data, data);
            this.data = (T)data;
        }
    }
}