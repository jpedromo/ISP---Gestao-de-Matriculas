using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class FilesUploadViewModel
    {
        public FilesUploadViewModel()
        {
            ficheiros = new List<Ficheiro>();
            upload = true;
        }

        public IEnumerable<Ficheiro> ficheiros { get; set; }

        public bool upload { get; set; }

    }
}