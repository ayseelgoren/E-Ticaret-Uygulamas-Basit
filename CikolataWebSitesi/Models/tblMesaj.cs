//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CikolataWebSitesi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblMesaj
    {
        public int mesajID { get; set; }
        public int kullaniciID { get; set; }
        public int urunID { get; set; }
        public int saticiID { get; set; }
        public string mesaj { get; set; }
        public Nullable<int> mesajNo { get; set; }
        public Nullable<bool> mesajSahibi { get; set; }
    
        public virtual tblKullanici tblKullanici { get; set; }
        public virtual tblUrun tblUrun { get; set; }
    }
}