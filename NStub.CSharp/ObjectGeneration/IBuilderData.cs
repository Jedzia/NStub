using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NStub.CSharp.ObjectGeneration
{
    /// <summary>
    /// Stores information about a test method for an IMemberBuilder.
    /// </summary>
    public interface IBuilderData
    {
        /// <summary>
        /// Determines whether this instance holds data for the specified builder type.
        /// </summary>
        /// <param name="builder">The requesting builder.</param>
        /// <returns>
        ///   <c>true</c> if this instance holds data for the specified builder type; otherwise, <c>false</c>.
        /// </returns>
        bool HasDataForType(IMemberBuilder builder);

        /// <summary>
        /// Gets a value indicating whether the information of this instance is complete.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        bool IsComplete { get; }

        /// <summary>
        /// Sets the data via the specified method info.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        void SetData(MethodInfo methodInfo);
    }

    /// <summary>
    /// Stores information about a test method for a property.
    /// </summary>
    internal class PropertyBuilderData : IBuilderData
    {
        public bool HasDataForType(IMemberBuilder builder)
        {
            return builder is PropertyBuilder;
        }

        private MethodInfo getAccessor;

        /// <summary>
        /// Gets or sets the get accessor.
        /// </summary>
        /// <value>
        /// The get accessor.
        /// </value>
        public MethodInfo GetAccessor
        {
            get { return getAccessor; }
            set { getAccessor = value; }
        }


        private MethodInfo setAccessor;

        /// <summary>
        /// Gets or sets the set accessor.
        /// </summary>
        /// <value>
        /// The set accessor.
        /// </value>
        public MethodInfo SetAccessor
        {
            get { return setAccessor; }
            set { setAccessor = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the information of this instance is complete.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplete
        {
            get { return setAccessor != null && getAccessor != null; }
        }

        /// <summary>
        /// Sets the data of the <see cref="GetAccessor"/> and <see cref="SetAccessor"/> via the name 
        /// of the specified <paramref name="methodInfo"/>.
        /// </summary>
        /// <param name="methodInfo">The info about the property accessor.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c><paramref name="methodInfo"/></c> 
        /// The specified MethodInfo is not from a property getter or setter.</exception>
        public void SetViaAccessorName(MethodInfo methodInfo)
        {
            if (methodInfo.Name.StartsWith("get_"))
            {
                getAccessor = methodInfo;
            }
            else if (methodInfo.Name.StartsWith("set_"))
            {
                setAccessor = methodInfo;
            }
            else
            {
                throw new ArgumentOutOfRangeException("The specified MethodInfo is not from a property getter or setter.", "methodInfo");
            }
        }


        #region IBuilderData Members


        public void SetData(MethodInfo methodInfo)
        {
            this.SetViaAccessorName(methodInfo);
        }

        #endregion
    }
}
