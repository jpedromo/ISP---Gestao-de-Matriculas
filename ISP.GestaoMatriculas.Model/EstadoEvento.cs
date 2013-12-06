using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Model
{
    public class EstadoEvento
    {
        public enum TipoEstado { Valido, Aviso, Erro }

        public int estadoEventoId { get; set; }

        public string descricao { get; set; }

        public int tipoId { get; set; }
        public TipoEstado tipoEstado
        {
            get { return (TipoEstado)this.tipoId; }
            set { this.tipoId = (int)value; }
        }

    }
}