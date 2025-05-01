using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public string Image { get; set; }
        [DisplayName("Category")]
        public int? CategoryId { get; set; } // Foreign key to Category
        public Category Category { get; set; } // Navigation property to Category
    }
}
