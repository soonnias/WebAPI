using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
