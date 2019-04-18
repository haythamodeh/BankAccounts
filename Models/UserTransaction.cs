namespace BankAccounts.Models
{
    public class UserTransaction
    {
        public User User {get; set;}
        public Transaction Transaction {get; set;}
    }
}