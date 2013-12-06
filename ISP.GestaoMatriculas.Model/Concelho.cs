using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Model
{
    [Serializable]
    public class Concelho
    {
        //key - interna
        public int concelhoId { get; set; }

        public string codigoConcelho { get; set; }               //checked

        public string nomeConcelho { get; set; }

    }
}