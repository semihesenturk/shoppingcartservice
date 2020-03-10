using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCartService.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartService.Data.DBContext
{
    public partial class ShoppingCartServiceContext : DbContext
    {
        public ShoppingCartServiceContext(DbContextOptions<ShoppingCartServiceContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Model> Models { get; set; }

        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Variant> Variants { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("dbo");

            builder.Entity<Model>(entity =>
            {
                entity.ToTable(nameof(Model));

                entity.Property(c => c.Name).HasMaxLength(500);
            });

            builder.Entity<Product>(entity =>
            {
                entity.ToTable(nameof(Product));

                entity.Property(c => c.Barcode).HasMaxLength(15).IsRequired();
                entity.Property(c => c.Description).HasMaxLength(500);
                entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
                entity.Property(c => c.Price).IsRequired();

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ModelId)
                    .HasConstraintName("FK_Product_ModelId");
            });

            builder.Entity<ShoppingCart>(entity =>
            {
                entity.ToTable(nameof(ShoppingCart));
            });

            builder.Entity<ShoppingCartItem>(entity =>
            {
                entity.ToTable(nameof(ShoppingCartItem));

                entity.Property(c => c.ProductId).IsRequired();
                entity.Property(c => c.ShoppingCartId).IsRequired();
                entity.Property(c => c.VariantId).IsRequired();


                entity.HasOne(d => d.ShoppingCart)
                    .WithMany(p => p.ShoppingCartItems)
                    .HasForeignKey(d => d.ShoppingCartId)
                    .HasConstraintName("FK_ShoppingCartItem_ShoppingCartId");

                entity.HasOne(d => d.Variant)
                    .WithMany(p => p.ShoppingCartItems)
                    .HasForeignKey(d => d.VariantId)
                    .HasConstraintName("FK_ShoppingCartItem_VariantId");
            });

            builder.Entity<Stock>(entity =>
            {
                entity.ToTable(nameof(Stock));

                entity.HasKey(c => new { c.ProductId, c.VariantId });

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Stock_ProductId");

                entity.HasOne(d => d.Variant)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.VariantId)
                    .HasConstraintName("FK_Stock_VariantId");
            });

            builder.Entity<Variant>(entity =>
            {
                entity.ToTable(nameof(Variant));

                entity.Property(p => p.Name).HasMaxLength(150).IsRequired();
                entity.Property(p => p.Description).HasMaxLength(500);
            });
        }
    }
}
