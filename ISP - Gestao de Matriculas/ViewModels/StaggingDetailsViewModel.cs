using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.ViewModels
{
    public class StaggingDetailsViewModel
    {
        public StaggingDetailsViewModel()
        {
            historicoStagging = new List<EventoStagging>();

        }

        public EventoStagging eventoStagging { get; set; }

        public IEnumerable<EventoStagging> historicoStagging { get; set; }

    }
}