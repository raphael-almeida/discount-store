namespace DiscountStore.Server.Messages
{
    /// <summary>
    /// Message to remove a basket.
    /// </summary>
    public class RemoveBasket
    {
        /// <summary>
        /// Creates a new instance of <see cref="RemoveBasket"/>.
        /// </summary>
        /// <param name="id">The id of the basket to be removed.</param>
        public RemoveBasket(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the id of the basket to be removed.
        /// </summary>
        public int Id { get; }
    }
}