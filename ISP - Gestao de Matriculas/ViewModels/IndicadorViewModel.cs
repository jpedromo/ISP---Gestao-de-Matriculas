using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class IndicadorViewModel
    {
        public IndicadorViewModel()
        {

        }

        public string Entidade { get; set; }
        public string Valor1 { get; set; }
        public string Valor2 { get; set; }
        public string Valor3 { get; set; }
        public string Valor4 { get; set; }
        public string Valor5 { get; set; }
        public string Valor6 { get; set; }
        public string Valor7 { get; set; }
        public string Valor8 { get; set; }
        public string Header1 { get; set; }
        public string Header2 { get; set; }
        public string Header3 { get; set; }
        public string Header4 { get; set; }
        public string Header5 { get; set; }
        public string Header6 { get; set; }
        public string Header7 { get; set; }
        public string Header8 { get; set; }
    }
}