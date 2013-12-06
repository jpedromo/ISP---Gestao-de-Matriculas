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
    public class RoleRepository : IRoleRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Role> All
        {
            get {
                return context.Roles;
            }
        }

        public IQueryable<Role> AllIncluding(params Expression<Func<Role, object>>[] includeProperties)
        {
            IQueryable<Role> query = context.Roles;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Role Find(int id)
        {
            return context.Roles.Find(id);
        }

        public void InsertOrUpdate(Role role)
        {
            if (role.RoleId == default(int))
            {
                context.Roles.Add(role);
            }
            else
            {
                context.Entry(role).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var role = context.Roles.Find(id);
            context.Roles.Remove(role);
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
