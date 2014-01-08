using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.BaseListViewModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class ApoliceListViewModel : ListViewModel
    {
        public ApoliceListViewModel()
        {
            apolicesEfetivas = new List<Apolice>();
            eventosStagging = new List<EventoStagging>();
            eventosProcessados = new List<EventoStagging>();
            apolice = string.Empty;
            matricula = string.Empty;
            arquivados = false;
            avisos = false;
            PageSize = 20;
            TabNumber = 1;
            dataInicio = DateTime.Now.Date.AddMonths(-1);
            dataFim = DateTime.Now.Date;
        }

        
        //public string Params { get; set; }

        public IEnumerable<Apolice> apolicesEfetivas { get; set; }
        public IEnumerable<EventoStagging> eventosStagging { get; set; }
        public IEnumerable<EventoStagging> eventosProcessados { get; set; }

        [ListViewFilter("fent")]
        [Display(Name = "Entidade")]
        public int entidade { get; set; }
        [ListViewFilter("fapol")]
        [Display(Name = "Apólice")]
        public string apolice { get; set; }
        [ListViewFilter("fmat")]
        [Display(Name = "Matrícula")]
        public string matricula { get; set; }
        [ListViewFilter("favi")]
        [Display(Name = "Avisos")]
        public bool avisos { get; set; }
        [ListViewFilter("dtini")]
        [Display(Name = "Data Início")]
        public DateTime dataInicio { get; set; }
        [ListViewFilter("dtfim")]
        [Display(Name = "Data Fim")]
        public DateTime dataFim { get; set; }

        [Display(Name = "Arquivados")]
        public bool arquivados { get; set; }
        
        public int totalNumberOfApolices { get; set; }
        public int totalNumberOfEventos { get; set; }
        public int totalNumberOfEventosProcessados { get; set; }
    }


    public class ApoliceCsvViewModel
    {
        [Display(Name="Entidade")]
        public string entidade { get; set; }
        [Display(Name="Número de Apólice")]
        public string apolice{ get; set; }
        [Display(Name="Data de Início")]
        public string dataInicio{ get; set; }
        [Display(Name="Data de Fim")]
        public string dataFim{ get; set; }
        [Display(Name="Número de Matricula")]
        public string matricula{ get; set; }
        [Display(Name="Nome do Tomador")]
        public string tomador{ get; set; }
        [Display(Name="Concelho de Circulação")]
        public string concelho{ get; set; }
    }

    public class EventoCsvViewModel
    {
        [Display(Name = "Entidade")]
        public string entidade { get; set; }
        [Display(Name = "Código de Operação")]
        public string operacao { get; set; }
        [Display(Name = "Número de Apólice")]
        public string apolice { get; set; }
        [Display(Name = "Data de Início")]
        public string dataInicio { get; set; }
        [Display(Name = "Data de Fim")]
        public string dataFim { get; set; }
        [Display(Name = "Número de Matricula")]
        public string matricula { get; set; }
        [Display(Name = "Nome do Tomador")]
        public string tomador { get; set; }
    }

    public class EventoProcessadoCsvViewModel
    {
        [Display(Name = "Entidade")]
        public string entidade { get; set; }
        [Display(Name = "Estado")]
        public string estado { get; set; }
        [Display(Name = "Código de Operação")]
        public string operacao { get; set; }
        [Display(Name = "Número de Apólice")]
        public string apolice { get; set; }
        [Display(Name = "Data de Início")]
        public string dataInicio { get; set; }
        [Display(Name = "Data de Fim")]
        public string dataFim { get; set; }
        [Display(Name = "Número de Matricula")]
        public string matricula { get; set; }
        [Display(Name = "Nome do Tomador")]
        public string tomador { get; set; }
    }
}