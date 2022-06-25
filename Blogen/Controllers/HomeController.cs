using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using blogen.Models;
namespace blogen.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.CatID = new SelectList(db.Categories.ToList(), "Id", "Title");
            ViewBag.PostsCounter = db.Posts.Count();
            ViewBag.CategoriesCounter = db.Categories.Count();
            ViewBag.UsersCounter = db.Users.Count();
            return View();
        }
        }
    }
