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
    
    public partial class LottoTicketBall
    {
        public int ID { get; set; }
        public int LottoTocketID { get; set; }
        public int LotteryBallID { get; set; }
    
        public virtual LotteryBall LotteryBall { get; set; }
        public virtual LottoTicket LottoTicket { get; set; }
    }
}
