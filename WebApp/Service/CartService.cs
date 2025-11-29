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

            int cartProductCount = cartItems != null ? cartItems.Sum(ci => ci.Quantity) : 0;

            Session.SetInt32("CartProductCount", cartProductCount);
        }

        public async Task SaveCartSessionAsync(List<CartItem> ls)
        {
            string jsoncart = JsonConvert.SerializeObject(ls, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            Session.SetString(CARTKEY, jsoncart);
            await SetCartProductCount();
        }

        public async Task ClearCartAsync()
        {
            Session.Remove(CARTKEY);

            AppUser user = await _userService.GetUser();
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart != null)
            {
                foreach (var item in cart.CartItems)
                {
                    _context.CartItems.Remove(item);
                }
            }
        }

        public async Task fetchNewDataAsync()
        {
            AppUser user = await _userService.GetUser();

            Cart cart = _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c => c.UserId == user.Id);
            if(cart != null)
            {
                SaveCartSessionAsync(cart.CartItems.ToList());

            } else
            {
                SaveCartSessionAsync(new List<CartItem>());
            }
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            if (!_userService.isLogin()) return null;
            string jsoncart = Session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            else
            {
                await fetchNewDataAsync();
                return await GetCartItems();          
            }
        }

        public async Task RemoveCartItemFromSession(int cartItemId)
        {
            var items = await GetCartItems();
            var itemToRemove = items.FirstOrDefault(i => i.Id == cartItemId);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                SaveCartSessionAsync(items);
            }
        }

        public async Task UpdateCartItemInSession(int oldId, int newQuantity)
        {
            var items = await GetCartItems();
            var itemToUpdate = items.FirstOrDefault(i => i.Id == oldId);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = newQuantity;
                SaveCartSessionAsync(items);
            }
        }

        public async Task CreateNewCartAsync()
        {
            AppUser user = await _userService.GetUser();
            Cart cart = new Cart();
            cart.UserId = user.Id;
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public List<CartItem> GetCartItemsFromDB()
        {
            AppUser user =  _userService.GetUser().Result;
            Cart cart = _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c => c.UserId == user.Id);
            if (cart != null)
            {
                return cart.CartItems.ToList();
            }
            return new List<CartItem>();
        }
    }
}
