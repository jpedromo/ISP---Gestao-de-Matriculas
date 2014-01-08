using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{

=======

namespace ISP.GestaoMatriculas.Model
{
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    public class ActionLog
    {
        public int ActionLogId { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Message { get; set; }

        public string IP { get; set; }

        public DateTime? DateTime { get; set; }
    }
}