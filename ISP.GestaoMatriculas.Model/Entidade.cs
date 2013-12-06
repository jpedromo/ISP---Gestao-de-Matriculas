using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Everis.Core.Data;

namespace ISP.GestaoMatriculas.Model
{

    public class Entidade : Entity<int>
    {
        public Entidade(){
            apolices = new List<Apolice>();
            apolicesHistorico = new List<ApoliceHistorico>();
            notificacoes = new List<Notificacao>();
            indicadores = new List<Indicador>();
        }

        public enum ScopeLevel:int  { Local=0, Global=1 }


        //Role de autorização para a entidade (internos)
        public int roleId { get; set; }
        public virtual Role role { get; set; }

        //Atributos da Entidade
        public string Nome { get; set; }

        public string codigoSeguradora { get; set; }                                  //checked

        [Display (Name="Nome do Responsável")]
        public string nomeResponsavel { get; set; }

        [Display (Name="Email do Responsável")]
        public string emailResponsavel { get; set; }

        [Display(Name = "Telefone do Responsável")]
        public string telefoneResponsavel { get; set; }

        public ScopeLevel scope { get; set; }

        [Display(Name  = "Ativo")]
        public bool ativo { get; set; }

        public virtual ICollection<Apolice> apolices { get; set; }

        public virtual ICollection<ApoliceHistorico> apolicesHistorico { get; set; }

        public virtual ICollection<Notificacao> notificacoes { get; set; }

        public virtual ICollection<Indicador> indicadores { get; set; }

        public virtual ICollection<EventoStagging> eventosStagging { get; set; }

        //Notificaçoes
        //Erros - Avisos
        //Indicadores
        //Relatorios - Disponibilizados pelo ISP ou por consulta?

    }
}