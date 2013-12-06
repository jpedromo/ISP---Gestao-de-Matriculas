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
    public class VeiculoRepository : IVeiculoRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Veiculo> All
        {
            get {
                return context.Veiculos;
            }
        }

        public IQueryable<Veiculo> AllIncluding(params Expression<Func<Veiculo, object>>[] includeProperties)
        {
            IQueryable<Veiculo> query = context.Veiculos;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Veiculo Find(int id)
        {
            return context.Veiculos.Find(id);
        }

        public void InsertOrUpdate(Veiculo veiculo){
            if (veiculo.veiculoId == default(int))
            {
                context.Veiculos.Add(veiculo);
            }
            else
            {
                context.Entry(veiculo).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var veiculo = context.Veiculos.Find(id);
            context.Veiculos.Remove(veiculo);
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
