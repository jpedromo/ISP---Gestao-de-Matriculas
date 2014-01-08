using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;
using System.Threading;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IValorSistemaRepository : IDisposable
    {
        IQueryable<ValorSistema> All { get; }
        IQueryable<ValorSistema> AllIncluding(params Expression<Func<ValorSistema, object>>[] includeProperties);
        ValorSistema Find(int id);
        List<ValorSistema> GetPorTipologia(string tipologia);
        List<ValorSistema> GetPorTipologia(string tipologia, Mutex cacheMutex);
        void InsertOrUpdate(ValorSistema valorSistema);
        void Delete(int id);
        void Save();
    }
}
