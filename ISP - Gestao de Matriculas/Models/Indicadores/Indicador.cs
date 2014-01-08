using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Models.Indicadores
{
    public abstract class Indicador
    {

        public int indicadorId { get; set; }

        public int? entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }

        public string descricao { get; set; }

        public double valor { get; set; }

        public abstract Indicador calcular(DomainModels db);

    }
}