using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Akka.Util.Internal;
using DiscountStore.Server.Domain.Basket;
using DiscountStore.Server.Domain.Discount;

namespace DiscountStore.DiscountRules.BulkPurchase
{
    /// <summary>
    /// An implementation of <see cref="IDiscountRule"/> with the rules to apply the discount in case of a bulk purchase of TSHIRT products.
    /// </summary>
    [Export(typeof(IDiscountRule))]
    public class DiscountOnBulkPurchase : IDiscountRule
    {
        /// <inheritdoc />
        public void ApplyDiscount(IList<Product> products)
        {
            var discountProductCode = "TSHIRT";
            var discountMinimumAmount = 3;
            var newPrice = 19.0m;

            if (products.Count(product => 
                    product.Code == discountProductCode) >= discountMinimumAmount)
                products.Where(product => 
                    product.Code == discountProductCode).ForEach(product => product.Price = newPrice);
        }
    }
}
