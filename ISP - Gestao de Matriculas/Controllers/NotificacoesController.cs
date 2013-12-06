using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Filters;
using WebMatrix.WebData;
using ISP.GestaoMatriculas.Contracts;

namespace ISP.GestaoMatriculas.Controllers
{

    [InitializeSimpleMembership(Order = 1)]
    [MenuDataFilter(Order = 2)]
    public class NotificacoesController : Controller
    {

        private INotificacaoRepository notificacoesRepository;
        private IUserProfileRepository usersRepository;
        private IEntidadeRepository entidadesRepository;

        public NotificacoesController(INotificacaoRepository notificacoesRepository, IUserProfileRepository usersRepository, IEntidadeRepository entidadesRepository)
        {
            this.notificacoesRepository = notificacoesRepository;
            this.usersRepository = usersRepository;
            this.entidadesRepository = entidadesRepository;
        }
        //
        // GET: /Notificacoes/

        public ActionResult Index()
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            Entidade entidade = user.entidade;

            List<Notificacao> NotificacoesToView;
            if (entidade.scope == Entidade.ScopeLevel.Global)
            {
                NotificacoesToView = notificacoesRepository.All.ToList();
            }
            else
            {
                NotificacoesToView = notificacoesRepository.All.Where(n => n.entidadeId == entidade.Id).ToList();
            }

            return View(NotificacoesToView);
        }

        //
        // GET: /Notificacoes/Details/5

        public ActionResult Details(int id)
        {
            Notificacao notificacao = notificacoesRepository.Find(id);

            return View(notificacao);
        }

        //
        // GET: /Notificacoes/Create

        public ActionResult Create()
        {
            ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome");
            ViewBag.tipo = new SelectList(Enum.GetValues(typeof(Notificacao.TipoNotificacao)).Cast<Notificacao.TipoNotificacao>()
    .Select(v => v.ToString()).ToList());

            return View();
        }

        //
        // POST: /Notificacoes/Create

        [HttpPost]
        public ActionResult Create(Notificacao notificacao)
        {
            if (ModelState.IsValid)
            {
                notificacao.dataCriacao = DateTime.Now;

                Entidade entidade = entidadesRepository.Find((int)notificacao.entidadeId);

                entidade.notificacoes.Add(notificacao);

                entidadesRepository.Save();
                notificacoesRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome");
            ViewBag.tipo = new SelectList(Enum.GetValues(typeof(Notificacao.TipoNotificacao)).Cast<Notificacao.TipoNotificacao>()
    .Select(v => v.ToString()).ToList());

            return View(notificacao);
        }
    }
}
