using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IErroEventoStaggingRepository : IDisposable
    {
        IQueryable<ErroEventoStagging> All { get; }
        IQueryable<ErroEventoStagging> AllIncluding(params Expression<Func<ErroEventoStagging, object>>[] includeProperties);
        ErroEventoStagging Find(int id);
        void InsertOrUpdate(ErroEventoStagging erroEvento);
        ErroEventoStagging Insert(ErroEventoStagging erroEvento);
        bool Update(ErroEventoStagging erroEvento);
        void Delete(int id);
        void Save();
    }
}
