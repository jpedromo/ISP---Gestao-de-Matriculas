using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    public class ValorSistema
    {
        public ValorSistema()
        {

        }

        [Key]
        [Column("CodValorSistemaId_PK")]
        public int valorSistemaId { get; set; }
        [Column("CodTipologia")]
        public string tipologia { get; set; }
        [Column("Valor")]
        public string valor { get; set; }
        [Column("NomDescricao")]
        public string descricao { get; set; }
        [Column("NomDescricaoLonga")]
        public string descricaoLonga { get; set; }
        [Column("FlgEditavel")]
        public bool editavel { get; set; }
      

    }
}