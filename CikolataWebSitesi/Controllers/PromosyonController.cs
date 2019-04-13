using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CikolataWebSitesi.Models;


namespace CikolataWebSitesi.Controllers.Promosyon
{
    public class tblPromosyonsController : Controller
    {
        private cikolataEntities db = new cikolataEntities();
        // GET: tblPromosyons
        public ActionResult Index()
        {
            var tblPromosyon = db.tblPromosyon.Include(t => t.tblKategori).Include(t => t.tblUrun);
            return View(tblPromosyon.ToList());

        }

        // GET: tblPromosyons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPromosyon tblPromosyon = db.tblPromosyon.Find(id);
            if (tblPromosyon == null)
            {
                return HttpNotFound();
            }
            return View(tblPromosyon);
        }

        // GET: tblPromosyons/Create
        public ActionResult Create()
        {
            ViewBag.kategoriID = new SelectList(db.tblKategori, "kategoriID", "kategoriAdi");
            ViewBag.urunID = new SelectList(db.tblUrun, "urunID", "urunAd");
            return View();
        }

        // POST: tblPromosyons/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "promosyonID,urunID,kategoriID,promosyonAdi,promosyonIndirimOrani,baslamaTarihi,bitisTarihi,promosyonResim")] tblPromosyon tblPromosyon)
        {
            if (ModelState.IsValid)
            {//kategori ve urun seçilmediyse.
                if (tblPromosyon.urunID == null && tblPromosyon.kategoriID == null)
                {
                    //urunler userinde gez
                    foreach (var urun in db.tblUrun)
                    {
                        promosyonEkle(tblPromosyon, urun);
                    }
                }
                //Urun Seçildiyse
                else if (tblPromosyon.kategoriID == null)
                {
                    tblUrun urun = db.tblUrun.Find(tblPromosyon.urunID);
                    promosyonEkle(tblPromosyon, urun);
                }
                //Kategori Seçildiyse
                else
                {
                    foreach (var urun in db.tblUrun)
                    {
                        //urunu bul
                        if (tblPromosyon.kategoriID == urun.kategoriID)
                        {
                            promosyonEkle(tblPromosyon, urun);
                        }
                    }
                }
                db.tblPromosyon.Add(tblPromosyon);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult SaticiCreate()
        {
            ViewBag.kategoriID = new SelectList(db.tblKategori, "kategoriID", "kategoriAdi");
            ViewBag.urunID = new SelectList(db.tblUrun, "urunID", "urunAd");
            return View();
        }

        // POST: tblPromosyons/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaticiCreate([Bind(Include = "promosyonID,urunID,kategoriID,promosyonAdi,promosyonIndirimOrani,baslamaTarihi,bitisTarihi,promosyonResim")] tblPromosyon tblPromosyon)
        {
            if (ModelState.IsValid)
            {//kategori ve urun seçilmediyse.
                if (tblPromosyon.urunID == null && tblPromosyon.kategoriID == null)
                {
                    //urunler userinde gez
                    foreach (var urun in db.tblUrun)
                    {
                        promosyonEkle(tblPromosyon, urun);
                    }
                }
                //Urun Seçildiyse
                else if (tblPromosyon.kategoriID == null)
                {
                    tblUrun urun = db.tblUrun.Find(tblPromosyon.urunID);
                    promosyonEkle(tblPromosyon, urun);
                }
                //Kategori Seçildiyse
                else
                {
                    foreach (var urun in db.tblUrun)
                    {
                        //urunu bul
                        if (tblPromosyon.kategoriID == urun.kategoriID)
                        {
                            promosyonEkle(tblPromosyon, urun);
                        }
                    }
                }
                db.tblPromosyon.Add(tblPromosyon);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public void promosyonEkle(tblPromosyon tblPromosyon, tblUrun urun)
        {
            tblFiyat fiyat = db.tblFiyat.Find(urun.fiyatID);
            //satici ıd varsa urunde onay tablosunu doldur
            if (urun.saticiID != null)
            {
                PromOnayDoldur(tblPromosyon, urun, fiyat);
            }
            else
            {//yoksa fiyat tablosunu güncelle
                FiyatGuncelle(fiyat, tblPromosyon);
            }
        }
        public void FiyatGuncelle(tblFiyat fiyat, tblPromosyon tblPromosyon)
        {
            fiyat.urunPromosyonID = tblPromosyon.promosyonID;
            fiyat.satisFiyati = fiyat.satisFiyati - ((fiyat.satisFiyati * tblPromosyon.promosyonIndirimOrani) / 100);
            db.Entry(fiyat).State = EntityState.Modified;
        }
        public void PromOnayDoldur(tblPromosyon tblPromosyon, tblUrun urun, tblFiyat fiyat)
        {
            tblPromOnay tblPromOnay = new tblPromOnay();
            tblPromOnay.onayDurumu = null;
            tblPromOnay.PromosyonId = tblPromosyon.promosyonID;
            tblPromOnay.urunID = urun.urunID;
            tblPromOnay.saticiId = urun.saticiID.Value;
            tblPromOnay.satisFiyat = fiyat.satisFiyati - ((fiyat.satisFiyati * tblPromosyon.promosyonIndirimOrani) / 100);
            db.tblPromOnay.Add(tblPromOnay);
        }
        // GET: tblPromosyons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPromosyon tblPromosyon = db.tblPromosyon.Find(id);
            if (tblPromosyon == null)
            {
                return HttpNotFound();
            }
            ViewBag.kategoriID = new SelectList(db.tblKategori, "kategoriID", "kategoriAdi", tblPromosyon.kategoriID);
            ViewBag.urunID = new SelectList(db.tblUrun, "urunID", "urunAd", tblPromosyon.urunID);
            return View(tblPromosyon);
        }

        // POST: tblPromosyons/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "promosyonID,urunID,kategoriID,promosyonAdi,promosyonIndirimOrani,baslamaTarihi,bitisTarihi,promosyonResim")] tblPromosyon tblPromosyon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPromosyon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kategoriID = new SelectList(db.tblKategori, "kategoriID", "kategoriAdi", tblPromosyon.kategoriID);
            ViewBag.urunID = new SelectList(db.tblUrun, "urunID", "urunAd", tblPromosyon.urunID);
            return View(tblPromosyon);
        }

        // GET: tblPromosyons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPromosyon tblPromosyon = db.tblPromosyon.Find(id);
            if (tblPromosyon == null)
            {
                return HttpNotFound();
            }
            return View(tblPromosyon);
        }

        // POST: tblPromosyons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblPromosyon tblPromosyon = db.tblPromosyon.Find(id);
            foreach (var item in db.tblFiyat)
            {
                if (item.urunPromosyonID == tblPromosyon.promosyonID)
                {
                    tblFiyat fiyat = db.tblFiyat.Find(item.fiyatID);
                    fiyat.satisFiyati = fiyat.satisFiyati + ((fiyat.satisFiyati * tblPromosyon.promosyonIndirimOrani) / 100);
                    db.Entry(fiyat).State = EntityState.Modified;
                }
            }
            foreach (var item in db.tblPromOnay)
            {
                if (item.PromosyonId == tblPromosyon.promosyonID)
                {
                    tblPromOnay tblPromOnay = db.tblPromOnay.Find(item.onayID);
                    db.tblPromOnay.Remove(tblPromOnay);
                }
            }
            db.tblPromosyon.Remove(tblPromosyon);
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
