namespace Oshop.API.Dtos
{
    public class ProductItemDto
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public ProductDto Product { get; set; }
    }
}