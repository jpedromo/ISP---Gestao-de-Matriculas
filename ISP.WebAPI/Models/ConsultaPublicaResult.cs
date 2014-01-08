using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.WebAPI.Models
{
    public class ConsultaPublicaResult
    {
        public ConsultaPublicaResult()
        {
            this.listagem = new List<ConsultaPublicaItem>();
        }

        public IEnumerable<ConsultaPublicaItem> listagem { get; set; }
    }

    public class ConsultaPublicaItem
    {
        public string numeroApolice { get; set; }

        public string seguradora { get; set; }

        public DateTime dataInicio { get; set; }
        public DateTime dataFim { get; set; }

        public string marcaVeiculo { get; set; }
        public string modeloVeiculo { get; set; }
    }
}