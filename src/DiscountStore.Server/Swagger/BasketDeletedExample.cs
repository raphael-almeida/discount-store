using DiscountStore.Server.Messages;
using Swashbuckle.AspNetCore.Filters;

namespace DiscountStore.Server.Swagger
{
    /// <summary>
    /// <see cref="IExamplesProvider"/> implementation to provide <see cref="BasketDeleted"/> examples.
    /// </summary>
    public class BasketDeletedExample : IExamplesProvider
    {
        /// <inheritdoc />
        public object GetExamples() => new BasketDeleted(7);
    }
}