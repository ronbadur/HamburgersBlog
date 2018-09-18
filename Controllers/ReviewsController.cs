
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
    public class ReviewsController : Controller
    {
        private ProjectContext db = new ProjectContext();


        // GET: Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews;
            return View(reviews.ToList());
        }

        // GET: Reviews/Create
        public ActionResult Add(int ResturantId)
        {
            var newReview = new Review();
            newReview.RestaurantID = ResturantId; // this will be sent from the ArticleDetails View, hold on :).

            return PartialView(newReview);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.ResturantId = new SelectList(db.Restaurants, "ResturantId", "Name");
            return View();
        }

        // POST: Review/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RestaurantID,Title,AuthorName,Content")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("../Restaurants/Index");
            }
            return View(review);
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