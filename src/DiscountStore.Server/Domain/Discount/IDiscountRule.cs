using System.Collections.Generic;
using DiscountStore.Server.Domain.Basket;

namespace DiscountStore.Server.Domain.Discount
{
    /// <summary>
    /// Interface to be implemented on Discount Rule Assemblies.
    /// </summary>
    public interface IDiscountRule
    {
        /// <summary>
        /// Applies the discount of a specific discount rule in the list of products.
        /// </summary>
        /// <param name="products">The list of products to apply the discount on.</param>
        void ApplyDiscount(IList<Product> products);
    }
}