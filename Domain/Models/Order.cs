using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int AddressId { get; set; } // Address ID to associate the order with a specific address
        public Address Address { get; set; } // Navigation property to the address

        public double Amount { get; set; } // Total amount of the order
        public string status { get; set; } // Status of the order (e.g., Pending, Completed, Cancelled)
        public DateTime CreatedAt { get; set; }

        public List<OrderProduct> OrderProducts { get; set; } // List of products in the order
    }
}
