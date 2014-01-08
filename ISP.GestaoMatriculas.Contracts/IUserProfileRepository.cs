﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IUserProfileRepository : IDisposable
    {
        IQueryable<UserProfile> All { get; }
        IQueryable<UserProfile> AllIncluding(params Expression<Func<UserProfile, object>>[] includeProperties);
        UserProfile Find(int id);
<<<<<<< HEAD
        UserProfile FindByUsername(string username);
        UserProfile GetUserByIDIncludeEntidade(int id);
        UserProfile GetUserByUsernameIncludeEntidade(string username);
        IQueryable<UserProfile> GetUserByEntidade(int entidadeId);
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        void InsertOrUpdate(UserProfile userProfile);
        void Delete(int id);
        void Save();
    }
}
