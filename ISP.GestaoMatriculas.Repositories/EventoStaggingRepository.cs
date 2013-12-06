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
    public class EventoStaggingRepository : IEventoStaggingRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<EventoStagging> All
        {
            get
            {
                return context.EventosStagging;
            }
        }

        public IQueryable<EventoStagging> AllIncluding(params Expression<Func<EventoStagging, object>>[] includeProperties)
        {
            IQueryable<EventoStagging> query = context.EventosStagging;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public EventoStagging Find(int id)
        {
            return context.EventosStagging.Find(id);
        }

        public void InsertOrUpdate(EventoStagging operationEntry)
        {
            if (operationEntry.eventoStaggingId == default(int))
            {
                context.EventosStagging.Add(operationEntry);
            }
            else
            {
                context.Entry(operationEntry).State = EntityState.Modified;
            }
        }

        public EventoStagging Insert(EventoStagging operationEntry)
        {
            operationEntry.eventoStaggingId = default(int);
            context.EventosStagging.Add(operationEntry);
            return operationEntry;
        }

        public bool Update(EventoStagging operationEntry)
        {
            try
            {
                context.Entry(operationEntry).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Delete(int id)
        {
            var operationEntry = context.EventosStagging.Find(id);
            context.EventosStagging.Remove(operationEntry);
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
