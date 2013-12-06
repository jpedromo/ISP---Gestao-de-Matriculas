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

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [MenuDataFilter(Order = 2)]
    public class EntidadesController : Controller
    {

        private IEntidadeRepository entidadesRepository;
        private IRoleRepository rolesRepository;
        private IUserProfileRepository usersRepository;

        public EntidadesController(IEntidadeRepository entidadesRepository, IRoleRepository rolesRepository, IUserProfileRepository usersRepository)
        {
            this.entidadesRepository = entidadesRepository;
            this.rolesRepository = rolesRepository;
            this.usersRepository = usersRepository;
        }
        //
        // GET: /Entidades/

        public ActionResult Index()
        {
            return View(entidadesRepository.All.Include("role"));
        }

        //
        // GET: /Entidades/Details/5

        public ActionResult Details(int id)
        {
            Entidade entidade = entidadesRepository.All.Include("role").Single(e => e.Id == id);

            return View(entidade);
        }

        //
        // GET: /Entidades/Create

        public ActionResult Create()
        {

            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName");
            ViewBag.scope = new SelectList(Enum.GetValues(typeof(Entidade.ScopeLevel)).Cast<Entidade.ScopeLevel>()
                .Select(v => v.ToString()).ToList());

            this.ViewBag.ativo = true;

            return View();
        }

        //
        // POST: /Entidades/Create

        [HttpPost]
        public ActionResult Create(Entidade entidade)
        {
            if (ModelState.IsValid)
            {
                entidadesRepository.InsertOrUpdate(entidade);

                entidadesRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName");
            ViewBag.scope = new SelectList(Enum.GetValues(typeof(Entidade.ScopeLevel)).Cast<Entidade.ScopeLevel>()
                .Select(v => v.ToString()).ToList());

            return View(entidade);
        }

        //
        // GET: /Entidades/Edit/5

        public ActionResult Edit(int id)
        {
            Entidade entidade = entidadesRepository.Find(id);

            //var scopeValues = Enum.GetValues(typeof(Entidade.ScopeLevel)).Cast<int>()
            //    .ToDictionary(p=> p, p => Enum.GetName(typeof(Entidade.ScopeLevel), p));

            var scopeValues = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(typeof(Entidade.ScopeLevel)))
            {
                scopeValues.Add((int)item, Enum.GetName(typeof(Entidade.ScopeLevel), item));
            }

            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName", entidade.roleId);
            ViewBag.scopeList = new SelectList(scopeValues, "Key", "Value", (int)entidade.scope);
            
            return View(entidade);
        }

        //
        // POST: /Entidades/Edit/5

        [HttpPost]
        public ActionResult Edit(Entidade entidade)
        {
            if (ModelState.IsValid)
            {
                entidadesRepository.InsertOrUpdate(entidade);
                entidadesRepository.Save();

                return RedirectToAction("Index");
            }

            List<String> scopeValues = Enum.GetValues(typeof(Entidade.ScopeLevel)).Cast<Entidade.ScopeLevel>()
                .Select(v => v.ToString()).ToList();

            ViewBag.roleId = new SelectList(rolesRepository.All, "roleId", "RoleName", entidade.roleId);
            ViewBag.scope = new SelectList(scopeValues, entidade.scope.ToString());

            return View(entidade);
        }

        public enum ManageMessageId
        {
            ChangeSuccess,
            ChangeInsuccess,     
        }
        //
        // GET: /Account/Manage

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
                user = usersRepository.All.Include("entidade").Single(u => u.UserId == WebSecurity.CurrentUserId);

                if (user.entidade.Id != editedEntity.entityId)
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
                ViewBag.NomeEntidade = entidade.Nome;
                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangeSuccess });
            }           
            // If we got this far, something failed, redisplay form
            return View(editedEntity);
        }

        //
        // GET: /Entidades/Deactivate/5

        public ActionResult Deactivate(int id)
        {
            Entidade entidade = null;
            try
            {
                entidade = entidadesRepository.All.Include("role").Single(e => e.Id == id);
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }
            catch (ArgumentNullException) { return this.HttpNotFound(); }

            return View(entidade);
        }

        //
        // POST: /Entidades/Deactivate/5

        [HttpPost]
        public ActionResult Deactivate(int id, FormCollection collection)
        {
            Entidade entidade = null;
            try
            {
                entidade = entidadesRepository.All.Include("role").Single(e => e.Id == id);
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }
            catch (ArgumentNullException) { return this.HttpNotFound(); }

            List<UserProfile> undoList = new List<UserProfile>();

            bool deactivationSuccess = true;
            foreach(UserProfile user in usersRepository.All.Where<UserProfile>(u => u.entidadeId == entidade.Id).ToList()){
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

                ViewBag.errorMessage = "Não é possível desativar a própria Entidade";
                usersRepository.Save();

                return View(entidade);
            }

            entidade.ativo = false;
            entidadesRepository.InsertOrUpdate(entidade);

            entidadesRepository.Save();
            return this.RedirectToAction("Index");
        }


        //
        // GET: /Entidades/Deactivate/5

        public ActionResult Activate(int id)
        {
            Entidade entidade = null;
            try
            {
                entidade = entidadesRepository.All.Include("role").Single(e => e.Id == id);
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }
            catch (ArgumentNullException) { return this.HttpNotFound(); }

            return View(entidade);
        }

        //
        // POST: /Entidades/Deactivate/5

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
            return this.RedirectToAction("Index");
        }

    }
}
