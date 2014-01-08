using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class PesquisaPublicaViewModel
    {
        public PesquisaPublicaViewModel()
        {
            this.resultado = new List<ApolicePublicaView>();
        }

        [Display(Name="Matrícula")]
        public string matricula { get; set; }
        [Display(Name = "Data de Pesquisa")]
        public DateTime dataPesquisa { get; set; }

        public IEnumerable<ApolicePublicaView> resultado { get; set; }
    }

    public class ApolicePublicaView
    {
        [Display(Name = "Número de Apólice")]
        public string numeroApolice { get; set; }

        [Display(Name = "Seguradora")]
        public string seguradora { get; set; }

        [Display(Name = "Data de Início")]
        public DateTime dataInicio { get; set; }
        [Display(Name = "Data de Fim")]
        public DateTime dataFim { get; set; }

        [Display(Name = "Marca do Veículo")]
        public string marcaVeiculo { get; set; }
        [Display(Name = "Modelo do Veículo")]
        public string modeloVeiculo { get; set; }
    }
}