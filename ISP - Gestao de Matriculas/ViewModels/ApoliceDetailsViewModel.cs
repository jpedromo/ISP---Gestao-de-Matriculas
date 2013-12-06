using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class ApoliceDetailsViewModel
    {
        public ApoliceDetailsViewModel()
        {
            historicoApolices = new List<ApoliceHistorico>();

        }

        public Apolice apolice { get; set; }

        public IEnumerable<ApoliceHistorico> historicoApolices { get; set; }

    }
}