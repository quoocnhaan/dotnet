namespace WebApp.Models
{
    public class CheckOutViewModel
    {
        public string FullName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public List<CartItem>? CartItems { get; set; }
    }
}
