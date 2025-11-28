using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [JsonIgnore] // prevent loop
        public Product? Product { get; set; }
        public int CartId { get; set; }

        [JsonIgnore] // prevent loop
        public Cart? Cart { get; set; }
        public int Quantity { get; set; }

        public double? GetTotal()
        {
            return Product?.GetPriceAfterDiscount() * Quantity;
        }

    }
}
