using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Models
{
    public class CandidatoApolice
    {
        public int candidatoId { get; set; }

        public int eventoId { get; set; }
        public virtual Evento evento { get; set; }

        //Atributos da Apolice
        public string numApolice { get; set; }
        public string segurNetID { get; set; }
        public string dataInicio { get; set; }
        public string dataFim { get; set; }

        //Atributos da Entidade
        public string entidadeId { get; set; }
        public string nomeEntidade { get; set; }
        public string nomeResponsavel { get; set; }
        public string emailResponsavel { get; set; }
        public string telefoneResponsavel { get; set; }

        //Atributos do Segurado
        public string nomeSegurado { get; set; }
        public string documentoIdentificacao { get; set; }
        public string numeroIdentificacao { get; set; }
        public string identificacaoFiscal { get; set; }

        //Atributos do veiculo
        public string matricula { get; set; }
        public string categoria { get; set; }
        public string ano { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }

        //Atributos do Concelho
        public String concelho { get; set; }
    }
}