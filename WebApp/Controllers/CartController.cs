using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApp.Common.Constants;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CartController : BaseController
    {


        public CartController(UserManager<AppUser> userManager, AppDBContext context)
            : base(userManager, context)
        {
        }
        public IActionResult Index(string id)
        {
            List<OrderProduct> orderProducts = _context.Orders
               .Where(o => o.UserId == id)           
               .SelectMany(o => o.OrderProducts)
               .Include(o => o.Product)
               .ToList();

            return View(orderProducts);
        }

        public IActionResult Add(int productId, string returnUrl, string queryString, int quantity = 1)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (currentUser == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                DateTime currentTime = DateTime.Today;
                string userId = currentUser.Id;

                Order? currentOrder = _context.Orders.FirstOrDefault(o => o.UserId == userId && o.CreatedAt == currentTime);
                if (currentOrder == null)
                {
                    currentOrder = new Order();
                    currentOrder.UserId = userId;
                    currentOrder.CreatedAt = currentTime;
                    currentOrder.Status = Status.PENDING;
                    _context.Orders.Add(currentOrder);

                    OrderProduct orderProduct = new OrderProduct();
                    orderProduct.Order = currentOrder;
                    orderProduct.ProductId = productId;
                    orderProduct.Quantity = quantity;
                    _context.OrderProducts.Add(orderProduct);
                }
                else
                {
                    OrderProduct? orderProduct = _context.OrderProducts.FirstOrDefault(op => op.OrderId == currentOrder.Id && op.ProductId == productId);
                    if (orderProduct == null)
                    {
                        orderProduct = new OrderProduct();
                        orderProduct.Order = currentOrder;
                        orderProduct.ProductId = productId;
                        orderProduct.Quantity = quantity;
                        _context.OrderProducts.Add(orderProduct);
                    }
                    else
                    {
                        orderProduct.Quantity += quantity;
                        _context.OrderProducts.Update(orderProduct);
                    }
                }
                Product product = _context.Products.Find(productId)!;
                product.Quantity -= quantity;
                _context.Products.Update(product);

                _context.SaveChanges();
            }
            if (returnUrl != "/")
            {
                returnUrl = Url.Content("~") + returnUrl + queryString;
            }
            return Redirect(returnUrl);
        }
    }
}
