using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    public class ApoliceHistorico : Apolice
    {
        public int ApoliceId { get; set; }
        public Apolice Apolice { get; set; }
    }
}