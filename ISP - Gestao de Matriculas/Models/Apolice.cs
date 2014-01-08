using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Models
{
    public class Apolice
    {
        public enum IdentificacaoEnum {BilheteIdentidade, CartaoCidadao, Passaporte};

        //Key
        public int apoliceId { get; set; }

        public int? entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }

        //Foreign Key para Segurado
        public int seguradoId { get; set; }
        public virtual Pessoa segurado { get; set; }

        //Foreign Key para Veiculo
        public int veiculoId { get; set; }
        public virtual Veiculo veiculo { get; set; }

        //Foreign Key para Concelho
        public int concelhoId { get; set; }
        public virtual Concelho concelho { get; set; }

        //Atributos da Apolice
        public string numApolice { get; set; }      
        public string segurNetID { get; set; }
        public DateTime dataInicio { get; set; }
        public DateTime dataFim { get; set; }

        public string estado { get; set; }

    }
}