using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface ICategoriaRepository : IDisposable
    {
        IQueryable<Categoria> All { get; }
        IQueryable<Categoria> AllIncluding(params Expression<Func<Categoria, object>>[] includeProperties);
        Categoria Find(int id);
        void InsertOrUpdate(Categoria categoria);
        void Delete(int id);
        void Save();
    }
}
