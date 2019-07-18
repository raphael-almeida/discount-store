using System.Threading.Tasks;
using Akka.Actor;
using DiscountStore.Server.Domain.Basket;
using DiscountStore.Server.Messages;
using DiscountStore.Server.Swagger;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DiscountStore.Server.Controllers
{
    /// <summary>
    /// Provide endpoints to handle requests to perform operations in a checkout basket.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public sealed class BasketController : ControllerBase
    {
        /// <summary>
        /// An instance of <see cref="IActorRef"/>.
        /// </summary>
        private readonly IActorRef _basketManagerActor;

        /// <summary>
        /// Creates a new instance of <see cref="BasketController"/>.
        /// </summary>
        /// <param name="basketManagerActorProvider"></param>
        public BasketController(BasketManagerActorProvider basketManagerActorProvider)
        {
            _basketManagerActor = basketManagerActorProvider();
        }

        /// <summary>
        /// Creates a new checkout basket.
        /// </summary>
        /// <returns>The id of the newly created basket.</returns>
        [HttpPost("")]
        [SwaggerResponse(200, Type = typeof(CreateBasketResponse), Description = "New basket successfully created with the returned id.")]
        [SwaggerResponseExample(200, typeof(CreateBasketResponseExample))]
        public async Task<ActionResult> CreateBasket()
        {
            var id = await _basketManagerActor.Ask(new CreateBasket());
            return StatusCode(200,id);
        }

        /// <summary>
        /// Puts a new product into a basket given by the basket id.
        /// </summary>
        /// <param name="basketId">Unique identifier of the basket to add the product.</param>
        /// <param name="addProduct">An object with details of the product to be added.</param>
        /// <returns>
        /// A <see cref="Task"/> wrapping an <see cref="ActionResult"/> with the result
        /// that can be: status code 404 for basket not found or product not found,
        /// or status code 200 for product successfully added to the given basket id.
        /// </returns>
        [HttpPut("{basketId}/addProduct")]
        [SwaggerRequestExample(typeof(AddProduct), typeof(AddProductExamples))]
        [SwaggerResponse(200, "Product added successfully to basket.", typeof(ProductAddedToBasket))]
        [SwaggerResponseExample(200, typeof(ProductAddedToBasketExample))]
        [SwaggerResponse(404, "Basket not found.", typeof(BasketNotFound))]
        [SwaggerResponseExample(404, typeof(BasketNotFoundExample))]
        [SwaggerResponse(404, "Product not found.", typeof(ProductNotFound))]
        [SwaggerResponseExample(404, typeof(ProductNotFoundExample))]
        public async Task<ActionResult> AddProduct(int basketId, [FromBody] AddProduct addProduct)
        {
            var result = await _basketManagerActor.Ask(addProduct);
            if(result is BasketNotFound basketNotFound)
                return StatusCode(404, basketNotFound);

            if (result is ProductNotFound productNotFound)
                return StatusCode(404, productNotFound);

            return StatusCode(200, result);
        }

        /// <summary>
        /// Gets the total amount of a checkout basket given by the basket id.
        /// </summary>
        /// <param name="basketId">Unique identifier of the basket to get the amount.</param>
        /// <returns>
        /// A <see cref="Task"/> wrapping an <see cref="ActionResult"/> with the result,
        /// which is a json object containing the items on the basket and the total amount in case of success,
        /// or error status code 404, if basket not found.
        /// </returns>
        [HttpGet("{basketId}/totalAmount")]
        [SwaggerRequestExample(typeof(int), typeof(GetTotalAmountByIdExamples))]
        [SwaggerResponse(200, "Total amount successfully retrieved.", typeof(TotalAmountResponse))]
        [SwaggerResponseExample(200, typeof(TotalAmountResponseExample))]
        [SwaggerResponse(404, "Basket not found.", typeof(BasketNotFound))]
        [SwaggerResponseExample(404, typeof(BasketNotFoundExample))]
        public async Task<ActionResult> GetTotalAmount(int basketId)
        {
            var totalAmount = await _basketManagerActor.Ask(new GetTotalAmountById(basketId));
            if (totalAmount is BasketNotFound basketNotFound)
                return StatusCode(404, basketNotFound);

            return StatusCode(200, totalAmount);
        }

        /// <summary>
        /// Deletes the basket given by its id.
        /// </summary>
        /// <param name="basketId">Unique identifier of the basket to be deleted.</param>
        /// <returns>
        /// A <see cref="Task"/> wrapping an <see cref="ActionResult"/> with the result,
        /// which is a json object containing basket deleted and a message in case of success,
        /// or error status code 404, if basket not found.
        /// </returns>
        [HttpDelete("{basketId}")]
        [SwaggerRequestExample(typeof(int), typeof(RemoveBasketExample))]
        [SwaggerResponse(200, "Basket deleted successfully.", typeof(BasketDeleted))]
        [SwaggerResponseExample(200, typeof(BasketDeletedExample))]
        [SwaggerResponse(404, "Basket not found.", typeof(BasketNotFound))]
        [SwaggerResponseExample(404, typeof(BasketNotFoundExample))]
        public async Task<ActionResult> RemoveBasket(int basketId)
        {
            var basketRemoved = await _basketManagerActor.Ask(new RemoveBasket(basketId));
            if (basketRemoved is BasketNotFound basketNotFound)
                return StatusCode(404, basketNotFound);

            return StatusCode(200, basketRemoved);
        }
    }
}

