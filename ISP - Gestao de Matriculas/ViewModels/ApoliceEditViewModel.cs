using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Models;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class ApoliceEditViewModel
    {
         public ApoliceEditViewModel()
        {
            historicoApolices = new List<ApoliceHistorico>();

        }

        public Apolice apolice { get; set; }

        public Pessoa tomador { get; set; }

        public Veiculo veiculo { get; set; }

        public IEnumerable<ApoliceHistorico> historicoApolices { get; set; }

        public int categoriaId { get; set; }

        public int concelhoId { get; set; }
    }
}