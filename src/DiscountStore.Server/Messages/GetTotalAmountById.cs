namespace DiscountStore.Server.Messages
{
    /// <summary>
    /// Message to get a total amount by id.
    /// </summary>
    public class GetTotalAmountById
    {
        /// <summary>
        /// Creates a new instance of <see cref="GetTotalAmountById"/> message.
        /// </summary>
        /// <param name="basketId">The id of the basket to get the total amount.</param>
        public GetTotalAmountById(int basketId)
        {
            BasketId = basketId;
        }

        /// <summary>
        /// Gets the id of the basket to get the total amount from.
        /// </summary>
        public int BasketId { get; }
    }
}