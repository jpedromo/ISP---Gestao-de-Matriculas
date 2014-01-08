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
<<<<<<< HEAD
        [Required(ErrorMessage="O campo '{0}' é obrigatório.")]
=======
        [Required]
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
<<<<<<< HEAD
        [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
        [DataType(DataType.Password)]
        [Display(Name = "Palavra passe atual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
        [StringLength(100, ErrorMessage = "A {0} deve ter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova palavra passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar palavra passe")]
        [Compare("NewPassword", ErrorMessage = "A nova palavra passe e a confirmação não coincidem.")]
=======
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
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
<<<<<<< HEAD
        [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
        [Display(Name = "Nome de Utilizador")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
        [DataType(DataType.Password)]
        [Display(Name = "Palavra passe")]
        public string Password { get; set; }

        [Display(Name = "Guardar credenciais ?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
=======
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    }

    public class RegisterModel
    {
<<<<<<< HEAD
        [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
        [Display(Name = "Nome de Utilizador")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Palavra passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar palavra passe")]
        [Compare("Password", ErrorMessage = "A palavra passe e a confirmação não coincidem.")]
=======
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
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
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

<<<<<<< HEAD
        
        [Display(Name = "Utilizador AD")]
        public string utilizadorAD { get; set; }

        [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
=======
        [Required]
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        [Display (Name = "Entidade associada")]
        public int entidadeId { get; set; }

    }

    public class EditUserModel
    {
        public int UserId { get; set; }

<<<<<<< HEAD
        [DataType(DataType.Password)]
        [Display(Name = "Nova palavra passe")]
        public string newPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova palavra passe")]
        [Compare("newPassword", ErrorMessage = "A palavra passe e a confirmação não coincidem.")]
=======
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "new Password")]
        public string newPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("newPassword", ErrorMessage = "The password and confirmation password do not match.")]
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
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

<<<<<<< HEAD
        [Display(Name = "Utilizador AD")]
        public string utilizadorAD { get; set; }

        [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
=======
        [Required]
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
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
