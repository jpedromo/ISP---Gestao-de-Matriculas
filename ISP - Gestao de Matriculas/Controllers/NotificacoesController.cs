using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Filters;
using WebMatrix.WebData;
using ISP.GestaoMatriculas.Contracts;
<<<<<<< HEAD
using Everis.Web.Mvc;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Controllers
{

    [InitializeSimpleMembership(Order = 1)]
<<<<<<< HEAD
    [Authorize(Order = 2)]
    //[MenuDataFilter(Order = 2)]
    public class NotificacoesController : SKController
=======
    [MenuDataFilter(Order = 2)]
    public class NotificacoesController : Controller
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    {

        private INotificacaoRepository notificacoesRepository;
        private IUserProfileRepository usersRepository;
        private IEntidadeRepository entidadesRepository;
<<<<<<< HEAD
        private IValorSistemaRepository valoresSistemaRepository;

        public NotificacoesController(INotificacaoRepository notificacoesRepository, IUserProfileRepository usersRepository, IEntidadeRepository entidadesRepository, IValorSistemaRepository valoresSistemaRepository)
=======

        public NotificacoesController(INotificacaoRepository notificacoesRepository, IUserProfileRepository usersRepository, IEntidadeRepository entidadesRepository)
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        {
            this.notificacoesRepository = notificacoesRepository;
            this.usersRepository = usersRepository;
            this.entidadesRepository = entidadesRepository;
<<<<<<< HEAD
            this.valoresSistemaRepository = valoresSistemaRepository;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }
        //
        // GET: /Notificacoes/

<<<<<<< HEAD
        public ActionResult Index(int? entidade)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            Entidade entidadeUtilizador = user.entidade;

            int entId = -1;

            if (entidade != null && (int)entidade <= entidadesRepository.All.Count())
            {
                entId = (int)entidade;

                if (entidadesRepository.Find(entId).nome == "ISP")
                {
                    entId = -1;
                }
            }

            IQueryable<Notificacao> NotificacoesToView;
            if (entidadeUtilizador.scope.valor == "GLOBAL")
            {
                NotificacoesToView = notificacoesRepository.All;
                if (entId > 0)
                {
                    NotificacoesToView = NotificacoesToView.Where(n => n.entidadeId == entId);
                    this.ViewBag.entidade = new SelectList(entidadesRepository.All, "entidadeId", "nome", entidadesRepository.Find(entId).entidadeId);
                }
                else
                {
                    this.ViewBag.entidade = new SelectList(entidadesRepository.All, "entidadeId", "nome");
                }
                    
            }
            else
            {
                NotificacoesToView = notificacoesRepository.All.Where(n => n.entidadeId == entidadeUtilizador.entidadeId);
            }
            return View(NotificacoesToView.ToList());
=======
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
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }

        //
        // GET: /Notificacoes/Details/5

        public ActionResult Details(int id)
        {
<<<<<<< HEAD

            Notificacao notificacao = notificacoesRepository.Find(id);
            notificacao.lida = true;

            notificacoesRepository.InsertOrUpdate(notificacao);
            notificacoesRepository.Save();
=======
            Notificacao notificacao = notificacoesRepository.Find(id);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            return View(notificacao);
        }

        //
        // GET: /Notificacoes/Create
<<<<<<< HEAD
        [Authorize(Roles = "Admin, ISP")]
        public ActionResult Create()
        {
            ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome");
            ViewBag.tipologiaId = new SelectList(valoresSistemaRepository.GetPorTipologia("TIPO_NOTIFICACAO"), "valorSistemaId", "descricao");
=======

        public ActionResult Create()
        {
            ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome");
            ViewBag.tipo = new SelectList(Enum.GetValues(typeof(Notificacao.TipoNotificacao)).Cast<Notificacao.TipoNotificacao>()
    .Select(v => v.ToString()).ToList());
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            return View();
        }

        //
        // POST: /Notificacoes/Create
<<<<<<< HEAD
        [Authorize(Roles = "Admin, ISP")]
=======

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
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
<<<<<<< HEAD

                this.Alerts.Success("Notificação criada com sucesso.");
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                return RedirectToAction("Index");
            }

            ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome");
<<<<<<< HEAD
            ViewBag.tipologiaId = new SelectList(valoresSistemaRepository.GetPorTipologia("TIPO_NOTIFICACAO"), "valorSistemaId", "descricao");

            this.Alerts.Danger("Erro na criação da notificação.");
=======
            ViewBag.tipo = new SelectList(Enum.GetValues(typeof(Notificacao.TipoNotificacao)).Cast<Notificacao.TipoNotificacao>()
    .Select(v => v.ToString()).ToList());

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            return View(notificacao);
        }
    }
}
