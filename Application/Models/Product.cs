namespace Application.Models
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string SellerId { get; set; }
        public int? AmountAvailable { get; set; }
        public float? Cost { get; set; }
    }
}
