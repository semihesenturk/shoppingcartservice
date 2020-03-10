using System.Collections;
using System.Collections.Generic;

namespace ShoppingCartService.Data.Entites
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }

        public int ModelId { get; set; }
        public Model Model { get; set; }
        public ICollection<Stock> Stocks { get; internal set; }
    }
}
