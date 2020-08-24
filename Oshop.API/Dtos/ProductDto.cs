namespace Oshop.API.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public bool SoldOut { get; set; }
        public int Weight { get; set; }
    }
}