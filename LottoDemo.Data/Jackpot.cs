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
    
    public partial class Jackpot
    {
        public int ID { get; set; }
        public int BalanceID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.DateTime CreationDate { get; set; }
        public int GameID { get; set; }
    
        public virtual Balance Balance { get; set; }
        public virtual LotteryGame LotteryGame { get; set; }
    }
}
