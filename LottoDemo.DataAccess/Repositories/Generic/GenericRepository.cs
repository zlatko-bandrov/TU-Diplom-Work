using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LottoDemo.DataAccess;

namespace LottoDemo.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private LotteryDemoDBEntities Context;
        private DbSet<T> Table;

        public GenericRepository(LotteryDemoDBEntities context)
        {
            this.Context = context;
            this.Table = context.Set<T>();
        }

        public virtual IQueryable<T> GetAsQuery(Expression<Func<T, bool>> predicate)
        {
            return this.Table.AsNoTracking().Where(predicate);
        }

        public virtual IEnumerable<T> GetAsList(Expression<Func<T, bool>> predicate)
        {
            return this.GetAsQuery(predicate).ToList();
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return this.GetAsQuery(predicate).FirstOrDefault<T>();
        }

        public virtual void Update(T item)
        {
            this.Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void Update(params T[] items)
        {
            foreach (T item in items)
            {
                this.Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public virtual void Remove(T item)
        {
            this.Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
        }

        public virtual void Remove(params T[] items)
        {
            foreach (T item in items)
            {
                this.Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
            }
        }

        public virtual void Add(T item)
        {
            this.Table.Add(item);
        }

        public virtual void Add(params T[] items)
        {
            foreach (T item in items)
            {
                this.Table.AddRange(items);
            }
        }
    }
}
