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
            Order? order = _context.Orders?
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.UserId == id);
            SetCartProductCount();
            return View(order);
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

        public IActionResult Update(int orderId, List<OrderProductUpdateModel> orderProducts, string returnUrl, string queryString)
        {
            Order? order = _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .SingleOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                foreach (var orderProductUpdate in orderProducts)
                {
                    var oldOrderProduct = order.OrderProducts?.FirstOrDefault(op => op.ProductId == orderProductUpdate.ProductId);
                    if (oldOrderProduct != null)
                    {
                        int oldQuantity = oldOrderProduct.Quantity ?? 0;
                        int quantityDifference = orderProductUpdate.Quantity - oldQuantity;

                        if (orderProductUpdate.Quantity == 0)
                        {
                            _context.OrderProducts.Remove(oldOrderProduct);
                        } else
                        {
                            oldOrderProduct.Quantity = orderProductUpdate.Quantity;
                            _context.OrderProducts.Update(oldOrderProduct);
                        }

                        var product = _context.Products.Find(orderProductUpdate.ProductId);
                        if (product != null)
                        {
                            product.Quantity -= quantityDifference;
                            _context.Products.Update(product);
                        }
                    }
                }
                _context.SaveChanges();
            }
            SetCartProductCount();
            return View("Index", order);
        }
    }
}
