using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Model;
using WebMatrix.WebData;

namespace ISP.GestaoMatriculas.Controllers
{
    public class MenuController : Controller
    {
        private IUserProfileRepository usersRepository;

        public MenuController(IUserProfileRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }
        
        //
        // GET: /Menu/

        public ActionResult Index()
        {

            int messagesToPreview = 5;

            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);

            if (user != null)
            {

                ViewData["NumNotificacoes"] = user.entidade.notificacoes.Count;

                List<Notificacao> unreadNotificacaoList = user.entidade.notificacoes.Where<Notificacao>(n => n.lida == false)
                    .OrderBy(n => n.dataCriacao).Reverse<Notificacao>().ToList();
                ViewData["NumNovasNotificacoes"] = unreadNotificacaoList.Count;

                messagesToPreview = (unreadNotificacaoList.Count > messagesToPreview ? messagesToPreview : unreadNotificacaoList.Count);
                ViewData["MessagesToPreview"] = messagesToPreview;

                List<Notificacao> notificacaoPreviewList = unreadNotificacaoList.GetRange(0, messagesToPreview);

                for (int i = 0; i < messagesToPreview; i++)
                {
                    ViewData["Notificacao" + (i) + ".ID"] = notificacaoPreviewList[i].notificacaoId.ToString();
                    ViewData["Notificacao" + (i) + ".ImageUrl"] = notificacaoPreviewList[i].imageUrl();
                    ViewData["Notificacao" + (i) + ".Tipo"] = notificacaoPreviewList[i].tipologia.descricao;
                    ViewData["Notificacao" + (i) + ".Mensagem"] = notificacaoPreviewList[i].mensagemToString();
                    ViewData["Notificacao" + (i) + ".Data"] = notificacaoPreviewList[i].dataCriacao.ToString();
                }

                //int numIndicadores = user.entidade.indicadores.Count;
                //ViewData["NumIndicadores"] = numIndicadores;
                //List<Indicador> listaIndicadores = user.entidade.indicadores.ToList();
                //for (int i = 0; i < numIndicadores; i++)
                //{
                //    ViewData["Indicador" + (i) + ".Descricao"] = listaIndicadores[i].descricao;
                //    ViewData["Indicador" + (i) + ".Valor"] = listaIndicadores[i].valor;
                //}
            }
            else
            {
                ViewData["NumNotificacoes"] = null;
                //ViewData["NumIndicadores"] = null;
            }

            return View();
        }

    }
}
