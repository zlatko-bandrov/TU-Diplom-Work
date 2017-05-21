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
    public class LottoGameUnitOfWork : BaseUnitOfWork
    {
        private LotteryGameRepository _gameRepository = null;
        public LotteryGameRepository GameRepository
        {
            get { return _gameRepository ?? (_gameRepository = new LotteryGameRepository(this.Context)); }
        }

        private GenericRepository<LottoDrawing> _drawRepository = null;
        public GenericRepository<LottoDrawing> DrawRepository
        {
            get { return this._drawRepository ?? (this._drawRepository = new GenericRepository<LottoDrawing>(this.Context)); }
        }

        private GenericRepository<LottoDrawingBall> _drawBallsRelationsRepository = null;
        public GenericRepository<LottoDrawingBall> DrawBallsRelationsRepository
        {
            get { return this._drawBallsRelationsRepository ?? (this._drawBallsRelationsRepository = new GenericRepository<LottoDrawingBall>(this.Context)); }
        }

        private GenericRepository<LotteryBall> _ballsRepository = null;
        public GenericRepository<LotteryBall> BallsRepository
        {
            get { return this._ballsRepository ?? (this._ballsRepository = new GenericRepository<LotteryBall>(this.Context)); }
        }

        private GenericRepository<GameWinningsTier> _winningsTiersRepository = null;
        public GenericRepository<GameWinningsTier> WinningsTiersRepository
        {
            get { return this._winningsTiersRepository ?? (this._winningsTiersRepository = new GenericRepository<GameWinningsTier>(this.Context)); }
        }

        private GenericRepository<DrawStatistic> _drawStatisticRepository = null;
        public GenericRepository<DrawStatistic> DrawStatisticsRepository
        {
            get { return this._drawStatisticRepository ?? (this._drawStatisticRepository = new GenericRepository<DrawStatistic>(this.Context)); }
        }

        private GenericRepository<WinningTicket> _winningTicketRepository = null;
        public GenericRepository<WinningTicket> WinningTicketsRepository
        {
            get { return this._winningTicketRepository ?? (this._winningTicketRepository = new GenericRepository<WinningTicket>(this.Context)); }
        }
    }
}