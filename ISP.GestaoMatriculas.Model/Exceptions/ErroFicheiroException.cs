using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISP.GestaoMatriculas.Model.Exceptions
{
    public class ErroFicheiroException : Exception
    {
        public List<ErroFicheiro> errosFicheiro {get; set;}

        public ErroFicheiroException()
        {
            errosFicheiro = new List<ErroFicheiro>();
        }
        
    }
}
