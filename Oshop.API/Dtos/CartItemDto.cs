namespace Oshop.API.Dtos
{
    public class CartItemDto
    {
        public string Id { get; set; }
        public string CartId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string ProductId { get; set; }
        public ProductDto Product { get; set; }
    }
}