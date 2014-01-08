using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.Model
{
    public class Pessoa
    {
        //key - interna
        [Key]
        [Column("CodTomadorSeguroId_PK")]
        public int pessoaId { get; set; }
        
        //Atributos do Segurado
        [Column("Nome")]
        [Display(Name = "Tomador")]
        public string nome { get; set; }                                //checked
        [Column("NrIdentificacao")]
        [Display(Name = "Nr Identificação")]
        public string numeroIdentificacao { get; set; }                 //checked
        [Column("NrNIF")]
        [Display(Name = "NIF")]
        public string nif { get; set; }                                 //checked
        [Column("NomMorada")]
        [Display(Name = "Morada")]
        public string morada { get; set; }                              //checked
        [Column("CodPostal")]
        [Display(Name = "Código Postal")]
        public string codigoPostal { get; set; }                        //checked
    }
}