using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class MatriculaViewModel
    {
        public MatriculaViewModel()
        {
          
        }

        public string matricula { get; set; }
        public string entidadeSeguradora { get; set; }
        public DateTime dataInicio { get; set; }
        public DateTime dataFim { get; set; }
        
    }
}