using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Emial { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
