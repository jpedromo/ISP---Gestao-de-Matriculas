using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Everis.Core.Data;

namespace ISP.GestaoMatriculas.Model
{
    public class Apolice : Entity<int>
    {
        //Foreign Key interna para entidade
        public int EntidadeId { get; set; }
        public virtual Entidade Entidade { get; set; }

        //Foreign Key para Tomador
        [ForeignKey("Tomador")]
        public int TomadorId { get; set; }
        public virtual Pessoa Tomador { get; set; }

        //Foreign Key para Veiculo
        public int VeiculoId { get; set; }
        public virtual Veiculo Veiculo { get; set; }

        //Foreign Key para Concelho
        [ForeignKey("Concelho")]
        public int ConcelhoCirculacaoId { get; set; }
        public virtual Concelho Concelho { get; set; }

        //Atributos da Apolice
        public string NumeroApolice { get; set; }                    //checked
        public string NumeroCertificadoProvisorio {get; set;}        //checked

        public DateTime DataInicio { get; set; }                     //checked, necessario construcao com dataInicio + HoraInicio(hora, minutos, segundos)
        public DateTime DataFim { get; set; }                        //checked, necessario construcao com dataFim + HoraFim(hora, minutos, segundos)
        public DateTime DataFimPlaneada { get; set; }
        public DateTime DataReporte { get; set; }


        //----------------------------------------------------

        //ForeignKey interna para Eventos
        [ForeignKey("EventoHistorico")]
        public int EventoHistoricoId { get; set; }                            // +idOcorrencia +codigoOperacao
        public virtual EventoHistorico EventoHistorico { get; set; }

        public List<AvisoApolice> AvisosApolice { get; set; }

        public Apolice()
        {
            this.AvisosApolice = new List<AvisoApolice>();
            this.Historico = new List<ApoliceHistorico>();
        }

        public ICollection<ApoliceHistorico> Historico { get; set; }


        public bool temAvisos()
        {
            return AvisosApolice.Count != 0;
        }
    }
}