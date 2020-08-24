namespace Oshop.API.Models
{
    public class OrderShipping
    {
        public string Id { get; set; }
        public string ShippingName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string  City { get; set; }
        public string State { get; set; }
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}