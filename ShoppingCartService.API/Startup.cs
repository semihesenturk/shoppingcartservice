using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ShoppingCartService.Data.DBContext;
using ShoppingCartService.Data.Entites;
using ShoppingCartService.Domain.Managers;
using ShoppingCartService.DomainModels.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ShoppingCartService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //dummy customerId. it comes from request object
        int currentCustomerId = 26;

        //Add some entities and datas for functional tests
        private void Seed(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ShoppingCartServiceContext>();

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

            var shoppingCart = new ShoppingCart()
            {
                CustomerId = currentCustomerId,
                Id = 1
            };

            var shoppingCartItems = new List<ShoppingCartItem>
            {
                new ShoppingCartItem()
            {
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
            context.ShoppingCartItems.AddRange(shoppingCartItems);

            context.SaveChanges();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ShoppingCartServiceContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"), ServiceLifetime.Scoped, ServiceLifetime.Scoped);
            services.AddScoped<IShoppingCartManager, ShoppingCartManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ShoppingCartService",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            Seed(serviceProvider);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCartService V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
