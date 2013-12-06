using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Web;
using Everis.Core.Data;

namespace ISP.GestaoMatriculas.Repositories
{
    public abstract class Respository<TDBCONTEXT, TENTITY> 
        where TENTITY:Entity<int>
        where TDBCONTEXT : DbContext, new()
    {

        protected TDBCONTEXT Context
        {
            get;
            private set;
        }

        protected DbSet<TENTITY> DbSet
        {
            get;
            set;
        }

        public IQueryable<TENTITY> All
        {
            get
            {
                return DbSet;
            }
        }

        public Respository()
        {
            this.Context = new TDBCONTEXT();
        }

        public IQueryable<TENTITY> AllIncluding(params Expression<Func<TENTITY, object>>[] includeProperties)
        {
            IQueryable<TENTITY> query = All;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TENTITY Find(int id)
        {
            return DbSet.Find(id);
        }

        public void InsertOrUpdate(TENTITY entity)
        {
            if (entity.Id == default(int))
            {
                DbSet.Add(entity);
            }
            else
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var apolice = DbSet.Find(id);
            DbSet.Remove(apolice);
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
