using System;

namespace ShoppingCartService.DomainModels
{
    public class AddProductToShoppingCartModel
    {
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
    }
}
