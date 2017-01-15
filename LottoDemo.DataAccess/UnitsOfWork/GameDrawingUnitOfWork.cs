using LottoDemo.Repositories.Repositories;
using LottoDemo.Repositories.UnitsOfWork.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Repositories.UnitsOfWork
{
    public class GameDrawingUnitOfWork : BaseUnitOfWork
    {
        private LotteryGameRepository lotteryGameRepo;
        public LotteryGameRepository LotteryGameRepo
        {
            get { return this.lotteryGameRepo ?? (this.lotteryGameRepo = new LotteryGameRepository(this.Context)); }
        }

        private LottoDrawingRepository lottoDrawingRepo;
        public LottoDrawingRepository LottoDrawingRepo
        {
            get { return this.lottoDrawingRepo ?? (this.lottoDrawingRepo = new LottoDrawingRepository(this.Context)); }
        }
    }
}