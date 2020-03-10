using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCartService.Data.DBContext;
using ShoppingCartService.Data.Entites;
using ShoppingCartService.Domain.Managers;
using ShoppingCartService.DomainModels;
using ShoppingCartService.DomainModels.Contracts;
using ShoppingCartService.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace ShoppingCartService.Test
{
    [TestClass]
    public class ShoppingCartManagementTest
    {
        private ServiceProvider ServiceProvider { get; set; }

        //dummy customerId. it comes from request object
        public int currentCustomerId = 26;

        //Preparing objects and datas for inmemory database
        private void Seed()
        {

            var context = ServiceProvider.GetService<ShoppingCartServiceContext>();

            var models = new List<Model> {
                new Model() { CreatedDate = DateTime.Now, Description = "Model First", Name = "First Model", UpdatedDate = DateTime.Now },
                new Model() { CreatedDate = DateTime.Now, Description = "Model Second", Name = "Second Model", UpdatedDate = DateTime.Now }
             };
            var variants = new List<Variant> {
                new Variant() { CreatedDate = DateTime.Now, Description = "Variant First", Name = "First Variant" },
                new Variant() { CreatedDate = DateTime.Now, Description = "Variant Second", Name = "First Second" }
            };
            var products = new List<Product> {
                 new Product()
            {
                Barcode = "443252",
                CreatedDate = DateTime.Now,
                Description = "Product first",
                Model = models[0],
                ModelId = 1,
                Name = "First Product",
                Price = 24.99m
            },
                 new Product()
            {
                Barcode = "443255",
                CreatedDate = DateTime.Now,
                Description = "Product second",
                Model = models[1],
                ModelId = 1,
                Name = "Second Product",
                Price = 29.99m
            }

        };
            var stocks = new List<Stock> {
              new Stock() { Product = products[0], Quantity = 5, Variant = variants[0] },
              new Stock() { Product = products[0], Quantity = 5, Variant = variants[1] },
              new Stock() { Product = products[1], Quantity = 5, Variant = variants[0] },
              new Stock() { Product = products[1], Quantity = 5, Variant = variants[1] }
            };


            ShoppingCart shoppingCart = new ShoppingCart()
            {
                CustomerId = currentCustomerId
            };

            var shoppingCartItems = new List<ShoppingCartItem>
            {
                new ShoppingCartItem()
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                Product = products[0],
                Variant = variants[0],
                Quantity = 1,
                ShoppingCart = shoppingCart
                }
            };

            context.Models.AddRange(models);
            context.Variants.AddRange(variants);
            context.Products.AddRange(products);
            context.Stocks.AddRange(stocks);
            context.ShoppingCarts.Add(shoppingCart);


            context.SaveChanges();
        }

        [TestInitialize]
        public void Init()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ShoppingCartServiceContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"), ServiceLifetime.Scoped, ServiceLifetime.Scoped);
            services.AddSingleton<IShoppingCartManager, ShoppingCartManager>();

            ServiceProvider = services.BuildServiceProvider();

            Seed();
        }

        [TestMethod]
        public void GetShoppingCartWithCustomerIdIsZero_Test()
        {
            int customerId = 0;

            IShoppingCartManager manager = ServiceProvider.GetService<IShoppingCartManager>();

            Assert.ThrowsException<GetShoppingCartByCustomerIdException>(() =>
            {
                manager.GetShoppingCartByCustomerId(customerId);
            });
        }

        [TestMethod]
        public void GetShoppingCartWithCustomerId_Test()
        {
            int customerId = currentCustomerId;

            IShoppingCartManager manager = ServiceProvider.GetService<IShoppingCartManager>();

            var shoppingCart = manager.GetShoppingCartByCustomerId(customerId);

        }

        [TestMethod]
        public void AddProductToShoppingCartForProductIdOrVariantIdOrQuantityIsZero_Test()
        {
            var productId = default(int);
            var variantId = default(int);
            var quantity = default(int);

            IShoppingCartManager manager = ServiceProvider.GetService<IShoppingCartManager>();

            Assert.ThrowsException<AddProductToShoppingCartException>(() =>
            {
                manager.AddProductToShoppingCart(new AddProductToShoppingCartModel
                {
                    ProductId = productId,
                    VariantId = variantId,
                    Quantity = quantity
                });
            });
        }

        [TestMethod]
        public void AddProductToShoppingCartWithValidDatas_Test()
        {
            var productId = 1;
            var variantId = 2;
            var quantity = 1;
            var shoppingCartId = 1;

            IShoppingCartManager manager = ServiceProvider.GetService<IShoppingCartManager>();

            var result = manager.AddProductToShoppingCart(new AddProductToShoppingCartModel
            {
                ProductId = productId,
                VariantId = variantId,
                Quantity = quantity,
                ShoppingCartId = shoppingCartId
            });


            Assert.AreEqual(true, result);
        }
    }
}
