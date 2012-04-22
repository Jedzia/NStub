// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBuilderData.cs" company="EvePanix">
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
    using System.Reflection;
    using NStub.Core;

    /// <summary>
    /// Stores information about a test method for a property.
    /// </summary>
    internal class PropertyBuilderData : IBuilderData
    {
        #region Fields

        private MethodInfo getAccessor;
        private MethodInfo setAccessor;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the get accessor.
        /// </summary>
        /// <value>
        /// The get accessor.
        /// </value>
        public MethodInfo GetAccessor
        {
            get
            {
                return this.getAccessor;
            }

            set
            {
                this.getAccessor = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the information of this instance is complete.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplete
        {
            get
            {
                return this.setAccessor != null && this.getAccessor != null;
            }
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        /// <exception cref="InvalidOperationException">Can't get the property name if both, <see cref="GetAccessor"/>
        /// and <see cref="SetAccessor"/>, aren't set.</exception>
        public string PropertyName
        {
            get
            {
                if (this.getAccessor != null)
                {
                    return this.getAccessor.Name.Replace("get_", string.Empty);
                }

                if (this.setAccessor != null)
                {
                    return this.setAccessor.Name.Replace("set_", string.Empty);
                }

                throw new InvalidOperationException(
                    "Can't get the property name if both, GetAccessor and SetAccessor, aren't set.");
            }
        }

        /// <summary>
        /// Gets or sets the set accessor.
        /// </summary>
        /// <value>
        /// The set accessor.
        /// </value>
        public MethodInfo SetAccessor
        {
            get
            {
                return this.setAccessor;
            }

            set
            {
                this.setAccessor = value;
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
            return this;
        }

        /// <summary>
        /// Determines whether this instance holds data for the specified builder type.
        /// </summary>
        /// <param name="builder">The requesting builder.</param>
        /// <returns>
        /// <c>true</c> if this instance holds data for the specified builder type; otherwise, <c>false</c>.
        /// </returns>
        public bool HasDataForType(IMemberBuilder builder)
        {
            return builder is PropertyBuilder;
        }

        /// <summary>
        /// Sets the data via the specified method info.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        public void SetData(object data)
        {
            Guard.CanBeAssignedTo<MethodInfo>(() => data, data);
            this.SetViaAccessorName((MethodInfo)data);
        }

        /// <summary>
        /// Sets the data of the <see cref="GetAccessor"/> and <see cref="SetAccessor"/> via the name 
        /// of the specified <paramref name="methodInfo"/>.
        /// </summary>
        /// <param name="methodInfo">The info about the property accessor.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c><paramref name="methodInfo"/></c> 
        /// The specified <see cref="MethodInfo"/> is not from a property getter or setter.</exception>
        public void SetViaAccessorName(MethodInfo methodInfo)
        {
            if (methodInfo.Name.StartsWith("get_"))
            {
                this.getAccessor = methodInfo;
            }
            else if (methodInfo.Name.StartsWith("set_"))
            {
                this.setAccessor = methodInfo;
            }
            else
            {
                throw new ArgumentOutOfRangeException("methodInfo", "The specified MethodInfo is not from a property getter or setter.");
            }
        }
    }
}