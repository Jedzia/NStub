namespace NStub.CSharp.Tests.FluentChecking
{
    using NStub.Core;

    public class CompareResult
    {

        private readonly bool result;
        private readonly string name;
        private readonly string comparer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="CompareResult"/> is result.
        /// </summary>
        /// <value>
        ///   <c>true</c> if result; otherwise, <c>false</c>.
        /// </value>
        public bool Result
        {
            get { return result; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the comparer.
        /// </summary>
        public string Comparer
        {
            get { return comparer; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareResult"/> class.
        /// </summary>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="name">The name.</param>
        /// <param name="comparer">The comparer.</param>
        public CompareResult(bool result, string name, string comparer)
        {
            Guard.NotNull(() => name, name);
            Guard.NotNull(() => comparer, comparer);
            this.result = result;
            this.name = name;
            this.comparer = comparer;
        }
    }
}