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
    public class HomeController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // POST: Home/Filter
        public ActionResult Filter(string area, string restaurantName, string authorName)
        {
            var results = db.Posts.AsQueryable();
            Area areaE;

            if (!string.IsNullOrEmpty(restaurantName))
            {
                results = from post in db.Posts
                          join restaurant in db.Restaurants on post.RestaurantID equals restaurant.RestaurantID
                          where restaurant.Name.ToLower().Contains(restaurantName.ToLower())
                          select post;
            }

            if (!string.IsNullOrEmpty(area) && Enum.TryParse(area, out areaE))
            {
                results = results.Where(post => post.Restaurant.Area == areaE);
            }

            if (!string.IsNullOrEmpty(authorName))
            {
                results = results.Where(post => post.AuthorName.ToLower().IndexOf(authorName.ToLower()) > -1);
            }


            return View("Index", results.ToList());
        }

        public ActionResult GroupByRestaurant()
        {
            // Group by and join
            var totalPosts = from post in db.Posts
                             group post by post.RestaurantID into g
                             join restaurant in db.Restaurants on g.Key equals restaurant.RestaurantID
                             select new GroupByRestaurantModel() { RestaurantName = restaurant.Name, TotalPosts = g.Sum(p => 1) };

            return View(totalPosts.ToList());
        }

        [HttpGet]
        public ActionResult GroupByRestaurantData()
        {
            // Group by and join
            var totalPosts = from post in db.Posts
                             group post by post.RestaurantID into g
                             join restaurant in db.Restaurants on g.Key equals restaurant.RestaurantID
                             select new GroupByRestaurantModel() { RestaurantName = restaurant.Name, TotalPosts = g.Sum(p => 1) };

            return Json(totalPosts.ToList(), JsonRequestBehavior.AllowGet);
        }

        // Get Home/WantMore
        [HttpPost]
        public ActionResult WantMore(string restaurantId, string currentPostId)
        {
            try
            {
                int postId = Int32.Parse(currentPostId);
                int priId = Int32.Parse(restaurantId);
                var alikePost = (from post in db.Posts
                                 where post.PostID != postId && post.RestaurantID == priId
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
