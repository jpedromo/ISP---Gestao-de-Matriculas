using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.GestaoMatriculas.Model
{

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