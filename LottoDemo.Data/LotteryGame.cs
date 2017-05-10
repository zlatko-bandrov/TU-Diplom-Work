//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LottoDemo.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class LotteryGame
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LotteryGame()
        {
            this.LottoDrawings = new HashSet<LottoDrawing>();
            this.LottoTickets = new HashSet<LottoTicket>();
        }
    
        public int ID { get; set; }
        public int CountryID { get; set; }
        public int JackpotID { get; set; }
        public System.Guid GameKey { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        public virtual Jackpot Jackpot { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LottoDrawing> LottoDrawings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LottoTicket> LottoTickets { get; set; }
    }
}
