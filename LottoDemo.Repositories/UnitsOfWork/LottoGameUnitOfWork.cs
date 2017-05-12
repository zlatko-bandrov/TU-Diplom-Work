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
        private LotteryGameRepository _gameRepository;
        public LotteryGameRepository GameRepository
        {
            get { return _gameRepository ?? (_gameRepository = new LotteryGameRepository(this.Context)); }
        }

        private GenericRepository<LottoDrawing> _drawRepository;
        public GenericRepository<LottoDrawing> DrawRepository
        {
            get { return this._drawRepository ?? (this._drawRepository = new GenericRepository<LottoDrawing>(this.Context)); }
        }

        private GenericRepository<LottoDrawingBall> _drawBallsRelationsRepository;
        public GenericRepository<LottoDrawingBall> DrawBallsRelationsRepository
        {
            get { return this._drawBallsRelationsRepository ?? (this._drawBallsRelationsRepository = new GenericRepository<LottoDrawingBall>(this.Context)); }
        }

        private GenericRepository<LotteryBall> _ballsRepository;
        public GenericRepository<LotteryBall> BallsRepository
        {
            get { return this._ballsRepository ?? (this._ballsRepository = new GenericRepository<LotteryBall>(this.Context)); }
        }
    }
}