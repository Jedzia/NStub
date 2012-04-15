namespace NStub.CSharp.Tests.FluentChecking
{
    public class FluentCodeWrapper<T, K>
    {
        private readonly T initialExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTypeReferenceBinder"/> class.
        /// </summary>
        /// <param name="method">The method to add a CodeTypeReference to.</param>
        /// <param name="reference">The reference to add.</param>
        internal FluentCodeWrapper(T initialExpression)
        {
            Guard.NotNull(() => initialExpression, initialExpression);
            this.initialExpression = initialExpression;
        }
    }
}