namespace DiscountStore.Server.Domain.Basket
{
    /// <summary>
    /// Represents a product to be used in a basket.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Creates a new instance of a <see cref="Product"/>.
        /// </summary>
        /// <param name="code">The product code.</param>
        /// <param name="name">The product name.</param>
        /// <param name="price">The product price.</param>
        public Product(string code, string name, decimal price)
        {
            Code = code;
            Name = name;
            Price = price;
        }

        /// <summary>
        /// Gets the product code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the product name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the product price.
        /// This property has the setter available so that discount rules can change the price of this <see cref="Product"/>.
        /// </summary>
        public decimal Price { get; set; }
    }
}