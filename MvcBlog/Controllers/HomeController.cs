using System.Linq;
using System.Net;
using System.Web.Mvc;
using MvcBlog.Models;
using PagedList;

namespace MvcBlog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ViewResult Index(int? page)
        {
            var posts = from s in db.Posts
                           select s;
            posts = posts.OrderByDescending(s => s.Createdtime);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
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
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details([Bind(Include = "ID,Content,Datetime")] Comment comment,int? id)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.Posts.Find(id).Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details");
            }

            return View(comment);
        }
    }
}