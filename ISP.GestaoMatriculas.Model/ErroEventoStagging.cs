using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Model
{
    public class ErroEventoStagging
    {
<<<<<<< HEAD
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
=======
        public enum tipoErroEventoStagging
        {
            Generico
        }

        public int ErroEventoStaggingId { get; set; }
        [ForeignKey("eventoStagging")]
        public int eventoStaggingId { get; set; }
        public virtual EventoStagging eventoStagging {get; set;}
        public tipoErroEventoStagging tipologia { get; set; }
        public string descricao { get; set; }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public string campo { get; set; }

    }
}