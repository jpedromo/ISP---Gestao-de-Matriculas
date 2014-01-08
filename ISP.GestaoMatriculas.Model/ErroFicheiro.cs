using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.Model
{
    [Serializable]
    public class ErroFicheiro
    {        
        //key
        [Key]
        [Column("CodErroFicheiroId_PK")]
        public int erroFicheiroId { get; set; }

        //Foreign Key para seguradora
        [ForeignKey("ficheiro")]
        [Column("CodFicheiroId_FK")]
        public int? ficheiroId { get; set; }
        public virtual Ficheiro ficheiro { get; set; }
        [ForeignKey("tipologia")]
        [Column("CodTipologiaId_FK")]
        public int tipologiaId { get; set; }
        public virtual ValorSistema tipologia { get; set; }
        //Atributos do ficheiro
        [Column("NomDescricao")]
        public string descricao { get; set; }
        [Column("DtValidacao")]
        public DateTime dataValidacao { get; set; }
    }
}