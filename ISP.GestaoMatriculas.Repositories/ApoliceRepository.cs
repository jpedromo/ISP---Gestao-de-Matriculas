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
using System.Data.Entity.Core.Objects;
using Microsoft.ApplicationServer.Caching;

namespace ISP.GestaoMatriculas.Repositories
{
    public class ApoliceRepository : IApoliceRepository
    {
        DomainModels context = new DomainModels();
        private DataCache m_cache = null;

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
        
        public void Dispose()
        {
            context.Dispose();
        }



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
    }
}
