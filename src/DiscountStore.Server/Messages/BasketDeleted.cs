namespace DiscountStore.Server.Messages
{
    /// <summary>
    /// Message to delete a basket.
    /// </summary>
    public class BasketDeleted
    {
        /// <summary>
        /// Creates a new instance of <see cref="BasketDeleted"/>.
        /// </summary>
        /// <param name="id">Id of the basket to be deleted.</param>
        public BasketDeleted(int id)
        {
            Id = id;
            Message = "Basket deleted.";
        }

        /// <summary>
        /// Gets the message of a basket deleted.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the id of the basket to be deleted.
        /// </summary>
        public int Id { get; }
    }
}