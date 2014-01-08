using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Filters;
using WebMatrix.WebData;
using ISP.GestaoMatriculas.Contracts;
using Everis.Web.Mvc;

namespace ISP.GestaoMatriculas.Controllers
{

    [InitializeSimpleMembership(Order = 1)]
    [Authorize(Order = 2)]
    //[MenuDataFilter(Order = 2)]
    public class NotificacoesController : SKController
    {

        private INotificacaoRepository notificacoesRepository;
        private IUserProfileRepository usersRepository;
        private IEntidadeRepository entidadesRepository;
        private IValorSistemaRepository valoresSistemaRepository;

        public NotificacoesController(INotificacaoRepository notificacoesRepository, IUserProfileRepository usersRepository, IEntidadeRepository entidadesRepository, IValorSistemaRepository valoresSistemaRepository)
        {
            this.notificacoesRepository = notificacoesRepository;
            this.usersRepository = usersRepository;
            this.entidadesRepository = entidadesRepository;
            this.valoresSistemaRepository = valoresSistemaRepository;
        }
        //
        // GET: /Notificacoes/

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
        }

        //
        // GET: /Notificacoes/Details/5

        public ActionResult Details(int id)
        {

            Notificacao notificacao = notificacoesRepository.Find(id);
            notificacao.lida = true;

            notificacoesRepository.InsertOrUpdate(notificacao);
            notificacoesRepository.Save();

            return View(notificacao);
        }

        //
        // GET: /Notificacoes/Create
        [Authorize(Roles = "Admin, ISP")]
        public ActionResult Create()
        {
            ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome");
            ViewBag.tipologiaId = new SelectList(valoresSistemaRepository.GetPorTipologia("TIPO_NOTIFICACAO"), "valorSistemaId", "descricao");

            return View();
        }

        //
        // POST: /Notificacoes/Create
        [Authorize(Roles = "Admin, ISP")]
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

                this.Alerts.Success("Notificação criada com sucesso.");
                return RedirectToAction("Index");
            }

            ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome");
            ViewBag.tipologiaId = new SelectList(valoresSistemaRepository.GetPorTipologia("TIPO_NOTIFICACAO"), "valorSistemaId", "descricao");

            this.Alerts.Danger("Erro na criação da notificação.");
            return View(notificacao);
        }
    }
}
