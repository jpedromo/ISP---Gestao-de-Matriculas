using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IEventoStaggingRepository : IDisposable
    {
        IQueryable<EventoStagging> All { get; }
        IQueryable<EventoStagging> AllIncluding(params Expression<Func<EventoStagging, object>>[] includeProperties);
        EventoStagging Find(int id);
        void InsertOrUpdate(EventoStagging evento);
        EventoStagging Insert(EventoStagging evento);
        bool Update(EventoStagging evento);
        void Delete(int id);
        void Save();
    }
}
