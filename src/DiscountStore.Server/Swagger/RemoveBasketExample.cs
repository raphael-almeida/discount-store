using DiscountStore.Server.Messages;
using Swashbuckle.AspNetCore.Filters;

namespace DiscountStore.Server.Swagger
{
    /// <summary>
    /// <see cref="IExamplesProvider"/> implementation to porvide <see cref="RemoveBasket"/> examples.
    /// </summary>
    public class RemoveBasketExample : IExamplesProvider
    {
        /// <inheritdoc />
        public object GetExamples() => 7;
    }
}