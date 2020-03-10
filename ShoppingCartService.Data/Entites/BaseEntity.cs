using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartService.Data.Entites
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
