using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using beltexam.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace beltexam.Controllers
{
    public class LoginController : Controller
    {

        private BeltContext _context;
 
        public LoginController(BeltContext context)
        {
            _context = context;
        }
 
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
            // Other code
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if(ModelState.IsValid){
                User NewUser = new User {
                    FirstName=model.FirstName,
                    LastName=model.LastName,
                    Email=model.Email, 
                    Password=model.Password
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                _context.Add(NewUser);
                _context.SaveChanges();
                User thisuser= _context.Users.SingleOrDefault(a => a.Email == model.Email);
                HttpContext.Session.SetInt32("id",(int)thisuser.UserId);
                return Redirect("/Home");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(string email, string password){
            User thisuser= _context.Users.SingleOrDefault(a => a.Email == email);
            if(thisuser != null){
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(thisuser, thisuser.Password, password))
                {
                    HttpContext.Session.SetInt32("id",(int)thisuser.UserId);
                    return Redirect("/Home");
                    //Handle success
                }
                
            }
            ViewBag.Error = "Invalid login information!";
            ViewBag.TestCase = 2; 
            return View("Index");
        }

        [HttpGet]
        [Route("/logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}