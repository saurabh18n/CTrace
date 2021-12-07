using CTrace.Data;
using CTrace.Helpers;
using CTrace.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var dbUser = _dbConnection.Users.Where(u => u.user_mobile == username)
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
                        new Claim(ClaimTypes.Name, dbUser.user_mobile),
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

        async public Task<IActionResult> LoginPost(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Username Or Password Empty";
                return View("login");
            }
            var dbUser = _dbConnection.Users.Where(u => u.user_mobile == username)
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
                        new Claim(ClaimTypes.Name, dbUser.user_mobile),
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

                ClaimsPrincipal cp = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    cp,
                    authProperties);

                return Redirect(Url.Action("index", "home"));


            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(Url.Action("index", "home"));
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous, HttpPost, ActionName("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register_post(string mobile, string password, string fname, string lname)
        {
            if(string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname))
            {
                ViewBag.ErrorMessage = "Missing Field Values";
                return View("register");
            }

            var alreadyCount = await _dbConnection.Users.CountAsync(u => u.user_mobile == mobile);

            if (alreadyCount > 0)
            {
                ViewBag.ErrorMessage = "Mobile Number Already Exists";
                return View("register");
            }

            string salt;
            string passwordhash = PasswordHasher.GenrateHash(password, out salt);
            await _dbConnection.Users.AddAsync(new User
            {
                user_mobile = mobile,
                user_fname = fname,
                user_lname = lname,
                user_salt = salt,
                user_pass = passwordhash,
                user_isadmin = false
            });
            _dbConnection.SaveChanges();
            return await LoginPost(mobile, password);
        }
    }
}
