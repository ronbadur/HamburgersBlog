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
    public class PrincessesController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Princesses
        public ActionResult Index(string searchName, string searchRoyaltyType, string searchCreationYear)
        {
            var results = db.Princesses.AsQueryable();
            int creationYear;
            KingdomType royaltyType;
            if (!string.IsNullOrEmpty(searchName))
            {
                results = results.Where(s => s.Name == searchName);
            }
            if (!string.IsNullOrEmpty(searchRoyaltyType) && Enum.TryParse(searchRoyaltyType, out royaltyType))
            {
                results = results.Where(s => s.RoyaltyType == royaltyType);
            }
            if (!string.IsNullOrEmpty(searchCreationYear) && int.TryParse(searchCreationYear, out creationYear))
            {
                results = results.Where(s => s.CreationYear == creationYear);
            }
            return View(results.ToList());
        }

        // GET: Princesses
        [HttpGet]
        public ActionResult IndexData(string searchName, string searchRoyaltyType, string creationYear)
        {
            var princesses = db.Princesses.ToList();

            return Json(princesses, JsonRequestBehavior.AllowGet);
        }

        // GET: Princesses/Details/5
        [Authorize(Users = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Princess princess = db.Princesses.Find(id);
            if (princess == null)
            {
                return HttpNotFound();
            }
            return View(princess);
        }

        // GET: Princesses/Create
        [Authorize(Users = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Princesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "Admin")]
        public ActionResult Create([Bind(Include = "PrincessID,Name,RoyaltyType,CreationYear,HairColor,Nationality,MovieName")] Princess princess)
        {
            if (ModelState.IsValid)
            {
                db.Princesses.Add(princess);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(princess);
        }

        // GET: Princesses/Edit/5
        [Authorize(Users = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Princess princess = db.Princesses.Find(id);
            if (princess == null)
            {
                return HttpNotFound();
            }
            return View(princess);
        }

        // POST: Princesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "Admin")]
        public ActionResult Edit([Bind(Include = "PrincessID,Name,RoyaltyType,CreationYear,HairColor,Nationality,MovieName")] Princess princess)
        {
            if (ModelState.IsValid)
            {
                db.Entry(princess).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(princess);
        }

        // GET: Princesses/Delete/5
        [Authorize(Users = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Princess princess = db.Princesses.Find(id);
            if (princess == null)
            {
                return HttpNotFound();
            }
            return View(princess);
        }

        // POST: Princesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Princess princess = db.Princesses.Find(id);
            db.Princesses.Remove(princess);
            db.SaveChanges();
            return RedirectToAction("Index");
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
