using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.Model
{
    public class ErroEventoStagging
    {
        [Key]
        [Column("CodErroEventoStaggingId_PK")]
        public int erroEventoStaggingId { get; set; }
        
        [ForeignKey("eventoStagging")]
        [Column("CodEventoStaggingId_FK")]
        public int? eventoStaggingId { get; set; }
        public virtual EventoStagging eventoStagging {get; set;}
        
        [ForeignKey("tipologia")]
        [Column("CodTipologiaId_FK")]
        public int tipologiaId { get; set; }
        public virtual ValorSistema tipologia { get; set; }
        [Column("NomDescricao")]
        [Display(Name="Descrição do Erro")]
        public string descricao { get; set; }
        [Column("NomCampo")]
        [Display(Name = "Campo")]
        public string campo { get; set; }

    }
}