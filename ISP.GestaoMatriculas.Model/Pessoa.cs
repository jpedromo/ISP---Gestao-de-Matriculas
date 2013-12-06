using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Model
{
    public class Pessoa
    {
        //key - interna
        public int pessoaId { get; set; }
        
        //Atributos do Segurado
        public string nome { get; set; }                                //checked
        public string numeroIdentificacao { get; set; }                 //checked
        public string nif { get; set; }                                 //checked
        public string morada { get; set; }                              //checked
        public string codigoPostal { get; set; }                        //checked
    }
}