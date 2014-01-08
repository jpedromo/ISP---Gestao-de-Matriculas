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
<<<<<<< HEAD
                    ViewData["Notificacao" + (i) + ".Tipo"] = notificacaoPreviewList[i].tipologia.descricao;
=======
                    ViewData["Notificacao" + (i) + ".Tipo"] = notificacaoPreviewList[i].tipoToString();
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                    ViewData["Notificacao" + (i) + ".Mensagem"] = notificacaoPreviewList[i].mensagemToString();
                    ViewData["Notificacao" + (i) + ".Data"] = notificacaoPreviewList[i].dataCriacao.ToString();
                }

<<<<<<< HEAD
                //int numIndicadores = user.entidade.indicadores.Count;
                //ViewData["NumIndicadores"] = numIndicadores;
                //List<Indicador> listaIndicadores = user.entidade.indicadores.ToList();
                //for (int i = 0; i < numIndicadores; i++)
                //{
                //    ViewData["Indicador" + (i) + ".Descricao"] = listaIndicadores[i].descricao;
                //    ViewData["Indicador" + (i) + ".Valor"] = listaIndicadores[i].valor;
                //}
=======
                int numIndicadores = user.entidade.indicadores.Count;
                ViewData["NumIndicadores"] = numIndicadores;
                List<Indicador> listaIndicadores = user.entidade.indicadores.ToList();
                for (int i = 0; i < numIndicadores; i++)
                {
                    ViewData["Indicador" + (i) + ".Descricao"] = listaIndicadores[i].descricao;
                    ViewData["Indicador" + (i) + ".Valor"] = listaIndicadores[i].calcular().valor;
                }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }
            else
            {
                ViewData["NumNotificacoes"] = null;
<<<<<<< HEAD
                //ViewData["NumIndicadores"] = null;
=======
                ViewData["NumIndicadores"] = null;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }

            return View();
        }

    }
}
