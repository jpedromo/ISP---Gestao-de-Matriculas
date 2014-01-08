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
    public class EventoHistoricoRepository : IEventoHistoricoRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<EventoHistorico> All
        {
            get
            {
                return context.EventosHistorico;
            }
        }

        public IQueryable<EventoHistorico> AllIncluding(params Expression<Func<EventoHistorico, object>>[] includeProperties)
        {
            IQueryable<EventoHistorico> query = context.EventosHistorico;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public EventoHistorico Find(int id)
        {
            return context.EventosHistorico.Find(id);
        }

        public void InsertOrUpdate(EventoHistorico eventoHistorico)
        {
            if (eventoHistorico.eventoHistoricoId == default(int))
            {
                context.EventosHistorico.Add(eventoHistorico);
            }
            else
            {
                context.Entry(eventoHistorico).State = EntityState.Modified;
            }
        }

        public EventoHistorico Insert(EventoHistorico eventoHistorico)
        {
            eventoHistorico.eventoHistoricoId = default(int);
            context.EventosHistorico.Add(eventoHistorico);
            return eventoHistorico;
        }

        public bool Update(EventoHistorico eventoHistorico)
        {
            try
            {
                context.Entry(eventoHistorico).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Delete(int id)
        {
            var eventoHistorico = context.EventosHistorico.Find(id);
            context.EventosHistorico.Remove(eventoHistorico);
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
