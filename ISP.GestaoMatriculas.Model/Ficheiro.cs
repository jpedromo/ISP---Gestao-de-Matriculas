using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using System.Xml.Serialization;
using ISP.GestaoMatriculas.Utils;
using System.IO;

namespace ISP.GestaoMatriculas.Model
{
    [Serializable]
    public class Ficheiro
    {
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
        public int ficheiroId { get; set; }

        //Foreign Key para seguradora
        [ForeignKey("entidade")]
        public int entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }
        
        //Atributos do ficheiro
        public string nomeFicheiro { get; set; }
        public string localizacao { get; set; }
        public EstadoFicheiro estado { get; set; }
        public bool erro { get; set; }
        public DateTime dataUpload { get; set; }
        public DateTime dataAlteracao { get; set; }
        public string userName { get; set; }

        public virtual ICollection<EventoStagging> eventosStagging { get; set; }
        public virtual ICollection<ErroFicheiro> errosFicheiro { get; set; }

        public Ficheiro()
        {
            this.errosFicheiro = new List<ErroFicheiro>();
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
                      
        }

        public bool ficheiroProcessado()
        {
            try
            {
                if (eventosStagging.First(e => e.estadoEvento != EventoStagging.estadoEventoStagging.processado) == null)
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