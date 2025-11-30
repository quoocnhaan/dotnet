using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Threading.Tasks;
using WebApp.Common.Constants;
using WebApp.Data;
using WebApp.Models;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class CartController : BaseController
    {
        private readonly CartService _cartService;

        public CartController(IDbContextFactory<AppDBContext> context, CartService cartService, UserService userService)
            : base(context, userService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userService.isLogin())
            {
                return RedirectToAction("Login", "User");
            }
            var warnings = await _cartService.ValidateAndFixCartAsync();
            if (warnings.Any())
            {
                TempData["CartWarnings"] = string.Join("<br>", warnings);
            }
            return View(await _cartService.GetCartItems());
        }

        //public IActionResult Add(int productId, string returnUrl, string queryString, int quantity = 1)
        //{
        //    var currentUser = _userManager.GetUserAsync(User).Result;
        //    if (currentUser == null)
        //    {
        //        return RedirectToAction("Login", "User");
        //    }
        //    else
        //    {
        //        DateTime currentTime = DateTime.Today;
        //        string userId = currentUser.Id;

        //        Order? currentOrder = _context.Orders.FirstOrDefault(o => o.UserId == userId && o.CreatedAt == currentTime);
        //        if (currentOrder == null)
        //        {
        //            currentOrder = new Order();
        //            currentOrder.UserId = userId;
        //            currentOrder.CreatedAt = currentTime;
        //            currentOrder.Status = Status.PENDING;
        //            _context.Orders.Add(currentOrder);

        //            OrderProduct orderProduct = new OrderProduct();
        //            orderProduct.Order = currentOrder;
        //            orderProduct.ProductId = productId;
        //            orderProduct.Quantity = quantity;
        //            _context.OrderProducts.Add(orderProduct);
        //        }
        //        else
        //        {
        //            OrderProduct? orderProduct = _context.OrderProducts.FirstOrDefault(op => op.OrderId == currentOrder.Id && op.ProductId == productId);
        //            if (orderProduct == null)
        //            {
        //                orderProduct = new OrderProduct();
        //                orderProduct.Order = currentOrder;
        //                orderProduct.ProductId = productId;
        //                orderProduct.Quantity = quantity;
        //                _context.OrderProducts.Add(orderProduct);
        //            }
        //            else
        //            {
        //                orderProduct.Quantity += quantity;
        //                _context.OrderProducts.Update(orderProduct);
        //            }
        //        }
        //        Product product = _context.Products.Find(productId)!;
        //        product.Quantity -= quantity;
        //        _context.Products.Update(product);

        //        _context.SaveChanges();
        //    }
        //    if (returnUrl != "/")
        //    {
        //        returnUrl = Url.Content("~") + returnUrl + queryString;
        //    }
        //    return Redirect(returnUrl);
        //}

        public async Task<IActionResult> Add(int productId, string returnUrl, string queryString, int quantity = 1)
        {
            if (!_userService.isLogin())
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                await _cartService.AddToCart(productId, quantity);

                if (returnUrl != "/")
                {
                    returnUrl = Url.Content("~") + returnUrl + queryString;
                }
                return Redirect(returnUrl);
            }
        }

        public async Task<IActionResult> UpdateAsync(List<CartItemUpdateModel> cartItems, string returnUrl, string queryString)
        {

            await _cartService.UpdateCart(cartItems);
            return RedirectToAction("Index");
        }
    }
}
