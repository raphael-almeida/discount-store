using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Xunit;

namespace DiscountStore.Server.Integration.Tests
{
    public class BasketIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string ApplicationJsonContentType = "application/json";
        private Encoding Encoding { get; } = Encoding.UTF8;

        public BasketIntegrationTests(WebApplicationFactory<Startup> factory) => Factory = factory;

        private WebApplicationFactory<Startup> Factory { get; }

        [Fact]
        public async Task CreateBasket_ShouldReturnNewId()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/basket", null);

            // Assert
            var id = ParseBasketId(response);
            id.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task AddProduct_WithValidProductCode_WithValidBasketId_ShouldReturnSuccess()
        {
            // Arrange
            var client = Factory.CreateClient();
            var createBasketResponse = await client.PostAsync("/api/basket", null);
            var basketId = ParseBasketId(createBasketResponse);
            var product = new { ProductCode = "TSHIRT", Quantity = 1, BasketId = basketId };
            var productJson = JObject.FromObject(product);
            var addProductContent = new StringContent(productJson.ToString(), Encoding, ApplicationJsonContentType);

            // Act
            var response = await client.PutAsync($"/api/basket/{basketId}/addProduct", addProductContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(content);
            responseJson["message"].Value<string>().Should().Be("Product added to basket.");
            responseJson["basketId"].Value<int>().Should().Be(basketId);
        }

        [Fact]
        public async Task AddProduct_WithInvalidBasketId_ShouldReturnBasketNotFound()
        {
            // Arrange
            var client = Factory.CreateClient();
            var basketId = -1;
            var product = new { ProductCode = "TSHIRT", Quantity = 1, BasketId = basketId };
            var productJson = JObject.FromObject(product);
            var addProductContent = new StringContent(productJson.ToString(), Encoding, ApplicationJsonContentType);
            
            // Act
            var response = await client.PutAsync($"/api/basket/{basketId}/addProduct", addProductContent);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(content);
            responseJson["message"].Value<string>().Should().Be("Basket not found.");
            responseJson["id"].Value<int>().Should().Be(basketId);
        }

        [Fact]
        public async Task AddProduct_WithValidBasketId_WithInvalidProductCode_ShouldReturnProductCodeNotFound()
        {
            // Arrange
            var client = Factory.CreateClient();
            var createBasketResponse = await client.PostAsync("/api/basket", null);
            var basketId = ParseBasketId(createBasketResponse);
            
            var productCode = "INVALID_PRODUCT_CODE";
            var product = new { ProductCode = productCode, Quantity = 1, BasketId = basketId };
            var productJson = JObject.FromObject(product);
            var addProductContent = new StringContent(productJson.ToString(), Encoding, ApplicationJsonContentType);
            
            // Act
            var response = await client.PutAsync($"/api/basket/{basketId}/addProduct", addProductContent);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(content);
            responseJson["message"].Value<string>().Should().Be($"Product {productCode} not found.");
            responseJson["code"].Value<string>().Should().Be(productCode);
        }

        [Theory]
        [InlineData("VOUCHER,TSHIRT,MUG", 32.50)]
        [InlineData("VOUCHER,TSHIRT,VOUCHER", 25.00)]
        [InlineData("TSHIRT,TSHIRT,TSHIRT,VOUCHER,TSHIRT", 81.00)]
        [InlineData("VOUCHER,TSHIRT,VOUCHER,VOUCHER,MUG,TSHIRT,TSHIRT", 74.50)]
        public async Task GetTotalAmount_ShouldReturnTotalAmountWithDiscount(string productList, decimal expectedTotalAmount)
        {
            // Arrange
            var client = Factory.CreateClient();
            var createBasketResponse = await client.PostAsync("/api/basket", null);
            var basketId = ParseBasketId(createBasketResponse);

            var products = productList.Split(',');
            foreach (var product in products)
            {
                var productObj = new { ProductCode = product, Quantity = 1, BasketId = basketId };
                var productJson = JObject.FromObject(productObj);
                var addProductContent = new StringContent(productJson.ToString(), Encoding, ApplicationJsonContentType);
                await client.PutAsync($"/api/basket/{basketId}/addProduct", addProductContent);
            }

            // Act
            var response = await client.GetAsync($"/api/basket/{basketId}/totalAmount");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(content);
            var items = responseJson["items"].Values<string>();
            var itemsList = string.Join(',', items);
            itemsList.Should().Be(productList);
            responseJson["total"].Value<decimal>().Should().Be(expectedTotalAmount);
        }

        [Fact]
        public async Task GetTotalAmount_WithInvalidBasketId_ShouldReturnBasketNotFound()
        {
            // Arrange 
            var client = Factory.CreateClient();
            var basketId = -1;

            // Act
            var response = await client.GetAsync($"/api/basket/{basketId}/totalAmount");

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(content);
            responseJson["message"].Value<string>().Should().Be("Basket not found.");
            responseJson["id"].Value<int>().Should().Be(basketId);
        }

        [Fact]
        public async Task RemoveBasket_WithValidBasketId_ShouldRemoveBasket()
        {
            // Arrange 
            var client = Factory.CreateClient();
            var createBasketResponse = await client.PostAsync("/api/basket", null);
            var basketId = ParseBasketId(createBasketResponse);

            // Act
            var response = await client.DeleteAsync($"/api/basket/{basketId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(content);
            responseJson["message"].Value<string>().Should().Be("Basket deleted.");
            responseJson["id"].Value<int>().Should().Be(basketId);
        }

        [Fact]
        public async Task RemoveBasket_WithInvalidBasketId_ShouldReturnBasketNotFound()
        {
            // Arrange 
            var client = Factory.CreateClient();
            var basketId = -1;

            // Act
            var response = await client.DeleteAsync($"/api/basket/{basketId}");

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(content);
            responseJson["message"].Value<string>().Should().Be("Basket not found.");
            responseJson["id"].Value<int>().Should().Be(basketId);
        }

        private int ParseBasketId(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync();
            var contentObj = JObject.Parse(content.Result);
            return contentObj["basketId"].Value<int>();
        }
    }
}
