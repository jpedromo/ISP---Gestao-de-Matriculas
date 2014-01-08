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
    public class EventoHistorico
    {
<<<<<<< HEAD
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
=======
        public int eventoHistoricoId { get; set; }
        public string idOcorrencia { get; set; }
        public string codigoOperacao { get; set; }
        public string seguradoraId { get; set; }
        
                
    }
}
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
