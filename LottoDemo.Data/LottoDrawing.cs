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
    
    public partial class LottoDrawing
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LottoDrawing()
        {
            this.LottoDrawingBalls = new HashSet<LottoDrawingBall>();
        }
    
        public int ID { get; set; }
        public int LotteryGameID { get; set; }
        public System.DateTime DrawTime { get; set; }
        public bool IsCalculated { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        public virtual LotteryGame LotteryGame { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LottoDrawingBall> LottoDrawingBalls { get; set; }
    }
}
