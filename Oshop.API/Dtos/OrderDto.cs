using System;
using System.Collections.Generic;

namespace Oshop.API.Dtos
{
    public class OrderDto
    {
        public string Id { get; set; }
        public DateTime? DatePlaced { get; set; } = DateTime.Now;
        public string Status { get; set; } = "pending";
        public string UserId { get; set; }
        public virtual List<ProductItemDto> ProductItems { get; set; }
        public virtual OrderShippingDto OrderShipping { get; set; }
        public string OrderShippingId { get; set; }
    }
}