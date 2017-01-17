using LottoDemo.Common.Services;
using LottoDemo.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Repositories.UnitsOfWork.Base
{
    public abstract class BaseUnitOfWork : IUnitOfWork, IDisposable
    {
        internal protected LotteryDemoDBEntities Context = new LotteryDemoDBEntities();

        public void CommitChanges()
        {
            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                this.Dispose(true);
                Log.Error(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
            }
        }

        public void Rollback()
        {
            this.Context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }

        #region Dispose Pattern

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
