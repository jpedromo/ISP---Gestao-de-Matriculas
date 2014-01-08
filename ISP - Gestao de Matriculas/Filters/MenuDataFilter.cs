using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Models;
using ISP.GestaoMatriculas.Model;
using WebMatrix.WebData;


namespace ISP.GestaoMatriculas.Filters
{
    public class MenuDataFilter : ActionFilterAttribute, IActionFilter
    {

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            DomainModels db = new DomainModels();
            int messagesToPreview = 5;

            UserProfile user = db.UserProfiles.Find(WebSecurity.CurrentUserId);

            if (user != null)
            {

                filterContext.Controller.ViewData["NumNotificacoes"] = user.entidade.notificacoes.Count;

                List<Notificacao> unreadNotificacaoList = user.entidade.notificacoes.Where<Notificacao>(n => n.lida == false)
                    .OrderBy(n => n.dataCriacao).Reverse<Notificacao>().ToList();
                filterContext.Controller.ViewData["NumNovasNotificacoes"] = unreadNotificacaoList.Count;
                   
                messagesToPreview = (unreadNotificacaoList.Count > messagesToPreview ? messagesToPreview : unreadNotificacaoList.Count);
                filterContext.Controller.ViewData["MessagesToPreview"] = messagesToPreview;

                List<Notificacao> notificacaoPreviewList = unreadNotificacaoList.GetRange(0, messagesToPreview);

                for(int i = 0; i < messagesToPreview; i++){
                    filterContext.Controller.ViewData["Notificacao" + (i) + ".ID"] = notificacaoPreviewList[i].notificacaoId.ToString();
                    filterContext.Controller.ViewData["Notificacao" + (i) + ".ImageUrl"] = notificacaoPreviewList[i].imageUrl();
                    filterContext.Controller.ViewData["Notificacao" + (i) + ".Tipo"] = notificacaoPreviewList[i].tipologia.descricao;
                    filterContext.Controller.ViewData["Notificacao" + (i) + ".Mensagem"] = notificacaoPreviewList[i].mensagemToString();
                    filterContext.Controller.ViewData["Notificacao" + (i) + ".Data"] = notificacaoPreviewList[i].dataCriacao.ToString();
                }

                int numIndicadores = user.entidade.indicadores.Count;
                filterContext.Controller.ViewData["NumIndicadores"] = numIndicadores;
                List<Indicador> listaIndicadores = user.entidade.indicadores.ToList();
                for (int i = 0; i < numIndicadores; i++)
                {
                    filterContext.Controller.ViewData["Indicador" + (i) + ".Descricao"] = listaIndicadores[i].descricao;
                    filterContext.Controller.ViewData["Indicador" + (i) + ".Valor"] = listaIndicadores[i].valor;
                }
            }
            else
            {
                filterContext.Controller.ViewData["NumNotificacoes"] = null;
                filterContext.Controller.ViewData["NumIndicadores"] = null;
            }

            this.OnActionExecuting(filterContext);
        }

    }
}