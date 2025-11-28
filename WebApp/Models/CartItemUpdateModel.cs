namespace WebApp.Models
{
    public class CartItemUpdateModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
