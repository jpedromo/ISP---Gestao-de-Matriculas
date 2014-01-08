using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.Model
{
    public class UsersContext : DbContext
    {
        public UsersContext()
<<<<<<< HEAD
            : base("DefaultConnection")
=======
            : base("ISPMatriculas")
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Membership> Membership { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OAuthMembership> OAuthMembership { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Membership>()
                .HasMany<Role>(r => r.Roles)
                .WithMany(u => u.Members)
                .Map(m => 
                {
                    m.ToTable("webpages_UsersInRoles");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                });
        }
    }
}
