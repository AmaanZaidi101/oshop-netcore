using System.Collections.Generic;
using System.Linq;

namespace Oshop.API.Dtos
{
    public class CartDto
    {
        public string Id { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public int TotalQuantity 
        {
            get { return CartItems.Sum(x => x.Quantity); }
        }

        public decimal TotalPrice
        {
            get { return CartItems.Sum(x => x.TotalPrice); }
        }

    }
}