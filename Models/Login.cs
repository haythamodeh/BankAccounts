using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccounts.Models
{
    public class Login
    {
        [NotMapped]
        [Required(ErrorMessage = "The Field is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "The Field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}