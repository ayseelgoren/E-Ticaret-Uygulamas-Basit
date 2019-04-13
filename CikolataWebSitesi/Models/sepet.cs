using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CikolataWebSitesi.Models
{
    public class sepet
    {

        private cikolataEntities db = new cikolataEntities();



        public static sepet AktifSepet
        {
            get
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx.Session["AktifSepet"] == null)
                    ctx.Session["AktifSepet"] = new sepet();
                return (sepet)ctx.Session["AktifSepet"];
            }
        }
        private List<SepetItem> urunler = new List<SepetItem>();

        public List<SepetItem> Urunler
        {
            get
            {
                return urunler;
            }
            set
            {
                urunler = value;
            }
        }

        public void sepeteEkle(SepetItem si)
        {

            //session varsa ürün ona eklenir yoksa yeni sepet sessionu oluşturulup eklenir
            if (HttpContext.Current.Session["AktifSepet"] != null)
            {
                sepet s = (sepet)HttpContext.Current.Session["AktifSepet"];
                si.durum = true;
                if (s.Urunler.Any(x => x.Urun.urunID == si.Urun.urunID))
                {
                    s.Urunler.FirstOrDefault(x => x.Urun.urunID == si.Urun.urunID).adet++;
                }
                else
                {
                    s.Urunler.Add(si);
                }

                uyesepeti(si);
            }
            else
            {
                sepet s = new sepet();
                si.durum = true;
                s.Urunler.Add(si);
                HttpContext.Current.Session["AktifSepet"] = s;
                uyesepeti(si);
            }
        }
        public void uyesepeti(SepetItem si)
        {
            //kullanıcı veritabanındaki tabloları dolar ****
            if (HttpContext.Current.Session["kullaniciId"] != null)
            {
                tblSepet sepet = new tblSepet();
                sepet.sptKullaniciID = Convert.ToInt32(HttpContext.Current.Session["kullaniciId"]/*kullanıcı ıd ye eşitlenmesi lazım*****/);
                db.tblSepet.Add(sepet);
                tblSepetDty sdty = new tblSepetDty();
                sdty.urunId = si.Urun.urunID;
                sdty.topFiyat = si.tutar;
                sdty.sepetDurum = true;
                sdty.sepetID = sepet.sepetID;
                sdty.kullaniciID = sepet.sptKullaniciID;
                sdty.adet = Convert.ToByte(si.adet.ToString());
                db.tblSepetDty.Add(sdty);
                db.SaveChanges();
            }
        }

        public double toplamtutar
        {
            //sepetteki ürünlerin toplam tutarını döndürür.
            get
            {  
                return Urunler.Sum(x => x.tutar);
            }
        }
    }

    public class SepetItem
    {
        public tblUrun Urun { get; set; }
        public int adet { get; set; }
        public bool durum { get; set; }
        public double tutar
        {
            get
            {
                return Urun.tblFiyat.satisFiyati * adet;
            }
        }
    }

}
