using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    [Serializable]
    public class ErroFicheiro
    {        
        //key
        public int erroFicheiroId { get; set; }

        //Foreign Key para seguradora
        [ForeignKey("ficheiro")]
        public int ficheiroId { get; set; }
        public virtual Ficheiro ficheiro { get; set; }
        
        //Atributos do ficheiro
        public string descricao { get; set; }
        public DateTime dataValidacao { get; set; }
    }
}