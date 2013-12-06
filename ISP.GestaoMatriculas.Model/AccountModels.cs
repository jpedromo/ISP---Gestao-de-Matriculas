using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        //Foreign Key para Entidade
        public int entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }

        public string UserName { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }

        [Display(Name = "Ativo")]
        public bool ativo { get; set; }
    }

    [Table("webpages_Membership")]
    public class Membership {
        public Membership()
        { Roles = new List<Role>();
            OAuthMemberships = new List<OAuthMembership>();
        }
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; } 
        public DateTime? CreateDate { get; set; }

        [StringLength(128)]
        public string ConfirmationToken{ get; set; }
        public bool? IsConfirmed { get; set; }
        public DateTime? LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }

        [Required, StringLength(128)]
        public string Password { get; set; }
        public DateTime? PasswordChangedDate { get; set; }

        [Required, StringLength(128)]
        public string PasswordSalt { get; set; }

        [StringLength(128)]
        public string PasswordVerificationToken { get; set; }
        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
        public ICollection<Role> Roles { get; set; }

        [ForeignKey("UserId")]
        public ICollection<OAuthMembership> OAuthMemberships { get; set; }
    }


    [Table("webpages_OAuthMembership")]
    public class OAuthMembership { 

        [Key, Column(Order = 0), StringLength(30)]
        public string Provider { get; set; }

        [Key, Column(Order = 1), StringLength(100)]
        public string ProviderUserId { get; set; }
        public int UserId { get; set; }

        [Column("UserId"), InverseProperty("OAuthMemberships")]
        public Membership User { get; set; }
    }


    [Table("webpages_Roles")]
    public class Role {

        public Role() 
        { 
            Members = new List<Membership>();
        }

        [Key]
        public int RoleId { get; set; }

        [StringLength(256)]
        public string RoleName { get; set; }

        public ICollection<Membership> Members { get; set; }
    }

}
