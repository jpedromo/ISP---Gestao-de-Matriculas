using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IApoliceRepository : IDisposable
    {
        IQueryable<Apolice> All { get; }
        IQueryable<Apolice> AllIncluding(params Expression<Func<Apolice, object>>[] includeProperties);
        Apolice Find(int id);
        void InsertOrUpdate(Apolice apolice);
        void Delete(int id);
        void Save();
    }
}
