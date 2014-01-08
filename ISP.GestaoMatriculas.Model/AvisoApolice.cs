using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Model
{
    public class AvisoApolice
    {
        public enum tipoAvisoApolice
        {
            Generico
        }

        public int AvisoApoliceId { get; set; }
        public int apoliceId { get; set; }
        public virtual Apolice apolice { get; set; }
        public tipoAvisoApolice tipologia { get; set; }
        public string descricao { get; set; }
        public string campo { get; set; }
    }
}