using System.Collections.Generic;

namespace Oshop.API.Models
{
    public class Cart
    {
        public string Id { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}