﻿
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

                // Find from text if it's a positive review
                bool isRecomended = 
                    RestuarantRecommendationByNLP.Instance.IsPositiveReview(Request, Response, review.Title + " " + review.Content);

                // Change restuarnt recomendation
                SaveIsRecomendedForRestuarant(review.RestaurantID, isRecomended);

                RestaurantInterest.Instance.AddUserInterestInRestaurant(Request, Response, review.RestaurantID, 3);
                
                return RedirectToAction("../Restaurants/Details", new { id = review.RestaurantID });
            }
            return View(review);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }

            return View(review);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult Edit([Bind(Include = "ReviewID,Title,AuthorName,RestaurantID,Content")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();

                // Find from text if it's a positive review
                bool isRecomended =
                    RestuarantRecommendationByNLP.Instance.IsPositiveReview(Request, Response, review.Title + " " + review.Content);

                // Change restuarnt recomendation
                SaveIsRecomendedForRestuarant(review.RestaurantID, isRecomended);

                return RedirectToAction("../Restaurants/Details", new { id = review.RestaurantID });
            }
            return View(review);
        }

        // GET: Posts/Delete/5
        [Authorize(Users = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Users = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("../Restaurants/Details", new { id = review.RestaurantID });
        }

        private void SaveIsRecomendedForRestuarant(int restId, bool isRecomended)
        {
            Restaurant restaurant = db.Restaurants.Find(restId);
            restaurant.RecommendationScore += isRecomended ? 1 : -1;
            db.Entry(restaurant).State = EntityState.Modified;
            db.SaveChanges();
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