using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Web;

using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.Repositories
{
    public class NotificacaoRepository : INotificacaoRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Notificacao> All
        {
            get {
                return context.Notificacoes;
            }
        }

        public IQueryable<Notificacao> AllIncluding(params Expression<Func<Notificacao, object>>[] includeProperties)
        {
            IQueryable<Notificacao> query = context.Notificacoes;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Notificacao Find(int id)
        {
            return context.Notificacoes.Find(id);
        }

        public void InsertOrUpdate(Notificacao notificacao){
            if (notificacao.notificacaoId == default(int))
            {
                context.Notificacoes.Add(notificacao);
            }
            else
            {
                context.Entry(notificacao).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var notificacao = context.Notificacoes.Find(id);
            context.Notificacoes.Remove(notificacao);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
