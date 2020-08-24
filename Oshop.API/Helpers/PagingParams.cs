using System;

namespace Oshop.API.Helpers
{
    public class PagingParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value;}
        }
        public string OrderBy { get; set; }
        public bool IsDescending { get; set; } = true;
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CategoryId { get; set; }
        public string ProductId { get; set; }
        public bool? soldOut { get; set; }
    }
}