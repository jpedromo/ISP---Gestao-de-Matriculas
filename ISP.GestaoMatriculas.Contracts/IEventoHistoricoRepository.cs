using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IEventoHistoricoRepository : IDisposable
    {
        IQueryable<EventoHistorico> All { get; }
        IQueryable<EventoHistorico> AllIncluding(params Expression<Func<EventoHistorico, object>>[] includeProperties);
        EventoHistorico Find(int id);
        void InsertOrUpdate(EventoHistorico evento);
        EventoHistorico Insert(EventoHistorico evento);
        bool Update(EventoHistorico evento);
        void Delete(int id);
        void Save();
    }
}
