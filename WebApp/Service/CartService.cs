using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Service
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CARTKEY = "cart";
        private ISession Session;
        private readonly AppDBContext _context;
        private readonly UserService _userService;

        public CartService(IHttpContextAccessor httpContextAccessor, AppDBContext context, UserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            Session = _httpContextAccessor.HttpContext.Session;
            _context = context;
            _userService = userService;
        }

        public async Task SetCartProductCount()
        {

            List<CartItem> cartItems = await GetCartItems();

            int cartProductCount = cartItems.Sum(ci => ci.Quantity);

            Session.SetInt32("CartProductCount", cartProductCount);
        }

        public void SaveCartSession(List<CartItem> ls)
        {
            string jsoncart = JsonConvert.SerializeObject(ls, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            Session.SetString(CARTKEY, jsoncart);
        }

        public void ClearCart()
        {
            Session.Remove(CARTKEY);
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            string jsoncart = Session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            else
            {
                AppUser user = await _userService.GetUser();
                Cart cart = _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c => c.UserId == user.Id);
                if (cart == null)
                {
                    return null;
                }
                else
                {
                    List <CartItem> list = cart.CartItems.ToList();
                    SaveCartSession(list);
                    return list;
                }
            }
        }

        public CartItem GetCartItem(int cartItemId)
        {
            string json = Session.GetString(CARTKEY);
            if (json == null)
                return null;

            var items = JsonConvert.DeserializeObject<List<CartItem>>(json);

            return items.FirstOrDefault(i => i.Id == cartItemId);
        }

        public async Task RemoveCartItemFromSession(int cartItemId)
        {
            var items = await GetCartItems();
            var itemToRemove = items.FirstOrDefault(i => i.Id == cartItemId);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                SaveCartSession(items);
            }
        }

        public async Task UpdateCartItemInSession(int oldId, int newQuantity)
        {
            var items = await GetCartItems();
            var itemToUpdate = items.FirstOrDefault(i => i.Id == oldId);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = newQuantity;
                SaveCartSession(items);
            }
        }

        public async Task AddCartItemToSession(CartItem cartItem)
        {
            var items = await GetCartItems();
            items.Add(cartItem);
            SaveCartSession(items);
        }
    }
}
