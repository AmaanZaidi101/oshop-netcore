using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oshop.API.Models
{
    public class Product
    {
        public string Id { get; set; }
        public virtual Category Category { get; set; }
        public string CategoryId { get; set; }
        public string ImageUrl { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
        public string Name { get; set; }
        public bool SoldOut { get; set; }
        public int Weight { get; set; }
        public virtual ICollection<ProductItem> ProductItems { get; set; }
        
    }
}