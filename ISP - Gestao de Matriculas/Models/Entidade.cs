using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ISP.GestaoMatriculas.Models.Indicadores;

namespace ISP.GestaoMatriculas.Models
{

    public class Entidade
    {
        public Entidade(){
            apolices = new List<Apolice>();
            notificacoes = new List<Notificacao>();
            indicadores = new List<Indicador>();
        }

        public enum ScopeLevel:int  { Local=0, Global=1 }
        //key
        public int entidadeId { get; set; }

        //Role de autorização para a entidade
        public int roleId { get; set; }
        public virtual Role role { get; set; }

        //Atributos da Entidade
        public string nome { get; set; }

        [Display (Name="Nome do Responsável")]
        public string nomeResponsavel { get; set; }

        [Display (Name="Email do Responsável")]
        public string emailResponsavel { get; set; }

        [Display(Name = "Telefone do Responsável")]
        public string telefoneResponsavel { get; set; }

        public int scopeId { get; set; }
        public ScopeLevel scope
        {
            get { return (ScopeLevel)this.scopeId; }
            set { this.scopeId = (int)value; }
        }

        [Display(Name  = "Ativo")]
        public bool ativo { get; set; }

        public virtual ICollection<Apolice> apolices { get; set; }

        public virtual ICollection<Notificacao> notificacoes { get; set; }

        public virtual ICollection<Indicador> indicadores { get; set; }

        //Notificaçoes
        //Erros - Avisos
        //Indicadores
        //Relatorios - Disponibilizados pelo ISP ou por consulta?

    }
}