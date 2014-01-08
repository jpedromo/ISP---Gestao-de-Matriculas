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
using Microsoft.ApplicationServer.Caching;
using System.Threading;

namespace ISP.GestaoMatriculas.Repositories
{
    public class ValorSistemaRepository : IValorSistemaRepository
    {
        DomainModels context = new DomainModels();
        private DataCache m_cache = null;

        public IQueryable<ValorSistema> All
        {
            get {
                return context.ValoresSistema;
            }
        }

        public IQueryable<ValorSistema> AllIncluding(params Expression<Func<ValorSistema, object>>[] includeProperties)
        {
            IQueryable<ValorSistema> query = context.ValoresSistema;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ValorSistema Find(int id)
        {
            return context.ValoresSistema.Find(id);
        }

        public List<ValorSistema> GetPorTipologia(string tipologia)
        {

            m_cache = CacheUtil.GetCache();
            DataCacheItemVersion version = null;

            List<ValorSistema> result = (List<ValorSistema>)m_cache.Get("ValoresSistema_" + tipologia, out version);
            if (result == null)
            {
                result = context.ValoresSistema.Where(v => v.tipologia == tipologia).ToList();

                m_cache.Put("ValoresSistema_" + tipologia, result);
            }
            return result;
        }

        public List<ValorSistema> GetPorTipologia(string tipologia, Mutex cacheMutex)
        {
            List<ValorSistema> result = null;

            if (cacheMutex != null)
            {
                cacheMutex.WaitOne();
                try
                {
                    result = GetPorTipologia(tipologia);
                }
                finally
                {
                    cacheMutex.ReleaseMutex();
                }
            }
            else
            {
                result = GetPorTipologia(tipologia);
            }

            return result;
        }

        public void InsertOrUpdate(ValorSistema valorSistema){
            if (valorSistema.valorSistemaId == default(int))
            {
                context.ValoresSistema.Add(valorSistema);
            }
            else
            {
                context.Entry(valorSistema).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var valorSistema = context.ValoresSistema.Find(id);
            context.ValoresSistema.Remove(valorSistema);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
