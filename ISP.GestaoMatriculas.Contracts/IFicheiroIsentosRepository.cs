using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IFicheiroIsentosRepository : IDisposable
    {
        IQueryable<FicheiroIsentos> All { get; }
        IQueryable<FicheiroIsentos> AllIncluding(params Expression<Func<FicheiroIsentos, object>>[] includeProperties);
        FicheiroIsentos Find(int id);
        void InsertOrUpdate(FicheiroIsentos ficheiro);
        FicheiroIsentos Insert(FicheiroIsentos ficheiro);
        bool Update(FicheiroIsentos ficheiro);
        void Delete(int id);
        void Save();
    }
}
