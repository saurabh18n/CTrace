using CTrace.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTrace.Controllers
{
    public class UserController : Controller
    {

        private DataContext _dbConnection;
        public UserController(DataContext ctx)
        {
            _dbConnection = ctx;
        }
        public async Task<IActionResult> Index()
        {
            var contacts = await _dbConnection.Contacts.Where(C => C.PrimaryUser.user_name == User.Identity.Name).ToListAsync();
            return View(contacts);
        }
        [Authorize]
        public IActionResult Contact()
        {
            DateTime Now = DateTime.Now;           
            return View();
        }
        [Authorize,ValidateAntiForgeryToken,ActionName("Contact")]
        public IActionResult Contact_post(string fname, string lname, string mobile,DateTime contacttime)
        {
            if(string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname) || string.IsNullOrEmpty(mobile) ||  contacttime == null)
            {
                ViewBag.ErrorMessage = "Missing Values Please try again";
                return View();
            }

            if(_dbConnection.Users.Any(u => u.user_name == mobile))
            {

            }
            else
            {

            }


            return View();
        }
    }
}
