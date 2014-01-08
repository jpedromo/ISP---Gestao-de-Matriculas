using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.WebAPI.Models
{
    public class ConsultaPrivadaResult
    {
        public ConsultaPrivadaResult()
        {
            this.listagem = new List<ConsultaPrivadaItem>();
        }

        public IEnumerable<ConsultaPrivadaItem> listagem { get; set; }
    }

    public class ConsultaPrivadaItem
    {
        public string seguradora { get; set; }

        public string numeroMatricula { get; set; }
        public DateTime dataInicio { get; set; }
        public DateTime dataFim { get; set; }
        public string numeroApolice { get; set; }

        public string categoriaVeiculo { get; set; }
        public string marcaVeiculo { get; set; }
        public string modeloVeiculo { get; set; }

        public string nomeTomador { get; set; }
        public string identificacaoTomador { get; set; }
        public string moradaTomador { get; set; }
        public string codigoPostalTomador { get; set; }
    }
}