using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    public class EventoStagging
    {
        public enum estadoEventoStagging
        {
            submetido,
            processamento,
            processado
        }

        public int eventoStaggingId { get; set; }
        public string idOcorrencia { get; set; }
        public string CodigoOperacao { get; set; }

        [ForeignKey("entidade")]
        public int? entidadId { get; set; }
        public virtual Entidade entidade { get; set; }

        public string nrApolice { get; set; }
        public string nrCertificadoProvisorio { get; set; }
        public string dataInicioCobertura { get; set; }
        public string dataFimCobertura { get; set; }
          
        public string matricula { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string anoConstrucao { get; set; }
        public string categoriaVeiculo { get; set; }
        public string concelhoCirculacao { get; set; }

        public string nomeTomadorSeguro { get; set; }
        public string moradaTomadorSeguro { get; set; }
        public string codigoPostalTomador { get; set; }
        public string nrIdentificacaoTomadorSeguro { get; set; }

        public estadoEventoStagging estadoEvento { get; set; }
        
        [ForeignKey("ficheiro")]
        public int? ficheiroID { get; set; }
        public virtual Ficheiro ficheiro { get; set; }

        public List<ErroEventoStagging> errosEventoStagging { get; set; }

        public EventoStagging()
        {
            errosEventoStagging = new List<ErroEventoStagging>();
        }

        public Apolice validar()
        {
            throw new ErroEventoStaggingException();
            //TODO
            return new Apolice();
        }
    }
}
