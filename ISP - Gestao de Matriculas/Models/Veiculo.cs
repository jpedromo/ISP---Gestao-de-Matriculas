using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Models
{
    public class Veiculo
    {
        //Key
        public int veiculoId { get; set; }

        //Foreign Key
        public int categoriaId { get; set; }
        public virtual Categoria categoria { get; set; }

        //atributos veiculo
        public string ano { get; set; }
        public string matricula { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
    }
}