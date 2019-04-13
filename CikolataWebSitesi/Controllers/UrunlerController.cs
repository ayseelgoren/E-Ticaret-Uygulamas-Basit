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
    public class UrunlerController : Controller
    {
        private cikolataEntities db = new cikolataEntities();
        // GET: Urunler
        public ActionResult Index()
        {
            return View();
        }

       
    }
}