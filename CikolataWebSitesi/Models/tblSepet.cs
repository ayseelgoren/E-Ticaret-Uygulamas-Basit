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
    
    public partial class tblSepet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblSepet()
        {
            this.tblSepetDty = new HashSet<tblSepetDty>();
            this.tblSiparisDetay = new HashSet<tblSiparisDetay>();
        }
    
        public int sepetID { get; set; }
        public int sptKullaniciID { get; set; }
    
        public virtual tblKullanici tblKullanici { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSepetDty> tblSepetDty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSiparisDetay> tblSiparisDetay { get; set; }
    }
}
