﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class UserProfileViewModel
    {
<<<<<<< HEAD
            [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
=======
            [Required]
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            public int userId { get; set; }

            [Display(Name = "Nome da pessoa")]
            public string nome { get; set; }

            [Display(Name = "Correio eletrónico")]
            public string email { get; set; }

            [Display(Name = "Contacto Telefónico")]
            public string telefone { get; set; }

            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password Antiga")]
            public string oldPassword { get; set; }

            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nova Password")]
            public string newPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Password")]
            [Compare("newPassword", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmNewPassword { get; set; }

<<<<<<< HEAD
            public string utilizadorAD { get; set; }

=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    }
}