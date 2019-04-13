using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CikolataWebSitesi.Models;

namespace CikolataWebSitesi.Controllers
{
    public class MesajController : Controller
    {
        private cikolataEntities db = new cikolataEntities();

        // GET: Mesaj
        public ActionResult Index()
        {
            var tblMesaj = db.tblMesaj.Include(t => t.tblKullanici).Include(t => t.tblUrun);
            return View(tblMesaj.ToList());
        }

        // GET: Mesaj/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMesaj tblMesaj = db.tblMesaj.Find(id);
            if (tblMesaj == null)
            {
                return HttpNotFound();
            }
            return View(tblMesaj);
        }

        // GET: Mesaj/Create
        public ActionResult Create()
        {
            ViewBag.kullaniciID = new SelectList(db.tblKullanici, "kullaniciID", "kullaniciAdi");
            ViewBag.urunID = new SelectList(db.tblUrun, "urunID", "urunAd");
            return View();
        }

        // POST: Mesaj/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mesajID,kullaniciID,urunID,saticiID,mesaj,mesajNo,mesajSahibi")] tblMesaj tblMesaj)
        {
            if (ModelState.IsValid)
            {
                db.tblMesaj.Add(tblMesaj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kullaniciID = new SelectList(db.tblKullanici, "kullaniciID", "kullaniciAdi", tblMesaj.kullaniciID);
            ViewBag.urunID = new SelectList(db.tblUrun, "urunID", "urunAd", tblMesaj.urunID);
            return View(tblMesaj);
        }

        // GET: Mesaj/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMesaj tblMesaj = db.tblMesaj.Find(id);
            if (tblMesaj == null)
            {
                return HttpNotFound();
            }
            ViewBag.kullaniciID = new SelectList(db.tblKullanici, "kullaniciID", "kullaniciAdi", tblMesaj.kullaniciID);
            ViewBag.urunID = new SelectList(db.tblUrun, "urunID", "urunAd", tblMesaj.urunID);
            return View(tblMesaj);
        }

        // POST: Mesaj/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mesajID,kullaniciID,urunID,saticiID,mesaj,mesajNo,mesajSahibi")] tblMesaj tblMesaj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblMesaj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kullaniciID = new SelectList(db.tblKullanici, "kullaniciID", "kullaniciAdi", tblMesaj.kullaniciID);
            ViewBag.urunID = new SelectList(db.tblUrun, "urunID", "urunAd", tblMesaj.urunID);
            return View(tblMesaj);
        }

        // GET: Mesaj/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMesaj tblMesaj = db.tblMesaj.Find(id);
            if (tblMesaj == null)
            {
                return HttpNotFound();
            }
            return View(tblMesaj);
        }

        // POST: Mesaj/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblMesaj tblMesaj = db.tblMesaj.Find(id);
            db.tblMesaj.Remove(tblMesaj);
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
