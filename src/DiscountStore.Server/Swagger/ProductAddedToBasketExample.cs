using DiscountStore.Server.Messages;
using Swashbuckle.AspNetCore.Filters;

namespace DiscountStore.Server.Swagger
{
    /// <summary>
    /// <see cref="IExamplesProvider"/> implementation to provide <see cref="ProductAddedToBasket"/> examples.
    /// </summary>
    public class ProductAddedToBasketExample : IExamplesProvider
    {
        /// <inheritdoc />
        public object GetExamples() => new ProductAddedToBasket(7);
    }
}