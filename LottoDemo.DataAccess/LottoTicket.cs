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
    
    public partial class LottoTicket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LottoTicket()
        {
            this.LottoTicketBalls = new HashSet<LottoTicketBall>();
        }
    
        public int ID { get; set; }
        public int UserID { get; set; }
        public int LotteryGameID { get; set; }
        public System.DateTime InputTime { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool IsCalculated { get; set; }
        public bool IsWinning { get; set; }
    
        public virtual LotteryGame LotteryGame { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LottoTicketBall> LottoTicketBalls { get; set; }
    }
}
