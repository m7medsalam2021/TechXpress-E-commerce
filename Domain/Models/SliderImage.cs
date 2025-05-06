using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SliderImage
    {
        public int Id { get; set; }

        [ValidateNever]
        public string Image { get; set; }

        public string Title { get; set; } //ملهاش لزمه
        [DisplayName("Sort Order")]
        [Range(1, 10, ErrorMessage = "Sort Order must be between 1 and 10.")]
        [Required(ErrorMessage = "Sort Order is required.")]
        public int SortOrder { get; set; }
    }
}
