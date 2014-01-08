using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IIndicadorRepository : IDisposable
    {
        IQueryable<Indicador> All { get; }
        IQueryable<Indicador> AllIncluding(params Expression<Func<Indicador, object>>[] includeProperties);
        Indicador Find(int id);
        IQueryable<Indicador> Find(int entidadeId, int tipologiaId);
        IQueryable<Indicador> Find(int entidadeId, int tipologiaId, DateTime date);
        IQueryable<Indicador> Find(int entidadeId, int tipologiaId, string aux, DateTime date);
        void InsertOrUpdate(Indicador indicador);
        void Delete(int id);
        void Save();
    }
}
