using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HamburgersBlog.DAL;
using HamburgersBlog.Models;

namespace HamburgersBlog.Controllers
{
    public class PostsController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
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

            // Incerment the counter for this princess
            int princessId = post.Princess.PrincessID;
            string princessName = db.Princesses.First(p => p.PrincessID == princessId).Name;
            Dictionary<string, int> counterDictionary = (Dictionary<string, int>)Session["counterDictionary"];
            if (counterDictionary == null)
            {
                counterDictionary = new Dictionary<string, int>();
            }
            if (counterDictionary.ContainsKey(princessName))
            {
                counterDictionary[princessName]++;
            }
            else
            {
                counterDictionary.Add(princessName, 1);
            }
            Session["counterDictionary"] = counterDictionary;

            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            var allPrincesses = db.Princesses;
            this.ViewData["princessesSelectable"] = (IEnumerable<HamburgersBlog.Models.Princess>)allPrincesses;
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,Title,AuthorName,PrincessID,Content")] Post post)
        {
            post.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var allPrincesses = db.Princesses;
            this.ViewData["princessesSelectable"] = (IEnumerable<HamburgersBlog.Models.Princess>)allPrincesses;
            return View(post);
        }

        // GET: Posts/Edit/5
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

            var allPrincesses = db.Princesses;
            this.ViewData["princessesSelectable"] = (IEnumerable<HamburgersBlog.Models.Princess>)allPrincesses;
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "Admin")]
        public ActionResult Edit([Bind(Include = "PostID,Title,AuthorName,PrincessID,Princess,Date,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var allPrincesses = db.Princesses;
            this.ViewData["princessesSelectable"] = (IEnumerable<HamburgersBlog.Models.Princess>)allPrincesses;
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Users = "Admin")]
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

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Posts
        public ActionResult Filter(string princessRoyaltyType, string authorName, string princessName)
        {
            var results = db.Posts.AsQueryable();
            KingdomType royaltyType;
            if (!string.IsNullOrEmpty(princessName))
            {
                results = from post in db.Posts
                          join princess in db.Princesses on post.PrincessID equals princess.PrincessID
                          where princess.Name == princessName
                          select post;
            }
            if (!string.IsNullOrEmpty(princessRoyaltyType) && Enum.TryParse(princessRoyaltyType, out royaltyType))
            {
                results = results.Where(p => p.Princess.RoyaltyType == royaltyType);
            }
            if (!string.IsNullOrEmpty(authorName))
            {
                results = results.Where(p => p.AuthorName == authorName);
            }
            return View("Index", results.ToList());
        }

        public ActionResult GroupByPrincess()
        {
            // Group by and join
            var totalPosts = from post in db.Posts
                             group post by post.PrincessID into g
                             join princess in db.Princesses on g.Key equals princess.PrincessID
                             select new GroupByPrincessModel() { PrincessName = princess.Name, TotalPosts = g.Sum(p => 1) };

            return View(totalPosts.ToList());
        }

        [HttpGet]
        public ActionResult GroupByPrincessData()
        {
            // Group by and join
            var totalPosts = from post in db.Posts
                             group post by post.PrincessID into g
                             join princess in db.Princesses on g.Key equals princess.PrincessID
                             select new GroupByPrincessModel() { PrincessName = princess.Name, TotalPosts = g.Sum(p => 1) };

            return Json(totalPosts.ToList(), JsonRequestBehavior.AllowGet);
        }

        // Get Posts/WantMore
        [HttpPost]
        public ActionResult WantMore(string princessId, string currentPostId)
        {
            try
            {
                int postId = Int32.Parse(currentPostId);
                int priId = Int32.Parse(princessId);
                var alikePost = (from post in db.Posts
                                 where post.PostID != postId && post.PrincessID == priId
                                 select post.PostID).SingleOrDefault();
                if (alikePost != 0)
                {
                    return Json(new Dictionary<string, object> { { "url", "/Home/Details/" + alikePost } });
                }
                else
                {
                    return Json(new Dictionary<string, object> { { "error", "didnt found any" } });
                }
            }
            catch (Exception ex)
            {
                return Json(new Dictionary<string, object> { { "error", ex.Message } });
            }


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
