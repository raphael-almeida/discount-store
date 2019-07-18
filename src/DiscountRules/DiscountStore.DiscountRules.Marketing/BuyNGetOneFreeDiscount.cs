using System.Collections.Generic;
using System.Composition;
using DiscountStore.Server.Domain.Basket;
using DiscountStore.Server.Domain.Discount;

namespace DiscountStore.DiscountRules.BuyXGetYFree
{
    /// <summary>
    /// An implementation of <see cref="IDiscountRule"/> with the rules to apply a discount of type 'buy x get one free' where x is two in this case.
    /// </summary>
    [Export(typeof(IDiscountRule))]
    public class BuyNGetOneFreeDiscount : IDiscountRule
    {
        /// <inheritdoc />
        public void ApplyDiscount(IList<Product> products)
        {
            var discountProductCode = "VOUCHER";
            var discountMinimumAmount = 2;

            var count = 0;
            foreach (var product in products)
            {
                if (product.Code != discountProductCode)
                    continue;

                count++;
                if (count < discountMinimumAmount)
                    continue;

                product.Price = 0.0m;
                count = 0;
            }
        }
    }
}
