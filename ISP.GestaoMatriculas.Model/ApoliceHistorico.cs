using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    public class ApoliceHistorico
    {

        //Key - Identificador Interno
        [Key]
        [Column("CodPeriodoCoberturaId_PK")]
        public int apoliceId { get; set; }

        //Foreign Key interna para entidade
        [ForeignKey("entidade")]
        [Column("CodEntidadeId_FK")]
        public int? entidadeId { get; set; }
        [Display(Name = "Entidade")]
        public virtual Entidade entidade { get; set; }

        //Foreign Key para Tomador
        [ForeignKey("tomador")]
        [Column("CodTomadorId_FK")]
        public int tomadorId { get; set; }
        [Display(Name = "Tomador")]
        public virtual Pessoa tomador { get; set; }

        //Foreign Key para Veiculo
        [ForeignKey("veiculo")]
        [Column("CodVeiculoId_FK")]
        public int veiculoId { get; set; }
        [Display(Name = "Veículo")]
        public virtual Veiculo veiculo { get; set; }

        //Foreign Key para Concelho
        [ForeignKey("concelho")]
        [Column("CodConcelhoCirculacaoId_FK")]
        public int? concelhoCirculacaoId { get; set; }
        [Display(Name = "Concelho")]
        public virtual Concelho concelho { get; set; }

        //Atributos da Apolice
        [Column("NrApolice")]
        [Display(Name = "Nr Apólice")]
        public string numeroApolice { get; set; }                   
        [Column("NrCertificadoProvisorio")]
        [Display(Name = "Nr Certificado Provisório")]
        public string numeroCertificadoProvisorio { get; set; }       

        [Column("DtInicio")]
        [Display(Name = "Data Início")]
        public DateTime dataInicio { get; set; }                     
        [Column("DtFim")]
        [Display(Name = "Data Fim")]
        public DateTime dataFim { get; set; }                       
        [Column("DtFimPlaneada")]
        [Display(Name = "Data Fim Planeada")]
        public DateTime dataFimPlaneada { get; set; }
        
        [Column("DtReporte")]
        [Display(Name = "Data Reporte")]
        public DateTime dataReporte { get; set; }
        [Column("NomUtilizadorReporte")]
        [Display(Name = "Utilizador")]
        public string utilizadorReporte { get; set; }

        [Column("DtArquivo")]
        [Display(Name = "Data Arquivo")]
        public DateTime dataArquivo { get; set; }
        [Column("NomUtilizadorArquivo")]
        [Display(Name = "Utilizador Arquivo")]
        public string utilizadorArquivo { get; set; }

        [Column("ValSLADias")]
        [Display(Name = "SLA Dias")]
        public int SLAdays { get; set; }
        [Column("ValSLAHoras")]
        [Display(Name = "SLA Horas")]
        public TimeSpan SLAhours { get; set; }

        [Display(Name = "SLA")]
        [NotMapped]
        public TimeSpan SLA
        {
            get
            {
                return SLAhours.Add(new TimeSpan(SLAdays, 0, 0, 0));
            }
            set
            {
                SLAdays = (int)value.TotalDays;
                SLAhours = value.Subtract(new TimeSpan(SLAdays, 0, 0, 0));
            }
        }


        //----------------------------------------------------

        [ForeignKey("eventoHistorico")]
        [Column("CodEventoHistoricoId_FK")]
        public int eventoHistoricoId { get; set; }                           
        public virtual EventoHistorico eventoHistorico { get; set; }

        public List<Aviso> avisosApoliceHistorico { get; set; }

        public ApoliceHistorico()
        {
        }

        public ApoliceHistorico(Apolice apolice)
        {
            //Foreign Key interna para entidade
            this.entidadeId = apolice.entidadeId; 
            
            //Foreign Key para Tomador
            this.tomadorId = apolice.tomadorId; 
            
            //Foreign Key para Veiculo
            this.veiculoId = apolice.veiculoId;
            
            //Foreign Key para Concelho
            this.concelhoCirculacaoId = apolice.concelhoCirculacaoId; 
            
            //Atributos da Apolice
            this.numeroApolice = apolice.numeroApolice;
            this.numeroCertificadoProvisorio = apolice.numeroCertificadoProvisorio; 
            
            this.dataInicio = apolice.dataInicio;
            this.dataFim = apolice.dataFim;
            this.dataFimPlaneada  = apolice.dataFimPlaneada;

            this.dataReporte = apolice.dataReporte;
            this.utilizadorReporte = apolice.utilizadorReporte;

            this.dataArquivo = DateTime.Now;

            this.SLA = apolice.SLA;

            //ForeignKey interna para Eventos
            this.eventoHistoricoId = apolice.eventoHistoricoId;
        }

    }
}