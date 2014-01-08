using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Models
{
    public class Pessoa
    {
        //key
        public int pessoaId { get; set; }

        //Foreign Key para DocumentoIdentificacao
        public int documentoIdentificacaoId { get; set; }
        public virtual DocumentoIdentificacao documentoIdentificacao { get; set; }
        
        //Atributos do Segurado
        public string nome { get; set; }
        public string numeroIdentificacao { get; set; }
        public string identificacaoFiscal { get; set; }
        
    }
}