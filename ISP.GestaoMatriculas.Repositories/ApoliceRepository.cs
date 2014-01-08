using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Web;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Model;
<<<<<<< HEAD
using System.Data.Entity.Core.Objects;
using Microsoft.ApplicationServer.Caching;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Repositories
{
    public class ApoliceRepository : IApoliceRepository
    {
        DomainModels context = new DomainModels();
<<<<<<< HEAD
        private DataCache m_cache = null;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

        public IQueryable<Apolice> All
        {
            get {
                return context.Apolices;
            }
        }

        public IQueryable<Apolice> AllIncluding(params Expression<Func<Apolice, object>>[] includeProperties)
        {
            IQueryable<Apolice> query = context.Apolices;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Apolice Find(int id)
        {
            return context.Apolices.Find(id);
        }

<<<<<<< HEAD

        public Apolice FindCache(int id)
        {
            m_cache = CacheUtil.GetCache();
            DataCacheItemVersion version = null;
            Apolice apo = (Apolice) m_cache.Get("Apolice_" + id.ToString(), out version);
            if (apo == null)
            {
                apo = context.Apolices.Include("tomador").Include("veiculo").Include("veiculo.categoria").Include("concelho").
                    Include("avisosApolice").Include("eventoHistorico").Single(a => a.apoliceId == id);

                m_cache.Put("Apolice_" + id.ToString(), apo);
            }

            return apo;
            //return context.Apolices.Find(id);
        }
        


        public void InsertOrUpdate(Apolice apolice){
            if (apolice.apoliceId == default(int))
            {
                apolice = context.Apolices.Add(apolice);
=======
        public void InsertOrUpdate(Apolice apolice){
            if (apolice.Id == default(int))
            {
                context.Apolices.Add(apolice);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }
            else
            {
                context.Entry(apolice).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var apolice = context.Apolices.Find(id);
            context.Apolices.Remove(apolice);
        }

        public void Save()
        {
            context.SaveChanges();
        }
<<<<<<< HEAD
        
=======

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public void Dispose()
        {
            context.Dispose();
        }

<<<<<<< HEAD


        public IList<Apolice> Search(Expression<Func<Apolice, bool>> filters, string sorting, List<string> includeList)
        {
            return GetQuery(context, filters, sorting).ToList();
        }

        /// <summary>
        /// Method used to build the query that will reflect the filter conditions and the sort expression.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filters"></param>
        /// <param name="sorting"></param>
        /// <param name="includeList"></param>
        /// <returns></returns>
        protected IQueryable<Apolice> GetQuery(DomainModels context, Expression<Func<Apolice, bool>> filters, string sorting)
        {

            IQueryable<Apolice> objectQuery = context.Apolices.Include("veiculo").Include("tomador").Include("concelho").Include("entidade"); ;
            var query = objectQuery.Where(filters == null ? t => 1 == 1 : filters);
            query = query.OrderBy(s => s.numeroApolice);
            return query;
        }
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    }
}
