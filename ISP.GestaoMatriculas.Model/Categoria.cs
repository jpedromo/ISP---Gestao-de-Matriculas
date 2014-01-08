using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.Model
{
    public class Categoria
    {
        //key
        [Key]
        [Column("CategoriaId_PK")]
        public int categoriaId { get; set; }
        [Column("CodCategoriaVeiculo")]
        public string codigoCategoriaVeiculo { get; set; }                  //checked
        [Column("Nome")]
        public string nome { get; set; }

    }
}