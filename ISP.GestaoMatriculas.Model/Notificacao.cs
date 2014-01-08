using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Model
{
    public class Notificacao
    {
        public Notificacao()
        {
            this.lida = false;
<<<<<<< HEAD
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
=======
        }

        public enum TipoNotificacao { SucessoFicheiro, ErroFicheiro, WarningFicheiro }

        public int notificacaoId { get; set; }

        public DateTime dataCriacao { get; set; }

        public bool lida { get; set; }

        public String mensagem { get; set; }

        public virtual Entidade entidade { get; set; }

        public int? entidadeId { get; set; }

        public int tipoId { get; set; }
        public TipoNotificacao tipo
        {
            get { return (TipoNotificacao)this.tipoId; }
            set { this.tipoId = (int)value; }
        }

        public string tipoToString()
        {
            switch(tipo){
                case TipoNotificacao.SucessoFicheiro: { return "Ficheiro Recebido"; }
                case TipoNotificacao.ErroFicheiro:{ return "Ficheiro com Erros"; }
                case TipoNotificacao.WarningFicheiro: { return "Ficheiro com Avisos"; }
                default: return "Notificacao";
            }
        }

        public string mensagemToString()
        {
            switch (tipo)
            {
                case TipoNotificacao.SucessoFicheiro: { return "Sucesso na recepção de Ficheiro. " + mensagem; }
                case TipoNotificacao.ErroFicheiro: { return "Detetados Erros no Ficheiro. " + mensagem; }
                case TipoNotificacao.WarningFicheiro: { return "Detetados Avisos no Ficheiro. " + mensagem; }
                default: return "Notificacao";
            }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }

        public string imageUrl()
        {
<<<<<<< HEAD
            switch (tipologia.valor)
            {
                case "NOTIFICACAO_ISP": { return "~/Images/Notificacoes/ISP_Logo.png"; }
                case "SUCESSO_RECECAO_FICHEIRO": { return "~/Images/Notificacoes/Success.png"; }
                case "INSUCESSO_RECECAO_FICHEIRO": { return "~/Images/Notificacoes/Error.png"; }
                case "SUCESSO_PROCESSAMENTO_FICHEIRO": { return "~/Images/Notificacoes/Success.png"; }
                case "ERRO_PROCESSAMENTO_FICHEIRO": { return "~/Images/Notificacoes/Error.png"; }
                case "AVISO_PROCESSAMENTO_FICHEIRO": { return "~/Images/Notificacoes/Warning.png"; }
=======
            switch (tipo)
            {
                case TipoNotificacao.SucessoFicheiro: { return "~/Images/Notificacoes/Success.png"; }
                case TipoNotificacao.ErroFicheiro: { return "~/Images/Notificacoes/Error.png"; }
                case TipoNotificacao.WarningFicheiro: { return "~/Images/Notificacoes/Warning.png"; }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                default: return "http://placehold.it/50x50";
            }
        }

    }
}