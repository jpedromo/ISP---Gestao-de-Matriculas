using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Model
{
    public class Pessoa
    {
        //key - interna
<<<<<<< HEAD
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
=======
        public int pessoaId { get; set; }
        
        //Atributos do Segurado
        public string nome { get; set; }                                //checked
        public string numeroIdentificacao { get; set; }                 //checked
        public string nif { get; set; }                                 //checked
        public string morada { get; set; }                              //checked
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public string codigoPostal { get; set; }                        //checked
    }
}