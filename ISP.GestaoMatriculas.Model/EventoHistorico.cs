using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.Model
{
    public class EventoHistorico
    {
        [Key]
        [Column("CodEventoHistoricoId_PK")]
        public int eventoHistoricoId { get; set; }
        [Column("ValIdOcorrencia")]
        [Display(Name = "ID Ocorrência")]
        public string idOcorrencia { get; set; }
        [ForeignKey("codigoOperacao")]
        [Column("CodOperacaoId_FK")]
        public int codigoOperacaoId { get; set; }
        [Display(Name = "Operação")]
        public virtual ValorSistema codigoOperacao { get; set; }
        [Column("DtReporte")]
        [Display(Name = "Data Reporte")]
        public DateTime dataReporte { get; set; }

        [ForeignKey("entidade")]
        [Column("CodEntidadeId_FK")]
        public int? entidadeId { get; set; }
        [Display(Name = "Entidade")]
        public virtual Entidade entidade { get; set; }

        public List<Aviso> avisosEventoHistorico { get; set; }

        public EventoHistorico()
        {
            avisosEventoHistorico = new List<Aviso>();
        }
    }
}
