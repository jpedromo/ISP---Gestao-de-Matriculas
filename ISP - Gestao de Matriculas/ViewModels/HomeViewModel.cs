using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            this.listaAvisos = new List<double>();
            this.listaAvisos = new List<double>();
        }

        public double nrErros { get; set; }
        public double nrApolices { get; set; }
        public double nrNovasApolices { get; set; }
        public List<double> listaAvisos { get; set; }
        public List<double> listaErros { get; set; }
        public List<double> listaEventos { get; set; }
        public List<double> listaForaSLA { get; set; }
        public List<string> listaDatas { get; set; }

    }
}