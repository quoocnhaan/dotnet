using WebApp.Data;

namespace WebApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public string ImageUrl { get; set; }

        public double GetPriceAfterDiscount()
        {
            double discountedPrice = Price * (1 - Discount);
            return Math.Round(discountedPrice, 2);
        }
    }
}
