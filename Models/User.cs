using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccounts.Models
{
    public class User
    {
        [Key]
        public int UserID {get; set;}

        [Required(ErrorMessage = "The First name field is required")]
        [MinLength(2, ErrorMessage = "Length must be at least 2 characters long")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "The Last name field is required")]
        [MinLength(2, ErrorMessage = "Length must be at least 2 characters long")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "The Field is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Must be at least 8 characters long")]
        public string Password { get; set; }

        [Required]
        public DateTime created_at {get; set;} = DateTime.Now;
        [Required]
        public DateTime updated_at {get; set;} = DateTime.Now;
        
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }

        public List<Transaction> TransactionsMade {get; set;}
    }
}