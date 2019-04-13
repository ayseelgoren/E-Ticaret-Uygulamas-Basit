using CikolataWebSitesi.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;


namespace CikolataWebSitesi.Controllers
{
    public class PromOnayController : Controller
    {
        
        private cikolataEntities db = new cikolataEntities();

        // GET: PromOnay
        public ActionResult Index()
        {
            var tblPromOnay = db.tblPromOnay.Include(t => t.tblKullanici).Include(t => t.tblPromosyon).Include(t => t.tblUrun);
            return View(tblPromOnay.ToList());
        }
        public ActionResult Onay(int str)
        {
            
                tblPromOnay promOnay = db.tblPromOnay.Find(str);
                tblFiyat fiyat = db.tblFiyat.Find(promOnay.tblUrun.fiyatID);
                fiyat.urunPromosyonID = promOnay.PromosyonId;
                fiyat.satisFiyati = fiyat.satisFiyati - ((fiyat.satisFiyati * promOnay.tblPromosyon.promosyonIndirimOrani) / 100);
                db.Entry(fiyat).State = EntityState.Modified;
                promOnay.onayDurumu = true;
                db.Entry(promOnay).State = EntityState.Modified;
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
