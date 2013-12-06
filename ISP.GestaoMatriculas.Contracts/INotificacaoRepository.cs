using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface INotificacaoRepository : IDisposable
    {
        IQueryable<Notificacao> All { get; }
        IQueryable<Notificacao> AllIncluding(params Expression<Func<Notificacao, object>>[] includeProperties);
        Notificacao Find(int id);
        void InsertOrUpdate(Notificacao notificacao);
        void Delete(int id);
        void Save();
    }
}
