<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
=======
﻿using Everis.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Model
{
    [Serializable]
<<<<<<< HEAD
    public class Concelho
    {
        //key - interna
        [Key]
        [Column("ConcelhoId_PK")]
        public int concelhoId { get; set; }
        [Column("CodConcelho")]
        public string codigoConcelho { get; set; }               //checked
        [Column("NomeConcelho")]
        public string nomeConcelho { get; set; }

=======
    public class Concelho : Entity<int>
    {
        public string Codigo { get; set; }               //checked
        public string Nome { get; set; }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    }
}