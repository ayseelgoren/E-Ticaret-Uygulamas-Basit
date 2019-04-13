using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CikolataWebSitesi.Models
{
    public class ViewModel
    {
        IEnumerable<tblUrun> Urun { get; set; }
        IEnumerable<tblMesaj> Mesaj { get; set; }


    }
}