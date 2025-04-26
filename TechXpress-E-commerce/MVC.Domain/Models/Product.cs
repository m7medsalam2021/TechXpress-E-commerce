using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
