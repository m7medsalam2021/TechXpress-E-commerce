using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SliderImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; } //ملهاش لزمه
        [DisplayName("Sort Order")]
        public int SortOrder { get; set; }
    }
}
