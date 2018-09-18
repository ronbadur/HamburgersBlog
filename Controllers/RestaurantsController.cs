﻿using System;
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
    public class RestaurantsController : Controller
    {
        private ProjectContext db = new ProjectContext();


        // GET: Restaurants
        public ActionResult Index()
        {
            var restaurants = db.Restaurants;
            return View(restaurants.ToList());
        }

        // GET: Restaurants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Restaurant restaurant = db.Restaurants.Find(id);

            if (restaurant == null)
            {
                return HttpNotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Filter
        public ActionResult Filter(string area, string restaurantName, int greaterRateThan)
        {
            var results = db.Restaurants.AsQueryable();
            Area areaE;

            if (!string.IsNullOrEmpty(restaurantName))
            {
                results = results.Where(restaurant => restaurant.Name.ToLower().IndexOf(restaurantName.ToLower()) > -1);
            }

            if (!string.IsNullOrEmpty(area) && Enum.TryParse(area, out areaE))
            {
                results = results.Where(restaurant => restaurant.Area == areaE);
            }

            results = results.Where(p => p.Rate > greaterRateThan);

            return View("Index", results.ToList());
        }
    }
}