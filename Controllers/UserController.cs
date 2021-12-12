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
using Microsoft.Extensions.DependencyInjection;

namespace CTrace.Controllers
{
    public class UserController : Controller
    {

        private DataContext _dbConnection;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UserController(DataContext ctx, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _dbConnection = ctx;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            Guid userid = new Guid(User.Claims.First(c => c.Type == "ID").Value);
            var contacts =  await GetContactStatus(userid);
            return View(contacts);
        }
        [Authorize]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken, ActionName("Contact")]
        public IActionResult Contact_post(string name, string mobile, DateTime contacttime)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(mobile) || contacttime == null)
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
                User primiryUser = _dbConnection.Users.Find(new Guid(User.Claims.First(c => c.Type == "ID").Value));
                User secondUser = _dbConnection.Users.First(u => u.user_mobile == mobile);
                Contact tempContect = new Contact { PrimaryUser = primiryUser, SecondUser = secondUser, CreatedBy = primiryUser, Time = contacttime, Reported = DateTime.Now };
                _dbConnection.Contacts.Add(tempContect);
                var Notification = new Notification() { User = secondUser, notif_redirect = "/user", notif_text = $"{primiryUser.user_name} has reported contact on {contacttime.ToString("dd MMM yyyy hh:mm:ss")}" };
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
                    user_name = name,
                    user_salt = salt,
                    user_pass = passwordhash,
                    user_isadmin = false
                };

                _dbConnection.Users.Add(tempUser);
                var userID = new Guid(User.Claims.First(c => c.Type == "ID").Value);
                User primiryUser = _dbConnection.Users.Find(userID);
                Contact tempContect = new Contact { PrimaryUser = primiryUser, SecondUser = tempUser, CreatedBy = primiryUser, Time = contacttime, Reported = DateTime.Now };
                _dbConnection.Contacts.Add(tempContect);
                var Notification = new Notification() { User = tempUser, notif_redirect = "/user", notif_text = $"{primiryUser.user_name} has reported contact" };
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
            //User pUser = _dbConnection.Users.Find(userid);
            var detection = new Detection() { CreatedById = userid, UserId= userid, DetectedAt= detectttime };
            _dbConnection.Detections.Add(detection);
            // Send Notification to people in contact in last 15 days.
            var query = from contact in _dbConnection.Set<Contact>().Where(c => c.PrimaryUserId == userid || c.SecondUserId == userid && c.Time.AddDays(15) >= detectttime)
                        select new Notification {
                            notif_created = DateTime.Now,
                            notif_redirect = "/user",
                            notif_text = $"Your Recent Contact {(contact.PrimaryUserId == userid ? contact.PrimaryUser:contact.SecondUser).user_name} as reported positive detection on {detectttime.ToString("dd MMM yyyy hh:mm:ss")}",
                            User = contact.PrimaryUserId == userid ? contact.SecondUser : contact.PrimaryUser,
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

        [Authorize,ActionName("notifclear")]
        public JsonResult NotificationClear()
        {
            Guid userid = new Guid(User.Claims.First(c => c.Type == "ID").Value);
            var notifs = _dbConnection.Notifications.Where(n => n.UserId == userid).ToList();
            _dbConnection.Notifications.RemoveRange(notifs);
            _dbConnection.SaveChanges();
            return Json(true);
        }
        [Authorize,HttpPost,ValidateAntiForgeryToken,ActionName("status")]
        public async Task<JsonResult> ContactStatus([FromBody] List<string> userids)
        {
            if(userids.Count() <= 0)
            {
                Response.StatusCode = 400;
                return Json("Error");
            }
            IEnumerable<Task<ContStatus>> ContactSQuery =
               from userid in userids
               select GetContactStatus(new Guid(userid));


            List<Task<ContStatus>> ContactTasks = ContactSQuery.ToList();
            var ContactResults = await Task.WhenAll(ContactTasks);
            
            return Json(ContactResults);
        }
        private async Task<ContStatus> GetContactStatus(Guid userid)
        {
            ContStatus ct;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _pr_db = scope.ServiceProvider.GetService<DataContext>();
                var query = from contact in _pr_db.Set<Contact>().Where(C => (C.PrimaryUserId == userid || C.SecondUserId == userid) && C.Reported >= DateTime.Now.AddDays(-15)).OrderByDescending(c => c.Time)
                            let detection = _pr_db.Set<Detection>().Where(d => (contact.PrimaryUserId == userid ? (d.UserId == contact.SecondUserId) : (d.UserId == contact.PrimaryUserId))).OrderByDescending(d => d.DetectedAt).FirstOrDefault()
                            select new ContactVM
                            {
                                Userid = contact.PrimaryUserId == userid ? contact.SecondUserId : contact.PrimaryUserId,
                                Name = (contact.PrimaryUserId == userid ? contact.SecondUser : contact.PrimaryUser).user_name,
                                Time = contact.Time,
                                Detected = detection != null ? true : false,
                            };
                IEnumerable<ContactVM> contacts = await query.ToListAsync();
                ct = new ContStatus() { userid = userid, Contacts = contacts };
            }           
            return ct;
        }
    }
}
