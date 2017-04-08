using LottoDemo.DataAccess;
using LottoDemo.Repositories.Generic;
using LottoDemo.Repositories.UnitsOfWork.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Repositories.UnitsOfWork
{
    public class UsersUnitOfWork : BaseUnitOfWork
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
    }
}
