namespace DiscountStore.Server.Messages
{
    /// <summary>
    /// Message of a basket not found.
    /// </summary>
    public class BasketNotFound
    {
        /// <summary>
        /// Creates a new instance of <see cref="BasketNotFound"/>.
        /// </summary>
        /// <param name="id"></param>
        public BasketNotFound(int id)
        {
            Id = id;
            Message = "Basket not found.";
        }

        /// <summary>
        /// Gets a text of a basket not found.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the id of a basket not found.
        /// </summary>
        public int Id { get; }
    }
}