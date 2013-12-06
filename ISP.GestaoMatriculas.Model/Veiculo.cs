using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Model
{
    public class Veiculo
    {
        //Key
        public int veiculoId { get; set; }

        //Foreign Key - interna
        public int categoriaId { get; set; }
        public virtual Categoria categoria { get; set; }

        //atributos veiculo
        public string anoConstrucao { get; set; }             //checked
        public string numeroMatricula { get; set; }           //checked
        public string marcaVeiculo { get; set; }              //checked
        public string modeloVeiculo { get; set; }             //checked
    }
}