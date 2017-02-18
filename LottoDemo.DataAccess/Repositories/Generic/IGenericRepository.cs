using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Repositories.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAsQuery(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAsList(Expression<Func<T, bool>> predicate);
        T Get(Expression<Func<T, bool>> predicate);
        void Add(params T[] items);
        void Add(T item);
        void Update(params T[] items);
        void Update(T item);
        void Remove(params T[] items);
        void Remove(T item);
    }
}
