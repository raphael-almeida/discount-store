using DiscountStore.Server.Messages;
using Swashbuckle.AspNetCore.Filters;

namespace DiscountStore.Server.Swagger
{
    /// <summary>
    /// <see cref="IExamplesProvider"/> implementation to provide <see cref="CreateBasketResponse"/> examples.
    /// </summary>
    public class CreateBasketResponseExample : IExamplesProvider
    {
        /// <inheritdoc />
        public object GetExamples() => new CreateBasketResponse(7);
    }
}