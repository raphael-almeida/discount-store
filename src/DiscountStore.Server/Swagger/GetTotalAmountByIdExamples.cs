using DiscountStore.Server.Messages;
using Swashbuckle.AspNetCore.Filters;

namespace DiscountStore.Server.Swagger
{
    /// <summary>
    /// <see cref="IExamplesProvider"/> implementation to provide <see cref="GetTotalAmountById"/> examples.
    /// </summary>
    public class GetTotalAmountByIdExamples : IExamplesProvider
    {
        /// <inheritdoc />
        public object GetExamples() => 7;
    }
}