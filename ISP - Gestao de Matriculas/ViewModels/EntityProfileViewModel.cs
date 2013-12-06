using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class EntityProfileViewModel
    {
        [Required]
        public int entityId { get; set; }

        [Display(Name = "Nome do Responsável")]
        public string nomeResponsavel { get; set; }

        [Display(Name = "Correio eletrónico do responsável")]
        public string emailResponsavel { get; set; }

        [Display(Name = "Contacto telefónico do responsável")]
        public string telefoneResponsavel { get; set; }
    }
}