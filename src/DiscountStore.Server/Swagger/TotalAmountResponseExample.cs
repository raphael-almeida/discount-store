using DiscountStore.Server.Messages;
using Swashbuckle.AspNetCore.Filters;

namespace DiscountStore.Server.Swagger
{
    /// <summary>
    /// <see cref="IExamplesProvider"/> implementation to provide <see cref="TotalAmountResponse"/> examples.
    /// </summary>
    public class TotalAmountResponseExample : IExamplesProvider
    {
        /// <inheritdoc />
        public object GetExamples() => 
            new TotalAmountResponse(new[] { "VOUCHER", "TSHIRT", "VOUCHER" }, 25.0m);
    }
}