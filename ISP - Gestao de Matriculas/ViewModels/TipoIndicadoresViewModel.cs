using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class TipoIndicadoresViewModel
    {
        public TipoIndicadoresViewModel()
        {
            nome = string.Empty;
        }

        [Display(Name = "Tipo")]
        public int idTipo { get; set; }
        [Display(Name="Tipo")]
        public string tipo { get; set; }
        [Display(Name = "Nome")]
        public string nome { get; set; }
        [Display(Name = "Data")]
        public DateTime data { get; set; }
        [Display(Name = "Ano")]
        public int dataAno
        {
            get { return this.data.Year; }
        }
        [Display(Name = "Mês")]
        public string dataMes
        {
            get
            {
                switch (this.data.Month)
                {
                    case (1): return "Janeiro";
                    case (2): return "Fevereiro";
                    case (3): return "Março";
                    case (4): return "Abril";
                    case (5): return "Maio";
                    case (6): return "Junho";
                    case (7): return "Julho";
                    case (8): return "Agosto";
                    case (9): return "Setembro";
                    case (10): return "Outrubro";
                    case (11): return "Novembro";
                    case (12): return "Dezembro";
                }
                return "NA";
            }
        }
        
    }
}