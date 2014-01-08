using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{

    public class Entidade
    {
        public Entidade(){
            apolices = new List<Apolice>();
            apolicesHistorico = new List<ApoliceHistorico>();
            notificacoes = new List<Notificacao>();
            indicadores = new List<Indicador>();
        }

        //key - interna
        [Key]
        [Column("CodEntidadeId_PK")]
        public int entidadeId { get; set; }

        //Role de autorização para a entidade (internos)
        [ForeignKey("role")]
        [Column("CodRoleId_FK")]
        public int roleId { get; set; }
        [Display(Name="Perfil")]
        public virtual Role role { get; set; }

        //Atributos da Entidade
        [Column("NomeEntidade")]
        [Display(Name = "Nome")]
        public string nome { get; set; }

        [Column("CodEntidade")]
        public string codigoEntidade { get; set; }                                  

        [Column("NomeResponsavel")]
        [Display (Name="Nome do Responsável")]
        public string nomeResponsavel { get; set; }

        [Column("XEmailResponsavel")]
        [Display (Name="Email do Responsável")]
        public string emailResponsavel { get; set; }

        [Column("NrTelefoneResponsavel")]
        [Display(Name = "Telefone do Responsável")]
        public string telefoneResponsavel { get; set; }

        [ForeignKey("scope")]
        [Column("CodScopeId_FK")]
        public int scopeId { get; set; }
        [Display(Name = "Scope")]
        public virtual ValorSistema scope { get; set; }
        [Column("FlgAtivo")]
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