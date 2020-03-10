using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartService.Data.Entites
{
    public class Variant : BaseEntity
    {
        public Variant()
        {
            ShoppingCartItems = new HashSet<ShoppingCartItem>();
            Stocks = new HashSet<Stock>();
        }
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
        public ICollection<Stock> Stocks { get; set; }

    }
}
