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
    
    public partial class WinningTicket
    {
        public int ID { get; set; }
        public int LottoTicketID { get; set; }
        public int DrawStatisticsID { get; set; }
    
        public virtual DrawStatistic DrawStatistic { get; set; }
        public virtual LottoTicket LottoTicket { get; set; }
    }
}
