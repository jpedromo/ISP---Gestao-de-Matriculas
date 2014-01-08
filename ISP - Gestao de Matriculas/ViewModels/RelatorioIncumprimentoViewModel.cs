using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using System.ComponentModel.DataAnnotations;
using ISP.GestaoMatriculas.BaseListViewModel;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class RelatorioIncumprimentoViewModel : ListViewModel
    {
        public RelatorioIncumprimentoViewModel()
        {
            this.resultado = new List<RegistoIncumprimentoView>();
            PageSize = 25;
        }

        public int totalNumberOfRecords { get; set; }

        [ListViewFilter("fent")]
        [Required(ErrorMessage="O campo '{0}' é obrigatório.")]
        [Display(Name="Entidade")]
        public int entidadeId { get; set; }

        [ListViewFilter("foper")]
        [Display(Name = "Operação")]
        public int? operacao { get; set; }

        [ListViewFilter("fdti")]
        [Display(Name = "Desde")]
        public DateTime dataInicio { get; set; }

        [ListViewFilter("fdtf")]
        [Display(Name = "Até")]
        public DateTime dataFim { get; set; }

        public IEnumerable<RegistoIncumprimentoView> resultado { get; set; }
    }

    public class RegistoIncumprimentoView
    {
        public enum TipoApolice { Ativo, Historico }

        [Display(Name = "Operacao")]
        public string operacao {get; set;}

        [Display(Name = "Apólice")]
        public string numeroApolice { get; set; }

        [Display(Name = "Entidade")]
        public string seguradora { get; set; }

        [Display(Name = "Matricula")]
        public string matricula {get; set;}

        [Display(Name = "Incumprimento de SLA")]
        public TimeSpan SLA {get; set;}

        [Display(Name = "Data de início")]
        public DateTime dataInicio { get; set; }
        [Display(Name = "Data de fim")]
        public DateTime dataFim { get; set; }

        public int registoId { get; set; }
        public TipoApolice tipo { get; set; }
    }

    public class RegistoIncumprimentoCsv
    {
        public enum TipoApolice { Ativo, Historico }

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

        [Display(Name = "Incumprimento de SLA")]
        public TimeSpan SLA { get; set; }
        
    }
}