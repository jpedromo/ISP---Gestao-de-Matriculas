using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ISP.GestaoMatriculas.Utils
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ReporteOcorrenciasMatricula
    {

        private ReporteOcorrenciasMatriculaCabecalho cabecalhoField;

        private ReporteOcorrenciasMatriculaOcorrencia[] ocorrenciaField;

        private ReporteOcorrenciasMatriculaOcorrenciaOrdenada[] ocorrenciaOrdenadaField;

        private string versaoField;

        private string dataVersaoField;

        public ReporteOcorrenciasMatricula()
        {
            this.versaoField = "1";
            this.dataVersaoField = "2014-01-01";
        }

        /// <remarks/>
        public ReporteOcorrenciasMatriculaCabecalho Cabecalho
        {
            get
            {
                return this.cabecalhoField;
            }
            set
            {
                this.cabecalhoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Ocorrencia")]
        public ReporteOcorrenciasMatriculaOcorrencia[] Ocorrencia
        {
            get
            {
                return this.ocorrenciaField;
            }
            set
            {
                this.ocorrenciaField = value;
            }
        }

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string Versao
        {
            get
            {
                return this.versaoField;
            }
            set
            {
                this.versaoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string DataVersao
        {
            get
            {
                return this.dataVersaoField;
            }
            set
            {
                this.dataVersaoField = value;
            }
        }


        public void ordenaOcorrencias()
        {
            this.OcorrenciaOrdenada = new ReporteOcorrenciasMatriculaOcorrenciaOrdenada[this.Ocorrencia.Count()];
            for (int i = 0; i < this.Ocorrencia.Count(); i++)
            {
                this.OcorrenciaOrdenada[i] = (ReporteOcorrenciasMatriculaOcorrenciaOrdenada) this.Ocorrencia[i];
                this.OcorrenciaOrdenada[i].OrdemOcorrencia = i;
            }
            this.OcorrenciaOrdenada = this.OcorrenciaOrdenada.OrderBy(o => o.CodigoOperacao).ThenBy(o => o.OrdemOcorrencia).ToArray();
        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ReporteOcorrenciasMatriculaCabecalho
    {

        private string iDReporteField;

        private string codigoEstatisticoField;

        /// <remarks/>
        public string IDReporte
        {
            get
            {
                return this.iDReporteField;
            }
            set
            {
                this.iDReporteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string CodigoEstatistico
        {
            get
            {
                return this.codigoEstatisticoField;
            }
            set
            {
                this.codigoEstatisticoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ReporteOcorrenciasMatriculaOcorrencia
    {

        private string iDOcorrenciaField;

        private ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao codigoOperacaoField;

        private string numeroApoliceField;

        private string numeroCertificadoProvisorioField;

        private string numeroMatriculaField;

        private string dataInicioField;

        private string horaInicioField;

        private string dataFimField;

        private string horaFimField;

        private string anoConstrucaoField;

        private string codigoCategoriaVeiculoField;

        private string marcaVeiculoField;

        private string modeloVeiculoField;

        private string codigoConcelhoCirculacaoField;

        private string nomeTomadorSeguroField;

        private string numeroIdentificacaoTomadorSeguroField;

        private string nIFTomadorSeguroField;

        private string moradaTomadorSeguroField;

        private string codigoPostalTomadorSeguroField;

        private string nomeProprietarioField;

        private string moradaProprietarioField;

        private string codigoPostalProprietarioField;

        private string nomeCondutorField;

        private string moradaCondutorField;

        private string codigoPostalCondutorField;

        private string nomeRegistoField;

        private string moradaRegistoField;

        private string codigoPostalRegistoField;

        /// <remarks/>
        public string IDOcorrencia
        {
            get
            {
                return this.iDOcorrenciaField;
            }
            set
            {
                this.iDOcorrenciaField = value;
            }
        }

        /// <remarks/>
        public ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao CodigoOperacao
        {
            get
            {
                return this.codigoOperacaoField;
            }
            set
            {
                this.codigoOperacaoField = value;
            }
        }

        /// <remarks/>
        public string NumeroApolice
        {
            get
            {
                return this.numeroApoliceField;
            }
            set
            {
                this.numeroApoliceField = value;
            }
        }

        /// <remarks/>
        public string NumeroCertificadoProvisorio
        {
            get
            {
                return this.numeroCertificadoProvisorioField;
            }
            set
            {
                this.numeroCertificadoProvisorioField = value;
            }
        }

        /// <remarks/>
        public string NumeroMatricula
        {
            get
            {
                return this.numeroMatriculaField;
            }
            set
            {
                this.numeroMatriculaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string DataInicio
        {
            get
            {
                return this.dataInicioField;
            }
            set
            {
                this.dataInicioField = value;
            }
        }

        /// <remarks/>
        public string HoraInicio
        {
            get
            {
                return this.horaInicioField;
            }
            set
            {
                this.horaInicioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string DataFim
        {
            get
            {
                return this.dataFimField;
            }
            set
            {
                this.dataFimField = value;
            }
        }

        /// <remarks/>
        public string HoraFim
        {
            get
            {
                return this.horaFimField;
            }
            set
            {
                this.horaFimField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string AnoConstrucao
        {
            get
            {
                return this.anoConstrucaoField;
            }
            set
            {
                this.anoConstrucaoField = value;
            }
        }

        /// <remarks/>
        public string CodigoCategoriaVeiculo
        {
            get
            {
                return this.codigoCategoriaVeiculoField;
            }
            set
            {
                this.codigoCategoriaVeiculoField = value;
            }
        }

        /// <remarks/>
        public string MarcaVeiculo
        {
            get
            {
                return this.marcaVeiculoField;
            }
            set
            {
                this.marcaVeiculoField = value;
            }
        }

        /// <remarks/>
        public string ModeloVeiculo
        {
            get
            {
                return this.modeloVeiculoField;
            }
            set
            {
                this.modeloVeiculoField = value;
            }
        }

        /// <remarks/>
        public string CodigoConcelhoCirculacao
        {
            get
            {
                return this.codigoConcelhoCirculacaoField;
            }
            set
            {
                this.codigoConcelhoCirculacaoField = value;
            }
        }

        /// <remarks/>
        public string NomeTomadorSeguro
        {
            get
            {
                return this.nomeTomadorSeguroField;
            }
            set
            {
                this.nomeTomadorSeguroField = value;
            }
        }

        /// <remarks/>
        public string NumeroIdentificacaoTomadorSeguro
        {
            get
            {
                return this.numeroIdentificacaoTomadorSeguroField;
            }
            set
            {
                this.numeroIdentificacaoTomadorSeguroField = value;
            }
        }

        /// <remarks/>
        public string NIFTomadorSeguro
        {
            get
            {
                return this.nIFTomadorSeguroField;
            }
            set
            {
                this.nIFTomadorSeguroField = value;
            }
        }

        /// <remarks/>
        public string MoradaTomadorSeguro
        {
            get
            {
                return this.moradaTomadorSeguroField;
            }
            set
            {
                this.moradaTomadorSeguroField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string CodigoPostalTomadorSeguro
        {
            get
            {
                return this.codigoPostalTomadorSeguroField;
            }
            set
            {
                this.codigoPostalTomadorSeguroField = value;
            }
        }

        /// <remarks/>
        public string NomeProprietario
        {
            get
            {
                return this.nomeProprietarioField;
            }
            set
            {
                this.nomeProprietarioField = value;
            }
        }

        /// <remarks/>
        public string MoradaProprietario
        {
            get
            {
                return this.moradaProprietarioField;
            }
            set
            {
                this.moradaProprietarioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string CodigoPostalProprietario
        {
            get
            {
                return this.codigoPostalProprietarioField;
            }
            set
            {
                this.codigoPostalProprietarioField = value;
            }
        }

        /// <remarks/>
        public string NomeCondutor
        {
            get
            {
                return this.nomeCondutorField;
            }
            set
            {
                this.nomeCondutorField = value;
            }
        }

        /// <remarks/>
        public string MoradaCondutor
        {
            get
            {
                return this.moradaCondutorField;
            }
            set
            {
                this.moradaCondutorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string CodigoPostalCondutor
        {
            get
            {
                return this.codigoPostalCondutorField;
            }
            set
            {
                this.codigoPostalCondutorField = value;
            }
        }

        /// <remarks/>
        public string NomeRegisto
        {
            get
            {
                return this.nomeRegistoField;
            }
            set
            {
                this.nomeRegistoField = value;
            }
        }

        /// <remarks/>
        public string MoradaRegisto
        {
            get
            {
                return this.moradaRegistoField;
            }
            set
            {
                this.moradaRegistoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string CodigoPostalRegisto
        {
            get
            {
                return this.codigoPostalRegistoField;
            }
            set
            {
                this.codigoPostalRegistoField = value;
            }
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

        public ReporteOcorrenciasMatriculaOcorrenciaOrdenada(ReporteOcorrenciasMatriculaOcorrencia reporteOcorrenciasMatriculaOcorrencia) 
        {
            //this.
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public enum ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao
    {

        /// <remarks/>
        C = 1,

        /// <remarks/>
        M = 10,

        /// <remarks/>
        A = 20,
    }

    //public class CompareCodigosOperacao : IComparer<string>
    //{
    //    // Because the class implements IComparer, it must define a 
    //    // Compare method. The method returns a signed integer that indicates 
    //    // whether s1 > s2 (return is greater than 0), s1 < s2 (return is negative),
    //    // or s1 equals s2 (return value is 0). This Compare method compares strings. 
    //    public int Compare(ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao s1, ReporteOcorrenciasMatriculaOcorrenciaCodigoOperacao s2)
    //    {
    //        if(;
    //    }
    //}
}