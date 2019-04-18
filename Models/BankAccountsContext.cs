using Microsoft.EntityFrameworkCore;
 
namespace BankAccounts.Models
{
    public class BankAccountsContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public BankAccountsContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get; set;}
    }
}