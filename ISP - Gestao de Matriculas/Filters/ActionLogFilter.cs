using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Models;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.ViewModels;

namespace ISP.GestaoMatriculas.Filters
{
    public class ActionLogFilter : ActionFilterAttribute, IActionFilter
{
    public string ParameterName { get; set; }

    void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
    {
        // TODO: Add your acction filter's tasks here
        // Log Action Filter Call
        DomainModels db = new DomainModels();

        //var result = (ApoliceCreationViewModel)filterContext.ActionParameters[ParameterName];
        /*var context = new UsersContext();
        var username = filterContext.HttpContext.User.Identity.Name;
        var user = context.UserProfiles.SingleOrDefault(u => u.UserName == username);
        user.RandomString = "Random1";
        */

        ActionLog log = new ActionLog()
        {
            Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
            Action = filterContext.ActionDescriptor.ActionName,
            IP = filterContext.HttpContext.Request.UserHostAddress,
            DateTime = filterContext.HttpContext.Timestamp,
            //Message = "" + username +" injected string " + user.RandomString
        };
        /*
        db.ActionLogs.Add(log);
        db.SaveChanges();
        context.SaveChanges();
        */
        this.OnActionExecuting(filterContext);
    }
}
}