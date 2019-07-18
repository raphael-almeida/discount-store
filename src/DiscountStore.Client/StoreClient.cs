using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace DiscountStore.Client
{
    /// <summary>
    /// Handles requests to Discount Store endpoitns.
    /// </summary>
    public class StoreClient
    {
        /// <summary>
        /// <see cref="HttpClient"/> instance.
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// <see cref="Lazy{T}"/> of <see cref="StoreClient"/> object that is a singleton object of <see cref="StoreClient"/>.
        /// </summary>
        private static readonly Lazy<StoreClient> StoreClientLazy = new Lazy<StoreClient>(() => new StoreClient());
        

        /// <summary>
        /// Creates a new instance of <see cref="StoreClient"/>
        /// </summary>
        private StoreClient()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Gets the value of <see cref="StoreClientLazy"/>.
        /// </summary>
        public static StoreClient Instance => StoreClientLazy.Value;

        /// <summary>
        /// Performs the http request to create a new basket.
        /// </summary>
        /// <returns>The basket id of the newly created basket.</returns>
        public int CreateBasket()
        {
            var response = _client.PostAsync("/api/basket", null).Result;
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Unable to create basket.");

            return ParseBasketId(response);
        }

        /// <summary>
        /// Performs the http request to add a new product to a basket.
        /// </summary>
        /// <param name="basketId">The basket id to add the product to.</param>
        /// <param name="product">An object containing product details.</param>
        public void AddProductToBasket(int basketId, object product)
        {
            var productJson = JObject.FromObject(product);
            var content = new StringContent(productJson.ToString(), Encoding.UTF8, "application/json");
            var response = _client.PutAsync($"/api/basket/{basketId}/addProduct", content).Result;
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Unable to add product.");
        }

        /// <summary>
        /// Performs the http request to get the amout details of a basket.
        /// </summary>
        /// <param name="basketId">The basket id to get the amount details.</param>
        /// <returns>A <see cref="JObject"/> instance with details of basket (amount and items).</returns>
        public JObject GetTotalAmount(int basketId)
        {
            var response = _client.GetAsync($"/api/basket/{basketId}/totalAmount").Result;
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Unable to get total amount.");

            return JObject.Parse(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Performs the http request to delete a basket.
        /// </summary>
        /// <param name="basketId">The id of the basket to be deleted.</param>
        public void RemoveBasket(int basketId)
        {
            var response = _client.DeleteAsync($"/api/basket/{basketId}").Result;
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Unable to remove basket {basketId}");
        }

        /// <summary>
        /// Parses an <see cref="HttpResponseMessage"/> with the response of a 'create basket' to get the basket id.
        /// </summary>
        /// <param name="response">The response from a 'create basket' request.</param>
        /// <returns>The basket id.</returns>
        private int ParseBasketId(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync();
            var contentObj = JObject.Parse(content.Result);
            return contentObj["basketId"].Value<int>();
        }
    }
}