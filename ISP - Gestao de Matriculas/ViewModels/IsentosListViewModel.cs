using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using System.ComponentModel.DataAnnotations;
using ISP.GestaoMatriculas.BaseListViewModel;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class IsentosListViewModel : ListViewModel
    {
        public IsentosListViewModel()
        {
            apolicesIsentos = new List<ApoliceIsento>();
            matricula = string.Empty;
            PageSize = 20;
            arquivados = false;
        }

        //public string Params { get; set; }

        public IEnumerable<ApoliceIsento> apolicesIsentos { get; set; }

        [ListViewFilter("fent")]
        [Display(Name = "Entidade")]
        public string entidadeResponsavel { get; set; }
        [ListViewFilter("fmat")]
        [Display(Name = "Matricula")]
        public string matricula { get; set; }
        [ListViewFilter("farq")]
        [Display(Name = "Arquivo")]
        public bool arquivados { get; set; }

        public int totalNumberOfIsentos { get; set; }
    }

    public class ApoliceIsentoToCsv
    {
        [Display(Name = "Entidade")]
        public string entidade { get; set; }
        [Display(Name = "Número de Matricula")]
        public string matricula { get; set; }
        [Display(Name = "Data de Início")]
        public string dataInicio { get; set; }
        [Display(Name = "Data de Fim")]
        public string dataFim { get; set; }
        [Display(Name = "Data de Modificação")]
        public string dataModificacao { get; set; }
        [Display(Name = "Info Confidencial")]
        public string confidencial { get; set; }
    }
}