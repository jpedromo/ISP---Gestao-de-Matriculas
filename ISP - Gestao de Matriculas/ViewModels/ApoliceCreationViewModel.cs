using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Models;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class ApoliceCreationViewModel
    {
        public Apolice apolice { get; set; }

<<<<<<< HEAD
        public Veiculo veiculo { get; set; }

        public Pessoa tomador { get; set; }

        public int? categoriaId { get; set; }

        public int? concelhoId { get; set; }
=======
        public Pessoa tomador { get; set; }

        public Veiculo veiculo { get; set; }

        public int categoriaId { get; set; }

        public int concelhoId { get; set; }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    }
}