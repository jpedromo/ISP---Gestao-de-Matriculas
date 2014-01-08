using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Model
{
    public class Veiculo
    {
        //Key
<<<<<<< HEAD
        [Key]
        [Column("CodVeiculoId_PK")]
        public int veiculoId { get; set; }

        //Foreign Key - interna
        [ForeignKey("categoria")]
        [Column("CodCategoriaId_FK")]
        public int? categoriaId { get; set; }
        [Display(Name = "Categoria")]
        public virtual Categoria categoria { get; set; }

        //atributos veiculo
        [Column("DtAnoConstrucao")]
        [Display(Name = "Ano Construção")]
        public string anoConstrucao { get; set; }             
        [Column("XNumeroMatricula")]
        [Display(Name = "Matrícula")]
        public string numeroMatricula { get; set; }           
        [Column("XNumeroMatriculaCorrigido")]
        [Display(Name = "Matrícula Corrigida")]
        public string numeroMatriculaCorrigido { get; set; }           
        [Column("NomMarcaVeiculo")]
        [Display(Name = "Marca")]
        public string marcaVeiculo { get; set; }              
        [Column("NomModeloVeiculo")]
        [Display(Name = "Modelo")]
        public string modeloVeiculo { get; set; }             
=======
        public int veiculoId { get; set; }

        //Foreign Key - interna
        public int categoriaId { get; set; }
        public virtual Categoria categoria { get; set; }

        //atributos veiculo
        public string anoConstrucao { get; set; }             //checked
        public string numeroMatricula { get; set; }           //checked
        public string marcaVeiculo { get; set; }              //checked
        public string modeloVeiculo { get; set; }             //checked
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    }
}