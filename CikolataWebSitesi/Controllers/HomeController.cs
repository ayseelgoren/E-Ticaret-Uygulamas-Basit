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
    public class HomeController : Controller
    {
        private cikolataEntities db = new cikolataEntities();
        public ActionResult Index()
        {

            promosyonSil();

            var tblUrun = db.tblUrun.Include(t => t.tblKategori).Include(t => t.tblKullanici).Include(t => t.tblStok);
            return View(tblUrun.ToList());
        }

        public void promosyonSil()
        {
            var date = DateTime.Now;
            foreach (var prom in db.tblPromosyon)
            {

                if (prom.bitisTarihi.Value.Year == date.Year && prom.bitisTarihi.Value.Month == date.Month && prom.bitisTarihi.Value.Day == date.Day)
                {
                    tblPromosyon tblPromosyon = db.tblPromosyon.Find(prom.promosyonID);
                    db.tblPromosyon.Remove(tblPromosyon);
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
                }
            }
            db.SaveChanges();
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
        public ActionResult Ara(string ara = null)
        {
            var aranan = db.tblUrun.Where(t => t.urunAd.Contains(ara)).ToList();
            return View(aranan.OrderByDescending(t => t.urunEklemeTarihi));
        }

    }
}












