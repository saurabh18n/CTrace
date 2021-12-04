using CTrace.Data;
using CTrace.Helpers;
using CTrace.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CTrace.Controllers
{
    public class AccountController : Controller
    {
        private DataContext _dbConnection;
        public AccountController(DataContext ctx)
        {
            _dbConnection = ctx;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ActionName("login"), ValidateAntiForgeryToken]
        async public Task<IActionResult> LoginPost(string username, string password, [FromQuery] string returnurl)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Username Or Password Empty";
                return View("login");
            }
            var dbUser = _dbConnection.Users.Where(u => u.user_name == username)
                      .FirstOrDefault<User>();

            if (dbUser == null)
            {
                ViewBag.ErrorMessage = "Username Or Password Incorrect";
                return View("login");
            }
            if (dbUser.user_failcount > 4)
            {
                ViewBag.ErrorMessage = "Account Locked due to Multiple login fails";
                return View("login");
            }

            bool isAuthenticated = PasswordHasher.VerifyHash(password, dbUser.user_pass, dbUser.user_salt);

            if (!isAuthenticated)
            {
                dbUser.user_failcount = ++dbUser.user_failcount;
                dbUser.user_lastlogin = DateTime.Now;
                _dbConnection.SaveChanges();
                ViewBag.ErrorMessage = $"Username Or Password Incorrect, Account will be locked after {5 - dbUser.user_failcount} Invalid Login Attempt";
                return View("login");
            }
            else
            {
                dbUser.user_failcount = 0;
                dbUser.user_lastlogin = DateTime.Now;
                await _dbConnection.SaveChangesAsync();

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, dbUser.user_name),
                        new Claim("FullName",$"{dbUser.user_fname} {dbUser.user_lname}"),
                        new Claim("ID",dbUser.user_id.ToString()),
                        new Claim(ClaimTypes.Role,(Boolean)dbUser.user_isadmin ? "Administrator" : "User"),
                    };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                if (string.IsNullOrEmpty(returnurl))
                {
                    return Redirect(Url.Action("index", "home"));
                }
                else
                {
                    return Redirect(returnurl);
                }

            }
        }
    }
}
