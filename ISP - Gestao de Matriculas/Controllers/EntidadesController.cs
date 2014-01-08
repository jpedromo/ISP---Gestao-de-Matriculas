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
<<<<<<< HEAD
using Everis.Web.Mvc;
using ISP.GestaoMatriculas.Repositories;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
<<<<<<< HEAD
    [Authorize(Order = 2)]
    //[MenuDataFilter(Order = 2)]
    public class EntidadesController : SKController
=======
    [MenuDataFilter(Order = 2)]
    public class EntidadesController : Controller
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    {

        private IEntidadeRepository entidadesRepository;
        private IRoleRepository rolesRepository;
        private IUserProfileRepository usersRepository;
<<<<<<< HEAD
        private IValorSistemaRepository valoresSistemaRepository;

        public EntidadesController(IEntidadeRepository entidadesRepository, IRoleRepository rolesRepository, IUserProfileRepository usersRepository, IValorSistemaRepository valoresSistemaRepository)
=======

        public EntidadesController(IEntidadeRepository entidadesRepository, IRoleRepository rolesRepository, IUserProfileRepository usersRepository)
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        {
            this.entidadesRepository = entidadesRepository;
            this.rolesRepository = rolesRepository;
            this.usersRepository = usersRepository;
<<<<<<< HEAD
            this.valoresSistemaRepository = valoresSistemaRepository;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }
        //
        // GET: /Entidades/

<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public ActionResult Index()
        {
            return View(entidadesRepository.All.Include("role"));
        }

        //
        // GET: /Entidades/Details/5

<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            Entidade entidade = entidadesRepository.All.Include("role").Single(e => e.entidadeId == id);
=======
        public ActionResult Details(int id)
        {
            Entidade entidade = entidadesRepository.All.Include("role").Single(e => e.Id == id);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            return View(entidade);
        }

        //
        // GET: /Entidades/Create

<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public ActionResult Create()
        {

            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName");
<<<<<<< HEAD
            ViewBag.scope = new SelectList(valoresSistemaRepository.GetPorTipologia("TIPO_SCOPE"),"valorSistemaId", "descricao" );
=======
            ViewBag.scope = new SelectList(Enum.GetValues(typeof(Entidade.ScopeLevel)).Cast<Entidade.ScopeLevel>()
                .Select(v => v.ToString()).ToList());
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            this.ViewBag.ativo = true;

            return View();
        }

        //
        // POST: /Entidades/Create
<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        [HttpPost]
        public ActionResult Create(Entidade entidade)
        {
            if (ModelState.IsValid)
            {
                entidadesRepository.InsertOrUpdate(entidade);

                entidadesRepository.Save();
                return RedirectToAction("Index");
            }

<<<<<<< HEAD
            this.ViewBag.ativo = true;
            this.ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName");
            this.ViewBag.scope = new SelectList(valoresSistemaRepository.GetPorTipologia("TIPO_SCOPE"), "valorSistemaId", "descricao");
=======
            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName");
            ViewBag.scope = new SelectList(Enum.GetValues(typeof(Entidade.ScopeLevel)).Cast<Entidade.ScopeLevel>()
                .Select(v => v.ToString()).ToList());
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            return View(entidade);
        }

        //
        // GET: /Entidades/Edit/5
<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            List<ValorSistema> tiposScope = valoresSistemaRepository.GetPorTipologia("TIPO_SCOPE");

=======

        public ActionResult Edit(int id)
        {
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            Entidade entidade = entidadesRepository.Find(id);

            //var scopeValues = Enum.GetValues(typeof(Entidade.ScopeLevel)).Cast<int>()
            //    .ToDictionary(p=> p, p => Enum.GetName(typeof(Entidade.ScopeLevel), p));
<<<<<<< HEAD
                       
            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName", entidade.roleId);
            ViewBag.scopeId = new SelectList(tiposScope, "valorSistemaId", "descricao", entidade.scopeId);
=======

            var scopeValues = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(typeof(Entidade.ScopeLevel)))
            {
                scopeValues.Add((int)item, Enum.GetName(typeof(Entidade.ScopeLevel), item));
            }

            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName", entidade.roleId);
            ViewBag.scopeList = new SelectList(scopeValues, "Key", "Value", (int)entidade.scope);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            
            return View(entidade);
        }

        //
        // POST: /Entidades/Edit/5
<<<<<<< HEAD
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
=======

        [HttpPost]
        public ActionResult Edit(Entidade entidade)
        {
            if (ModelState.IsValid)
            {
                entidadesRepository.InsertOrUpdate(entidade);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                entidadesRepository.Save();

                return RedirectToAction("Index");
            }

<<<<<<< HEAD
            
            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName", entidade.roleId);
            ViewBag.scopeId = new SelectList(valoresSistemaRepository.GetPorTipologia("TIPO_SCOPE"), "valorSistemaId", "descricao");
=======
            List<String> scopeValues = Enum.GetValues(typeof(Entidade.ScopeLevel)).Cast<Entidade.ScopeLevel>()
                .Select(v => v.ToString()).ToList();

            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName", entidade.roleId);
            ViewBag.scope = new SelectList(scopeValues, entidade.scope.ToString());
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            return View(entidade);
        }

        public enum ManageMessageId
        {
            ChangeSuccess,
            ChangeInsuccess,     
        }
        //
        // GET: /Account/Manage

<<<<<<< HEAD
        public ActionResult Manage()
        {
            UserProfile user = usersRepository.GetUserByIDIncludeEntidade(WebSecurity.CurrentUserId);

            EntityProfileViewModel model = new EntityProfileViewModel{ entityId = user.entidade.entidadeId,
                nomeResponsavel = user.entidade.nomeResponsavel, emailResponsavel = user.entidade.emailResponsavel,
                 telefoneResponsavel = user.entidade.telefoneResponsavel};


            ViewBag.ReturnUrl = Url.Action("Manage");
            ViewBag.NomeEntidade = user.entidade.nome;
=======
        public ActionResult Manage(ManageMessageId? message)
        {
            UserProfile user = usersRepository.All.Include("entidade").Single(u => u.UserId == WebSecurity.CurrentUserId);

            EntityProfileViewModel model = new EntityProfileViewModel{ entityId = user.entidade.Id,
                nomeResponsavel = user.entidade.nomeResponsavel, emailResponsavel = user.entidade.emailResponsavel,
                 telefoneResponsavel = user.entidade.telefoneResponsavel};

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangeSuccess ? "User Profile Saved"
                : "";

            ViewBag.ReturnUrl = Url.Action("Manage");
            ViewBag.NomeEntidade = user.entidade.Nome;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
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
<<<<<<< HEAD
                user = usersRepository.GetUserByIDIncludeEntidade(WebSecurity.CurrentUserId);

                if (user.entidade.entidadeId != editedEntity.entityId)
=======
                user = usersRepository.All.Include("entidade").Single(u => u.UserId == WebSecurity.CurrentUserId);

                if (user.entidade.Id != editedEntity.entityId)
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
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
<<<<<<< HEAD
                ViewBag.NomeEntidade = entidade.nome;

                this.Alerts.Success("Entidade alterada com sucesso.");
                return RedirectToAction("Manage");
            }           
            // If we got this far, something failed, redisplay form
            this.Alerts.Danger("Erro ao alterar entidade.");
=======
                ViewBag.NomeEntidade = entidade.Nome;
                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangeSuccess });
            }           
            // If we got this far, something failed, redisplay form
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            return View(editedEntity);
        }

        //
        // GET: /Entidades/Deactivate/5
<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public ActionResult Deactivate(int id)
        {
            Entidade entidade = null;
            try
            {
<<<<<<< HEAD
                entidade = entidadesRepository.All.Include("role").Single(e => e.entidadeId == id);
=======
                entidade = entidadesRepository.All.Include("role").Single(e => e.Id == id);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }
            catch (ArgumentNullException) { return this.HttpNotFound(); }

            return View(entidade);
        }

        //
        // POST: /Entidades/Deactivate/5
<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        [HttpPost]
        public ActionResult Deactivate(int id, FormCollection collection)
        {
            Entidade entidade = null;
            try
            {
<<<<<<< HEAD
                entidade = entidadesRepository.All.Include("role").Single(e => e.entidadeId == id);
=======
                entidade = entidadesRepository.All.Include("role").Single(e => e.Id == id);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }
            catch (ArgumentNullException) { return this.HttpNotFound(); }

            List<UserProfile> undoList = new List<UserProfile>();

            bool deactivationSuccess = true;
<<<<<<< HEAD
            foreach(UserProfile user in usersRepository.GetUserByEntidade(entidade.entidadeId).ToList()){
=======
            foreach(UserProfile user in usersRepository.All.Where<UserProfile>(u => u.entidadeId == entidade.Id).ToList()){
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
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

<<<<<<< HEAD
                this.Alerts.Danger("Não é possível desativar a própria entidade.");
=======
                ViewBag.errorMessage = "Não é possível desativar a própria Entidade";
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                usersRepository.Save();

                return View(entidade);
            }

            entidade.ativo = false;
            entidadesRepository.InsertOrUpdate(entidade);

            entidadesRepository.Save();
<<<<<<< HEAD
            usersRepository.Save();
            this.Alerts.Success("Entidade desativada com sucesso.");
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            return this.RedirectToAction("Index");
        }


        //
        // GET: /Entidades/Deactivate/5
<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public ActionResult Activate(int id)
        {
            Entidade entidade = null;
            try
            {
<<<<<<< HEAD
                entidade = entidadesRepository.All.Include("role").Single(e => e.entidadeId == id);
=======
                entidade = entidadesRepository.All.Include("role").Single(e => e.Id == id);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }
            catch (ArgumentNullException) { return this.HttpNotFound(); }

            return View(entidade);
        }

        //
        // POST: /Entidades/Deactivate/5
<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
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
<<<<<<< HEAD
            this.Alerts.Success("Entidade ativada com sucesso.");
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            return this.RedirectToAction("Index");
        }

    }
}
