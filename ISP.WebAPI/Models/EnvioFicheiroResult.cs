using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.WebAPI.Models
{
    public class EnvioFicheiroResult
    {
        public string seguradora { get; set; }

        public DateTime? dataRecepcao { get; set; }
        public string nomeFicheiro { get; set; }
    }

}