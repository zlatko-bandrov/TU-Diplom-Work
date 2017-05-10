using LottoDemo.Repositories.Repositories;
using LottoDemo.Repositories.UnitsOfWork.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Repositories.UnitsOfWork
{
    public class LotteryGameUnitOfWork : BaseUnitOfWork
    {
        private LotteryGameRepository _gameRepository;
        public LotteryGameRepository GameRepository
        {
            get { return _gameRepository ?? (_gameRepository = new LotteryGameRepository(this.Context)); }
        }

    }
}
