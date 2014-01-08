using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ISP.GestaoMatriculas.Model
{

    public class ReporteOcorrenciasFNMPAS : ReporteOcorrenciasMatricula
    {
        private ReporteOcorrenciasMatriculaOcorrenciaOrdenada[] ocorrenciaOrdenadaField;
                
        public ReporteOcorrenciasMatriculaOcorrenciaOrdenada[] OcorrenciaOrdenada
        {
            get
            {
                return this.ocorrenciaOrdenadaField;
            }
            set
            {
                this.ocorrenciaOrdenadaField = value;
            }
        }

        public ReporteOcorrenciasFNMPAS(ReporteOcorrenciasMatricula reporteOcorrenciasMatricula)
        {
            this.Ocorrencia = (ReporteOcorrenciasMatriculaOcorrencia[]) reporteOcorrenciasMatricula.Ocorrencia.Clone();
            this.Cabecalho = new ReporteOcorrenciasMatriculaCabecalho { CodigoEstatistico = reporteOcorrenciasMatricula.Cabecalho.CodigoEstatistico,
                                                                        IDReporte = reporteOcorrenciasMatricula.Cabecalho.IDReporte};
            this.DataVersao = reporteOcorrenciasMatricula.DataVersao;
            this.Versao = reporteOcorrenciasMatricula.Versao;
        }

        public void ordenaOcorrencias()
        {
            this.OcorrenciaOrdenada = new ReporteOcorrenciasMatriculaOcorrenciaOrdenada[this.Ocorrencia.Count()];
            for (int i = 0; i < this.Ocorrencia.Count(); i++)
            {
                this.OcorrenciaOrdenada[i] = new ReporteOcorrenciasMatriculaOcorrenciaOrdenada(this.Ocorrencia[i]);
                this.OcorrenciaOrdenada[i].OrdemOcorrencia = i;               
                
            }
            this.OcorrenciaOrdenada = this.OcorrenciaOrdenada.OrderBy(o => o.CodigoOperacao, new ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacaoComp()).ThenBy(o => o.OrdemOcorrencia).ToArray();
        }

    }       
        
    public class ReporteOcorrenciasMatriculaOcorrenciaOrdenada : ReporteOcorrenciasMatriculaOcorrencia
    {
        private int ordemOcorrencia;
                
        /// <remarks/>
        public int OrdemOcorrencia
        {
            get
            {
                return this.ordemOcorrencia;
            }
            set
            {
                this.ordemOcorrencia = value;
            }
        }

        public ReporteOcorrenciasMatriculaOcorrenciaOrdenada()
        { }

        public ReporteOcorrenciasMatriculaOcorrenciaOrdenada(ReporteOcorrenciasMatriculaOcorrencia reporteOcorrenciasMatriculaOcorrencia) 
        {
            this.AnoConstrucao = reporteOcorrenciasMatriculaOcorrencia.AnoConstrucao;
            this.CodigoCategoriaVeiculo = reporteOcorrenciasMatriculaOcorrencia.CodigoCategoriaVeiculo;
            this.CodigoConcelhoCirculacao = reporteOcorrenciasMatriculaOcorrencia.CodigoConcelhoCirculacao;
            this.CodigoOperacao = reporteOcorrenciasMatriculaOcorrencia.CodigoOperacao;
            this.CodigoPostalTomadorSeguro = reporteOcorrenciasMatriculaOcorrencia.CodigoPostalTomadorSeguro;
            this.DataFim = reporteOcorrenciasMatriculaOcorrencia.DataFim;
            this.DataInicio = reporteOcorrenciasMatriculaOcorrencia.DataInicio;
            this.HoraFim = reporteOcorrenciasMatriculaOcorrencia.HoraFim;
            this.HoraInicio = reporteOcorrenciasMatriculaOcorrencia.HoraInicio;
            this.IDOcorrencia = reporteOcorrenciasMatriculaOcorrencia.IDOcorrencia;
            this.MarcaVeiculo = reporteOcorrenciasMatriculaOcorrencia.MarcaVeiculo;
            this.ModeloVeiculo = reporteOcorrenciasMatriculaOcorrencia.ModeloVeiculo;
            this.MoradaTomadorSeguro = reporteOcorrenciasMatriculaOcorrencia.MoradaTomadorSeguro;
            this.NIFTomadorSeguro = reporteOcorrenciasMatriculaOcorrencia.NIFTomadorSeguro;
            this.NomeTomadorSeguro = reporteOcorrenciasMatriculaOcorrencia.NomeTomadorSeguro;
            this.NumeroApolice = reporteOcorrenciasMatriculaOcorrencia.NumeroApolice;
            this.NumeroCertificadoProvisorio = reporteOcorrenciasMatriculaOcorrencia.NumeroCertificadoProvisorio;
            this.NumeroIdentificacaoTomadorSeguro = reporteOcorrenciasMatriculaOcorrencia.NumeroIdentificacaoTomadorSeguro;
            this.NumeroMatricula = reporteOcorrenciasMatriculaOcorrencia.NumeroMatricula;
        }
    }

    public class ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacaoComp : IComparer<ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao>
    {
        // Compares by Height, Length, and Width. 
        public int Compare(ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao x, ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao y)
        {
            int xValue = 0, yValue = 0;

            switch (x)
            {
                case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.C: xValue = 1; break;
                case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.M: xValue = 2; break;
                case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.A: xValue = 3; break; 
            }

            switch (y)
            {
                case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.C: yValue = 1; break;
                case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.M: yValue = 2; break;
                case ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao.A: yValue = 3; break;
            }

            return xValue - yValue;
        }
    }
    
}