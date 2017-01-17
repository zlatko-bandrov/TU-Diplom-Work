﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LotteryDemoDBEntities : DbContext
    {
        public LotteryDemoDBEntities()
            : base("name=LotteryDemoDBEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Balance> Balances { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<DrawingNumber> DrawingNumbers { get; set; }
        public virtual DbSet<Jackpot> Jackpots { get; set; }
        public virtual DbSet<LotteryGame> LotteryGames { get; set; }
        public virtual DbSet<LottoDrawing> LottoDrawings { get; set; }
        public virtual DbSet<LottoTicket> LottoTickets { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<LotteryGameSetting> LotteryGameSettings { get; set; }
    
        public virtual int CreateNewLottoDrawing(Nullable<int> lotteryGame, Nullable<System.DateTime> drawTime, Nullable<byte> numberOne, Nullable<byte> numberTwo, Nullable<byte> numberThree, Nullable<byte> numberFour, Nullable<byte> numberFive, Nullable<byte> numberSix, Nullable<byte> numberSeven)
        {
            var lotteryGameParameter = lotteryGame.HasValue ?
                new ObjectParameter("LotteryGame", lotteryGame) :
                new ObjectParameter("LotteryGame", typeof(int));
    
            var drawTimeParameter = drawTime.HasValue ?
                new ObjectParameter("DrawTime", drawTime) :
                new ObjectParameter("DrawTime", typeof(System.DateTime));
    
            var numberOneParameter = numberOne.HasValue ?
                new ObjectParameter("NumberOne", numberOne) :
                new ObjectParameter("NumberOne", typeof(byte));
    
            var numberTwoParameter = numberTwo.HasValue ?
                new ObjectParameter("NumberTwo", numberTwo) :
                new ObjectParameter("NumberTwo", typeof(byte));
    
            var numberThreeParameter = numberThree.HasValue ?
                new ObjectParameter("NumberThree", numberThree) :
                new ObjectParameter("NumberThree", typeof(byte));
    
            var numberFourParameter = numberFour.HasValue ?
                new ObjectParameter("NumberFour", numberFour) :
                new ObjectParameter("NumberFour", typeof(byte));
    
            var numberFiveParameter = numberFive.HasValue ?
                new ObjectParameter("NumberFive", numberFive) :
                new ObjectParameter("NumberFive", typeof(byte));
    
            var numberSixParameter = numberSix.HasValue ?
                new ObjectParameter("NumberSix", numberSix) :
                new ObjectParameter("NumberSix", typeof(byte));
    
            var numberSevenParameter = numberSeven.HasValue ?
                new ObjectParameter("NumberSeven", numberSeven) :
                new ObjectParameter("NumberSeven", typeof(byte));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateNewLottoDrawing", lotteryGameParameter, drawTimeParameter, numberOneParameter, numberTwoParameter, numberThreeParameter, numberFourParameter, numberFiveParameter, numberSixParameter, numberSevenParameter);
        }
    }
}
