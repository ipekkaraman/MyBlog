using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;
using PagedList;

namespace MvcBlog.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ViewResult Index(int? page)
        {
            var posts = from s in db.Posts
                        select s;
            posts = posts.OrderByDescending(s => s.Createdtime);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
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

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Content,Createdtime,Author")] Post post)
        {
            string Path=" ";
           
            if (Request.Files.Count > 1)
            {
               for (int i = 0; i < Request.Files.Count; i++) {
                 string fileName = Guid.NewGuid().ToString().Replace("-", "");

                 string extension = System.IO.Path.GetExtension(Request.Files[i].FileName);
                 Path = "~/Images/" + fileName + extension;

                  Request.Files[i].SaveAs(Server.MapPath(Path));

                  MvcBlog.Models.Image image = new MvcBlog.Models.Image();
                  image.Path = Path;
                  image.Post = post;
                  db.Images.Add(image);
                  post.Images.Add(image);
                 }
             }
            if (ModelState.IsValid)
            { 
                db.Posts.Add(post);
                db.SaveChanges();
                 
                return RedirectToAction("Index");
            }

            return View(post);
        }

             
        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
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

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
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

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);

            //delete images of the post
            for(int i=0;i<post.Images.Count;i++)
            {
                Image image=post.Images.ElementAt(i);
                db.Images.Remove(image);
            }
            //delete comments of the post
            for (int i = 0; i < post.Comments.Count; i++)
            {
                Comment comment = post.Comments.ElementAt(i);
                db.Comments.Remove(comment);
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
    }
}
