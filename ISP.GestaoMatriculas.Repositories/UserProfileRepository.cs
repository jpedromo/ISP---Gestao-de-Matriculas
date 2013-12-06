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
    public class UserProfileRepository : IUserProfileRepository
    {
        DomainModels context = new DomainModels();

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
