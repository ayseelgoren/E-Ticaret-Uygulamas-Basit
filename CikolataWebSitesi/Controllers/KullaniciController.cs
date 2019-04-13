using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CikolataWebSitesi.Models;

namespace CikolataWebSitesi.Controllers
{
    public class KullaniciController : Controller
    {
        private cikolataEntities db = new cikolataEntities();
        
        //[Authorize(Roles="ziyaretci")]
        public ActionResult UyeOl()
        {
            ViewBag.kullaniciRolID = new SelectList(db.tblKullaniciRol, "kullaniciRolID", "kullaniciRolAdi");
            return View();
            
        }

        // [Authorize(Roles = "ziyaretci")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UyeOl([Bind(Include = "kullaniciID,kullaniciRolID,kullaniciAdi,kullanciSoyad,kullaniciSifre,kullaniciKayitTarihi,telefon,mail")] tblKullanici tblKullanici)
        {
            if (ModelState.IsValid)
            {
                db.tblKullanici.Add(tblKullanici);
                db.SaveChanges();
                return RedirectToAction("GirisYap", "Kullanici");
            }

            ViewBag.kullaniciRolID = new SelectList(db.tblKullaniciRol, "kullaniciRolID", "kullaniciRolAdi", tblKullanici.kullaniciRolID);
            return View(tblKullanici);

        }
        //[Authorize(Roles = "üye")]
        public ActionResult GirisYap()
        {
            return View();
        }

        // [Authorize(Roles = "üye")]
        [HttpPost]
        public ActionResult GirisYap(tblKullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                //bool sonuc = Membership.ValidateUser(kullanici.kullaniciAdi, kullanici.kullaniciSifre);
                //if (sonuc)
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                //else
                //    ViewBag.Mesaj = "Hatalı giriş!!";
                var login = db.tblKullanici.Where(m => m.mail == kullanici.mail).FirstOrDefault();

                if (login.mail == kullanici.mail && login.kullaniciSifre == kullanici.kullaniciSifre)
                {
                    Session.Add("kullanici", login.kullaniciAdi);
                    Session.Add("kullaniciId", login.kullaniciID);
                    Session.Add("kullaniciRolId", login.kullaniciRolID);
                    Session.Add("kullanicimail", login.mail);
                    ViewBag.isim = Session["kullanici"].ToString();
                    if (login.kullaniciRolID == 1 || login.kullaniciRolID == null)
                        return RedirectToAction("Index", "Home");
                    else if (login.kullaniciRolID == 2)
                        return RedirectToAction("Index", "Urun");
                    else if (login.kullaniciRolID == 3)
                        return RedirectToAction("Create","SaticiUrun");

                }
                else
                {
                    ViewBag.Mesaj = "Kullanıcı Adı veya Şifreyi Kontrol Ediniz / Yeni Kayıt Olduysanız ONAY Verilmesini Beklemelisiniz!";
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            return View();
        }
        public ActionResult CikisYap()
        {
            Session.Remove("kullanici");

            // Session Clear ile de tüm oluşturulanlar silinir.
            Session.Clear();

            return RedirectToAction("GirisYap", "Kullanici");
        }

        public ActionResult Profil()
        {
            int kId = Convert.ToInt32(Session["kullaniciId"]);
            if (ModelState.IsValid)
            {
                tblKullanici tblKullanici = db.tblKullanici.Find(kId);
                return View(tblKullanici);
            }

            return HttpNotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profil([Bind(Include = "kullaniciID,kullaniciRolID,kullaniciAdi,kullanciSoyad,kullaniciSifre,kullaniciKayitTarihi,mail,telefon,adres")] tblKullanici tblKullanici)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKullanici).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profil", "Kullanici");
            }
            return View(tblKullanici);
        }
        //public ActionResult ProfilGuncelle(int )
        //{

        //}

        public ActionResult Favori()
        {
            int kullanici = Convert.ToInt32(Session["kullaniciId"]);
            var tblFav = db.tblFavori.Where(m => m.kullaniciID == kullanici);
            return View(tblFav.ToList());

        }
        public ActionResult FavoriEkle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {

                tblFavori favori1 = new tblFavori();
                favori1.kullaniciID = Convert.ToInt32(Session["kullaniciId"]);
                tblUrun urun = db.tblUrun.Find(id);
                favori1.urunID = urun.urunID;
                favori1.favoriDurumu = true;
                db.tblFavori.Add(favori1);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");


            }

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "favoriID,urunID,kullaniciID,favoriDurumu")] tblFavori tblFavori)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.tblFavoriSet.Add(tblFavori);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.kullaniciID = new SelectList(db.tblKullanici, "kullaniciID", "kullaniciAdi", tblFavori.kullaniciID);
        //    ViewBag.urunID = new SelectList(db.tblUrun, "urunID", "urunAd", tblFavori.urunID);
        //    return View(tblFavori);
        //}

        //GET: Kullanici
        //public ActionResult Index().
        //{
        //    var tblKullanici = db.tblKullanici.Include(t => t.tblKullaniciRol);
        //    return View(tblKullanici.ToList());
        //}

        //// GET: Kullanici/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblKullanici tblKullanici = db.tblKullanici.Find(id);
        //    if (tblKullanici == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblKullanici);
        //}

        //// GET: Kullanici/Create
        //public ActionResult Create()
        //{
        //    ViewBag.kullaniciRolID = new SelectList(db.tblKullaniciRol, "kullaniciRolID", "kullaniciRolAdi");
        //    return View();
        //}

        //// POST: Kullanici/Create
        //// Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        //// daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "kullaniciID,kullaniciRolID,kullaniciAdi,kullanciSoyad,kullaniciSifre,kullaniciKayitTarihi")] tblKullanici tblKullanici)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.tblKullanici.Add(tblKullanici);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.kullaniciRolID = new SelectList(db.tblKullaniciRol, "kullaniciRolID", "kullaniciRolAdi", tblKullanici.kullaniciRolID);
        //    return View(tblKullanici);
        //}

        //// GET: Kullanici/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblKullanici tblKullanici = db.tblKullanici.Find(id);
        //    if (tblKullanici == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.kullaniciRolID = new SelectList(db.tblKullaniciRol, "kullaniciRolID", "kullaniciRolAdi", tblKullanici.kullaniciRolID);
        //    return View(tblKullanici);
        //}

        //// POST: Kullanici/Edit/5
        //// Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        //// daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "kullaniciID,kullaniciRolID,kullaniciAdi,kullanciSoyad,kullaniciSifre,kullaniciKayitTarihi")] tblKullanici tblKullanici)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tblKullanici).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.kullaniciRolID = new SelectList(db.tblKullaniciRol, "kullaniciRolID", "kullaniciRolAdi", tblKullanici.kullaniciRolID);
        //    return View(tblKullanici);
        //}

        //// GET: Kullanici/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblKullanici tblKullanici = db.tblKullanici.Find(id);
        //    if (tblKullanici == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblKullanici);
        //}

        //// POST: Kullanici/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    tblKullanici tblKullanici = db.tblKullanici.Find(id);
        //    db.tblKullanici.Remove(tblKullanici);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
