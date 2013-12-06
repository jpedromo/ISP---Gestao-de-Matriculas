using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IVeiculoRepository : IDisposable
    {
        IQueryable<Veiculo> All { get; }
        IQueryable<Veiculo> AllIncluding(params Expression<Func<Veiculo, object>>[] includeProperties);
        Veiculo Find(int id);
        void InsertOrUpdate(Veiculo veiculo);
        void Delete(int id);
        void Save();
    }
}
