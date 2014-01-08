using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Models;
using ISP.GestaoMatriculas.Model;
using System.Data;
using WebMatrix.WebData;
using ISP.GestaoMatriculas.ViewModels;
using ISP.GestaoMatriculas.Filters;
using System.Data.Entity;
using ISP.GestaoMatriculas.Contracts;
using Everis.Web.Mvc;
using ISP.GestaoMatriculas.Repositories;

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [Authorize(Order = 2)]
    //[MenuDataFilter(Order = 2)]
    public class EntidadesController : SKController
    {

        private IEntidadeRepository entidadesRepository;
        private IRoleRepository rolesRepository;
        private IUserProfileRepository usersRepository;
        private IValorSistemaRepository valoresSistemaRepository;

        public EntidadesController(IEntidadeRepository entidadesRepository, IRoleRepository rolesRepository, IUserProfileRepository usersRepository, IValorSistemaRepository valoresSistemaRepository)
        {
            this.entidadesRepository = entidadesRepository;
            this.rolesRepository = rolesRepository;
            this.usersRepository = usersRepository;
            this.valoresSistemaRepository = valoresSistemaRepository;
        }
        //
        // GET: /Entidades/

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(entidadesRepository.All.Include("role"));
        }

        //
        // GET: /Entidades/Details/5

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            Entidade entidade = entidadesRepository.All.Include("role").Single(e => e.entidadeId == id);

            return View(entidade);
        }

        //
        // GET: /Entidades/Create

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName");
            ViewBag.scope = new SelectList(valoresSistemaRepository.GetPorTipologia("TIPO_SCOPE"),"valorSistemaId", "descricao" );

            this.ViewBag.ativo = true;

            return View();
        }

        //
        // POST: /Entidades/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Entidade entidade)
        {
            if (ModelState.IsValid)
            {
                entidadesRepository.InsertOrUpdate(entidade);

                entidadesRepository.Save();
                return RedirectToAction("Index");
            }

            this.ViewBag.ativo = true;
            this.ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName");
            this.ViewBag.scope = new SelectList(valoresSistemaRepository.GetPorTipologia("TIPO_SCOPE"), "valorSistemaId", "descricao");

            return View(entidade);
        }

        //
        // GET: /Entidades/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            List<ValorSistema> tiposScope = valoresSistemaRepository.GetPorTipologia("TIPO_SCOPE");

            Entidade entidade = entidadesRepository.Find(id);

            //var scopeValues = Enum.GetValues(typeof(Entidade.ScopeLevel)).Cast<int>()
            //    .ToDictionary(p=> p, p => Enum.GetName(typeof(Entidade.ScopeLevel), p));
                       
            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName", entidade.roleId);
            ViewBag.scopeId = new SelectList(tiposScope, "valorSistemaId", "descricao", entidade.scopeId);
            
            return View(entidade);
        }

        //
        // POST: /Entidades/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Entidade entidade)
        {
            Entidade entidadeAnterior = null;
            try
            {
                entidadeAnterior = entidadesRepository.Find(entidade.entidadeId);
            }
            catch (InvalidOperationException)
            {
                return this.HttpNotFound();
            }
            if (entidadeAnterior == null)
            {
                return this.HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                entidadeAnterior.nome = entidade.nome;
                entidadeAnterior.nomeResponsavel = entidade.nomeResponsavel;
                entidadeAnterior.emailResponsavel = entidade.emailResponsavel;
                entidadeAnterior.telefoneResponsavel = entidade.telefoneResponsavel;
                entidadeAnterior.scope = entidade.scope;
                entidadeAnterior.roleId = entidade.roleId;

                entidadesRepository.InsertOrUpdate(entidadeAnterior);
                entidadesRepository.Save();

                return RedirectToAction("Index");
            }

            
            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName", entidade.roleId);
            ViewBag.scopeId = new SelectList(valoresSistemaRepository.GetPorTipologia("TIPO_SCOPE"), "valorSistemaId", "descricao");

            return View(entidade);
        }

        public enum ManageMessageId
        {
            ChangeSuccess,
            ChangeInsuccess,     
        }
        //
        // GET: /Account/Manage

        public ActionResult Manage()
        {
            UserProfile user = usersRepository.GetUserByIDIncludeEntidade(WebSecurity.CurrentUserId);

            EntityProfileViewModel model = new EntityProfileViewModel{ entityId = user.entidade.entidadeId,
                nomeResponsavel = user.entidade.nomeResponsavel, emailResponsavel = user.entidade.emailResponsavel,
                 telefoneResponsavel = user.entidade.telefoneResponsavel};


            ViewBag.ReturnUrl = Url.Action("Manage");
            ViewBag.NomeEntidade = user.entidade.nome;
            return View(model);
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(EntityProfileViewModel editedEntity)
        {
            UserProfile user = null;

            try
            {
                user = usersRepository.GetUserByIDIncludeEntidade(WebSecurity.CurrentUserId);

                if (user.entidade.entidadeId != editedEntity.entityId)
                {
                    throw new InvalidOperationException("Authenticated user does have permission to edit this Entity");
                }
            }
            catch (InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                Entidade entidade = user.entidade;

                entidade.nomeResponsavel = editedEntity.nomeResponsavel;
                entidade.emailResponsavel = editedEntity.emailResponsavel;
                entidade.telefoneResponsavel = editedEntity.telefoneResponsavel;

                usersRepository.InsertOrUpdate(user);
                usersRepository.Save();

                ViewBag.ReturnUrl = Url.Action("Manage");
                ViewBag.NomeEntidade = entidade.nome;

                this.Alerts.Success("Entidade alterada com sucesso.");
                return RedirectToAction("Manage");
            }           
            // If we got this far, something failed, redisplay form
            this.Alerts.Danger("Erro ao alterar entidade.");
            return View(editedEntity);
        }

        //
        // GET: /Entidades/Deactivate/5
        [Authorize(Roles = "Admin")]
        public ActionResult Deactivate(int id)
        {
            Entidade entidade = null;
            try
            {
                entidade = entidadesRepository.All.Include("role").Single(e => e.entidadeId == id);
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }
            catch (ArgumentNullException) { return this.HttpNotFound(); }

            return View(entidade);
        }

        //
        // POST: /Entidades/Deactivate/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Deactivate(int id, FormCollection collection)
        {
            Entidade entidade = null;
            try
            {
                entidade = entidadesRepository.All.Include("role").Single(e => e.entidadeId == id);
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }
            catch (ArgumentNullException) { return this.HttpNotFound(); }

            List<UserProfile> undoList = new List<UserProfile>();

            bool deactivationSuccess = true;
            foreach(UserProfile user in usersRepository.GetUserByEntidade(entidade.entidadeId).ToList()){
                if (UserUtils.deactivate(user))
                {
                    undoList.Add(user);
                    usersRepository.InsertOrUpdate(user);
                }
                else{
                    deactivationSuccess = false;
                    break;
                }
            }

            if(!deactivationSuccess){
                foreach(UserProfile user in undoList){
                    UserUtils.activate(user);
                    usersRepository.InsertOrUpdate(user);
                }
            }

            if(!deactivationSuccess){

                this.Alerts.Danger("Não é possível desativar a própria entidade.");
                usersRepository.Save();

                return View(entidade);
            }

            entidade.ativo = false;
            entidadesRepository.InsertOrUpdate(entidade);

            entidadesRepository.Save();
            usersRepository.Save();
            this.Alerts.Success("Entidade desativada com sucesso.");
            return this.RedirectToAction("Index");
        }


        //
        // GET: /Entidades/Deactivate/5
        [Authorize(Roles = "Admin")]
        public ActionResult Activate(int id)
        {
            Entidade entidade = null;
            try
            {
                entidade = entidadesRepository.All.Include("role").Single(e => e.entidadeId == id);
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }
            catch (ArgumentNullException) { return this.HttpNotFound(); }

            return View(entidade);
        }

        //
        // POST: /Entidades/Deactivate/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Activate(int id, FormCollection collection)
        {
            Entidade entidade = null;
            try
            {
                entidade = entidadesRepository.Find(id);
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }

            entidade.ativo = true;
            entidadesRepository.InsertOrUpdate(entidade);

            entidadesRepository.Save();
            this.Alerts.Success("Entidade ativada com sucesso.");
            return this.RedirectToAction("Index");
        }

    }
}
