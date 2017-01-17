using LottoDemo.DataAccess;
using LottoDemo.Repositories.Generic;
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
        public LotteryGameRepository LotteryGameRepository
        {
            get { return this.lotteryGameRepo ?? (this.lotteryGameRepo = new LotteryGameRepository(this.Context)); }
        }

        private GenericRepository<LottoDrawing> lottoDrawingRepo;
        public GenericRepository<LottoDrawing> LottoDrawingRepository
        {
            get { return this.lottoDrawingRepo ?? (this.lottoDrawingRepo = new GenericRepository<LottoDrawing>(this.Context)); }
        }

        private GenericRepository<LotteryGameSetting> gameSettingsRepository;
        public GenericRepository<LotteryGameSetting> GameSettingsRepository
        {
            get { return this.gameSettingsRepository ?? (this.gameSettingsRepository = new GenericRepository<LotteryGameSetting>(this.Context)); }
        }
    }
}