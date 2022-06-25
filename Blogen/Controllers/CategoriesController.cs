using blogen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace blogen.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Title = "Categories";
            List<Category> categoriesList = db.Categories.ToList();
            return View(categoriesList);
        }

        public ActionResult Search(string searchFilter)
        {
            ViewBag.Title = "Categories";
            ViewBag.searchFilter = searchFilter;
            List<Category> categoriesList = db.Categories
                .Where( c => c.Title.Contains(searchFilter))
                .ToList();
            return View("index",categoriesList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Create Category";
            return View();
        }
        [HttpPost]
        public ActionResult Create( Category category)
        {
            if (ModelState.IsValid)
            {
                category.UpdateDate = DateTime.Now;

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Edit Category";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.UpdateDate = DateTime.Now;

                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            ViewBag.Title = "Delete Category";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(category);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}