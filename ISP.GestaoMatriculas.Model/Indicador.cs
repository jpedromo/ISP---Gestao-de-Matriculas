using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP.GestaoMatriculas.Model;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    public class Indicador
    {
        [Key]
        [Column("CodIndicadorId_PK")]
        public int indicadorId { get; set; }

        [ForeignKey("entidade")]
        [Column("CodEntidadeId_FK")]
        public int? entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }

        [ForeignKey("tipologia")]
        [Column("CodTipologiaId_FK")]
        public int tipologiaId { get; set; }
        public virtual ValorSistema tipologia { get; set; }

        //[NotMapped]
        //public string tipologiaDescricao {
        //    get { return GetIndicadorDescricao(this.tipologia);}
        //}

        //campo auxiliar para criação de agragações
        [Column("CodSubTipo")]
        public string subTipo { get; set; }

        [Column("FlgPublico")]
        public bool publico { get; set; }
        [Column("XDescricao")]
        public string descricao { get; set; }

        //Data correspondente ao indicador. Para indicadores mensais é assumida apenas o 1 dia do mês
        [Column("DtIndicador")]
        public DateTime dataIndicador { get; set; }
        [Column("ValValor")]
        public double valor { get; set; }

        
        //public static string GetIndicadorDescricao(TipoIndicador tipo)
        //{
        //    switch (tipo)
        //    {
        //        case TipoIndicador.Generico:
        //            return "Indicador genérico";
        //        case TipoIndicador.NrEventos:
        //            return "Número de pedidos de reporte total";
        //        case TipoIndicador.NrEventosProcessados:
        //            return "Número de pedidos de reporte total processados";
        //        case TipoIndicador.NrErrosEventos:
        //            return "Número total de erros de reporte";
        //        case TipoIndicador.NrErrosEventosTipo:
        //            return "Número total de erros de reporte por tipologia de erro";
        //        case TipoIndicador.NrAvisosApolice:
        //            return "Número total de avisos de reporte";
        //        case TipoIndicador.NrAvisosApoliceTipo:
        //            return "Número total de avisos de report por tipologia de aviso";
        //        case TipoIndicador.NrEventosOperacao:
        //            return "Número total de eventos processados por operação";
        //        case TipoIndicador.NrOperacoesForaSLA:
        //            return "Número de eventos processados fora do SLA";
        //        case TipoIndicador.NrOperacoesDentroSLA:
        //            return "Número de eventos processados respeitando o SLA";
        //        case TipoIndicador.TempoMedioDesviosSLA:
        //            return "Tempo médio dos desviod dos eventos processados fora SLA";
        //    }
        //    return "NA";
        //}
    }
}