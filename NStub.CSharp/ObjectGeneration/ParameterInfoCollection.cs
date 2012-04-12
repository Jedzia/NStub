// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterInfoCollection.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.CSharp.ObjectGeneration
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// List of <see cref="ParameterInfo"/>'s with Interface detection for thetypes of the added parameters.
    /// </summary>
    internal class ParameterInfoCollection : IEnumerable<ParameterInfo>
    {
        #region Fields

        private readonly List<ParameterInfo> parameters = new List<ParameterInfo>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of parameters.
        /// </summary>
        public int Count
        {
            get
            {
                return this.parameters.Count;
            }
        }

        /// <summary>
        /// Gets the number of parameters with standard arguments. ( Currently all but interfaces ).
        /// </summary>
        public int CountOfStandardTypes
        {
            get
            {
                return this.parameters.Count(e => !e.ParameterType.IsInterface);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has parameters of a interface type stored.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has interfaces in the parameter types; otherwise, <c>false</c>.
        /// </value>
        public bool HasInterfaces { get; private set; }

        /// <summary>
        /// Gets the parameters where the parameter-type isn't an interface.
        /// </summary>
        public IEnumerable<ParameterInfo> StandardTypes
        {
            get
            {
                return this.parameters.Where(e => !e.ParameterType.IsInterface);
            }
        }

        #endregion

        /// <summary>
        /// Adds the specified parameter info to this list.
        /// </summary>
        /// <param name="parameterInfo">The parameter info to add.</param>
        public void AddParameterInfo(ParameterInfo parameterInfo)
        {
            this.parameters.Add(parameterInfo);
            if (parameterInfo.ParameterType.IsInterface)
            {
                this.HasInterfaces = true;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ParameterInfo> GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }
    }
}