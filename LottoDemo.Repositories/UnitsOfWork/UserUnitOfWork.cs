using LottoDemo.DataAccess;
using LottoDemo.Repositories.Generic;
using LottoDemo.Repositories.UnitsOfWork.Base;

namespace LottoDemo.Repositories.UnitsOfWork
{
    public class UserUnitOfWork : BaseUnitOfWork
    {
        private GenericRepository<User> lotteryUserRepo;
        public GenericRepository<User> UserRepository
        {
            get { return this.lotteryUserRepo ?? (this.lotteryUserRepo = new GenericRepository<User>(this.Context)); }
        }

        private GenericRepository<Balance> userBalanceRepo;
        public GenericRepository<Balance> BalanceRepository
        {
            get { return this.userBalanceRepo ?? (this.userBalanceRepo = new GenericRepository<Balance>(this.Context)); }
        }

        private GenericRepository<Currency> balanceCurrencyRepo;
        public GenericRepository<Currency> CurrencyRepository
        {
            get { return this.balanceCurrencyRepo ?? (this.balanceCurrencyRepo = new GenericRepository<Currency>(this.Context)); }
        }

        private GenericRepository<LottoTicket> lottoTicketRepo;
        public GenericRepository<LottoTicket> LottoTicketRepository
        {
            get { return this.lottoTicketRepo ?? (this.lottoTicketRepo = new GenericRepository<LottoTicket>(this.Context)); }
        }

        private GenericRepository<LottoTicketBall> _lottoTicketBallRepository;
        public GenericRepository<LottoTicketBall> LottoTicketBallRepository
        {
            get { return this._lottoTicketBallRepository ?? (this._lottoTicketBallRepository = new GenericRepository<LottoTicketBall>(this.Context)); }
        }

        private GenericRepository<LotteryBall> _lotteryBallRepository;
        public GenericRepository<LotteryBall> LotteryBallRepository
        {
            get { return this._lotteryBallRepository ?? (this._lotteryBallRepository = new GenericRepository<LotteryBall>(this.Context)); }
        }
    }
}
