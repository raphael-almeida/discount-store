using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using DiscountStore.Server.Domain.Basket;

namespace DiscountStore.Server.Domain.Discount
{
    /// <summary>
    /// Represents a discount engine.
    /// <remarks>
    /// This class is responsible for loading all <see cref="IDiscountRule"/> instances and applying all of them to the products list of a basket.
    /// It exposes a public method, <see cref="CalculateTotalAmount"/> which will calculate all possible discounts on the list of products of a basket.
    /// </remarks> 
    /// </summary>
    public class DiscountEngine
    {
        /// <summary>
        /// Gets list of <see cref="IDiscountRule"/>.
        /// </summary>
        private IEnumerable<IDiscountRule> DiscountRules { get;  }

        /// <summary>
        /// Creates a new instance of <see cref="DiscountEngine"/>.
        /// Injects the <see cref="IDiscountRule"/> instances into <see cref="DiscountRules"/> property.
        /// </summary>
        public DiscountEngine()
        {
            var assemblies = GetDiscountAssemblies();
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies);

            using (var container = configuration.CreateContainer())
            {
                DiscountRules = container.GetExports<IDiscountRule>();
            }
        }

        /// <summary>
        /// Calculates the total amount of a list of products.
        /// </summary>
        /// <param name="products"></param>
        /// <returns>The amount with the discount(s) applied.</returns>
        public decimal CalculateTotalAmount(IList<Product> products)
        {
            if (products == null || products.Count == 0)
                return 0m;

            foreach (var discountRule in DiscountRules)
                discountRule.ApplyDiscount(products);
            
            return products.Sum(product => product.Price);
        }
        
        /// <summary>
        /// Gets the list of <see cref="Assembly"/> that implements <see cref="IDiscountRule"/> on the current directory.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Assembly> GetDiscountAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dlls = Directory.EnumerateFiles(Environment.CurrentDirectory, "DiscountStore.DiscountRules.*.dll",SearchOption.AllDirectories);
            foreach (var dll in dlls)
            {
                var dllFullPath = Path.GetFullPath(dll);
                var assembly = Assembly.LoadFile(dllFullPath);
                foreach (var item in assembly.GetTypes())
                {
                    if (!item.IsClass) continue;
                    if (item.GetInterfaces().Contains(typeof(IDiscountRule)))
                        assemblies.Add(assembly);
                }
            }
            return assemblies;
        }
    }
}