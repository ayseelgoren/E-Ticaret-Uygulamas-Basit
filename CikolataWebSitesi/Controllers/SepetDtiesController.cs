using CikolataWebSitesi.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace CikolataWebSitesi.Controllers
{
    public class SepetDtiesController : Controller
    {
        private cikolataEntities db = new cikolataEntities();

        // GET: tblSepetDties
        public PartialViewResult Index()
        {
            //kullanıcı girişi varsa sepetdetaydaki ürünleri listelemek üsere ürün listesini aç
            if (Session["kullanici"] != null)
            {
                var tblSepetDty = db.tblSepetDty.Include(t => t.tblSepet).Include(t => t.tblUrun);
                return PartialView(tblSepetDty.ToList());
            }
            else
            {//ziyaretçi parça view'i aç
                return PartialView("ziyaretciSepet", (sepet)HttpContext.Session["AktifSepet"]);
            }
        }

        public void sepetUrunDusur(int id)
        {
            //sepet sessionı boş değilse
            if (HttpContext.Session["AktifSepet"] != null)
            {
                sepet s = (sepet)HttpContext.Session["AktifSepet"];
                //sepet session adetten birden fazla varsa
                if (s.Urunler.FirstOrDefault(x => x.Urun.urunID == id).adet > 1)
                {
                    //kullanıcı null değilse****
                    if (Session["kullaniciId"] != null)
                    {
                        foreach (tblSepetDty item in db.tblSepetDty)
                        {
                            //sepet deyatda kullanıcının aybı üründen eklemişse önceden bul ve adetini değiştir ve değişikliği kaydet **** 
                            if (item.urunId == id && item.kullaniciID == int.Parse(Session["kullaniciId"].ToString()))
                            {
                                item.adet--;
                                item.topFiyat = item.topFiyat - item.tblUrun.tblFiyat.satisFiyati;
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                                //sessiondada o ürünü düşür.
                                s.Urunler.FirstOrDefault(x => x.Urun.urunID == id).adet--;
                            }
                        }
                    }
                    else
                        //sessionda o ürünü düşür.
                        s.Urunler.FirstOrDefault(x => x.Urun.urunID == id).adet--;
                }
                //sepet session da üründen 1 tane varsa  
                else if (s.Urunler.FirstOrDefault(x => x.Urun.urunID == id).adet == 1)
                {
                    //o ürünü bul
                    SepetItem si = s.Urunler.FirstOrDefault(x => x.Urun.urunID == id);
                    //kullanıcı null değilse ***
                    if (Session["kullaniciId"] != null)
                    {
                        foreach (tblSepetDty item in db.tblSepetDty)
                        {
                            //o ürün ve kullanıcının bulunduğu sepetdetay kaydını bul****
                            if (item.urunId == id && item.kullaniciID == int.Parse(Session["kullaniciId"].ToString()))
                            {//sepet durumunu false yap
                                item.sepetDurum = false;
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                                //ve sessiondan sil
                                s.Urunler.Remove(si);
                            }
                        }
                    }
                    //kullanıcı girişi yoksa sessiondan o ürünü kaldır.
                    else
                        s.Urunler.Remove(si);
                }
            }
        }
        public void sepetUrunArttir(int id)
        {
            sepet s = (sepet)HttpContext.Session["AktifSepet"];
            //session boşmu dolumu
            if (HttpContext.Session["AktifSepet"] != null)
            {//kullanıcı girişi varmınesne başvurusu nesnenin örnegine ayarlanmadı hatası ***
                if (Session["kullaniciId"] != null)
                {
                    foreach (tblSepetDty item in db.tblSepetDty)
                    {
                        //o ürünün ve kullanıcının bulunduğu kayıtın adet sayısını arttır.*****
                        if (item.urunId == id && item.kullaniciID == int.Parse(Session["kullaniciId"].ToString()))
                        {
                            item.adet++;
                            item.topFiyat = item.topFiyat - item.tblUrun.tblFiyat.satisFiyati;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                            //sepet sessiondada arttır.
                            s.Urunler.FirstOrDefault(x => x.Urun.urunID == id).adet++;
                        }
                    }
                }
                else
                    //kullanıcı yoksa sessiondaki sepette ürünün sayısını arttır 
                    s.Urunler.FirstOrDefault(x => x.Urun.urunID == id).adet++;
            }
        }
        public void satisTamamla(int id)
        {
            foreach (tblSepetDty item in db.tblSepetDty)
            { //eğer sepet detayda kullanıcı ıdli kişinin sepet durumu true(var) olan ürünlerin sepet durumunu false(yok yap ve o ürünleri satiş tablosuna kaydeder.) 
                if (item.kullaniciID == id && item.sepetDurum == true)
                {
                    item.sepetDurum = false;
                    db.Entry(item).State = EntityState.Modified;
                    tblSiparisDetay tblSiparisDetay = new tblSiparisDetay();
                    tblSiparisDetay.kullaniciID = id;
                    tblSiparisDetay.sepetID = item.sepetID;
                    db.tblSiparisDetay.Add(tblSiparisDetay);
                    db.SaveChanges();
                }
            }
        }

        public void uyesessionsepetdoldur()
        {
            if (HttpContext.Session["AktifSepet"] != null)
            {
                tblSepetDty tblSepetDty = new tblSepetDty();
                foreach (var item in sepet.AktifSepet.Urunler)
                {
                    foreach (tblSepet s in db.tblSepet)
                    {
                        if (s.sptKullaniciID == Convert.ToInt32(Session["kullaniciId"]))
                        {
                            tblSepetDty.sepetID = s.sepetID;
                        }
                    }
                    tblSepetDty.kullaniciID = Convert.ToInt32(Session["kullaniciId"]);
                    tblSepetDty.urunId = item.Urun.urunID;
                    tblSepetDty.sepetDurum = true;
                    tblSepetDty.topFiyat = item.tutar;
                    tblSepetDty.adet = Convert.ToByte(item.adet);
                    db.tblSepetDty.Add(tblSepetDty);
                    db.SaveChanges();
                }

            }
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
