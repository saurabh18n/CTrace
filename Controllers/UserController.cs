using CTrace.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTrace.Models;
using CTrace.ViewModels;
using CTrace.Helpers;

namespace CTrace.Controllers
{
    public class UserController : Controller
    {

        private DataContext _dbConnection;
        public UserController(DataContext ctx)
        {
            _dbConnection = ctx;
        }

        [Authorize]
        public IActionResult Index()
        {

            //var query = from contact in _dbConnection.Set<Contact>().Where(C => C.PrimaryUser.user_mobile == User.Identity.Name && C.Reported >= DateTime.Now.AddDays(-15))
            //            join detection in _dbConnection.Set<Detection>().Where(d => d.DetectedAt >= DateTime.Now.AddDays(-15))
            //            on contact.SecondUserId equals detection.UserId
            //            into tempt
            //            from tc in tempt.DefaultIfEmpty()

            //            select new ContactVM
            //            {
            //                Name = contact.SecondUser.user_fname + " " + contact.SecondUser.user_lname,
            //                Time = contact.Time,
            //                Detected = tc != null ? tc.DetectedAt.ToString() : string.Empty
            //            };
            //var contacts1 = query.ToList<ContactVM>();


            var query2 = from contact in _dbConnection.Set<Contact>().Where(C => C.SecondUser.user_mobile == User.Identity.Name && C.Reported >= DateTime.Now.AddDays(-15))
                         join detection in _dbConnection.Set<Detection>().Where(d => d.DetectedAt >= DateTime.Now.AddDays(-15))
                         on contact.PrimaryUserId equals detection.UserId into tempt
                         from tc in tempt.DefaultIfEmpty()
                         select new ContactVM
                         {
                             Name = contact.PrimaryUser.user_fname + " " + contact.PrimaryUser.user_lname,
                             Time = contact.Time,
                             Detected = tc != null ? tc.DetectedAt.ToString() : string.Empty
                         };
            var contacts2 = query2.ToList<ContactVM>();

            //contacts1.AddRange(contacts2);
            //var Contacts = contacts1.OrderByDescending(c => c.Time);
            return View(contacts2);
        }
        [Authorize]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken, ActionName("Contact")]
        public IActionResult Contact_post(string fname, string lname, string mobile, DateTime contacttime)
        {
            if (string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname) || string.IsNullOrEmpty(mobile) || contacttime == null)
        {
                ViewBag.ErrorMessage = "Missing/Invalid Values Please try again";
                return View();
            }
            else if (contacttime > DateTime.Now)
            {
                ViewBag.ErrorMessage = "Can not report future Contact";
                return View();
            }

            if (_dbConnection.Users.Any(u => u.user_mobile == mobile))
            {
                User primiryUser = _dbConnection.Users.Find(Convert.ToInt32(User.Claims.First(c => c.Type == "ID").Value));
                User secondUser = _dbConnection.Users.First(u => u.user_mobile == mobile);
                Contact tempContect = new Contact { PrimaryUser = primiryUser, SecondUser = secondUser, CreatedBy = primiryUser, Time = contacttime, Reported = DateTime.Now };
                _dbConnection.Contacts.Add(tempContect);
                var Notification = new Notification() { User = secondUser, notif_redirect = "/user", notif_text = $"{primiryUser.user_fname} {primiryUser.user_lname} has reported contact" };
                _dbConnection.Notifications.Add(Notification);
                _dbConnection.SaveChanges();
            }
            else
            {
                //Create New User
                string salt;
                string passwordhash = PasswordHasher.GenrateHash(mobile, out salt);
                User tempUser = new User
                {
                    user_mobile = mobile,
                    user_fname = fname,
                    user_lname = lname,
                    user_salt = salt,
                    user_pass = passwordhash,
                    user_isadmin = false
                };

                _dbConnection.Users.Add(tempUser);
                var userID = new Guid(User.Claims.First(c => c.Type == "ID").Value);
                User primiryUser = _dbConnection.Users.Find(userID);
                Contact tempContect = new Contact { PrimaryUser = primiryUser, SecondUser = tempUser, CreatedBy = primiryUser, Time = contacttime, Reported = DateTime.Now };
                _dbConnection.Contacts.Add(tempContect);
                var Notification = new Notification() { User = tempUser, notif_redirect = "/user", notif_text = $"{primiryUser.user_fname} {primiryUser.user_lname} has reported contact" };
                _dbConnection.Notifications.Add(Notification);
                _dbConnection.SaveChanges();
            }
            return View();
        }

        [Authorize, ActionName("detection")]
        public IActionResult Detection()
        {
            return View();
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken, ActionName("detection")]
        public IActionResult Detection_post(DateTime detectttime)
        {
            if (detectttime.AddDays(15) < DateTime.Now)
            {
                ViewBag.ErrorMessage = "Already Passed 15 Days Since Detection";
                return View();
            }
            else if (detectttime > DateTime.Now)
            {
                ViewBag.ErrorMessage = "Can not report future Detection";
                return View();
            }
            Guid userid = new Guid(User.Claims.First(c => c.Type == "ID").Value);
            User pUser = _dbConnection.Users.Find(userid);
            var detection = new Detection() { CreatedBy = pUser, User=pUser,DetectedAt= detectttime };
            _dbConnection.Detections.Add(detection);
            // Send Notification to people in contact in last 15 days.
            var query = from contact in _dbConnection.Set<Contact>().Where(c => c.PrimaryUser == pUser || c.SecondUser == pUser && c.Time.AddDays(15) >= detectttime)
                        select new Notification {
                            notif_created = DateTime.Now,
                            notif_redirect = "/user",
                            notif_text = $"{(contact.PrimaryUser == pUser ? contact.PrimaryUser:contact.SecondUser).user_fname} { (contact.PrimaryUser == pUser ? contact.PrimaryUser : contact.SecondUser).user_lname }  as reported positive detection",
                            User = contact.PrimaryUser == pUser ? contact.SecondUser : contact.PrimaryUser,
                        };
            var notifications = query.ToList();
            _dbConnection.Notifications.AddRange(notifications);
            _dbConnection.SaveChanges();
            return View();
        }

        [Authorize]
        public JsonResult Notification()
        {
            Guid userid =new Guid(User.Claims.First(c => c.Type == "ID").Value);
            var notifs = _dbConnection.Notifications.Where(n=> n.UserId == userid).ToList();
            return Json(notifs);
        }
    }
}
