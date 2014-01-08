using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.Model
{
    public class Aviso
    {
        [Key]
        [Column("CodAvisoId_PK")]
        public int avisoId { get; set; }

        [ForeignKey("apolice")]
        [Column("CodPeriodoCoberturaId_FK")]
        public int? apoliceId { get; set; }
        public virtual Apolice apolice { get; set; }

        [ForeignKey("apoliceHistorico")]
        [Column("CodPeriodoCoberturaHistoricoId_FK")]
        public int? apoliceHistoricoId { get; set; }
        public virtual ApoliceHistorico apoliceHistorico { get; set; }

        [ForeignKey("eventoStagging")]
        [Column("CodEventoStaggingId_FK")]
        public int? eventoStaggingId { get; set; }
        public virtual EventoStagging eventoStagging { get; set; }

        [ForeignKey("eventoHistorico")]
        [Column("CodEventoHistoricoId_FK")]
        public int? eventoHistoricoId { get; set; }
        public virtual EventoHistorico eventoHistorico { get; set; }

        [ForeignKey("tipologia")]
        [Column("CodTipologiaId_FK")]
        public int tipologiaId { get; set; }
        [Display(Name = "Tipologia")]
        public virtual ValorSistema tipologia { get; set; }
        [Column("NomDescricao")]
        [Display(Name = "Descrição")]
        public string descricao { get; set; }
        [Column("NomCampo")]
        [Display(Name = "Campo")]
        public string campo { get; set; }
    }
}