using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Model
{
    [Serializable]
    public class ErroFicheiro
    {        
        //key
<<<<<<< HEAD
        [Key]
        [Column("CodErroFicheiroId_PK")]
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public int erroFicheiroId { get; set; }

        //Foreign Key para seguradora
        [ForeignKey("ficheiro")]
<<<<<<< HEAD
        [Column("CodFicheiroId_FK")]
        public int? ficheiroId { get; set; }
        public virtual Ficheiro ficheiro { get; set; }
        [ForeignKey("tipologia")]
        [Column("CodTipologiaId_FK")]
        public int tipologiaId { get; set; }
        public virtual ValorSistema tipologia { get; set; }
        //Atributos do ficheiro
        [Column("NomDescricao")]
        public string descricao { get; set; }
        [Column("DtValidacao")]
=======
        public int ficheiroId { get; set; }
        public virtual Ficheiro ficheiro { get; set; }
        
        //Atributos do ficheiro
        public string descricao { get; set; }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public DateTime dataValidacao { get; set; }
    }
}