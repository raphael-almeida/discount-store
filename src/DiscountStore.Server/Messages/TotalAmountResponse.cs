using System.Collections.Generic;

namespace DiscountStore.Server.Messages
{
    /// <summary>
    /// Message to get the total amount response.
    /// </summary>
    public class TotalAmountResponse
    {
        /// <summary>
        /// Creates a new instance of <see cref="TotalAmountResponse"/>
        /// </summary>
        /// <param name="items">List of items (products) of a basket.</param>
        /// <param name="total">Total amount of a basket.</param>
        public TotalAmountResponse(IEnumerable<string> items, decimal total)
        {
            Items = items;
            Total = total;
        }

        /// <summary>
        /// Gets the list of items of the basket.
        /// </summary>
        public IEnumerable<string> Items { get; }

        /// <summary>
        /// Gets the total amount of the basket.
        /// </summary>
        public decimal Total { get; }
    }
}