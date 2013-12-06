using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class ApoliceListViewModel
    {
        public ApoliceListViewModel()
        {
            apolicesEfetivas = new List<Apolice>();
            apolice = string.Empty;
            matricula = string.Empty;
            erros = false;
            apagados = false;
            avisos = false;
        }

        //public string Params { get; set; }

        public IEnumerable<Apolice> apolicesEfetivas { get; set; }

        public int entidade { get; set; }
        public string apolice { get; set; }
        public string matricula { get; set; }
        public bool avisos { get; set; }

        public bool apagados { get; set; }

        public bool erros { get; set; }


        //IEnumerable<ApoliceErro> ApolicesErradas { get; set; }

    }
}