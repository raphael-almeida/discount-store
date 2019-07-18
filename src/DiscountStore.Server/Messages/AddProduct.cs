namespace DiscountStore.Server.Messages
{
    /// <summary>
    /// Message to add a new product to a basket.
    /// </summary>
    public class AddProduct
    {
        /// <summary>
        /// Creates a new instance of <see cref="AddProduct"/> message.
        /// </summary>
        /// <param name="productCode">The code of the product to be added.</param>
        /// <param name="quantity">The quantity of the product to be added</param>
        /// <param name="basketId">The basket Id of the product to be added.</param>
        public AddProduct(string productCode, int quantity, int basketId)
        {
            ProductCode = productCode;
            Quantity = quantity;
            BasketId = basketId;
        }

        /// <summary>
        /// Gets the quantity of the product to be added.
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        /// Gets the basket id of the product to be added.
        /// </summary>
        public int BasketId { get;  }

        /// <summary>
        /// Gets the product code to be added.
        /// </summary>
        public string ProductCode { get; }
    }
}