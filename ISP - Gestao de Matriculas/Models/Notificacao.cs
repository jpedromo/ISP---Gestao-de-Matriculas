using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP.GestaoMatriculas.Models
{
    public class Notificacao
    {
        public Notificacao()
        {
            this.lida = false;
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
        }

        public string imageUrl()
        {
            switch (tipo)
            {
                case TipoNotificacao.SucessoFicheiro: { return "~/Images/Notificacoes/Success.png"; }
                case TipoNotificacao.ErroFicheiro: { return "~/Images/Notificacoes/Error.png"; }
                case TipoNotificacao.WarningFicheiro: { return "~/Images/Notificacoes/Warning.png"; }
                default: return "http://placehold.it/50x50";
            }
        }

    }
}