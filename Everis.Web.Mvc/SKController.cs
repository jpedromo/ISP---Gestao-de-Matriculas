using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Everis.Web.Mvc
{
    public abstract class SKController : Controller
    {
        _Alerts alerts = null;
        public _Alerts Alerts
        {
            get
            {
                if (alerts == null) alerts = new _Alerts(this.TempData);
                return alerts;
            } 
        }

        public class _Alerts
        {
            TempDataDictionary tempData = null;
            public _Alerts(TempDataDictionary tempData)
            {
                this.tempData = tempData;
            }

            public void Danger(string message)
            {
                tempData.Add(AlertType.DANGER, message);
            }

            public void Info(string message)
            {
                tempData.Add(AlertType.INFO, message);
            }

            public void Success(string message)
            {
                tempData.Add(AlertType.SUCCESS, message);
            }

            public void Warning(string message)
            {
                tempData.Add(AlertType.WARNING, message);
            }
        }
    }
}
