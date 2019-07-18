namespace DiscountStore.Server.Messages
{
    /// <summary>
    /// Message to a basket created.
    /// </summary>
    public class CreateBasketResponse
    {
        /// <summary>
        /// Creates a new instance of <see cref="CreateBasketResponse"/>.
        /// </summary>
        /// <param name="basketId"></param>
        public CreateBasketResponse(int basketId)
        {
            BasketId = basketId;
            Message = $"Basket created successfully with id {basketId}.";
        }

        /// <summary>
        /// Gets the id of the basket created.
        /// </summary>
        public int BasketId { get; }

        /// <summary>
        /// Gets the text of the basket created.
        /// </summary>
        public string Message { get; }
    }
}