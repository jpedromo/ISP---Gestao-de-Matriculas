using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    public class ErroEventoStagging
    {
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
        public string campo { get; set; }

    }
}