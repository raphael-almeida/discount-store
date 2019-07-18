using DiscountStore.Server.Messages;
using Swashbuckle.AspNetCore.Filters;

namespace DiscountStore.Server.Swagger
{
    /// <summary>
    /// <see cref="IExamplesProvider"/> implementation to provide <see cref="ProductNotFound"/> examples.
    /// </summary>
    public class ProductNotFoundExample : IExamplesProvider
    {
        /// <inheritdoc />
        public object GetExamples() => new ProductNotFound("PURSE");
    }
}