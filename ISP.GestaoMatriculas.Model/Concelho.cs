using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.Model
{
    [Serializable]
    public class Concelho
    {
        //key - interna
        [Key]
        [Column("ConcelhoId_PK")]
        public int concelhoId { get; set; }
        [Column("CodConcelho")]
        public string codigoConcelho { get; set; }               //checked
        [Column("NomeConcelho")]
        public string nomeConcelho { get; set; }

    }
}