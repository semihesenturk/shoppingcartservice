namespace ShoppingCartService.Data.Entites
{
    public class Stock
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public int VariantId { get; set; }


        public Product Product { get; set; }

        public Variant Variant { get; set; }
    }
}
