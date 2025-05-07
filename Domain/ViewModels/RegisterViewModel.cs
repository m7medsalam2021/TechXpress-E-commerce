using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="FullName is Required")]
        public string FullName { get; set; }
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Password Not Match")]

        public string ConfirmPassword { get; set; }
    }
}
