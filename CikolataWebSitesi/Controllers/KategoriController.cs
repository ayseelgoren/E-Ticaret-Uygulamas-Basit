using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CikolataWebSitesi.Models;

namespace HizliUygulamaGelistirme.Controllers
{
    public class KategoriController : Controller
    {
        private cikolataEntities db = new cikolataEntities();
        // GET: Kategori
        public ActionResult Index(int id)
        {
            var tblurun = db.tblUrun.Where(x => x.kategoriID == id);

            return View(tblurun.ToList());
        }


        public ActionResult Index2()
        {
            return View(db.tblKategori.ToList());
        }

        // GET: Kategori/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKategori tblKategori = db.tblKategori.Find(id);
            if (tblKategori == null)
            {
                return HttpNotFound();
            }
            return View(tblKategori);
        }

        // GET: Kategori/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategori/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "kategoriID,kategoriAdi")] tblKategori tblKategori)
        {
            if (ModelState.IsValid)
            {
                db.tblKategori.Add(tblKategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblKategori);
        }

        // GET: Kategori/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKategori tblKategori = db.tblKategori.Find(id);
            if (tblKategori == null)
            {
                return HttpNotFound();
            }
            return View(tblKategori);
        }

        // POST: Kategori/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "kategoriID,kategoriAdi")] tblKategori tblKategori)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKategori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblKategori);
        }

        // GET: Kategori/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKategori tblKategori = db.tblKategori.Find(id);
            if (tblKategori == null)
            {
                return HttpNotFound();
            }
            return View(tblKategori);
        }

        // POST: Kategori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            foreach (var urun in db.tblUrun)
            {
                if (urun.kategoriID == id)
                {
                    //kategoriye ait urunleri bul ve sil
                    //urun tablosunda urun idsini göndererek o ıd'li urunu çeker adını tblUrun yapar
                    tblUrun tblurun = db.tblUrun.Find(urun.urunID);
                    //promosyon tablosu içinde gezip promosyonun urunıd'sı  ile istenen urununıd'sıni eşleştirme 
                    foreach (var promosyon in db.tblPromosyon)
                    {
                        //promosyon tablosunda ürüne ait promosyon varsa bul ve sil
                        if (promosyon.urunID == urun.urunID)
                        {
                            tblPromosyon prom = db.tblPromosyon.Find(promosyon.promosyonID);
                            foreach (var fiyat in db.tblFiyat)
                            {
                                //fiyat tablosunda o promosyon tanımlı ise
                                if (fiyat.urunPromosyonID == prom.promosyonID)
                                {
                                    tblFiyat tblFiyat = db.tblFiyat.Find(fiyat.fiyatID);
                                    foreach (var furun in db.tblUrun)
                                    {
                                        //aynı fiyatı kullanan diğer urunlerin fiyatlarını null yapar.
                                        if (furun.fiyatID == fiyat.fiyatID)
                                        {
                                            tblUrun tblUrun = db.tblUrun.Find(furun.urunID);
                                            tblUrun.fiyatID = 0;
                                            tblUrun.durumu = tblUrun.durumu;
                                            tblUrun.kategoriID = tblUrun.kategoriID;
                                            tblUrun.resim = tblUrun.resim;
                                            tblUrun.saticiID = tblUrun.saticiID;
                                            tblUrun.stokID = tblUrun.stokID;
                                            tblUrun.urunAciklama = tblUrun.urunAciklama;
                                            tblUrun.urunAd = tblUrun.urunAd;
                                            tblUrun.urunEklemeTarihi = tblUrun.urunEklemeTarihi;
                                            tblUrun.urunID = tblUrun.urunID;
                                            db.Entry(tblUrun).State = EntityState.Modified;
                                        }
                                    }
                                    db.tblFiyat.Remove(tblFiyat);
                                }
                            }
                            db.tblPromosyon.Remove(prom);
                        }
                    }
                    //urunun fiyatı,stogu,resim varsa sil
                    //if (urun.resimID!=null)
                    //{
                    //    tblResim tblResim = db.tblResim.Find(urun.resimID);
                    //    db.tblResim.Remove(tblResim);
                    //}
                    if (urun.stokID != null)
                    {
                        tblStok tblStok = db.tblStok.Find(urun.stokID);
                        db.tblStok.Remove(tblStok);
                    }
                    db.tblUrun.Remove(tblurun);
                }
            }
            tblKategori tblKategori = db.tblKategori.Find(id);
            db.tblKategori.Remove(tblKategori);
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