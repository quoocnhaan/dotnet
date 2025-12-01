using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        [StringLength(450)]// chiều dài bằng userid trong bảng user dùng cho identity
        public string UserId { get; set; } = null!;
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string receiverName { get; set; }
        public string receiverPhone { get; set; }
        public string receiverAddress { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; }
    

    public double GetTotal()
        {
            return OrderProducts?.Sum(op => op.GetTotal()) ?? 0;
        }
    }
}
