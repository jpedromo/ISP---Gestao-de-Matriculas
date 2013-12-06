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
    [MenuDataFilter(Order = 2)]
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
