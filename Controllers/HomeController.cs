﻿using System;
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

        // GET: Home/Details/5
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

            // Incerment the counter for this restaurant
            int restaurantId = post.Restaurant.RestaurantID;
            string restaurantName = db.Restaurants.First(r => r.RestaurantID == restaurantId).Name;
            Dictionary<string, int> counterDictionary = (Dictionary<string, int>)Session["counterDictionary"];
            if (counterDictionary == null)
            {
                counterDictionary = new Dictionary<string, int>();
            }
            if (counterDictionary.ContainsKey(restaurantName))
            {
                counterDictionary[restaurantName]++;
            }
            else
            {
                counterDictionary.Add(restaurantName, 1);
            }
            Session["counterDictionary"] = counterDictionary;

            return View(post);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            var allRestaurants = db.Restaurants;
            this.ViewData["restaurantsSelectable"] = (IEnumerable<HamburgersBlog.Models.Restaurant>)allRestaurants;
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,Title,AuthorName,RestaurantID,Content")] Post post)
        {
            post.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var allRestaurants = db.Restaurants;
            this.ViewData["restaurantsSelectable"] = (IEnumerable<HamburgersBlog.Models.Restaurant>)allRestaurants;
            return View(post);
        }

        // GET: Home/Edit/5
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

            var allRestaurants = db.Restaurants;
            this.ViewData["restaurantsSelectable"] = (IEnumerable<HamburgersBlog.Models.Restaurant>)allRestaurants;
            return View(post);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "Admin")]
        public ActionResult Edit([Bind(Include = "PostID,Title,AuthorName,RestaurantID,Restaurant,Date,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var allRestaurants = db.Restaurants;
            this.ViewData["restaurantsSelectable"] = (IEnumerable<HamburgersBlog.Models.Restaurant>)allRestaurants;
            return View(post);
        }

        // GET: Home/Delete/5
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

        // POST: Home/Delete/5
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

        // GET: Home/Filter
        public ActionResult Filter(string area, string authorName, string restaurantName)
        {
            var results = db.Posts.AsQueryable();
            Area areaE;

            if (!string.IsNullOrEmpty(restaurantName))
            {
                results = from post in db.Posts
                          join restaurant in db.Restaurants on post.RestaurantID equals restaurant.RestaurantID
                          where restaurant.Name == restaurantName
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
                             select new GroupByPrincessModel() { RestaurantName = restaurant.Name, TotalPosts = g.Sum(p => 1) };

            return View(totalPosts.ToList());
        }

        [HttpGet]
        public ActionResult GroupByRestaurantData()
        {
            // Group by and join
            var totalPosts = from post in db.Posts
                             group post by post.RestaurantID into g
                             join restaurant in db.Restaurants on g.Key equals restaurant.RestaurantID
                             select new GroupByPrincessModel() { RestaurantName = restaurant.Name, TotalPosts = g.Sum(p => 1) };

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
