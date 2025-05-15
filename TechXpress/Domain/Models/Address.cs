using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string AddressLine { get; set; }


        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
    }
}