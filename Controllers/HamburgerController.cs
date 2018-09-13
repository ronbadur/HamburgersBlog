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
    public class HamburgersController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Resturants
        public ActionResult Index()
        {
            var hamburgers = db.Hamburgers;
            return View(hamburgers.ToList());
        }
    }
}