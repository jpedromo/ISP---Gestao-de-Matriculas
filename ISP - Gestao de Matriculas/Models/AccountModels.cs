using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using WebMatrix.WebData;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.Models
{

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Correio eletrónico")]
        public string email { get; set; }

        [Display(Name = "Nome da pessoa")]
        public string nome { get; set; }

        [Display(Name = "Contacto Telefónico")]
        public string telefone { get; set; }

        [Required]
        [Display(Name = "Utilizador Ativo")]
        public bool ativo { get; set; }

        [Required]
        [Display (Name = "Entidade associada")]
        public int entidadeId { get; set; }

    }

    public class EditUserModel
    {
        public int UserId { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "new Password")]
        public string newPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("newPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }

        [Display(Name = "Correio eletrónico")]
        public string email { get; set; }

        [Display(Name = "Nome da pessoa")]
        public string nome { get; set; }

        [Display(Name = "Contacto Telefónico")]
        public string telefone { get; set; }

        [Required]
        [Display(Name = "Utilizador Ativo")]
        public bool ativo { get; set; }

        [Required]
        [Display(Name = "Entidade associada")]
        public int entidadeId { get; set; }

    }



    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    public class UserUtils
    {
        public static bool deactivate(UserProfile user)
        {
            if (WebSecurity.CurrentUserName == user.UserName)
            {
                return false;
            }

            user.ativo = false;
            return true;
        }

        public static bool activate(UserProfile user)
        {
            if (WebSecurity.CurrentUserName == user.UserName)
            {
                return false;
            }
            if (user.entidade == null || !(user.entidade.ativo))
            {
                return false;
            }

            user.ativo = true;
            return true;
        }
    }

}
