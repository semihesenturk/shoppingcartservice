using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartService.DomainModels.Contracts
{
    public interface IShoppingCartManager
    {
        bool AddProductToShoppingCart(AddProductToShoppingCartModel addProductToShoppingCartModel);

        ShoppingCartModel GetShoppingCartByCustomerId(int customerId);
    }
}
