using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.BaseListViewModel;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class FilesUploadListViewModel : ListViewModel
    {
        public FilesUploadListViewModel()
        {
            ficheiros = new List<Ficheiro>();
            PageSize = 20;
        }

        public IEnumerable<Ficheiro> ficheiros { get; set; }
        public int totalNumberOfFicheiros { get; set; }

        [ListViewFilter("fent")]
        [Display(Name = "Entidade")]
        public int entidadeId { get; set; }


    }

    public class FicheiroToCsv
    {
        [Display(Name = "Entidade")]
        public string entidade { get; set; }
        [Display(Name = "Nome")]
        public string nome { get; set; }
        [Display(Name = "Estado")]
        public string estado { get; set; }
        [Display(Name = "Data Upload")]
        public string dataUpload { get; set; }
        [Display(Name = "Data Alteração")]
        public string dataAlteracao { get; set; }
        [Display(Name = "Username")]
        public string username { get; set; }
    }
}