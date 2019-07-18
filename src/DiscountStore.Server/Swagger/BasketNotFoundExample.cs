using DiscountStore.Server.Messages;
using Swashbuckle.AspNetCore.Filters;

namespace DiscountStore.Server.Swagger
{
    /// <summary>
    /// <see cref="IExamplesProvider"/> implementation to provide <see cref="BasketNotFound"/> examples.
    /// </summary>
    public class BasketNotFoundExample : IExamplesProvider
    {
        /// <inheritdoc />
        public object GetExamples() => new BasketNotFound(9);
    }
}