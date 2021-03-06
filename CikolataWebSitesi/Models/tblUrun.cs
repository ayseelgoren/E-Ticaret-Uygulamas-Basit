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
    
    public partial class tblUrun
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblUrun()
        {
            this.tblFavori = new HashSet<tblFavori>();
            this.tblMesaj = new HashSet<tblMesaj>();
            this.tblPromosyon = new HashSet<tblPromosyon>();
            this.tblSepetDty = new HashSet<tblSepetDty>();
        }
    
        public int urunID { get; set; }
        public Nullable<int> saticiID { get; set; }
        public int kategoriID { get; set; }
        public Nullable<int> fiyatID { get; set; }
        public Nullable<int> stokID { get; set; }
        public string urunAd { get; set; }
        public string urunAciklama { get; set; }
        public System.DateTime urunEklemeTarihi { get; set; }
        public bool durumu { get; set; }
        public string resim { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFavori> tblFavori { get; set; }
        public virtual tblFiyat tblFiyat { get; set; }
        public virtual tblKategori tblKategori { get; set; }
        public virtual tblKullanici tblKullanici { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMesaj> tblMesaj { get; set; }
        public virtual tblPromOnay tblPromOnay { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPromosyon> tblPromosyon { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSepetDty> tblSepetDty { get; set; }
        public virtual tblStok tblStok { get; set; }
    }
}
