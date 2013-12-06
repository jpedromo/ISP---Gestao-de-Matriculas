using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    public class Apolice
    {

        //Key - Identificador Interno
        public int apoliceId { get; set; } 

        //Foreign Key interna para entidade
        public int? entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }

        //Foreign Key para Tomador
        [ForeignKey("tomador")]
        public int tomadorId { get; set; }
        public virtual Pessoa tomador { get; set; }

        //Foreign Key para Veiculo
        public int veiculoId { get; set; }
        public virtual Veiculo veiculo { get; set; }

        //Foreign Key para Concelho
        [ForeignKey("concelho")]
        public int concelhoCirculacaoId { get; set; }
        public virtual Concelho concelho { get; set; }

        //Atributos da Apolice
        public string numeroApolice { get; set; }                    //checked
        public string numeroCertificadoProvisorio {get; set;}        //checked

        public DateTime dataInicio { get; set; }                     //checked, necessario construcao com dataInicio + HoraInicio(hora, minutos, segundos)
        public DateTime dataFim { get; set; }                        //checked, necessario construcao com dataFim + HoraFim(hora, minutos, segundos)
        public DateTime dataFimPlaneada { get; set; }
        public DateTime dataReporte { get; set; }


        //----------------------------------------------------

        //ForeignKey interna para Eventos
        [ForeignKey("eventoHistorico")]
        public int eventoHistoricoId { get; set; }                            // +idOcorrencia +codigoOperacao
        public virtual EventoHistorico eventoHistorico { get; set; }

        public List<AvisoApolice> avisosApolice { get; set; }

        public Apolice()
        {
            this.avisosApolice = new List<AvisoApolice>();
        }


        public bool temAvisos()
        {
            return avisosApolice.Count != 0;
        }
    }
}