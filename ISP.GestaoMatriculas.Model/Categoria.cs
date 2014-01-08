using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Model
{
    public class Categoria
    {
        //key
<<<<<<< HEAD
        [Key]
        [Column("CategoriaId_PK")]
        public int categoriaId { get; set; }
        [Column("CodCategoriaVeiculo")]
        public string codigoCategoriaVeiculo { get; set; }                  //checked
        [Column("Nome")]
=======
        public int categoriaId { get; set; }

        public string codigoCategoriaVeiculo { get; set; }                  //checked

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public string nome { get; set; }

    }
}