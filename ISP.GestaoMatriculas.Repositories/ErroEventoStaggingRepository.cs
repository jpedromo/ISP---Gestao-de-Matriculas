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

namespace ISP.GestaoMatriculas.Repositories
{
    public class ErroEventoStaggingRepository : IErroEventoStaggingRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<ErroEventoStagging> All
        {
            get
            {
                return context.ErrosEventoStagging;
            }
        }

        public IQueryable<ErroEventoStagging> AllIncluding(params Expression<Func<ErroEventoStagging, object>>[] includeProperties)
        {
            IQueryable<ErroEventoStagging> query = context.ErrosEventoStagging;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ErroEventoStagging Find(int id)
        {
            return context.ErrosEventoStagging.Find(id);
        }

        public void InsertOrUpdate(ErroEventoStagging erroEventoStagging)
        {
            if (erroEventoStagging.eventoStaggingId == default(int))
            {
                context.ErrosEventoStagging.Add(erroEventoStagging);
            }
            else
            {
                context.Entry(erroEventoStagging).State = EntityState.Modified;
            }
        }

        public ErroEventoStagging Insert(ErroEventoStagging erroEventoStagging)
        {
            erroEventoStagging.eventoStaggingId = default(int);
            context.ErrosEventoStagging.Add(erroEventoStagging);
            return erroEventoStagging;
        }

        public bool Update(ErroEventoStagging erroEventoStagging)
        {
            try
            {
                context.Entry(erroEventoStagging).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Delete(int id)
        {
            var erroEventoStagging = context.ErrosEventoStagging.Find(id);
            context.ErrosEventoStagging.Remove(erroEventoStagging);
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
