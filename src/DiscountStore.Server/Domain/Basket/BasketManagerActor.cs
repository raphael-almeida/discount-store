using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using DiscountStore.Server.Domain.Discount;
using DiscountStore.Server.Messages;

namespace DiscountStore.Server.Domain.Basket
{
    /// <summary>
    /// An implementation of <see cref="ReceiveActor"/> to handle commands related to accounts.
    /// <remarks>
    /// The commands available are:
    /// - <see cref="CreateBasket"/> to create a new basket;
    /// - <see cref="AddProduct"/> to add a new product to a basket;
    /// - <see cref="GetTotalAmountById"/> to get the total amount of a basket;
    /// - <see cref="RemoveBasket"/> to delete a basket;
    /// By implementing <see cref="ReceiveActor"/> this class guarantees that concurrent invocations of the above operations
    /// are supported through the Actor model (https://en.wikipedia.org/wiki/Actor_model) which uses the concept of 'messages'
    /// and does not requrire the use of locks.
    /// </remarks>
    /// </summary>
    public class BasketManagerActor : ReceiveActor
    {
        /// <summary>
        /// Internal counter to handle the basket ids. Since this is used only inside <see cref="CreateNewBasket"/>,
        /// we do not need to use locks.
        /// </summary>
        private int _basketIdCounter;

        /// <summary>
        /// <see cref="Lazy{T}"/> instnace of <see cref="DiscountEngine"/>.
        /// </summary>
        private readonly Lazy<DiscountEngine> _discountEngineLazy = new Lazy<DiscountEngine>(InitializeDiscountEngine);

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}"/> of the baskets currently available.
        /// </summary>
        private readonly Dictionary<int, DiscountStore.Server.Domain.Basket.Basket> _baskets = new Dictionary<int, DiscountStore.Server.Domain.Basket.Basket>();

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}"/> of the products currently available for purchase.
        /// </summary>
        private readonly Dictionary<string, Product> _productsCatalog = new Dictionary<string, Product>()
        {
            { "VOUCHER", new Product("VOUCHER", "Purchase Voucher", 5.00m) },
            { "TSHIRT", new Product("TSHIRT", "Cotton T-Shirt", 20.00m) },
            { "MUG", new Product("MUG", "Coffee Mug", 7.50m) }
        };

        /// <summary>
        /// Creates a new instance of <see cref="BasketManagerActor"/> and asigns each of the messages to their respective actions.
        /// </summary>
        public BasketManagerActor()
        {
            Receive<CreateBasket>(message => CreateNewBasket());
            Receive<AddProduct>(message => AddProduct(message));
            Receive<GetTotalAmountById>(message => GetTotalAmount(message));
            Receive<RemoveBasket>(message => RemoveBasket(message));
        }

        /// <summary>
        /// Creates a new basket.
        /// </summary>
        private void CreateNewBasket()
        {
            int GenerateNewBasketId()
            {
                _basketIdCounter++;
                return _basketIdCounter;
            }

            var id = GenerateNewBasketId();
            var newBasket = new DiscountStore.Server.Domain.Basket.Basket(id);
            _baskets.Add(newBasket.Id, newBasket);
            Sender.Tell(new CreateBasketResponse(id));
        }

        /// <summary>
        /// Adds a new product to a basket.
        /// </summary>
        /// <param name="message">The message with details of the product to be added.</param>
        private void AddProduct(AddProduct message)
        {
            if (!_baskets.ContainsKey(message.BasketId))
            {
                Sender.Tell(new BasketNotFound(message.BasketId));
                return;
            }
            if (message.ProductCode == null || !_productsCatalog.ContainsKey(message.ProductCode))
            {
                Sender.Tell(new ProductNotFound(message.ProductCode));
                return;
            }
            var basket = _baskets[message.BasketId];
            basket.AddProduct(_productsCatalog[message.ProductCode], message.Quantity);
            Sender.Tell(new ProductAddedToBasket(message.BasketId));
        }

        /// <summary>
        /// Gets the total amount details of a specific basket.
        /// </summary>
        /// <param name="message">The message with details of the basket to get the total amount from.</param>
        private void GetTotalAmount(GetTotalAmountById message)
        {
            if (_baskets.TryGetValue(message.BasketId, out var basket))
            {
                var total = _discountEngineLazy.Value.CalculateTotalAmount(basket.Products);
                var products = basket.Products.Select(product => product.Code);
                var result = new TotalAmountResponse(products, total);
                Sender.Tell(result);
            }
            else
                Sender.Tell(new BasketNotFound(message.BasketId));
        }

        /// <summary>
        /// Deletes a basket.
        /// </summary>
        /// <param name="message">The message with details of the basket to be deleted.</param>
        private void RemoveBasket(RemoveBasket message)
        {
            if (_baskets.ContainsKey(message.Id))
            {
                _baskets.Remove(message.Id);
                Sender.Tell(new BasketDeleted(message.Id));
            }
            else
                Sender.Tell(new BasketNotFound(message.Id));
        }
        
        /// <summary>
        /// Initializes the <see cref="DiscountEngine"/> instance.
        /// </summary>
        /// <returns></returns>
        private static DiscountEngine InitializeDiscountEngine()
        {
            return new DiscountEngine();
        }
    }

}
