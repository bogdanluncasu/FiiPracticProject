using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using MyRestaurant.Models;


namespace Restaurant.Controllers
{
    public class HomeController : Controller
    {
        private MyRestaurantDatabaseContext db = new MyRestaurantDatabaseContext();

        //
        // GET: /Home/
        public ActionResult Index()
        {
            
            ViewBag.Chelner = new SelectList(db.Chelners, "Id", "ChelnerName");
            ViewBag.Comanda = new SelectList(db.MeniuClients, "Id", "Meniu");
            return View();
        }

        //
        // POST: /Home/Create
        public Chelner getChelner(Client client)
        {


            Random random = new Random();
            var mynr = random.Next(0, db.Chelners.Count() - 1);

            var firstOrDefault = db.Chelners.FirstOrDefault(x => x.Id == mynr);

            return firstOrDefault;
        }

        public int GetId(int id = 1)
        {
            Client x;

            x = db.Clients.Find(id);
            while (x != null)
            {

                id = id + 1;
                x = db.Clients.Find(id);

            }
            return id;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Client client)
        {

            client.Id = GetId();
            var someone = getChelner(client);
            client.Comanda = 0;
            client.ChelnerNr = someone.Id;

            client.Masa = -1;

            for (var i = 1; i <= 20; i++)
            {
                if (db.Clients.Find(i) == null)
                {
                    client.Masa = i;
                    break;
                }
            }




            if (client.Masa == -1)
            {
                ViewBag.MasaState = "Toate mesele sunt ocupate.Va rugam reveniti mai tarziu";
            }
           

            if (ModelState.IsValid && client.Masa != -1)
            {
                TempData["Message"] = string.Format("A-ti rezervat cu succes masa cu numarul {0}", client.Masa);
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Chelner = new SelectList(db.Chelners, "Id", "ChelnerName", client.Chelner);
            ViewBag.Comanda = new SelectList(db.MeniuClients, "Id", "Meniu", client.Comanda);

            return View(client);
        }


       //sectiune pentru administrarea tabelelor clienti
        public ActionResult Adminyourtables()
        {
            return View(db.Clients.ToList());
        }

       
       

        public ActionResult Deleteatable(int id =0)
        {
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }
        public ActionResult AddToTable(int id=0)
        {
            TempData["Id"] = string.Format("{0}&", id);
               
                return View(db.MeniuClients.ToList());
           

        }

        public ActionResult AddToTable1(int id=0,int idmeniu = 0)
        {
            
            Client mkd = db.Clients.FirstOrDefault(x=>x.Id==id);
            if (mkd != null)
            {
                mkd.Comanda = idmeniu;

                db.Entry(mkd).State = EntityState.Modified;

                db.SaveChanges();
            }
            return RedirectToAction("Adminyourtables");
            
            
        }
        public ActionResult Meniu()
        {
            return View(db.MeniuClients.ToList());
        }

        //elibereaza o masa 
        [HttpPost, ActionName("Deleteatable")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteatableConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Adminyourtables");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}