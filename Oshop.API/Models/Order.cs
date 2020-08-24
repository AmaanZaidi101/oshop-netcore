using System;
using System.Collections.Generic;

namespace Oshop.API.Models
{
    public class Order
    {
        public string Id { get; set; }
        public DateTime DatePlaced { get; set; }
        public string Status { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<ProductItem> ProductItems { get; set; }
        public virtual OrderShipping OrderShipping { get; set; }
        
    }
    
}