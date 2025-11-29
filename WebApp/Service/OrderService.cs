using WebApp.Common.Constants;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Service
{
    public class OrderService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CARTKEY = "cart";
        private ISession Session;
        private readonly AppDBContext _context;
        private readonly UserService _userService;
        private readonly CartService _cartService;

        public OrderService(IHttpContextAccessor httpContextAccessor, AppDBContext context, UserService userService, CartService cartService)
        {
            _httpContextAccessor = httpContextAccessor;
             Session = _httpContextAccessor.HttpContext.Session;
            _context = context;
            _userService = userService;
            _cartService = cartService;
        }

        public async Task CheckOutAsync()
        {
            var user = await _userService.GetUser();
            if (user == null)
                return;

            List<CartItem> cartItems = await _cartService.GetCartItems();
            if (cartItems == null || cartItems.Count == 0)
                return;

            Order order = new Order
            {
                UserId = user.Id,
                CreatedAt = DateTime.Today,
                Status = Status.PENDING,
                OrderProducts = cartItems.Select(ci => new OrderProduct
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.GetPriceAfterDiscount()
                }).ToList()
            };

            _context.Orders.Add(order);

            foreach (var cartItem in cartItems)
            {
                var product = await _context.Products
                    .FindAsync(cartItem.ProductId);

                if (product != null)
                {
                    if (product.Quantity < cartItem.Quantity)
                    {
                        throw new Exception($"Not enough stock for product: {product.Name}");
                    }

                    product.Quantity -= cartItem.Quantity;
                    _context.Products.Update(product);
                }
            }
            _cartService.ClearCartAsync();

            _context.SaveChanges();
        }
    }
}
