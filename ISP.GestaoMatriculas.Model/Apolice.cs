using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ISP.GestaoMatriculas.Model
{
    public class Apolice 
    {

        //Key - Identificador Interno
        [Key]
        [Column("CodPeriodoCoberturaId_PK")]
        public int apoliceId { get; set; } 

        //Foreign Key interna para entidade
        [ForeignKey("entidade")]
        [Column("CodEntidadeId_FK")]
        public int? entidadeId { get; set; }
        [Display(Name="Entidade")]
        public virtual Entidade entidade { get; set; }

        //Foreign Key para Tomador
        [ForeignKey("tomador")]
        [Column("CodTomadorSeguroId_FK")]
        public int tomadorId { get; set; }
        public virtual Pessoa tomador { get; set; }

        //Foreign Key para Veiculo
        [ForeignKey("veiculo")]
        [Column("CodVeiculoId_FK")]
        public int veiculoId { get; set; }
        public virtual Veiculo veiculo { get; set; }

        //Foreign Key para Concelho
        [ForeignKey("concelho")]
        [Column("CodConcelhoCirculacaoId_FK")]
        public int? concelhoCirculacaoId { get; set; }
        [Display(Name = "Concelho")]
        public virtual Concelho concelho { get; set; }

        //Atributos da Apolice
        [Column("NrApolice")]
        [Display(Name="Nr Apólice")]
        public string numeroApolice { get; set; }                    
        [Column("NrCertificadoProvisorio")]
        [Display(Name = "Nr Certificado Provisório")]
        public string numeroCertificadoProvisorio {get; set;}        

        [Column("DtInicio")]
        [Display(Name = "Data Início")]
        public DateTime dataInicio { get; set; }                     //checked, necessario construcao com dataInicio + HoraInicio(hora, minutos, segundos)
        [Column("DtFim")]
        [Display(Name = "Data Fim")]
        public DateTime dataFim { get; set; }                        //checked, necessario construcao com dataFim + HoraFim(hora, minutos, segundos)
        [Column("DtFimPlaneada")]
        [Display(Name = "Data Fim Planeada")]
        public DateTime dataFimPlaneada { get; set; }

        [Column("DtReporte")]
        [Display(Name = "Data Reporte")]
        public DateTime dataReporte { get; set; }
        [Column("NomUtilizadorReporte")]
        [Display(Name = "Utilizador Reporte")]
        public string utilizadorReporte { get; set; }

        [Column("DtRegisto")]
        [Display(Name = "Data Registo")]
        public DateTime dataRegisto { get; set; }
        //Data de Arquivo existe apenas em ApoliceHistorico

        [Column("ValSLADias")]
        [Display(Name = "SLA Dias")]
        public int SLAdays { get; set; }
        [Column("ValSLAHoras")]
        [Display(Name = "SLA Horas")]
        public TimeSpan SLAhours { get; set; }

        [NotMapped]
        [Display(Name="Incumprimento de SLA")]
        public TimeSpan SLA {
            get
            {
                return SLAhours.Add(new TimeSpan(SLAdays, 0, 0, 0));
            }
            set{
                    SLAdays = (int)value.TotalDays;
                    SLAhours = value.Subtract(new TimeSpan(SLAdays, 0, 0, 0));
               }
            }

        [Column("FlgAvisos")]
        public bool avisos { get; set; }


        //----------------------------------------------------

        //ForeignKey interna para Eventos
        [ForeignKey("eventoHistorico")]
        [Column("CodEventoHistoricoId_FK")]
        public int eventoHistoricoId { get; set; }                            // +idOcorrencia +codigoOperacao
        public virtual EventoHistorico eventoHistorico { get; set; }

        public List<Aviso> avisosApolice { get; set; }

        public Apolice()
        {
            this.avisosApolice = new List<Aviso>();
            this.avisos = false;

            this.dataReporte = DateTime.Now;
            this.dataRegisto = DateTime.Now;
        }

        public Apolice(EventoStagging eventoValidado, int? concelhoId, int? categoriaId, int operacaoId, int horaLimiteSLA, int horasExtensaoSLA)
        {   
            DateTime dateAux;
            TimeSpan horaAux;

            this.avisosApolice = new List<Aviso>();

            DateTime.TryParseExact(eventoValidado.dataInicioCobertura, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out dateAux);

            TimeSpan.TryParseExact(eventoValidado.dataInicioCobertura, "hhmmss", new CultureInfo("pt-PT"), TimeSpanStyles.None, out horaAux);

            this.dataInicio = dateAux.Add(horaAux);

            DateTime.TryParseExact(eventoValidado.dataFimCobertura, "yyyyMMdd", new CultureInfo("pt-PT"), DateTimeStyles.None, out dateAux);
            TimeSpan.TryParseExact(eventoValidado.horaFimCobertura, "hhmmss", new CultureInfo("pt-PT"), TimeSpanStyles.None, out horaAux);
            this.dataFim = dateAux.Add(horaAux);

            if(eventoValidado.codigoOperacao == "C")
            {
                this.dataFimPlaneada = dataFim;
            }
            else{
                //Caso a operação não seja de criação, a data final planeada já se deve encontrar preenchida
                this.dataFimPlaneada = (DateTime)eventoValidado.dataFimPlaneada;
            }
            this.dataReporte = eventoValidado.dataUltimaAlteracaoErro;
            this.dataRegisto = DateTime.Now;
            this.utilizadorReporte = eventoValidado.utilizadorReporte;

            switch (eventoValidado.codigoOperacao)
            {
                case "C": 
                    {
                        DateTime dataReferencia = this.dataInicio.Date.Add(new TimeSpan(horaLimiteSLA, 0, 0));
                        DateTime dataLimite;

                        if (this.dataInicio <= dataReferencia){
                            dataLimite = this.dataInicio.Date.AddDays(1); //Devia ser Adiciona dias úteis
                        }else{
                            dataLimite = dataReferencia.Add(new TimeSpan(horasExtensaoSLA, 0, 0)); //Devia ser Adiciona dias úteis
                        }

                        if(this.dataReporte >= dataLimite){
                            this.SLA = this.dataReporte - dataLimite;
                        }
                        else{
                            this.SLA = new TimeSpan(0);
                        }
                        break;
                    }

                case "M":
                    {
                        this.SLA = new TimeSpan(0);
                        break;
                    }

                case "A": 
                    {
                        DateTime dataReferencia = this.dataFim.Date.Add(new TimeSpan(horaLimiteSLA, 0, 0));
                        DateTime dataLimite;

                        if (this.dataInicio <= dataReferencia)
                        {
                            dataLimite = this.dataFim.Date.AddDays(1); //Devia ser Adiciona dias úteis
                        }
                        else
                        {
                            dataLimite = dataReferencia.Add(new TimeSpan(horasExtensaoSLA, 0, 0)); //Devia ser Adiciona dias úteis //Devia ser Adiciona dias úteis
                        }

                        if (this.dataReporte >= dataLimite)
                        {
                            this.SLA = this.dataReporte - dataLimite;
                        }
                        else
                        {
                            this.SLA = new TimeSpan(0);
                        }
                        break;
                    }
            }

            this.concelhoCirculacaoId = concelhoId;
            this.entidadeId = eventoValidado.entidadeId.Value;
            
            this.numeroApolice = eventoValidado.nrApolice;
            this.numeroCertificadoProvisorio = eventoValidado.nrCertificadoProvisorio;
            this.tomador = new Pessoa { codigoPostal = eventoValidado.codigoPostalTomador, morada = eventoValidado.moradaTomadorSeguro,
                nif = eventoValidado.nifTomadorSeguro, nome = eventoValidado.nomeTomadorSeguro, numeroIdentificacao = eventoValidado.nrIdentificacaoTomadorSeguro};
            this.veiculo = new Veiculo { anoConstrucao = eventoValidado.anoConstrucao, categoriaId = categoriaId, marcaVeiculo = eventoValidado.marca,
                modeloVeiculo = eventoValidado.modelo, numeroMatricula = eventoValidado.matricula, numeroMatriculaCorrigido = eventoValidado.matricula.Replace("-","")};

            this.eventoHistorico = new EventoHistorico { entidadeId = eventoValidado.entidadeId.Value, codigoOperacaoId = operacaoId, idOcorrencia = eventoValidado.idOcorrencia, dataReporte = this.dataReporte};
            
            foreach(Aviso a in eventoValidado.avisosEventoStagging)
            {
                this.avisosApolice.Add(a);
                a.apolice = this;
                a.eventoStagging = null;
            }
            this.avisos = this.temAvisos();
            eventoValidado.avisosEventoStagging.Clear();
        }
        
        public bool temAvisos()
        {
            return avisosApolice.Count != 0;
        }
    }
}
