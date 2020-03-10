using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartService.Data.Entites
{
    public class ShoppingCart : BaseEntity
    {

        public int CustomerId { get; set; }

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart()
        {
            ShoppingCartItems = new List<ShoppingCartItem>();
        }

    }
}
