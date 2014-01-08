using System.Web;
using System.Web.Mvc;

namespace ISP.GestaoMatriculas
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}