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
    public class tblSiparisDetaysController : Controller
    {
        private cikolataEntities db = new cikolataEntities();

        // GET: tblSiparisDetays
        public ActionResult Index()
        {
            var tblSiparisDetay = db.tblSiparisDetay.Include(t => t.tblSepet);
            return View(tblSiparisDetay.ToList());
        }

        // GET: tblSiparisDetays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSiparisDetay tblSiparisDetay = db.tblSiparisDetay.Find(id);
            if (tblSiparisDetay == null)
            {
                return HttpNotFound();
            }
            return View(tblSiparisDetay);
        }

        // GET: tblSiparisDetays/Create
        public ActionResult Create()
        {
            ViewBag.sepetID = new SelectList(db.tblSepet, "sepetID", "sepetID");
            return View();
        }

        // POST: tblSiparisDetays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "siparisDtyID,sepetID,kullaniciID,siparisTarihi")] tblSiparisDetay tblSiparisDetay)
        {
            if (ModelState.IsValid)
            {
                db.tblSiparisDetay.Add(tblSiparisDetay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.sepetID = new SelectList(db.tblSepet, "sepetID", "sepetID", tblSiparisDetay.sepetID);
            return View(tblSiparisDetay);
        }

        // GET: tblSiparisDetays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSiparisDetay tblSiparisDetay = db.tblSiparisDetay.Find(id);
            if (tblSiparisDetay == null)
            {
                return HttpNotFound();
            }
            ViewBag.sepetID = new SelectList(db.tblSepet, "sepetID", "sepetID", tblSiparisDetay.sepetID);
            return View(tblSiparisDetay);
        }

        // POST: tblSiparisDetays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "siparisDtyID,sepetID,kullaniciID,siparisTarihi")] tblSiparisDetay tblSiparisDetay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSiparisDetay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.sepetID = new SelectList(db.tblSepet, "sepetID", "sepetID", tblSiparisDetay.sepetID);
            return View(tblSiparisDetay);
        }

        // GET: tblSiparisDetays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSiparisDetay tblSiparisDetay = db.tblSiparisDetay.Find(id);
            if (tblSiparisDetay == null)
            {
                return HttpNotFound();
            }
            return View(tblSiparisDetay);
        }

        // POST: tblSiparisDetays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSiparisDetay tblSiparisDetay = db.tblSiparisDetay.Find(id);
            db.tblSiparisDetay.Remove(tblSiparisDetay);
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
