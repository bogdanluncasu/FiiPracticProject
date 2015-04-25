using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyRestaurant.Models;

namespace MyRestaurant.Controllers
{
    public class MeniuController : Controller
    {
        private MyRestaurantDatabaseContext db = new MyRestaurantDatabaseContext();

        //
        // GET: /Meniu/

        public ActionResult Index()
        {
            return View(db.MeniuClients.ToList());
        }

        //
        // GET: /Meniu/Details/5

        public ActionResult Details(int id = 0)
        {
            MeniuClient meniuclient = db.MeniuClients.Find(id);
            if (meniuclient == null)
            {
                return HttpNotFound();
            }
            return View(meniuclient);
        }

        //
        // GET: /Meniu/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Meniu/Create
        public int GetId(int id = 1)
        {
            MeniuClient x;

            x = db.MeniuClients.Find(id);
            while (x != null)
            {

                id = id + 1;
                x = db.MeniuClients.Find(id);

            }
            return id;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MeniuClient meniuclient)
        {
            meniuclient.Id = GetId();
            if (ModelState.IsValid)
            {
                db.MeniuClients.Add(meniuclient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(meniuclient);
        }

        //
        // GET: /Meniu/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MeniuClient meniuclient = db.MeniuClients.Find(id);
            if (meniuclient == null)
            {
                return HttpNotFound();
            }
            return View(meniuclient);
        }

        //
        // POST: /Meniu/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MeniuClient meniuclient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meniuclient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(meniuclient);
        }

        //
        // GET: /Meniu/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MeniuClient meniuclient = db.MeniuClients.Find(id);
            if (meniuclient == null)
            {
                return HttpNotFound();
            }
            return View(meniuclient);
        }

        //
        // POST: /Meniu/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MeniuClient meniuclient = db.MeniuClients.Find(id);
            db.MeniuClients.Remove(meniuclient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}