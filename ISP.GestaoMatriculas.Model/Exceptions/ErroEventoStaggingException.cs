using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISP.GestaoMatriculas.Model.Exceptions
{
    public class ErroEventoStaggingException : Exception
    {
        public List<ErroEventoStagging> errosEventoStagging {get; set;}

        public ErroEventoStaggingException()
        {
            errosEventoStagging = new List<ErroEventoStagging>();
        }
        
    }
}
