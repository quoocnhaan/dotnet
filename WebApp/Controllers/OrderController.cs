using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class OrderController : BaseController
    {
        public OrderService _orderService { get; }
        public CartService _cartService { get; }

        public OrderController(IDbContextFactory<AppDBContext> context, UserService userService, OrderService orderService, CartService cartService) : base(context, userService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckOutAsync(CheckOutViewModel model) 
        {
            await _orderService.CheckOutAsync(model);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CheckOutAsync()
        {
            CheckOutViewModel model = new CheckOutViewModel();
            model.CartItems = await _cartService.GetCartItems();
            model.PhoneNumber = string.Empty;
            model.Address = string.Empty;
            model.FullName = string.Empty;
            return View(model);
        }
    }
}
