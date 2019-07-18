using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace DiscountStore.Client
{
    public class Program
    {
        /// <summary>
        /// Entry point of the client application.
        /// </summary>
        static void Main()
        {
            Console.WriteLine("Welcome to Discount Store!");
            Console.WriteLine();
            Console.WriteLine("> Enter 'help' for instructions.");
            var lastCreatedBasket = 0;
            while (true)
            {
                var command = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(command))
                    continue;

                if (command.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
                    break;
                if (command.Equals("help", StringComparison.InvariantCultureIgnoreCase))
                    ShowHelp();
                else if (command.Equals("create basket", StringComparison.InvariantCultureIgnoreCase))
                    lastCreatedBasket = CreateBasket();
                else if (command.Equals("add product", StringComparison.InvariantCultureIgnoreCase))
                    AddProduct(lastCreatedBasket);
                else if (command.Equals("get total", StringComparison.InvariantCultureIgnoreCase))
                    ShowTotalAmountDetails(lastCreatedBasket);
                else if (command.Equals("remove basket", StringComparison.InvariantCultureIgnoreCase))
                    RemoveBasket(lastCreatedBasket);
                else
                    Console.WriteLine("Invalid command.");

            }
            Console.ReadKey();
        }

        /// <summary>
        /// Shows instructions on how to use the client via command line.
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine("> The following commands are available:");
            Console.WriteLine("- 'create basket' creates a new basket.");
            Console.WriteLine("- 'add product' adds a product to the last created basket, or to a specific basket id.");
            Console.WriteLine("- 'get total' gets the total amount of the last created basket, or of a specific basket id.");
            Console.WriteLine("- 'remove basket' removes a basket and all products in it.");
        }

        /// <summary>
        /// Creates a new basket.
        /// </summary>
        /// <returns>The id of the newly created basket.</returns>
        private static int CreateBasket()
        {
            var storeClient = StoreClient.Instance;
            int basketId;
            try
            {
                basketId = storeClient.CreateBasket();
            }
            catch (Exception e)
            {
                Console.WriteLine($"> Error creating basket: {e.Message}");
                return 0;
            }
            Console.WriteLine($"> Created a new Basket with id: {basketId}.");
            return basketId;
        }

        /// <summary>
        /// Adds a new product to a basket given by its id.
        /// </summary>
        /// <param name="basketId">The id of the basket to add the product.</param>
        private static void AddProduct(int basketId)
        {
            var storeClient = StoreClient.Instance;
            Console.WriteLine("> Enter the product code:");
            var productCode = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(productCode))
            {
                Console.WriteLine("> Invalid product code.");
                return;
            }
            Console.WriteLine("> Enter the quantity or press 'Enter' to add one product:");
            int quantity;
            var quantityString = Console.ReadLine();
            if (quantityString == string.Empty)
                quantity = 1;

            else if (!int.TryParse(quantityString, out quantity) || quantity < 1)
            {
                Console.WriteLine("> Invalid quantity.");
                return;
            }
            if (basketId > 0)
            {
                Console.WriteLine($"> Enter the basket id or press 'Enter' to use id {basketId}:");
                var customBasketId = Console.ReadLine();
                if (customBasketId != string.Empty)
                {
                    if (!int.TryParse(customBasketId, out basketId))
                    {
                        Console.WriteLine("> Invalid basket id.");
                        return;
                    }
                }
            }
            var product = new { ProductCode = productCode, Quantity = quantity, BasketId = basketId };
            try
            {
                storeClient.AddProductToBasket(basketId, product);
            }
            catch (Exception e)
            {
                Console.WriteLine($"> Error adding product: {e.Message}");
                return;
            }

            Console.WriteLine($"> Product {product.ProductCode} added to basket {basketId}.");
        }

        /// <summary>
        /// Shows the total amount and items of a basket given by its basket id.
        /// </summary>
        /// <param name="basketId">The id of the basket to show the amount details.</param>
        private static void ShowTotalAmountDetails(int basketId)
        {
            if (basketId > 0)
            {
                Console.WriteLine($"> Enter the basket id or press 'Enter' to use id {basketId}:");
                var customBasketId = Console.ReadLine();
                if (customBasketId != string.Empty)
                {
                    if (!int.TryParse(customBasketId, out basketId))
                    {
                        Console.WriteLine("> Invalid basket id.");
                        return;
                    }
                }
            }

            JObject response;
            try
            {
                response = StoreClient.Instance.GetTotalAmount(basketId);
            }
            catch (Exception e)
            {
                Console.WriteLine($"> Error getting total: {e.Message}");
                return;
            }
            var total = string.Format(CultureInfo.GetCultureInfo("es-ES"), "{0:C}", response["total"]);

            Console.WriteLine($"Items: {string.Join(" ", response["items"])}");
            Console.WriteLine($"Total: {total}");
        }

        /// <summary>
        /// Removes a basket given by its basket id.
        /// </summary>
        /// <param name="basketId">The id of the basket to be removed.</param>
        private static void RemoveBasket(int basketId)
        {
            if (basketId > 0)
            {
                Console.WriteLine($"Enter the basket id or press 'Enter' to use id {basketId}:");
                var customBasketId = Console.ReadLine();
                if (customBasketId != string.Empty)
                {
                    if (!int.TryParse(customBasketId, out basketId))
                    {
                        Console.WriteLine("Invalid basket id.");
                        return;
                    }
                }
            }
            try
            {
                StoreClient.Instance.RemoveBasket(basketId);
            }
            catch (Exception e)
            {
                Console.WriteLine($"> Error removing basket: {e.Message}");
                return;
            }
            Console.WriteLine($"Basket {basketId} removed successfully.");
        }
    }
}
