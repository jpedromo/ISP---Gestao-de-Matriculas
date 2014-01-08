using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Models
{
    public class Evento
    {
        public enum TipoEvento { Criacao, Modificacao, Anulamento }

        public int eventoId { get; set; }

        public int entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }

        public int tipoId { get; set; }
        public TipoEvento tipoEvento
        {
            get { return (TipoEvento)this.tipoId; }
            set { this.tipoId = (int)value; }
        }

        public String apoliceAlvoId { get; set; }
        public DateTime dataGeracao { get; set; }

    }
}