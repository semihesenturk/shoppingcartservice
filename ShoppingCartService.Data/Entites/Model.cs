using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartService.Data.Entites
{
    public class Model : BaseEntity
    {
        public Model()
        {
            Products = new HashSet<Product>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
