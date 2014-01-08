using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IAvisoRepository : IDisposable
    {
        IQueryable<Aviso> All { get; }
        IQueryable<Aviso> AllIncluding(params Expression<Func<Aviso, object>>[] includeProperties);
        Aviso Find(int id);
        void InsertOrUpdate(Aviso userProfile);
        void Delete(int id);
        void Save();
    }
}
