using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.Model
{
    public class Config
    {
        //key - interna
        [Key]
        [Column("ConfigId_PK")]
        public int configId { get; set; }
        
        //Atributos do parâmetro de configuração
        [Column("Nome")]
        public string nome { get; set; }                   
        [Column("Descricao")]
        public string descricao { get; set; }              
        [Column("ValorNumerico")]
        public double valNumerico { get; set; }               
        [Column("ValorBooleano")]
        public bool valBooleano { get; set; }               
        [Column("ValorAlfanumerico")]
        public string valAlfanumerico { get; set; }               
    }
}