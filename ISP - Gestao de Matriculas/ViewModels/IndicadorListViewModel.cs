using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class IndicadorListViewModel
    {
        public IndicadorListViewModel()
        {
            this.dataInicio = DateTime.Now.Date;
            this.dataFim = DateTime.Now.Date;
            this.indicadores = new List<IndicadorViewModel>();
        }

        public List<IndicadorViewModel> indicadores { get; set; }
        
        [UIHint("Date")]
        public DateTime? dataInicio { get; set; }
        [UIHint("Date")]
        public DateTime? dataFim { get; set; }

        
        public int tipoIndicadorId { get; set; }
        public string tipoIndicador { get; set; }
        public string DescricaoTipoIndicador { get; set; }
    }
}