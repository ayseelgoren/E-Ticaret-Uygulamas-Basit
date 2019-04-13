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
    public class UrunController : Controller
    {
        private cikolataEntities db = new cikolataEntities();

        // GET: Urun
        public ActionResult Index()
        {
            var tblUrun = db.tblUrun.Include(t => t.tblFiyat).Include(t => t.tblKategori).Include(t => t.tblKullanici).Include(t => t.tblPromOnay).Include(t => t.tblStok);
            return View(tblUrun.ToList());
        }

        // GET: Urun/Details/5
        public ActionResult Detay(int id)
        {
            return View(db.tblUrun.FirstOrDefault(x => x.urunID == id));
        }

        // GET: Urun/Create
        public ActionResult Create()
        {
            ViewBag.fiyatID = new SelectList(db.tblFiyat, "fiyatID", "fiyatID");
            ViewBag.kategoriID = new SelectList(db.tblKategori, "kategoriID", "kategoriAdi");
            ViewBag.saticiID = new SelectList(db.tblKullanici, "kullaniciID", "kullaniciAdi");
            ViewBag.urunID = new SelectList(db.tblPromOnay, "onayID", "onayID");
            ViewBag.stokID = new SelectList(db.tblStok, "stokID", "stokID");

            return View();
        }

        // POST: Urun/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "urunID,saticiID,kategoriID,fiyatID,stokID,urunAd,urunAciklama,urunEklemeTarihi,durumu,resim")] tblUrun tblUrun, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string resimAdi = System.IO.Path.GetFileName(file.FileName);
                string adres = Server.MapPath("/images/" + resimAdi);
                file.SaveAs(adres);
                tblUrun.resim = resimAdi;
            }
            if (ModelState.IsValid)
            {
                db.tblUrun.Add(tblUrun);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.fiyatID = new SelectList(db.tblFiyat, "fiyatID", "fiyatID", tblUrun.fiyatID);
            ViewBag.kategoriID = new SelectList(db.tblKategori, "kategoriID", "kategoriAdi", tblUrun.kategoriID);
           
                ViewBag.saticiID = new SelectList(db.tblKullanici, "kullaniciID", "kullaniciAdi", tblUrun.saticiID);
            
           
            ViewBag.urunID = new SelectList(db.tblPromOnay, "onayID", "onayID", tblUrun.urunID);
            ViewBag.stokID = new SelectList(db.tblStok, "stokID", "stokID", tblUrun.stokID);
            return View(tblUrun);
        }
        public ActionResult Dashboard()
        {
           
            return View();
        }

            // GET: Urun/Edit/5
            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUrun tblUrun = db.tblUrun.Find(id);
            if (tblUrun == null)
            {
                return HttpNotFound();
            }
            ViewBag.fiyatID = new SelectList(db.tblFiyat, "fiyatID", "fiyatID", tblUrun.fiyatID);
            ViewBag.kategoriID = new SelectList(db.tblKategori, "kategoriID", "kategoriAdi", tblUrun.kategoriID);
            ViewBag.saticiID = new SelectList(db.tblKullanici, "kullaniciID", "kullaniciAdi", tblUrun.saticiID);
            ViewBag.urunID = new SelectList(db.tblPromOnay, "onayID", "onayID", tblUrun.urunID);
            ViewBag.stokID = new SelectList(db.tblStok, "stokID", "stokID", tblUrun.stokID);
            return View(tblUrun);
        }

        // POST: Urun/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "urunID,saticiID,kategoriID,fiyatID,stokID,urunAd,urunAciklama,urunEklemeTarihi,durumu,resim")] tblUrun tblUrun)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblUrun).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fiyatID = new SelectList(db.tblFiyat, "fiyatID", "fiyatID", tblUrun.fiyatID);
            ViewBag.kategoriID = new SelectList(db.tblKategori, "kategoriID", "kategoriAdi", tblUrun.kategoriID);
            ViewBag.saticiID = new SelectList(db.tblKullanici, "kullaniciID", "kullaniciAdi", tblUrun.saticiID);
            ViewBag.urunID = new SelectList(db.tblPromOnay, "onayID", "onayID", tblUrun.urunID);
            ViewBag.stokID = new SelectList(db.tblStok, "stokID", "stokID", tblUrun.stokID);
            return View(tblUrun);
        }

        // GET: Urun/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUrun tblUrun = db.tblUrun.Find(id);
            if (tblUrun == null)
            {
                return HttpNotFound();
            }
            return View(tblUrun);
        }

        // POST: Urun/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblUrun tblUrun = db.tblUrun.Find(id);
            db.tblUrun.Remove(tblUrun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void SepeteEkle(int id)
        {
            SepetItem si = new SepetItem();
            tblUrun u = db.tblUrun.Find(id);
            si.Urun = u;
            si.adet = 1;
            sepet s = new sepet();
            s.sepeteEkle(si);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUrun tblUrun = db.tblUrun.Find(id);
            if (tblUrun == null)
            {
                return HttpNotFound();
            }
            return View(tblUrun);
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
