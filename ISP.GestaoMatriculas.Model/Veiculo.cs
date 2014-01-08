using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    public class Veiculo
    {
        //Key
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
    }
}