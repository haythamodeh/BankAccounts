using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccounts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private BankAccountsContext dbContext;

        public HomeController(BankAccountsContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            int? userID = HttpContext.Session.GetInt32("userID");
            if(userID == null)
            {
                TempData["error"] = "You need to be logged in to view this page";
                return RedirectToAction("Login");
            }
            User retrievedUser = dbContext.Users
                .Include(u => u.TransactionsMade)
                .FirstOrDefault(u => u.UserID == userID);
            
            UserTransaction aUserTransaction = new UserTransaction();
            aUserTransaction.User = retrievedUser;
            return View("success", aUserTransaction);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("login");
        }

        [HttpPost("login")]
        public IActionResult LoginValidate(Login user)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("login");
                }
                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(user, userInDb.Password, user.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("login");
                }   
                HttpContext.Session.SetInt32("userID", userInDb.UserID);
                return RedirectToAction("Success");
            }
            return View("login");
        }

        [HttpPost("create")]
        public IActionResult CreateUser(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already taken");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                return RedirectToAction("Login");
            }
            else
                return View("Index");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }




        //////////////////////////////////////////////////////




        [HttpPost("new/amount")]
        public IActionResult Newtransaction(Transaction newtransaction)
        {
            int? userID = HttpContext.Session.GetInt32("userID");
            if(userID == null) return RedirectToAction("Login");

            User retrievedUser = dbContext.Users
                .Include(u => u.TransactionsMade)
                .FirstOrDefault(u => u.UserID == userID);

            float account = 0;
            foreach(var i in retrievedUser.TransactionsMade)
            {
                account += i.Amount;
            }
            if(newtransaction.Amount > 0)
            {
                newtransaction.UserID = (int)userID;
                dbContext.Add(newtransaction);
                dbContext.SaveChanges();
                return RedirectToAction("Success");
            }
            else if(newtransaction.Amount < 0)
            {
                if(account - (-1 * newtransaction.Amount) < 0)
                {
                    System.Console.WriteLine("Not enough money;");
                    TempData["InvalidWithdrawal"] = "Insufficent Funds";
                    return RedirectToAction("Success");
                }
                else
                {
                    account -= (-1 * newtransaction.Amount);
                    newtransaction.UserID = (int)userID;
                    dbContext.Add(newtransaction);
                    dbContext.SaveChanges();
                    return RedirectToAction("Success");
                }
            }
            else
            {
                System.Console.WriteLine("failed transaction submission");
                return RedirectToAction("Success");
            }


        }

    }
}
