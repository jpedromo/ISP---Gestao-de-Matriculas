using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace ISP.GestaoMatriculas.Model
{
    public class EventoStagging
    {        
        [Key]
        [Column("CodEventoStaggingId_PK")]
        public int eventoStaggingId { get; set; }
        [Column("ValIdOcorrencia")]
        [Display(Name = "ID Ocorrência")]
        public string idOcorrencia { get; set; }
        [Display(Name = "Operação")]
        //[ForeignKey("codigoOperacao")]
        //[Column("CodigoOperacaoId_FK")]
        //public int? codigoOperacaoId { get; set; }
        public string codigoOperacao { get; set; }

        [ForeignKey("entidade")]
        [Column("CodEntidadeId_FK")]
        public int? entidadeId { get; set; }
        [Display(Name = "Entidade")]
        public virtual Entidade entidade { get; set; }

        [Column("NrApolice")]
        [Display(Name = "Nr Apólice")]
        public string nrApolice { get; set; }
        [Column("NrCertificadoProvisorio")]
        [Display(Name = "Nr Certificado provisório")]
        public string nrCertificadoProvisorio { get; set; }
        [Column("DtInicio")]
        [Display(Name = "Data Início")]
        public string dataInicioCobertura { get; set; }
        [Column("DtHoraInicio")]
        [Display(Name = "Hora Início")]
        public string horaInicioCobertura { get; set; }
        [Column("DtFim")]
        [Display(Name = "Data Fim")]
        public string dataFimCobertura { get; set; }
        [Column("DtHoraFim")]
        [Display(Name = "Hora Fim")]
        public string horaFimCobertura { get; set; }

        [Column("XMatricula")]
        [Display(Name = "Matricula")]
        public string matricula { get; set; }
        [Column("XMatriculaCorrigida")]
        [Display(Name = "MatriculaCorrigida")]
        public string matriculaCorrigida { get; set; }
        [Column("NomMarca")]
        [Display(Name = "Marca")]
        public string marca { get; set; }
        [Column("NomModelo")]
        [Display(Name = "Modelo")]
        public string modelo { get; set; }
        [Column("DtAnoConstrucao")]
        [Display(Name = "Ano de construção")]
        public string anoConstrucao { get; set; }
        [Column("CodCategoriaVeiculo")]
        [Display(Name = "Código de categoria")]
        public string codigoCategoriaVeiculo { get; set; }
        [Column("CodConcelhoCirculacao")]
        [Display(Name = "Código de concelho de circulação")]
        public string codigoConcelhoCirculacao { get; set; }

        [Column("NomeTomadorSeguro")]
        [Display(Name = "Tomador")]
        public string nomeTomadorSeguro { get; set; }
        [Column("NomMoradaTomadorSeguro")]
        [Display(Name = "Morada")]
        public string moradaTomadorSeguro { get; set; }
        [Column("CodPostalTomadorSeguro")]
        [Display(Name = "Código postal")]
        public string codigoPostalTomador { get; set; }
        [Column("NrIdentificacaoTomadorSeguro")]
        [Display(Name = "Número de identificação")]
        public string nrIdentificacaoTomadorSeguro { get; set; }
        [Column("NrNIFTomadorSeguro")]
        [Display(Name = "NIF")]
        public string nifTomadorSeguro { get; set; }
        [Column("FlgDeleted")]
        [Display(Name = "Arquivado")]
        public bool arquivado { get; set; }

        [Column("DtFimPlaneada")]
        public DateTime? dataFimPlaneada { get; set; } //Data Fim da primeira criação caso já exista um registo. 

        //Dados de Controlo
        [Column("TotErrosCumulativos")]
        [Display(Name = "Total de erros")]
        public int totalErrosCumulativo { get; set; }
        [Column("TotAvisosCumulativos")]
        [Display(Name = "Total de avisos")]
        public int totalAvisosCumulativo { get; set; }
        [Column("DtReporte")]
        [Display(Name = "Data de reporte da ocorrência")]
        public DateTime dataReporte { get; set; }
        [Display(Name = "Utilizador de reporte da ocorrência")]
        [Column("NomUtilizadorReporte")]
        public string utilizadorReporte { get; set; }

        
        [Display(Name = "Data de criação do registo")]
        [Column("DtRegisto")]
        public DateTime dataRegisto { get; set; }

        [Column("DtUltimaAlteracao")]
        [Display(Name = "Data da última alteração")]
        public DateTime dataUltimaAlteracaoErro { get; set; }

        [Column("DtCorrecaoErro")]
        [Display(Name = "Data de correção do erro")]
        public DateTime? dataCorrecaoErro { get; set; }

        [Display(Name = "Utilizador de correção do erro")]
        [Column("NomeUtilizadorCorrecao")]
        public string utilizadorCorrecao { get; set; }

        [Display(Name = "Data de arquivo do erro")]
        [Column("DtArquivo")]
        public DateTime? dataArquivo { get; set; }
        [Display(Name = "Utilizador de arquivo do erro")]
        [Column("UtilizadorArquivo")]
        public string utilizadorArquivo { get; set; }

        [ForeignKey("estadoEvento")]
        [Column("CodEstadoEventoId_FK")]
        [Display(Name = "Estado")]
        public int estadoEventoId { get; set; }
        [Display(Name = "Estado")]
        public virtual ValorSistema estadoEvento { get; set; }

        [ForeignKey("ficheiro")]
        [Column("CodFicheiroId_FK")]
        public int? ficheiroID { get; set; }
        [Display(Name = "Ficheiro")]
        public virtual Ficheiro ficheiro { get; set; }

        public List<ErroEventoStagging> errosEventoStagging { get; set; }
        public List<Aviso> avisosEventoStagging { get; set; }

        public EventoStagging()
        {
            this.errosEventoStagging = new List<ErroEventoStagging>();
            this.avisosEventoStagging = new List<Aviso>();

            this.codigoOperacao = null;

            this.totalErrosCumulativo = 0;
            this.totalAvisosCumulativo = 0;

            this.dataReporte = DateTime.Now;
            this.dataRegisto = DateTime.Now;
            this.dataUltimaAlteracaoErro = DateTime.Now;
        }

        public EventoStagging(EventoStagging erroSomatorioAritmetico)
        {
            this.errosEventoStagging = new List<ErroEventoStagging>();
            this.avisosEventoStagging = new List<Aviso>();

            this.anoConstrucao = erroSomatorioAritmetico.anoConstrucao;
            this.codigoCategoriaVeiculo = erroSomatorioAritmetico.codigoCategoriaVeiculo;
            this.codigoOperacao = erroSomatorioAritmetico.codigoOperacao; //Mantemos o codigo de operação inicial
            this.codigoPostalTomador = erroSomatorioAritmetico.codigoPostalTomador;
            this.codigoConcelhoCirculacao = erroSomatorioAritmetico.codigoConcelhoCirculacao;
            this.dataFimCobertura = erroSomatorioAritmetico.dataFimCobertura;
            this.dataInicioCobertura = erroSomatorioAritmetico.dataInicioCobertura;

            this.entidadeId = erroSomatorioAritmetico.entidadeId;
            //this.estadoEvento = erroSomatorioAritmetico.estadoEvento;
            this.horaFimCobertura = erroSomatorioAritmetico.horaFimCobertura;
            this.horaInicioCobertura = erroSomatorioAritmetico.horaInicioCobertura;
            this.idOcorrencia = erroSomatorioAritmetico.idOcorrencia;
            this.marca = erroSomatorioAritmetico.marca;
            this.matricula = erroSomatorioAritmetico.matricula;
            this.matriculaCorrigida = matricula.Replace("-", "");
            this.modelo = erroSomatorioAritmetico.modelo;
            this.moradaTomadorSeguro = erroSomatorioAritmetico.moradaTomadorSeguro;
            this.nomeTomadorSeguro = erroSomatorioAritmetico.nomeTomadorSeguro;
            this.nrApolice = erroSomatorioAritmetico.nrApolice;
            this.nrCertificadoProvisorio = erroSomatorioAritmetico.nrCertificadoProvisorio;
            this.nrIdentificacaoTomadorSeguro = erroSomatorioAritmetico.nrIdentificacaoTomadorSeguro;
            this.nifTomadorSeguro = erroSomatorioAritmetico.nifTomadorSeguro;

            this.totalErrosCumulativo = erroSomatorioAritmetico.totalErrosCumulativo;
            this.totalAvisosCumulativo = erroSomatorioAritmetico.totalAvisosCumulativo;
            this.dataReporte = erroSomatorioAritmetico.dataReporte;
            this.dataRegisto = DateTime.Now;
            this.dataUltimaAlteracaoErro = erroSomatorioAritmetico.dataUltimaAlteracaoErro;
        }

        public EventoStagging(Apolice apoliceProducao, int estadoId)
        {
            if (this.codigoOperacao == null)
            {
                this.codigoOperacao = apoliceProducao.eventoHistorico.codigoOperacao.valor;
            }

            this.errosEventoStagging = new List<ErroEventoStagging>();
            this.avisosEventoStagging = new List<Aviso>();

            this.anoConstrucao = apoliceProducao.veiculo.anoConstrucao;
            this.codigoCategoriaVeiculo = apoliceProducao.veiculo.categoriaId == null ? null : apoliceProducao.veiculo.categoria.codigoCategoriaVeiculo;
            this.codigoPostalTomador = apoliceProducao.tomador.codigoPostal;
            if (apoliceProducao.concelho != null)
            {
                this.codigoConcelhoCirculacao = apoliceProducao.concelho.codigoConcelho;
            }
            this.dataFimCobertura = (apoliceProducao.dataFim - apoliceProducao.dataFim.TimeOfDay).ToString("yyyyMMdd");
            this.dataInicioCobertura = (apoliceProducao.dataInicio - apoliceProducao.dataInicio.TimeOfDay).ToString("yyyyMMdd");

            if (apoliceProducao.entidade != null)
            {
                this.entidadeId = apoliceProducao.entidade.entidadeId;
            }
            else
            {
                this.entidadeId = apoliceProducao.entidadeId;
            }

            this.horaFimCobertura = apoliceProducao.dataFim.TimeOfDay.ToString("hhmmss");
            this.horaInicioCobertura = apoliceProducao.dataInicio.TimeOfDay.ToString("hhmmss");
            this.dataFimPlaneada = apoliceProducao.dataFimPlaneada;
            this.idOcorrencia = apoliceProducao.eventoHistorico.idOcorrencia;
            this.marca = apoliceProducao.veiculo.marcaVeiculo;
            this.matricula = apoliceProducao.veiculo.numeroMatricula;
            this.matriculaCorrigida = matricula != null ? matricula.Replace("-", "") : null;
            this.modelo = apoliceProducao.veiculo.modeloVeiculo;
            this.moradaTomadorSeguro = apoliceProducao.tomador.morada;
            this.nomeTomadorSeguro = apoliceProducao.tomador.nome;
            this.nrApolice = apoliceProducao.numeroApolice;
            this.nrCertificadoProvisorio = apoliceProducao.numeroCertificadoProvisorio;
            this.nrIdentificacaoTomadorSeguro = apoliceProducao.tomador.numeroIdentificacao;
            this.nifTomadorSeguro = apoliceProducao.tomador.nif;
            this.arquivado = false;
            this.estadoEventoId = estadoId;

            this.totalErrosCumulativo = 0;
            this.totalAvisosCumulativo = apoliceProducao.avisosApolice.Count;
            this.dataReporte = apoliceProducao.dataReporte;
            this.dataUltimaAlteracaoErro = apoliceProducao.dataReporte;
        }

        public void esmagaDados(ReporteOcorrenciasMatriculaOcorrenciaOrdenada ocorrencia, Ficheiro ficheiro, int operacaoEvento)
        {
            //ocorrencia.CodigoPostalProprietario; Dado a mais no XSD.
            //ocorrencia.CodigoPostalCondutor;
            this.entidadeId = ficheiro.entidadeId;

            //podemos receber ocorrencias via WebService (Ficheiros virtuais não existentes fisicamente)
            if (ficheiro.ficheiroId != default(int))
            {
                this.ficheiroID = ficheiro.ficheiroId;
                this.utilizadorReporte = ficheiro.userName;
            }

            if (!(ocorrencia.AnoConstrucao == null || ocorrencia.AnoConstrucao == string.Empty))
            {
                this.anoConstrucao = ocorrencia.AnoConstrucao;
            }
            if (!(ocorrencia.CodigoCategoriaVeiculo == null || ocorrencia.CodigoCategoriaVeiculo == string.Empty))
            {
                this.codigoCategoriaVeiculo = ocorrencia.CodigoCategoriaVeiculo;
            }
            if (!(ocorrencia.CodigoConcelhoCirculacao == null || ocorrencia.CodigoConcelhoCirculacao == string.Empty))
            {
                this.codigoConcelhoCirculacao= ocorrencia.CodigoConcelhoCirculacao;
            }
            if (ocorrencia.CodigoOperacao != null)
            {
                switch (ocorrencia.CodigoOperacao)
                {
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.C: this.codigoOperacao = "C"; break;
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.M: this.codigoOperacao = "M"; break;
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.A: this.codigoOperacao = "A"; break;
                }
            }

            if (!(ocorrencia.CodigoPostalTomadorSeguro == null || ocorrencia.CodigoPostalTomadorSeguro == string.Empty))
            {
                this.codigoPostalTomador = ocorrencia.CodigoPostalTomadorSeguro;
            }
            if (!(ocorrencia.DataFim == null || ocorrencia.DataFim == string.Empty))
            {
                this.dataFimCobertura = ocorrencia.DataFim;
            }
            if (!(ocorrencia.DataInicio == null || ocorrencia.DataInicio == string.Empty))
            {
                this.dataInicioCobertura = ocorrencia.DataInicio;
            }
            if (!(ocorrencia.HoraFim == null || ocorrencia.HoraFim == string.Empty))
            {
                this.horaFimCobertura = ocorrencia.HoraFim;
            }
            if (!(ocorrencia.HoraInicio == null || ocorrencia.HoraInicio == string.Empty))
            {
                this.horaInicioCobertura = ocorrencia.HoraInicio;
            }
            if (!(ocorrencia.IDOcorrencia == null || ocorrencia.IDOcorrencia == string.Empty))
            {
                this.idOcorrencia = ocorrencia.IDOcorrencia;
            }
            if (!(ocorrencia.MarcaVeiculo == null || ocorrencia.MarcaVeiculo == string.Empty))
            {
                this.marca = ocorrencia.MarcaVeiculo;
            }
            if (!(ocorrencia.ModeloVeiculo == null || ocorrencia.ModeloVeiculo == string.Empty))
            {
                this.modelo = ocorrencia.ModeloVeiculo;
            }
            if (!(ocorrencia.MoradaTomadorSeguro == null || ocorrencia.MoradaTomadorSeguro == string.Empty))
            {
                this.moradaTomadorSeguro = ocorrencia.MoradaTomadorSeguro;
            }
            if (!(ocorrencia.NIFTomadorSeguro == null || ocorrencia.NIFTomadorSeguro == string.Empty))
            {
                this.nifTomadorSeguro = ocorrencia.NIFTomadorSeguro;
            }
            if (!(ocorrencia.NomeTomadorSeguro == null || ocorrencia.NomeTomadorSeguro == string.Empty))
            {
                this.nomeTomadorSeguro = ocorrencia.NomeTomadorSeguro;
            }
            if (!(ocorrencia.NumeroApolice == null || ocorrencia.NumeroApolice == string.Empty))
            {
                this.nrApolice = ocorrencia.NumeroApolice;
            }
            if (!(ocorrencia.NumeroCertificadoProvisorio == null || ocorrencia.NumeroCertificadoProvisorio == string.Empty))
            {
                this.nrCertificadoProvisorio = ocorrencia.NumeroCertificadoProvisorio;
            }
            if (!(ocorrencia.NumeroIdentificacaoTomadorSeguro == null || ocorrencia.NumeroIdentificacaoTomadorSeguro == string.Empty))
            {
                this.nrIdentificacaoTomadorSeguro = ocorrencia.NumeroIdentificacaoTomadorSeguro;
            }
            if (!(ocorrencia.NumeroMatricula == null || ocorrencia.NumeroMatricula == string.Empty))
            {
                this.matricula = ocorrencia.NumeroMatricula;
                this.matriculaCorrigida = matricula.Replace("-", "");
            }

            this.dataReporte = ficheiro.dataAlteracao;
            this.dataUltimaAlteracaoErro = ficheiro.dataAlteracao;
        }

        public void esmagaDados(EventoStagging novoEvento)
        {
            //ocorrencia.CodigoPostalProprietario; Dado a mais no XSD.
            //ocorrencia.CodigoPostalCondutor;

            if (novoEvento.codigoOperacao != null)
            {
                this.codigoOperacao = novoEvento.codigoOperacao;
            }

            if (this.entidadeId == null)
            {
                this.entidadeId = novoEvento.entidadeId;
            }

            if (!(novoEvento.anoConstrucao == null || novoEvento.anoConstrucao == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.anoConstrucao = novoEvento.anoConstrucao;
            }
            if (!(novoEvento.codigoCategoriaVeiculo == null || novoEvento.codigoCategoriaVeiculo == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.codigoCategoriaVeiculo = novoEvento.codigoCategoriaVeiculo;
            }
            if (!(novoEvento.codigoConcelhoCirculacao == null || novoEvento.codigoConcelhoCirculacao == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.codigoConcelhoCirculacao = novoEvento.codigoConcelhoCirculacao;
            }

            if (!(novoEvento.codigoPostalTomador == null || novoEvento.codigoPostalTomador == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.codigoPostalTomador = novoEvento.codigoPostalTomador;
            }
            if (!(novoEvento.dataFimCobertura == null || novoEvento.dataFimCobertura == string.Empty))
            {
                this.dataFimCobertura = novoEvento.dataFimCobertura;
            }
            if (!(novoEvento.dataInicioCobertura == null || novoEvento.dataInicioCobertura == string.Empty))
            {
                this.dataInicioCobertura = novoEvento.dataInicioCobertura;
            }
            if (!(novoEvento.horaFimCobertura == null || novoEvento.horaFimCobertura == string.Empty))
            {
                this.horaFimCobertura = novoEvento.horaFimCobertura;
            }
            if (!(novoEvento.horaInicioCobertura == null || novoEvento.horaInicioCobertura == string.Empty))
            {
                this.horaInicioCobertura = novoEvento.horaInicioCobertura;
            }
            if (!(novoEvento.idOcorrencia == null || novoEvento.idOcorrencia == string.Empty))
            {
                this.idOcorrencia = novoEvento.idOcorrencia;
            }
            if (!(novoEvento.marca == null || novoEvento.marca == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.marca = novoEvento.marca;
            }
            if (!(novoEvento.modelo == null || novoEvento.modelo == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.modelo = novoEvento.modelo;
            }
            if (!(novoEvento.moradaTomadorSeguro == null || novoEvento.moradaTomadorSeguro == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.moradaTomadorSeguro = novoEvento.moradaTomadorSeguro;
            }
            if (!(novoEvento.nifTomadorSeguro == null || novoEvento.nifTomadorSeguro == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.nifTomadorSeguro = novoEvento.nifTomadorSeguro;
            }
            if (!(novoEvento.nomeTomadorSeguro == null || novoEvento.nomeTomadorSeguro == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.nomeTomadorSeguro = novoEvento.nomeTomadorSeguro;
            }
            if (!(novoEvento.nrApolice == null || novoEvento.nrApolice == string.Empty))
            {
                if (this.nrApolice == null)
                {
                    this.nrApolice = novoEvento.nrApolice;
                }
            }
            if (!(novoEvento.nrCertificadoProvisorio == null || novoEvento.nrCertificadoProvisorio == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.nrCertificadoProvisorio = novoEvento.nrCertificadoProvisorio;
            }
            if (!(novoEvento.nrIdentificacaoTomadorSeguro == null || novoEvento.nrIdentificacaoTomadorSeguro == string.Empty) && novoEvento.codigoOperacao != "A")
            {
                this.nrIdentificacaoTomadorSeguro = novoEvento.nrIdentificacaoTomadorSeguro;
            }
            if (!(novoEvento.matricula == null || novoEvento.matricula == string.Empty))
            {
                this.matricula = novoEvento.matricula;
                this.matriculaCorrigida = matricula.Replace("-", "");
            }

            this.dataUltimaAlteracaoErro = novoEvento.dataUltimaAlteracaoErro;
            this.utilizadorReporte = novoEvento.utilizadorReporte;

            this.ficheiroID = novoEvento.ficheiroID;
        }


        public void processaEvento()
        {
        
        }
                 
    }
}
