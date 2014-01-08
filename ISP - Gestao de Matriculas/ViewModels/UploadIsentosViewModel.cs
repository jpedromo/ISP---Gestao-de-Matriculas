using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.BaseListViewModel;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class UploadIsentosViewModel : ListViewModel
    {
        public UploadIsentosViewModel()
        {
            ficheiros = new List<FicheiroIsentos>();
            PageSize = 20;
        }

        public IEnumerable<FicheiroIsentos> ficheiros { get; set; }
        public int totalNumberOfFicheiros { get; set; }

        [ListViewFilter("fent")]
        [Display(Name = "Entidade")]
        public int entidadeId { get; set; }

    }
}