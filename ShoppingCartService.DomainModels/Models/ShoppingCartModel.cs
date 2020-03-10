using ShoppingCartService.Data.Entites;
using System;
using System.Collections.Generic;


namespace ShoppingCartService.DomainModels
{
    public class ShoppingCartModel
    {
        public int CustomerId { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
