using LottoDemo.Common.Services;
using LottoDemo.DataAccess;
using LottoDemo.Repositories.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.BusinessLogic.Services
{
    public class LottoUsersService
    {
        private decimal InitialUserBalance
        {
            get
            {
                decimal temp = 0;
                if (Decimal.TryParse(ConfigurationManager.AppSettings["InitialUserBalance"], out temp))
                {
                    return temp;
                }

                return 1000;
            }
        }

        private UsersUnitOfWork UnitOfWork = new UsersUnitOfWork();


        public bool CreateNewUser(string umbracoUsername)
        {
            try
            {
                var newUser = this.GetNewUser(umbracoUsername);
                this.UnitOfWork.UserRepository.Add(newUser);
                this.UnitOfWork.CommitChanges();
            }
            catch (Exception ex)
            {
                Log.Error(typeof(LottoUsersService), string.Format("{0} - {1}", MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                this.UnitOfWork.Rollback();
                return false;
            }

            return true;
        }


        private User GetNewUser(string umbracoUsername)
        {
            var balanceCurrency = this.UnitOfWork.CurrencyRepository.AsQuery().FirstOrDefault();
            if (balanceCurrency == null)
            {
                throw new ArgumentNullException("User Balance Currency", "The default lottery system currency could not be found!");
            }

            User newUser = new User
            {
                Username = umbracoUsername,
                CreationDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            newUser.Balance = new Balance
            {
                Value = this.InitialUserBalance,
                CurrencyID = balanceCurrency.ID,
                CreationDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            return newUser;
        }
    }
}
