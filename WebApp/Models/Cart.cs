using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [StringLength(450)]// chiều dài bằng userid trong bảng user dùng cho identity
        public string UserId { get; set; } = null!;

        [JsonIgnore] // prevent loop
        public ICollection<CartItem>? CartItems { get; set; }

        public double GetTotal()
        {
            return CartItems?.Sum(ci => ci.GetTotal()) ?? 0;
        }
    }
}
