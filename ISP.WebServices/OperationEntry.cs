using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.WebServices
{
    public class Reporte
    {
        public List<OperationEntry> OperationList;
    }

    public class OperationEntry
    {
        public string CodigoOperacao { get; set; }
        public string IdSeguradora { get; set; }

        public string Apolice { get; set; }
        public string Matricula { get; set; }
        public DateTime DataInicioCobertura { get; set; }
        public DateTime DataFimCobertura { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public System.Int32 AnoConstrucao { get; set; }

        public System.Int32 CategoriaVeiculo { get; set; }

    }
}