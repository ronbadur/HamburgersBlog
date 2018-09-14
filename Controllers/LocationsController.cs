using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HamburgersBlog.DAL;
using HamburgersBlog.Models;

namespace HamburgersBlog.Controllers
{
    public class LocationsController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Locations
        public ActionResult Index()
        {
            return View(db.Restaurants.ToList());
        }
    }
}