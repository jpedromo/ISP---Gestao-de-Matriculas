using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{
    public class ApoliceIsento
    {
        [Key]
        public int apoliceIsentoId { get; set; }

        [Column("CodEntidadeId_FK")]
        [ForeignKey("entidade")]
        public int entidadeId { get; set; }
        public virtual Entidade entidade { get; set; }
        //username

        [Column("XMatricula")]
        [Display(Name="Matricula")]
        public string matricula { get; set; }

        [Column("XMatriculaCorrigida")]
        [Display(Name = "Matricula")]
        public string matriculaCorrigida { get; set; }

        [Column("NomEntidadeResponsavel")]
        [Display(Name = "Entidade")]
        public string entidadeResponsavel { get; set; }

        [Column("XMensagem")]
        [Display(Name = "Mensagem")]
        public string mensagem { get; set; }

        [Column("DtInicio")]
        [Display(Name = "Data de Início")]
        public DateTime dataInicio { get; set; }

        [Column("DtFim")]
        [UIHint("NullableDateTime")]
        [Display(Name = "Data de Fim")]
        public DateTime? dataFim {get; set;}

        [Column("DtReporte")]
        [Display(Name = "Data de Reporte")]
        public DateTime dataReporte { get; set; }

        [Column("DtCriacao")]
        [Display(Name = "Data de Criacao")]
        public DateTime dataCriacao { get; set; }

        [Column("DtModificacao")]
        [Display(Name = "Data de Modificação")]
        public DateTime dataModificacao { get; set; }

        [Column("FlgConfidencial")]
        [Display(Name = "Confidencial")]
        public bool confidencial { get; set; }

        [Column("FlgOrigemFicheiro")]
        [Display(Name = "Origem Ficheiro")]
        public bool origemFicheiro { get; set; }

        [Column("FlgArquivo")]
        [Display(Name = "Arquivo")]
        public bool arquivo { get; set; }

        [Column("CodFicheiroIsentosId_FK")]
        [ForeignKey("ficheiroIsentos")]
        public int? ficheiroIsentosId { get; set; }
        public virtual FicheiroIsentos ficheiroIsentos { get; set; }

        public ApoliceIsento()
        {
            this.arquivo = false;
            this.confidencial = false;
            this.origemFicheiro = false;
            this.dataReporte = DateTime.Now;
        }



    }
}
