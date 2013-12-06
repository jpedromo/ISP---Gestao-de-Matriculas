using Everis.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Model
{
    [Serializable]
    public class Concelho : Entity<int>
    {
        public string Codigo { get; set; }               //checked
        public string Nome { get; set; }
    }
}