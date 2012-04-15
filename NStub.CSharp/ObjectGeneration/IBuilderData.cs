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

        /// <summary>
        /// Gets the data of this instance.
        /// </summary>
        /// <returns>The stored data.</returns>
        object GetData();
    }
}
