using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CikolataWebSitesi.Models;

namespace CikolataWebSitesi.Controllers
{
    public class SiparisController : Controller
    {
        private cikolataEntities db = new cikolataEntities();
        // GET: Siparis
        public ActionResult Index(int id)
        {
           
            var tblsepet = db.tblSepetDty.Where(x => x.kullaniciID ==id);
            
            foreach(var item in tblsepet)
            {
                if(item.sepetDurum==true)
                {
                    item.sepetDurum = false;
                   
                 
                }

            }
            Session.Remove("AktifSepet");
            
            db.Entry(tblsepet).State = EntityState.Modified;
            db.SaveChanges();
            return View(tblsepet.ToList());
        }

    }
}