using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using blogen.Models;
using PagedList;

namespace blogen.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index(int? page, int? PageSize)
        {
            ViewBag.Title = "Posts";

            // Check if it's the first request to posts action
            int pageIndex = page.HasValue ? (int) page : 1;
            int defaSize = (PageSize ?? 5);
            ViewBag.psize = defaSize;

            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "5",Text="5"},
                new SelectListItem() { Value = "10",Text="10"},
                new SelectListItem() { Value = "15",Text="15"},
                new SelectListItem() { Value = "20",Text="20"},
                new SelectListItem() { Value = "25",Text="25"}
            };
            var posts = db.Posts.Include(p => p.Category).OrderBy(p => p.ID).ToPagedList(pageIndex, defaSize);
            return View(posts);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Title = "Post Details";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(c => c.Category).Where(p => p.ID == id).FirstOrDefault();

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create Post";
            ViewBag.CatID = new SelectList(db.Categories.ToList(), "Id", "Title");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create([Bind(Include = "ID,Title,CatID,Body,PostImage")] Post post)
        {
            if (ModelState.IsValid)
            {
                if(post.PostImage!=null) {
                string path = Path.Combine( AppDomain.CurrentDomain.BaseDirectory ,
                    ConfigurationManager.AppSettings["PostsImagePath"].ToString() );

                string fileName = Path.GetFileNameWithoutExtension(post.PostImage.FileName);

                // To Get File Entension
                string FileExtension = Path.GetExtension(post.PostImage.FileName);

                fileName = DateTime.Now.ToString("yyyyMM dd") + "-" + fileName.Trim() + FileExtension;

                // Its now the time to complete the path to store in server
                post.ImagePath = Path.Combine(path, fileName);

                post.PostImage.SaveAs(post.ImagePath);

                }


                post.UpdateDate = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatID = new SelectList(db.Categories, "Id", "Title", post.CatID);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Edit";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatID = new SelectList(db.Categories, "Id", "Title", post.CatID);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,CatID,UpdateDate,body,ImagePath,postImage")] Post post)
        {
            if (ModelState.IsValid)
            {
                if ( System.IO.File.Exists(post.ImagePath) )
                {
                    System.IO.File.Delete(post.ImagePath);
                }
                if (post.PostImage != null)
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                ConfigurationManager.AppSettings["PostsImagePath"].ToString());

                    string fileName = Path.GetFileNameWithoutExtension(post.PostImage.FileName);

                    // To Get File Entension
                    string FileExtension = Path.GetExtension(post.PostImage.FileName);

                    fileName = DateTime.Now.ToString("yyyyMM dd") + "-" + fileName.Trim() + FileExtension;

                    // Its now the time to complete the path to store in server
                    post.ImagePath = Path.Combine(path, fileName);

                    post.PostImage.SaveAs(post.ImagePath);
                }

                post.UpdateDate = DateTime.Now;

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatID = new SelectList(db.Categories, "Id", "Title", post.CatID);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Title = "Delete";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            if (post.PostImage != null)
            {
                System.IO.File.Delete(post.ImagePath);
            }
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetAll()
        {
            var posts = db.Posts.Select(
                 p => new
                 {
                     ID = p.ID,
                     Title =  p.Title,
                    Category =  p.Category.Title,
                    UpdateDate = p.UpdateDate.ToString(),
                 }
                 ).ToList();

            return Json( new { data = posts }, JsonRequestBehavior.AllowGet);
        }
    }
}
