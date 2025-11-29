using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class OrderController : BaseController
    {
        public OrderService _orderService { get; }

        public OrderController(IDbContextFactory<AppDBContext> context, UserService userService, OrderService orderService) : base(context, userService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckOut() 
        {
            _orderService.CheckOutAsync().Wait();
            return RedirectToAction("Index", "Cart");
        }
    }
}
