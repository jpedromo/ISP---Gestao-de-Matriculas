using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everis.Web.Mvc
{
    public class Alerts
    {

    }

    public static class AlertType
    {
        public const string SUCCESS = "SUCCESS";
        public const string INFO = "INFO";
        public const string WARNING = "WARNING";
        public const string DANGER = "DANGER";

        public static string[] ALL
        {
            get { return new[] { SUCCESS, INFO, WARNING, DANGER }; }
        }
    }
}
