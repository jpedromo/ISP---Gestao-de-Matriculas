using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using System.ComponentModel.DataAnnotations;
using ISP.GestaoMatriculas.BaseListViewModel;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class RelatorioInatividadeViewModel : ListViewModel
    {
        public RelatorioInatividadeViewModel()
        {
            this.resultado = new List<RegistoInatividadeView>();
            PageSize = 25;
        }

        public int totalNumberOfRecords { get; set; }

        [ListViewFilter("fent")]
        [Display(Name = "Entidade")]
        public int entidadeId { get; set; }

        [Display(Name="Data de inatividade")]
        public DateTime dataInatividade { get; set; }

        public IEnumerable<RegistoInatividadeView> resultado { get; set; }
    }

    public class RegistoInatividadeView
    {
        [Display(Name = "Operacao")]
        public string operacao {get; set;}

        [Display(Name = "Apólice")]
        public string numeroApolice { get; set; }

        [Display(Name = "Entidade")]
        public string seguradora { get; set; }

        [Display(Name = "Matricula")]
        public string matricula {get; set;}

        [Display(Name = "Data de início")]
        public DateTime dataInicio { get; set; }

        [Display(Name = "Data de fim")]
        public DateTime dataFim { get; set; }

        [Display(Name = "Data do último reporte")]
        public DateTime dataUltimoReporte { get; set; }

        public int registoId { get; set; }
    }

    public class RegistoInatividadeToCsv
    {
        [Display(Name = "Operacao")]
        public string operacao { get; set; }

        [Display(Name = "Apólice")]
        public string numeroApolice { get; set; }

        [Display(Name = "Entidade")]
        public string seguradora { get; set; }

        [Display(Name = "Matricula")]
        public string matricula { get; set; }

        [Display(Name = "Data de início")]
        public DateTime dataInicio { get; set; }

        [Display(Name = "Data de fim")]
        public DateTime dataFim { get; set; }

        [Display(Name = "Data do último reporte")]
        public DateTime dataUltimoReporte { get; set; }

    }
}