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
<<<<<<< HEAD
using Microsoft.ApplicationServer.Caching;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        DomainModels context = new DomainModels();
<<<<<<< HEAD
        private DataCache m_cache = null;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

        public IQueryable<UserProfile> All
        {
            get {
                return context.UserProfiles;
            }
        }

        public IQueryable<UserProfile> AllIncluding(params Expression<Func<UserProfile, object>>[] includeProperties)
        {
            IQueryable<UserProfile> query = context.UserProfiles;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public UserProfile Find(int id)
        {
            return context.UserProfiles.Find(id);
<<<<<<< HEAD

            m_cache = CacheUtil.GetCache();
            DataCacheItemVersion version = null;
            UserProfile result = (UserProfile)m_cache.Get("UserProfileFind_" + id, out version);
            if (result == null)
            {
                result = context.UserProfiles.Find(id);

                m_cache.Put("UserProfileFind_" + id, result);
            }

            return result;
        }

        public UserProfile FindByUsername(string username)
        {
            m_cache = CacheUtil.GetCache();
            DataCacheItemVersion version = null;
            UserProfile result = (UserProfile)m_cache.Get("UserProfileFindByUsername_" + username, out version);
            if (result == null)
            {
                result = context.UserProfiles.Single(u => u.UserName == username);

                m_cache.Put("UserProfileFindByUsername_" + username, result);
            }

            return result;
        }

        public UserProfile GetUserByIDIncludeEntidade(int id)
        {
            m_cache = CacheUtil.GetCache();
            DataCacheItemVersion version = null;
            UserProfile result = (UserProfile)m_cache.Get("UserProfileGetUserByIDIncludeEntidade_" + id, out version);
            if (result == null)
            {
                result = context.UserProfiles.Include("entidade").Single(u => u.UserId == id);
                m_cache.Put("UserProfileGetUserByIDIncludeEntidade_" + id, result);
            }

            return result;
        }

        public UserProfile GetUserByUsernameIncludeEntidade(string username)
        {
            m_cache = CacheUtil.GetCache();
            DataCacheItemVersion version = null;
            UserProfile result = (UserProfile)m_cache.Get("UserProfileGetUserByUsernameIncludeEntidade_" + username, out version);
            if (result == null)
            {
                result = context.UserProfiles.Include("entidade").Single(u => u.UserName == username);
                m_cache.Put("UserProfileGetUserByUsernameIncludeEntidade_" + username, result);
            }

            return result;
        }

        public IQueryable<UserProfile> GetUserByEntidade(int entidadeId)
        {
            m_cache = CacheUtil.GetCache();
            DataCacheItemVersion version = null;
            IQueryable<UserProfile> result = (IQueryable<UserProfile>)m_cache.Get("UserProfileGetUserByEntidade" + entidadeId, out version);
            if (result == null)
            {
                result = context.UserProfiles.Where(u => u.entidadeId == entidadeId);
                m_cache.Put("UserProfileGetUserByEntidade_" + entidadeId, result);
            }

            return result;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }

        public void InsertOrUpdate(UserProfile userProfile){
            if (userProfile.UserId == default(int))
            {
                context.UserProfiles.Add(userProfile);
            }
            else
            {
                context.Entry(userProfile).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var userProfile = context.UserProfiles.Find(id);
            context.UserProfiles.Remove(userProfile);
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
