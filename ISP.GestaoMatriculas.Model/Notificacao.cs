using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    public class Notificacao
    {
        public Notificacao()
        {
            this.lida = false;
            this.email = false;
            this.enviadaEmail = false;
        }
                
        [Key]
        [Column("CodNotificacaoId_PK")]
        public int notificacaoId { get; set; }

        [Column("DtCriacao")]
        public DateTime dataCriacao { get; set; }

        [Column("FlgLida")]
        public bool lida { get; set; }

        [Column("FlgEmail")]
        public bool email { get; set; }

        [Column("FlgEnviadoEmail")]
        public bool enviadaEmail { get; set; }
        [Column("dtEnvioEmail")]
        public DateTime? dataEnvioEmail { get; set; }

        [Column("XMensagem")]
        public String mensagem { get; set; }

        [ForeignKey("entidade")]
        [Column("CodEntidadeId_FK")]
        public int? entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }

        [ForeignKey("tipologia")]
        [Column("CodTipologiaId_FK")]
        public int tipologiaId { get; set; }
        public virtual ValorSistema tipologia { get; set; }

        //[NotMapped]
        //public TipoNotificacao tipo
        //{
        //    get { return (TipoNotificacao)this.tipologiaId; }
        //    set { this.tipologiaId = (int)value; }
        //}

        //public string tipoToString()
        //{
        //    switch(tipo){
        //        case TipoNotificacao.NotificacaoISP: { return "ISP"; }
        //        case TipoNotificacao.SucessoReceberFicheiro: { return "Ficheiro Recebido com Sucesso"; }
        //        case TipoNotificacao.InsucessoReceberFicheiro: { return "Erro ao Receber Ficheiro"; }
        //        case TipoNotificacao.SucessoProcessamentoFicheiro: { return "Processamento com Sucesso"; }
        //        case TipoNotificacao.ErroProcessamentoFicheiro: { return "Processamento com Erros"; }
        //        case TipoNotificacao.AvisoProcessamentoFicheiro: { return "Processamento com Avisos"; }
        //        default: return "Notificacao";
        //    }
        //}

        public string mensagemToString()
        {
            return mensagem;
        }

        public string imageUrl()
        {
            switch (tipologia.valor)
            {
                case "NOTIFICACAO_ISP": { return "~/Images/Notificacoes/ISP_Logo.png"; }
                case "SUCESSO_RECECAO_FICHEIRO": { return "~/Images/Notificacoes/Success.png"; }
                case "INSUCESSO_RECECAO_FICHEIRO": { return "~/Images/Notificacoes/Error.png"; }
                case "SUCESSO_PROCESSAMENTO_FICHEIRO": { return "~/Images/Notificacoes/Success.png"; }
                case "ERRO_PROCESSAMENTO_FICHEIRO": { return "~/Images/Notificacoes/Error.png"; }
                case "AVISO_PROCESSAMENTO_FICHEIRO": { return "~/Images/Notificacoes/Warning.png"; }
                default: return "http://placehold.it/50x50";
            }
        }

    }
}