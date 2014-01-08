using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Models;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class StaggingEditViewModel
    {
         public StaggingEditViewModel()
        {
            historicoStagging = new List<EventoStagging>();
        }

        public EventoStagging eventoStagging { get; set; }

        public IEnumerable<EventoStagging> historicoStagging { get; set; }

        /*public int categoriaId { get; set; }

        public int concelhoId { get; set; }*/
    }
}