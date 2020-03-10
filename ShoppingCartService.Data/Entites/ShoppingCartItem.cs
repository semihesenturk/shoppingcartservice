using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartService.Data.Entites
{
    public class ShoppingCartItem : BaseEntity
    {

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int ShoppingCartId { get; set; }

        public int VariantId { get; set; }


        public Product Product { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public Variant Variant { get; set; }
    }
}
