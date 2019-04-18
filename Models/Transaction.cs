using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class Transaction 
    {
        [Key]
        public int TransactionID { get; set; }

        [Required(ErrorMessage = "Please enter an amount")]
        public float Amount { get; set; }

        [Required]
        public DateTime created_at {get; set;} = DateTime.Now;

        public int UserID {get; set;}
        public User User {get; set;}
    }
}