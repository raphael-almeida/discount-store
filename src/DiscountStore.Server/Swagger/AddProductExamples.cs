using DiscountStore.Server.Messages;
using Swashbuckle.AspNetCore.Filters;

namespace DiscountStore.Server.Swagger
{
    /// <summary>
    /// <see cref="IExamplesProvider"/> implementation to provide <see cref="AddProduct"/> examples.
    /// </summary>
    public class AddProductExamples : IExamplesProvider
    {
        /// <inheritdoc />
        public object GetExamples() => new AddProduct("TSHIRT", 2, 7);
    }
}
