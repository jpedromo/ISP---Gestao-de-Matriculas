using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IFicheiroRepository : IDisposable
    {
        IQueryable<Ficheiro> All { get; }
        IQueryable<Ficheiro> AllIncluding(params Expression<Func<Ficheiro, object>>[] includeProperties);
        Ficheiro Find(int id);
        void InsertOrUpdate(Ficheiro ficheiro);
        Ficheiro Insert(Ficheiro ficheiro);
        bool Update(Ficheiro ficheiro);
        void Delete(int id);
        void Save();
    }
}
