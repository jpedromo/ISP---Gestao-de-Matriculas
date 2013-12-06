using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Model
{
    public class Categoria
    {
        //key
        public int categoriaId { get; set; }

        public string codigoCategoriaVeiculo { get; set; }                  //checked

        public string nome { get; set; }

    }
}