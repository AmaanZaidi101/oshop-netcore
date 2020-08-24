using System.ComponentModel.DataAnnotations.Schema;

namespace Oshop.API.Models
{
    public class ProductItem
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalPrice { get; set; }
        public virtual Product Product { get; set; }
        public string ProductId { get; set; }
    }
}