using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using blogen.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace blogen.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Users
        public ActionResult Index()
        {
            ViewBag.Title = "Users";
            List<ApplicationUser> users = db.Users.ToList();
            return View(users);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Add User";
            return View();
        }
        public ActionResult Delete(string id)
        {
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);

        }
        [HttpPost]
        public ActionResult DeleteConfirmed(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.FindById(id);
            userManager.Delete(user);
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.Title = "Login in";
            return View();
        }


    }
}