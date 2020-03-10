using ShoppingCartService.Data.DBContext;
using ShoppingCartService.DomainModels;
using ShoppingCartService.DomainModels.Contracts;
using ShoppingCartService.Core.Exceptions;
using System.Collections.Concurrent;
using System.Linq;

namespace ShoppingCartService.Domain.Managers
{
    public class ShoppingCartManager : IShoppingCartManager
    {

        private static readonly ConcurrentDictionary<int, object> LockShoppingCartByProducts = new System.Collections.Concurrent.ConcurrentDictionary<int, object>();
        public ShoppingCartServiceContext ShoppingCartServiceContext { get; set; }

        public ShoppingCartManager(ShoppingCartServiceContext shoppingCartServiceContext)
        {
            ShoppingCartServiceContext = shoppingCartServiceContext;
        }

        public ShoppingCartModel GetShoppingCartByCustomerId(int customerId)
        {
            var shoppingCartModel = new ShoppingCartModel();

            var shoppingCart = this.ShoppingCartServiceContext.ShoppingCarts
                .Where(s => s.CustomerId == customerId).FirstOrDefault();

            if (shoppingCart == null)
            {
                throw new GetShoppingCartByCustomerIdException();
            }

            shoppingCartModel.CustomerId = shoppingCart.CustomerId;
            shoppingCartModel.ShoppingCartItems = shoppingCart.ShoppingCartItems;

            return shoppingCartModel;
        }

        public bool AddProductToShoppingCart(AddProductToShoppingCartModel addProductToShoppingCartModel)
        {
            bool result = false;

            if (addProductToShoppingCartModel.ProductId == 0 || addProductToShoppingCartModel.ShoppingCartId == 0 || addProductToShoppingCartModel.VariantId == 0)
            {
                throw new AddProductToShoppingCartException();
            }

            if (CheckStock(addProductToShoppingCartModel.ProductId, addProductToShoppingCartModel.VariantId, addProductToShoppingCartModel.Quantity))
            {
                lock (ShoppingCartManager.LockShoppingCartByProducts.GetOrAdd(addProductToShoppingCartModel.ShoppingCartId, _ => new object()))
                {
                    //Check stocks before adding products to shoppingcart

                    var shoppingCartItem = this.ShoppingCartServiceContext.ShoppingCartItems
                        .FirstOrDefault(t => t.ProductId == addProductToShoppingCartModel.ProductId && t.VariantId == addProductToShoppingCartModel.VariantId);

                    if (shoppingCartItem == null)
                    {
                        shoppingCartItem = new Data.Entites.ShoppingCartItem
                        {
                            ProductId = addProductToShoppingCartModel.ProductId,
                            VariantId = addProductToShoppingCartModel.VariantId,
                            ShoppingCartId = addProductToShoppingCartModel.ShoppingCartId
                        };

                        this.ShoppingCartServiceContext.ShoppingCartItems.Add(shoppingCartItem);
                    }

                    shoppingCartItem.Quantity += addProductToShoppingCartModel.Quantity;

                    this.ShoppingCartServiceContext.SaveChanges();

                    result = true;
                }
            }

            return result;
        }

        //Check Stocks For Products
        private bool CheckStock(int productId, int variantId, int quantity)
        {
            var checkStockResult = this.ShoppingCartServiceContext.Stocks
                .Where(s => s.ProductId == productId && s.VariantId == variantId && s.Quantity >= quantity)
                .FirstOrDefault();
            if (checkStockResult != null)
            {
                return true;
            }

            return false;
        }
    }
}
