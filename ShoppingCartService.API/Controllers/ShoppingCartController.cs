using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.DomainModels;
using ShoppingCartService.DomainModels.Contracts;

namespace ShoppingCartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        public IShoppingCartManager ShoppingCartManager { get; }

        public ShoppingCartController(IShoppingCartManager shoppingCartManager)
        {
            this.ShoppingCartManager = shoppingCartManager;
        }

        /// <summary>
        /// Get Shopping Cart by customer Id
        /// </summary>
        /// <param name="customerId"></param>
        [HttpGet]
        public ShoppingCartModel GetShoppingCartByCustomerId(int customerId)
        {
            var shoppingCart = this.ShoppingCartManager.GetShoppingCartByCustomerId(customerId);

            return shoppingCart;
        }

        /// <summary>
        /// Add product with quantity on current users shopping cart
        /// </summary>
        /// <param name="addProductToShoppingCartModel"></param>
        [HttpPost]
        public void AddProductToShoppingCart(AddProductToShoppingCartModel addProductToShoppingCartModel)
        {
            this.ShoppingCartManager.AddProductToShoppingCart(addProductToShoppingCartModel);
        }

    }
}