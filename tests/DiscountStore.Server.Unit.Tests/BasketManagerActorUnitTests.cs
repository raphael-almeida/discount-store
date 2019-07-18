using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.Xunit;
using DiscountStore.Server.Domain.Basket;
using DiscountStore.Server.Messages;
using FluentAssertions;
using Xunit;

namespace DiscountStore.Server.Unit.Tests
{
    public class BasketManagerActorUnitTests : TestKit
    {
        [Fact]
        public async Task BasketManagerActor_OnCreateNewBasket_ShouldRetrieveNewBasket()
        {
            // Arrange
            var accountActor = Sys.ActorOf(Props.Create(() => new BasketManagerActor()));

            // Act
            var response = await accountActor.Ask<CreateBasketResponse>(new CreateBasket());

            // Assert
            response.BasketId.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task BasketManagerActor_OnAddProduct_WithValidBasketId_ShouldAddProduct()
        {
            // Arrange
            var accountActor = Sys.ActorOf(Props.Create(() => new BasketManagerActor()));
            var response = await accountActor.Ask<CreateBasketResponse>(new CreateBasket());

            // Act
            var productAdded = await accountActor.Ask<ProductAddedToBasket>(new AddProduct("TSHIRT", 1, response.BasketId));

            // Assert
            productAdded.BasketId.Should().Be(response.BasketId);
            productAdded.Message.Should().Be("Product added to basket.");
        }

        [Fact]
        public async Task BasketManagerActor_OnAddProduct_WithValidBasketId_WithInvalidProductCode_ShouldReturnInvalidProduct()
        {
            // Arrange
            var accountActor = Sys.ActorOf(Props.Create(() => new BasketManagerActor()));
            var response = await accountActor.Ask<CreateBasketResponse>(new CreateBasket());
            var invalidProductCode = "INVALID_PRODUCT_CODE";

            // Act
            var productAdded = await accountActor.Ask<ProductNotFound>(new AddProduct(invalidProductCode, 1, response.BasketId));

            // Assert
            productAdded.Code.Should().Be(invalidProductCode);
            productAdded.Message.Should().Be($"Product {invalidProductCode} not found.");
        }

        [Fact]
        public async Task BasketManagerActor_OnAddProduct_WithInvalidBasketId_ShouldReturnBasketNotFound()
        {
            // Arrange
            var accountActor = Sys.ActorOf(Props.Create(() => new BasketManagerActor()));
            var id = -1;

            // Act
            var productAdded = await accountActor.Ask<BasketNotFound>(new AddProduct("TSHIRT", 1, id));

            // Assert
            productAdded.Id.Should().Be(id);
            productAdded.Message.Should().Be("Basket not found.");
        }

        [Theory]
        [InlineData("VOUCHER,TSHIRT,MUG", 32.50)]
        [InlineData("VOUCHER,TSHIRT,VOUCHER", 25.00)]
        [InlineData("TSHIRT,TSHIRT,TSHIRT,VOUCHER,TSHIRT", 81.00)]
        [InlineData("VOUCHER,TSHIRT,VOUCHER,VOUCHER,MUG,TSHIRT,TSHIRT", 74.50)]
        public async Task BasketManagerActor_OnGetTotalAmount_WithProucts_ShouldReturnTotalAmount(string productList, decimal expectedTotal)
        {
            // Arrange
            var accountActor = Sys.ActorOf(Props.Create(() => new BasketManagerActor()));
            var response = await accountActor.Ask<CreateBasketResponse>(new CreateBasket());

            var products = productList.Split(',');
            foreach (var product in products)
            {
                await accountActor.Ask<ProductAddedToBasket>(new AddProduct(product, 1, response.BasketId));
            }

            // Act
            var result = await accountActor.Ask<TotalAmountResponse>(new GetTotalAmountById(response.BasketId));

            // Assert
            var items = string.Join(',', result.Items);
            items.Should().Be(productList);
            result.Total.Should().Be(expectedTotal);
        }

        [Fact]
        public async Task BasketManagerActor_OnGetTotalAmount_WithInvalidBasketId_ShouldReturnBasketNotFound()
        {
            // Arrange
            var accountActor = Sys.ActorOf(Props.Create(() => new BasketManagerActor()));
            var id = -1;

            // Act
            var result = await accountActor.Ask<BasketNotFound>(new GetTotalAmountById(id));

            // Assert
            result.Id.Should().Be(id);
            result.Message.Should().Be("Basket not found.");
        }

        [Fact]
        public async Task BasketManagerActor_OnRemoveBasket_WithValidBasketId_ShouldRemoveBasket()
        {
            // Arrange
            var accountActor = Sys.ActorOf(Props.Create(() => new BasketManagerActor()));
            var response = await accountActor.Ask<CreateBasketResponse>(new CreateBasket());

            // Act
            var basketDeleted = await accountActor.Ask<BasketDeleted>(new RemoveBasket(response.BasketId));

            // Assert
            basketDeleted.Message.Should().Be("Basket deleted.");
            basketDeleted.Id.Should().Be(response.BasketId);
        }

        [Fact]
        public async Task BasketManagerActor_OnRemoveBasket_WithInvalidBasketId_ShouldReturnBasketNotFound()
        {
            // Arrange
            var accountActor = Sys.ActorOf(Props.Create(() => new BasketManagerActor()));
            var id = -1;

            // Act
            var result = await accountActor.Ask<BasketNotFound>(new RemoveBasket(id));

            // Assert
            result.Id.Should().Be(id);
            result.Message.Should().Be("Basket not found.");
        }
    }
}
