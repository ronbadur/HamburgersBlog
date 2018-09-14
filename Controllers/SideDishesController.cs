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
    public class SideDishesController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Side Dishes
        public ActionResult Index()
        {
            var sideDishes = db.SideDishes;
            return View(sideDishes.ToList());
        }
    }
}