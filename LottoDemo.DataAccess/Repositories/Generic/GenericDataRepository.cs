using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LottoDemo.Data;

namespace LottoDemo.DataAccess.Repositories.Generic
{
    public class GenericDataRepository<T> : IGenericDataRepository<T>, IDisposable where T : class
    {
        private LotteryDemoDBEntities _context;

        private bool _disposed = false;

        public GenericDataRepository()
            : this(new LotteryDemoDBEntities())
        {
        }

        public GenericDataRepository(LotteryDemoDBEntities context)
        {
            this._context = context;
        }

        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new LotteryDemoDBEntities())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<T>();
            }
            return list;
        }

        public virtual IList<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new LotteryDemoDBEntities())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList<T>();
            }
            return list;
        }

        public virtual T GetSingle(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            using (var context = new LotteryDemoDBEntities())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public virtual void Update(params T[] items)
        {
            using (var context = new LotteryDemoDBEntities())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public virtual void Remove(params T[] items)
        {
            using (var context = new LotteryDemoDBEntities())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }

        public void Add(params T[] items)
        {
            dbSet.Add(entity);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }
            }

            this._disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
