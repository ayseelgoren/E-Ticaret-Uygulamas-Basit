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


namespace CikolataWebSitesi.Models
{
   
    public class StoredProsedure
    {

        IEnumerable<sp_encoksatbesurun_Result> enCoksatanUrun { get; set; }
        IEnumerable<sp_encoksatisyapilankategori_Result> enCoksatisKategori { get; set; }
        IEnumerable<sp_encoksepeteurunlerilkbes_Result> enCoksatanilkBes { get; set; }
       
       

    }
}