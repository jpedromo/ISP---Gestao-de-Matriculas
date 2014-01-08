using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using System.Xml.Serialization;
<<<<<<< HEAD
using ISP.GestaoMatriculas.Model;
using System.IO;
using System.Xml.Schema;
using System.Reflection;
using System.Xml;
using System.ComponentModel.DataAnnotations;
=======
using ISP.GestaoMatriculas.Utils;
using System.IO;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Model
{
    [Serializable]
    public class Ficheiro
    {
<<<<<<< HEAD
        
        //key
        [Key]
        [Column("CodFicheiroId_PK")]
=======
        public enum EstadoFicheiro
        {
            submetido,
            pendente,
            emProcessamento,
            processado,
            notificado,
            cancelado,
            erro
        }

        //key
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public int ficheiroId { get; set; }

        //Foreign Key para seguradora
        [ForeignKey("entidade")]
<<<<<<< HEAD
        [Column("CodEntidadeId_FK")]
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public int entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }
        
        //Atributos do ficheiro
<<<<<<< HEAD
        [Column("NomeFicheiro")]
        [Display(Name="Nome Ficheiro")]
        public string nomeFicheiro { get; set; }
        [Column("XLocalizacao")]
        public string localizacao { get; set; }
        
        [ForeignKey("estado")]
        [Column("CodEstadoId_FK")]
        public int? estadoId { get; set; }
        [Display(Name = "Estado")]
        public virtual ValorSistema estado { get; set; }

        [Column("TotRegistos")]
        public int totalRegistos { get; set; }
        [Column("TotRegistosProcessados")]
        public int totalRegistosProcessados { get; set; }
        [Column("TotEventosErro")]
        public int numEventosErro { get; set; }
        [Column("TotEventosAviso")]
        public int numEventosAviso { get; set; }

        [Column("DtUpload")]
        [Display(Name = "Data Upload")]
        public DateTime dataUpload { get; set; }
        [Column("DtAlteracao")]
        [Display(Name = "Data Alteração")]
        public DateTime dataAlteracao { get; set; }
        [Column("NomeUtilizador")]
        [Display(Name = "Utilizador")]
=======
        public string nomeFicheiro { get; set; }
        public string localizacao { get; set; }
        public EstadoFicheiro estado { get; set; }
        public bool erro { get; set; }
        public DateTime dataUpload { get; set; }
        public DateTime dataAlteracao { get; set; }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public string userName { get; set; }

        public virtual ICollection<EventoStagging> eventosStagging { get; set; }
        public virtual ICollection<ErroFicheiro> errosFicheiro { get; set; }

        public Ficheiro()
        {
            this.errosFicheiro = new List<ErroFicheiro>();
<<<<<<< HEAD
            this.totalRegistosProcessados = 0;
            this.numEventosAviso = 0;
            this.numEventosErro = 0;
        }


        public bool validar()
        {

            //Assembly a = Assembly.GetExecutingAssembly();
            //Stream stream = a.GetManifestResourceStream("FNMPAS.xsd");

            //XmlSchema x = XmlSchema.Read(stream, null);
                        
            //XmlSchemaSet schemas = new XmlSchemaSet();
            //schemas.Add(x);

            //XDocument doc = XDocument.Load(this.localizacao);
            //string msg = "";
            //doc.Validate(schemas, (o, e) =>
            //{
            //    msg += e.Message + Environment.NewLine;
            //});


            //throw new ErroFicheiroException();
            //TODO
            return true;
=======
        }

        public bool validar( )
        {
            //throw new ErroFicheiroException();
            //TODO
            return true;
        }
                      

        public void carregaEventos()
        {
            ReporteOcorrenciasMatricula reporte;

            XmlSerializer serializer = new XmlSerializer(typeof(ReporteOcorrenciasMatricula));
            using (FileStream fileStream = new FileStream(this.localizacao, FileMode.Open))
            {
                reporte = (ReporteOcorrenciasMatricula)serializer.Deserialize(fileStream);
            }

            reporte.ordenaOcorrencias();

            for (int i = 0; i < reporte.OcorrenciaOrdenada.Count(); i++ )
            {
                ReporteOcorrenciasMatriculaOcorrenciaOrdenada ocorrencia = reporte.OcorrenciaOrdenada[i];

                EventoStagging evento = new EventoStagging();
                //evento.eventoStaggingId = ;
                evento.ficheiroID = this.ficheiroId;

                evento.idOcorrencia = ocorrencia.IDOcorrencia;
                switch (ocorrencia.CodigoOperacao)
                {
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.C: evento.CodigoOperacao = "C"; break;
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.M: evento.CodigoOperacao = "M"; break;
                    case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.A: evento.CodigoOperacao = "A"; break;
                }

                evento.nrApolice = ocorrencia.NumeroApolice;
                evento.nrCertificadoProvisorio = ocorrencia.NumeroCertificadoProvisorio;


                evento.dataInicioCobertura = ocorrencia.DataInicio;
                evento.dataFimCobertura = ocorrencia.DataFim;

                evento.matricula = ocorrencia.NumeroMatricula;
                evento.marca = ocorrencia.MarcaVeiculo;
                evento.modelo = ocorrencia.ModeloVeiculo;
                evento.categoriaVeiculo = ocorrencia.CodigoCategoriaVeiculo;
                evento.anoConstrucao = ocorrencia.AnoConstrucao;

                evento.nomeTomadorSeguro = ocorrencia.NomeTomadorSeguro;
                evento.nrIdentificacaoTomadorSeguro = ocorrencia.NumeroIdentificacaoTomadorSeguro;
                evento.moradaTomadorSeguro = ocorrencia.MoradaTomadorSeguro;
                evento.codigoPostalTomador = ocorrencia.CodigoPostalTomadorSeguro;
                evento.concelhoCirculacao = ocorrencia.CodigoConcelhoCirculacao;

                evento.estadoEvento = EventoStagging.estadoEventoStagging.submetido;
                evento.entidadId = this.entidadeId;

                this.eventosStagging.Add(evento);
            }
                      
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }

        public bool ficheiroProcessado()
        {
            try
            {
<<<<<<< HEAD
                if (eventosStagging.First(e => e.estadoEvento.valor == "PENDENTE" ||
                    e.estadoEvento.valor == "EM_PROCESSAMENTO") == null)
=======
                if (eventosStagging.First(e => e.estadoEvento != EventoStagging.estadoEventoStagging.processado) == null)
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                    return true;
                else
                    return false;
            }
            catch (InvalidOperationException ex)
            {
                return true;
            }
        }
    }
}