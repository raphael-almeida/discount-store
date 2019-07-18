namespace DiscountStore.Server.Messages
{
    /// <summary>
    /// Message of a product not found.
    /// </summary>
    public class ProductNotFound
    {
        /// <summary>
        /// Creates a new instance of <see cref="ProductNotFound"/>.
        /// </summary>
        /// <param name="code">Code of the product not found.</param>
        public ProductNotFound(string code)
        {
            Code = code;
            Message = $"Product {code} not found.";
        }

        /// <summary>
        /// Gets the code of the product not found.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the text message of a product not found.
        /// </summary>
        public string Message { get; }
        
    }
}