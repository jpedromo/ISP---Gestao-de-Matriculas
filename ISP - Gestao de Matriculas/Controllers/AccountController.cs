using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ISP.GestaoMatriculas.Filters;
using ISP.GestaoMatriculas.Models;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.ViewModels;
using System.Data.Entity;
using ISP.GestaoMatriculas.Contracts;
<<<<<<< HEAD
using Everis.Web.Mvc;
using System.Security.Principal;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [Authorize(Order = 2)]
<<<<<<< HEAD
    //[MenuDataFilter(Order = 3)]
    public class AccountController : SKController
=======
    [MenuDataFilter(Order = 3)]
    public class AccountController : Controller
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    {
        private readonly IUserProfileRepository usersRepository;
        private readonly IEntidadeRepository entidadesRepository;
        private readonly IRoleRepository rolesRepository;

        public AccountController(IUserProfileRepository usersRepository, IEntidadeRepository entidadesRepository, IRoleRepository rolesRepository)
        {
            this.usersRepository = usersRepository;
            this.entidadesRepository = entidadesRepository;
            this.rolesRepository = rolesRepository;
        }

<<<<<<< HEAD
        
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(usersRepository.All.Include("entidade"));
=======
        //Problema: Nao contacta a base de dados caso seja esta a primeira chamada. Acesso restringido em Web.config
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(usersRepository.All.Include("Entidade"));
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;
<<<<<<< HEAD
            return View(new LoginModel { ReturnUrl = returnUrl });
=======
            return View();
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
<<<<<<< HEAD
                    UserProfile user = usersRepository.FindByUsername(model.UserName);
=======
                    UserProfile user = usersRepository.All.Single(u => u.UserName == model.UserName);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                    if (!user.ativo)
                    {
                        ModelState.AddModelError("", "Este Utilizador encontra-se inactivo. Contacte um administrador do sistema.");
                        return View(model);
                    }
                    if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                    {
                        return RedirectToLocal(returnUrl);
                    }
                }
            }
            catch (InvalidOperationException) {/* utilizador não existe */ }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

<<<<<<< HEAD

        public ActionResult LoginAD(string returnUrl)
        {
            //WindowsIdentity ident = WindowsIdentity.GetCurrent();
            //WindowsPrincipal wind_princ = new WindowsPrincipal(ident);
            UserProfile user;
            try
            {
                user = usersRepository.All.Single(u => u.utilizadorAD == User.Identity.Name);
            }
            catch (InvalidOperationException)
            {
                user = null;
            }

            if (user != null && user.ativo && WebSecurity.Login(user.UserName, "utilizadorAD", persistCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }
            ViewBag.username = User.Identity.Name;
            return this.View();
        }

=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            List<Entidade> listaEntidadesToClone = entidadesRepository.All.ToList();
            //Shallow copy
            List<Entidade> listaEntidades = listaEntidadesToClone.GetRange(0, listaEntidadesToClone.Count);

            foreach(Entidade e in listaEntidades){
<<<<<<< HEAD
                e.nome = "" + e.entidadeId + " - " + e.nome;
=======
                e.Nome = "" + e.Id + " - " + e.Nome;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }

            this.ViewBag.entidadeId = new SelectList(listaEntidades, "entidadeId", "nome");
            this.ViewBag.ativo = true;
            return View();
        }

        //
        // POST: /Account/Register

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    Entidade entidade =entidadesRepository.Find(model.entidadeId);
                    Role roleAssociado = rolesRepository.Find(entidade.roleId);

                    if (model.ativo && !entidade.ativo)
                    {
                        ModelState.AddModelError("", "Erro: a entidade seleccionada encontra-se desactivada");
                    }
                    else
                    {
<<<<<<< HEAD
                        if (!(model.utilizadorAD == string.Empty || model.utilizadorAD == null))
                        {
                            model.Password = "utilizadorAD";
                        }

                        WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new
                        {
                            EntidadeId_FK = model.entidadeId,
                            Nome = model.nome,
                            ValTelefone = model.telefone,
                            ValEmail = model.email,
                            FlgAtivo = model.ativo,
                            ValUtilizadorAD = model.utilizadorAD
=======

                        WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new
                        {
                            entidadeId = model.entidadeId,
                            nome = model.nome,
                            telefone = model.telefone,
                            email = model.email,
                            ativo = model.ativo
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                        });

                        Roles.AddUserToRole(model.UserName, roleAssociado.RoleName);

                        //WebSecurity.Login(model.UserName, model.Password);

                        return RedirectToAction("Index");
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            List<Entidade> listaEntidadesToClone = entidadesRepository.All.ToList();
            //Shallow copy
            List<Entidade> listaEntidades = listaEntidadesToClone.GetRange(0, listaEntidadesToClone.Count);

            foreach (Entidade e in listaEntidades)
            {
<<<<<<< HEAD
                e.nome = "" + e.entidadeId + " - " + e.nome;
=======
                e.Nome = "" + e.Id + " - " + e.Nome;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }
            this.ViewBag.entidadeId = new SelectList(listaEntidades, "entidadeId", "nome");
            this.ViewBag.ativo = true;

            return View(model);
        }


        //
        // GET: /Account/Deactivate/5
        [Authorize(Roles = "Admin")]
        public ActionResult Deactivate(int id)
        {
            UserProfile user = null;
            try
            {
<<<<<<< HEAD
                user = usersRepository.GetUserByIDIncludeEntidade(id);
=======
                user = usersRepository.All.Include("entidade").Single(u => u.UserId == id);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }
            return this.View(user);
        }

        //
        // POST: /Account/Deactivate/5

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id, FormCollection collection)
        {
            UserProfile user = null;
            try
            {
<<<<<<< HEAD
                user = usersRepository.GetUserByIDIncludeEntidade(id);
=======
                user = usersRepository.All.Include("entidade").Single(u => u.UserId == id);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }

            if (!UserUtils.deactivate(user))
            {
<<<<<<< HEAD
                this.Alerts.Danger("É impossível desativar a própria conta.");
=======
                ViewBag.errorMessage = "É impossível desativar a própria conta.";
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                return this.View(user);
            }

            usersRepository.InsertOrUpdate(user);
            usersRepository.Save();

<<<<<<< HEAD
            this.Alerts.Success("Utilizador desativado com sucesso.");
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            return this.RedirectToAction("Index");

        }

        //
        // GET: /Account/Activate/5
        [Authorize(Roles = "Admin")]
        public ActionResult Activate(int id)
        {
            UserProfile user = null;
            try
            {
<<<<<<< HEAD
                user = usersRepository.GetUserByIDIncludeEntidade(id);
=======
                user = usersRepository.All.Include("entidade").Single(u => u.UserId == id);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }
            return this.View(user);
        }

        //
        // POST: /Account/Activate/5

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int id, FormCollection collection)
        {
            UserProfile user = null;
            try
            {
<<<<<<< HEAD
                user = usersRepository.GetUserByIDIncludeEntidade(id);
=======
                user = usersRepository.All.Include("entidade").Single(u => u.UserId == id);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }
            catch (InvalidOperationException) { return this.HttpNotFound(); }

            if (!UserUtils.activate(user))
            {
<<<<<<< HEAD
                this.Alerts.Danger("Não é possível ativar a conta.");
=======
                ViewBag.errorMessage = "Não é possível ativar a conta.";
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                return this.View(user);
            }
            usersRepository.InsertOrUpdate(user);
            usersRepository.Save();

<<<<<<< HEAD
            this.Alerts.Success("Utilizador ativado com sucesso.");
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            return this.RedirectToAction("Index");

        }

        //
        // GET: /Account/Manage
<<<<<<< HEAD
        public ActionResult Manage()
        {
            UserProfile user = usersRepository.GetUserByIDIncludeEntidade(WebSecurity.CurrentUserId);

            UserProfileViewModel model = new UserProfileViewModel { userId = user.UserId, nome = user.nome, email = user.email, telefone = user.telefone, utilizadorAD = user.utilizadorAD };
            
            ViewBag.ReturnUrl = Url.Action("Manage");
            ViewBag.UserName = user.UserName;
            ViewBag.AssociatedEntity = user.entidade.nome;
=======

        public ActionResult Manage(ManageMessageId? message)
        {
            UserProfile user = usersRepository.All.Include("entidade").Single(u => u.UserId == WebSecurity.CurrentUserId);

            UserProfileViewModel model = new UserProfileViewModel { userId = user.UserId, nome = user.nome, email = user.email, telefone = user.telefone };

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangeSuccess ? "User Profile Saved"
                : "";
            
            ViewBag.ReturnUrl = Url.Action("Manage");
            ViewBag.UserName = user.UserName;
            ViewBag.AssociatedEntity = user.entidade.Nome;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            return View(model);
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(UserProfileViewModel editedUser)
        {
            UserProfile user = null;
            bool changePasswordSucceeded = true; //Assume no error until proven wrong
<<<<<<< HEAD
            string successMessage = string.Empty;
            string errorMessage = string.Empty;

            try
            {
                user = usersRepository.GetUserByIDIncludeEntidade(editedUser.userId);
=======

            try
            {
                user = usersRepository.All.Include("entidade").Single(u => u.UserId == editedUser.userId);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

                if (user.UserName != User.Identity.Name)
                {
                    throw new InvalidOperationException("Authenticated user does not match the user to edit");
                }
            }
            catch (InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            if (ModelState.IsValid)
            {
<<<<<<< HEAD
                if (!(user.utilizadorAD == string.Empty || user.utilizadorAD == null))
                {
                    editedUser.newPassword = "utilizadorAD";
                }

                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                if ((editedUser.newPassword != "") && (editedUser.newPassword != null) && (user.utilizadorAD == null || user.utilizadorAD == string.Empty)){
=======
                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                if ((editedUser.newPassword != "") && (editedUser.newPassword != null)){
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                    
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, editedUser.oldPassword, editedUser.newPassword);
<<<<<<< HEAD
                        successMessage += "Password alterada com sucesso.\n";
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }
                }

                if (changePasswordSucceeded)
                {
                    user.nome = editedUser.nome;
                    user.email = editedUser.email;
                    user.telefone = editedUser.telefone;

                    usersRepository.InsertOrUpdate(user);
                    usersRepository.Save();

                    ViewBag.ReturnUrl = Url.Action("Manage");
<<<<<<< HEAD
                    ViewBag.AssociatedEntity = user.entidade.nome;
                    ViewBag.UserName = user.UserName;

                    successMessage += "Utilizador editado com sucesso.\n";
                    this.Alerts.Success(successMessage);
                    return RedirectToAction("Manage");
=======
                    ViewBag.AssociatedEntity = user.entidade.Nome;
                    ViewBag.UserName = user.UserName;
                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangeSuccess });
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                }
                else
                {
                    ViewBag.ReturnUrl = Url.Action("Manage");
<<<<<<< HEAD
                    ViewBag.AssociatedEntity = user.entidade.nome;
                    ViewBag.UserName = user.UserName;

                    errorMessage += "Password incorreta ou inválida.";
                    this.Alerts.Danger(errorMessage);
=======
                    ViewBag.AssociatedEntity = user.entidade.Nome;
                    ViewBag.UserName = user.UserName;
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                }
            }
            // If we got this far, something failed, redisplay form
            return View(editedUser);
        }

        //
        // GET: /Account/Details/5
<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public ActionResult Details(int id)
        {
            UserProfile user = null;
            
            try
            {
<<<<<<< HEAD
                user = usersRepository.GetUserByIDIncludeEntidade( id);
=======
                user = usersRepository.All.Include("entidade").Single(u => u.UserId == id);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            return View(user);
        }


        //
        // GET: /Account/Edit/5
<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        public ActionResult Edit(int id)
        {
            //Apolice apolice = this.db.Apolices.Find(id);
            UserProfile user = null;

            EditUserModel editedUser = null;
            try
            {
<<<<<<< HEAD
                user = usersRepository.GetUserByIDIncludeEntidade(id);

                editedUser = new EditUserModel { UserId = id, nome = user.nome, email = user.email, telefone = user.telefone, entidadeId = user.entidadeId, ativo = user.ativo, newPassword = "", ConfirmNewPassword = "", utilizadorAD = user.utilizadorAD };
=======
                user = usersRepository.All.Include("entidade").Single(u => u.UserId == id);

                editedUser = new EditUserModel { UserId = id, nome = user.nome, email = user.email, telefone = user.telefone, entidadeId = user.entidadeId, ativo = user.ativo, newPassword = "", ConfirmNewPassword = "" };
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            List<Entidade> listaEntidadesToClone = entidadesRepository.All.ToList();
            //Shallow copy
            List<Entidade> listaEntidades = listaEntidadesToClone.GetRange(0, listaEntidadesToClone.Count);

            foreach (Entidade e in listaEntidades)
            {
<<<<<<< HEAD
                e.nome = "" + e.entidadeId + " - " + e.nome;
=======
                e.Nome = "" + e.Id + " - " + e.Nome;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }
            this.ViewBag.entidadeId = new SelectList(listaEntidades, "entidadeId", "nome", editedUser.entidadeId);
            this.ViewBag.userName = usersRepository.Find(editedUser.UserId).UserName;
            this.ViewBag.ativo = editedUser.ativo;

            return View(editedUser);
        }

        //
        // POST: /Account/Edit/5
<<<<<<< HEAD
        [Authorize(Roles = "Admin")]
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserModel editedUser)
        {
            List<Entidade> listaEntidadesToClone, listaEntidades = null;
<<<<<<< HEAD
            string errorMessage = string.Empty;
            string successMessage = string.Empty;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

            if (ModelState.IsValid)
            {

<<<<<<< HEAD
                UserProfile user = usersRepository.GetUserByIDIncludeEntidade(editedUser.UserId);
=======
                UserProfile user = usersRepository.All.Include("entidade").Single(u => u.UserId == editedUser.UserId);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

                user.nome = editedUser.nome;
                user.telefone = editedUser.telefone;
                user.email = editedUser.email;

<<<<<<< HEAD
                if (!(editedUser.utilizadorAD == string.Empty || editedUser.utilizadorAD == null))
                {
                    editedUser.newPassword = "utilizadorAD";
                    editedUser.ConfirmNewPassword = "utilizadorAD";
                    user.utilizadorAD = editedUser.utilizadorAD;
                }
                else
                {
                    user.utilizadorAD = null;
                }

                if (editedUser.ativo && !user.entidade.ativo)
                {
                    errorMessage += "Erro: a entidade seleccionada encontra-se desactivada\n";

                    this.Alerts.Danger(errorMessage);
                    return View(editedUser);
=======
                if (editedUser.ativo && !user.entidade.ativo)
                {
                    ModelState.AddModelError("", "Erro: a entidade seleccionada encontra-se desactivada");
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                }
                else
                {
                    if ((WebSecurity.CurrentUserName == user.UserName) && (editedUser.ativo == false))
                    {
<<<<<<< HEAD
                        errorMessage = "É impossível desativar a própria conta.";
=======
                        ViewBag.errorMessage = "É impossível desativar a própria conta.";
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

                        listaEntidadesToClone = entidadesRepository.All.ToList();
                        //Shallow copy
                        listaEntidades = listaEntidadesToClone.GetRange(0, listaEntidadesToClone.Count);

                        foreach (Entidade e in listaEntidades)
                        {
<<<<<<< HEAD
                            e.nome = "" + e.entidadeId + " - " + e.nome;
=======
                            e.Nome = "" + e.Id + " - " + e.Nome;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                        }
                        this.ViewBag.entidadeId = new SelectList(listaEntidades, "entidadeId", "nome", editedUser.entidadeId);
                        this.ViewBag.userName = usersRepository.Find(editedUser.UserId).UserName;
                        this.ViewBag.ativo = editedUser.ativo;

<<<<<<< HEAD
                        this.Alerts.Danger(errorMessage);
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                        return View(editedUser);
                    }
                }

                if (user.entidadeId != editedUser.entidadeId)
                {
                    user.entidadeId = editedUser.entidadeId;

                    //Update Roles
                    String[] userRoles = Roles.GetRolesForUser(user.UserName);
                    if (userRoles.Length > 0)
                    {
                        Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                    }
<<<<<<< HEAD
                    Entidade newEntity = entidadesRepository.All.Include("role").Single(e => e.entidadeId == editedUser.entidadeId);
                    Roles.AddUserToRole(user.UserName, newEntity.role.RoleName);
                }

                if ((editedUser.newPassword != "") && (editedUser.newPassword != null)
                    || editedUser.utilizadorAD != user.utilizadorAD)
                {
                    string recoveryToken = WebSecurity.GeneratePasswordResetToken(user.UserName, 1);
                    WebSecurity.ResetPassword(recoveryToken, editedUser.newPassword);
                    successMessage += "Password alterada com sucesso\n";
=======
                    Entidade newEntity = entidadesRepository.All.Include("role").Single(e => e.Id == editedUser.entidadeId);
                    Roles.AddUserToRole(user.UserName, newEntity.role.RoleName);
                }

                if ((editedUser.newPassword != "") && (editedUser.newPassword != null))
                {
                    string recoveryToken = WebSecurity.GeneratePasswordResetToken(user.UserName, 1);
                    WebSecurity.ResetPassword(recoveryToken, editedUser.newPassword);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                }

                usersRepository.InsertOrUpdate(user);
                usersRepository.Save();

<<<<<<< HEAD
                successMessage += "Utilizador editado com sucesso\n";
                this.Alerts.Success(successMessage);
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
                return this.RedirectToAction("Index");
            }

            listaEntidadesToClone = entidadesRepository.All.ToList();
            //Shallow copy
            listaEntidades = listaEntidadesToClone.GetRange(0, listaEntidadesToClone.Count);

            foreach (Entidade e in listaEntidades)
            {
<<<<<<< HEAD
                e.nome = "" + e.entidadeId + " - " + e.nome;
=======
                e.Nome = "" + e.Id + " - " + e.Nome;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            }
            this.ViewBag.entidadeId = new SelectList(listaEntidades, "entidadeId", "nome", editedUser.entidadeId);
            this.ViewBag.userName = usersRepository.Find(editedUser.UserId).UserName;
            this.ViewBag.ativo = editedUser.ativo;

            return View(editedUser);
        }


<<<<<<< HEAD
        //#region ExternalLogins
        ////
        //// POST: /Account/Disassociate

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Disassociate(string provider, string providerUserId)
        //{
        //    string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
        //    ManageMessageId? message = null;

        //    // Only disassociate the account if the currently logged in user is the owner
        //    if (ownerAccount == User.Identity.Name)
        //    {
        //        // Use a transaction to prevent the user from deleting their last login credential
        //        using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
        //        {
        //            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //            if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
        //            {
        //                OAuthWebSecurity.DeleteAccount(provider, providerUserId);
        //                scope.Complete();
        //                message = ManageMessageId.RemoveLoginSuccess;
        //            }
        //        }
        //    }

        //    return RedirectToAction("Manage", new { Message = message });
        //}

        ////
        //// POST: /Account/ExternalLogin

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //}

        ////
        //// GET: /Account/ExternalLoginCallback

        //[AllowAnonymous]
        //public ActionResult ExternalLoginCallback(string returnUrl)
        //{
        //    AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //    if (!result.IsSuccessful)
        //    {
        //        return RedirectToAction("ExternalLoginFailure");
        //    }

        //    if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
        //    {
        //        return RedirectToLocal(returnUrl);
        //    }

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        // If the current user is logged in add the new account
        //        OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else
        //    {
        //        // User is new, ask for their desired membership name
        //        string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
        //        ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
        //        ViewBag.ReturnUrl = returnUrl;
        //        return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
        //    }
        //}

        ////
        //// POST: /Account/ExternalLoginConfirmation

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        //{
        //    string provider = null;
        //    string providerUserId = null;

        //    if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
        //    {
        //        return RedirectToAction("Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Insert a new user into the database
        //        using (UsersContext db = new UsersContext())
        //        {
        //            UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
        //            // Check if user already exists
        //            if (user == null)
        //            {
        //                // Insert name into the profile table
        //                db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
        //                db.SaveChanges();

        //                OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
        //                OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

        //                return RedirectToLocal(returnUrl);
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
        //            }
        //        }
        //    }

        //    ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        ////
        //// GET: /Account/ExternalLoginFailure

        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        //[AllowAnonymous]
        //[ChildActionOnly]
        //public ActionResult ExternalLoginsList(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        //}

        //[ChildActionOnly]
        //public ActionResult RemoveExternalLogins()
        //{
        //    ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
        //    List<ExternalLogin> externalLogins = new List<ExternalLogin>();
        //    foreach (OAuthAccount account in accounts)
        //    {
        //        AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

        //        externalLogins.Add(new ExternalLogin
        //        {
        //            Provider = account.Provider,
        //            ProviderDisplayName = clientData.DisplayName,
        //            ProviderUserId = account.ProviderUserId,
        //        });
        //    }

        //    ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //    return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        //}
        //#endregion
=======
        #region ExternalLogins
        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }
        #endregion
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangeSuccess,
            ChangeInsuccess,
            RemoveLoginSuccess
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

    }
}
