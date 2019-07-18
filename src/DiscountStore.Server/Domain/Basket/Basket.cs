using System;
using System.Collections.Generic;

namespace DiscountStore.Server.Domain.Basket
{
    /// <summary>
    /// Basket representation, containing an unique identifier and a list of products.
    /// </summary>
    public class Basket
    {
        /// <summary>
        /// Creates a new instance of a <see cref="Basket"/>.
        /// </summary>
        /// <param name="id">The id of the basket to be created</param>
        public Basket(int id)
        {
            Id = id;
            Products = new List<Product>();
        }

        /// <summary>
        /// Gets the id of the current basket.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the list of products of the current basket.
        /// </summary>
        public IList<Product> Products { get; }

        /// <summary>
        /// Adds one or more products to the curent basket.
        /// </summary>
        /// <param name="product">The <see cref="Product"/> to be added.</param>
        /// <param name="quantity">Quantity of products to be added.</param>
        public void AddProduct(Product product, int quantity)
        {
            if(quantity < 1)
                throw new ArgumentException("Invalid quantity of products");

            for (var i = 0; i < quantity; i++)
                Products.Add(new Product(product.Code, product.Name, product.Price));
        }
    }
}