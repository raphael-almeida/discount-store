namespace DiscountStore.Server.Messages
{
    /// <summary>
    /// Message to inform that a product has been added to basket.
    /// </summary>
    public class ProductAddedToBasket
    {
        /// <summary>
        /// Creates a new instance of <see cref="ProductAddedToBasket"/>
        /// </summary>
        /// <param name="basketId">Basket unique identification</param>
        public ProductAddedToBasket(int basketId)
        {
            BasketId = basketId;
            Message = "Product added to basket.";
        }

        /// <summary>
        /// Gets the text message of a product added to the basket.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the basket identification.
        /// </summary>
        public int BasketId { get; }
    }
}