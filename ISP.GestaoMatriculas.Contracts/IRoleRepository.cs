using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IRoleRepository : IDisposable
    {
        IQueryable<Role> All { get; }
        IQueryable<Role> AllIncluding(params Expression<Func<Role, object>>[] includeProperties);
        Role Find(int id);
        void InsertOrUpdate(Role userProfile);
        void Delete(int id);
        void Save();
    }
}
