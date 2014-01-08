using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Filters;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Contracts;

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership (Order = 1)]
<<<<<<< HEAD
    //[MenuDataFilter(Order = 2)]
=======
    [MenuDataFilter(Order = 2)]
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    public class ActionLogController : Controller
    {
        //
        // GET: /Default1/

        //DomainModels db = new DomainModels();

        //public ActionResult Index()
        //{
        //    var logs = this.db.ActionLogs.OrderByDescending(al => al.DateTime).ToList();
        //    return View(logs);
        //}

    }
}
