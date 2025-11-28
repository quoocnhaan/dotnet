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
            AppUser? _user = await GetUserAsync();
            if (_user == null)
            {
                return RedirectToAction("Login", "User");
            }
            await _cartService.SetCartProductCount();
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
            AppUser? _user = await GetUserAsync();

            if (_user == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                List<CartItem> cartItems = await _cartService.GetCartItems();
                if (cartItems == null)
                {
                    Cart cart = new Cart();
                    cart.UserId = _user.Id;
                    _context.Carts.Add(cart);

                    CartItem cartItem = new CartItem();
                    cartItem.Cart = cart;
                    cartItem.ProductId = productId;
                    cartItem.Quantity = quantity;
                    _context.CartItems.Add(cartItem);
                }
                else
                {
                    CartItem? cartItem = cartItems.Find(_ => _.ProductId == productId);
                    if (cartItem == null)
                    {
                        Cart cart = _context.Carts.FirstOrDefault(c => c.UserId == _user.Id)!;
                        cartItem = new CartItem();
                        cartItem.Cart = cart;
                        cartItem.ProductId = productId;
                        cartItem.Quantity = quantity;
                        _context.CartItems.Add(cartItem);
                        _cartService.AddCartItemToSession(cartItem);
                    }
                    else
                    {
                        cartItem.Quantity += quantity;
                        _context.CartItems.Update(cartItem);
                        _cartService.UpdateCartItemInSession(cartItem.Id, cartItem.Quantity);
                    }
                }
                _context.SaveChanges();
            }
            if (returnUrl != "/")
            {
                returnUrl = Url.Content("~") + returnUrl + queryString;
            }
            return Redirect(returnUrl);
        }


        //public IActionResult Update(List<OrderProductUpdateModel> orderProducts, string returnUrl, string queryString)
        //{

        //        foreach (var orderProductUpdate in orderProducts)
        //        {
        //            var oldOrderProduct = _context.OrderProducts?.FirstOrDefault(op => op.Id == orderProductUpdate.Id);
        //            if (oldOrderProduct != null)
        //            {
        //                int oldQuantity = oldOrderProduct.Quantity ?? 0;
        //                int quantityDifference = orderProductUpdate.Quantity - oldQuantity;

        //                if (orderProductUpdate.Quantity == 0)
        //                {
        //                    _context.OrderProducts.Remove(oldOrderProduct);
        //                } else
        //                {
        //                    oldOrderProduct.Quantity = orderProductUpdate.Quantity;
        //                    _context.OrderProducts.Update(oldOrderProduct);
        //                }

        //                //var product = _context.Products.Find(orderProductUpdate.ProductId);
        //                //if (product != null)
        //                //{
        //                //    product.Quantity -= quantityDifference;
        //                //    _context.Products.Update(product);
        //                //}
                    
        //        }
        //        _context.SaveChanges();
        //    }
        //    SetCartProductCount();
        //    return RedirectToAction("Index");
        //}

        public IActionResult Update(List<CartItemUpdateModel> cartItems, string returnUrl, string queryString)
        {

            foreach (var newItem in cartItems)
            {
                var oldItem = _context.CartItems?.FirstOrDefault(ci => ci.Id == newItem.Id);
                if (oldItem != null)
                {
                    int oldQuantity = oldItem.Quantity;
                    int quantityDifference = newItem.Quantity - oldQuantity;

                    if (newItem.Quantity == 0)
                    {
                        _context.CartItems.Remove(oldItem);
                        _cartService.RemoveCartItemFromSession(oldItem.Id);
                    }
                    else
                    {
                        oldItem.Quantity = newItem.Quantity;
                        _context.CartItems.Update(oldItem);
                        _cartService.UpdateCartItemInSession(oldItem.Id, newItem.Quantity);
                    }

                    //var product = _context.Products.Find(orderProductUpdate.ProductId);
                    //if (product != null)
                    //{
                    //    product.Quantity -= quantityDifference;
                    //    _context.Products.Update(product);
                    //}

                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
