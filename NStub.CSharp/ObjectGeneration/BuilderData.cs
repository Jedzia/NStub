namespace NStub.CSharp.ObjectGeneration
{
    /// <summary>
    /// Stores information for an IMemberBuilder. 
    /// </summary>
    /// <typeparam name="T">The type of the stored data.</typeparam>
    public class BuilderData<T> : IBuilderData
    {
        private readonly T data;

        /// <summary>
        /// Gets the data of this instance.
        /// </summary>
        public T Data
        {
            get { return data; }
        } 

        #region IBuilderData Members

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BuilderData"/> class.
        /// </summary>
        public BuilderData(T dataObject)
        {
            Guard.NotNull(() => dataObject, dataObject);
            this.data = dataObject;
        }

        /// <summary>
        /// Determines whether this instance holds data for the specified builder type.
        /// </summary>
        /// <param name="builder">The requesting builder.</param>
        /// <returns>
        ///   <c>true</c> if this instance holds data for the specified builder type; otherwise, <c>false</c>.
        /// </returns>
        public bool HasDataForType(IMemberBuilder builder)
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the information of this instance is complete.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is complete; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplete
        {
            get { return true; }
        }

        /// <summary>
        /// Sets the data via the specified method info.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        public void SetData(System.Reflection.MethodInfo methodInfo)
        {
        }

        #endregion

        #region IBuilderData Members


        /// <summary>
        /// Gets the data of this instance.
        /// </summary>
        /// <returns>
        /// The stored data.
        /// </returns>
        public object GetData()
        {
            return data;
        }

        #endregion
    }
}