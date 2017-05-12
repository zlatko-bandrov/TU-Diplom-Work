using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Repositories.UnitsOfWork.Base
{
    public interface IUnitOfWork
    {
        void CommitChanges();

        void Rollback();
    }
}
