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
    
    public partial class tblPromosyon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPromosyon()
        {
            this.tblFiyat = new HashSet<tblFiyat>();
            this.tblPromOnay = new HashSet<tblPromOnay>();
        }
    
        public int promosyonID { get; set; }
        public Nullable<int> urunID { get; set; }
        public string promosyonAdi { get; set; }
        public double promosyonIndirimOrani { get; set; }
        public Nullable<System.DateTime> baslamaTarihi { get; set; }
        public Nullable<System.DateTime> bitisTarihi { get; set; }
        public string promosyonResim { get; set; }
        public Nullable<int> kategoriID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFiyat> tblFiyat { get; set; }
        public virtual tblKategori tblKategori { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPromOnay> tblPromOnay { get; set; }
        public virtual tblUrun tblUrun { get; set; }
    }
}
