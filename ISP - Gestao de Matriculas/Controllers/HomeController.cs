using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Filters;
using WebMatrix.WebData;
using ISP.GestaoMatriculas.Models;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Model.Indicadores;
using ISP.GestaoMatriculas.Contracts;
using System.Data.Entity;

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [MenuDataFilter(Order = 2)]
    public class HomeController : Controller
    {
        private IUserProfileRepository usersRepository;

        public HomeController(IUserProfileRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }


        public ActionResult Index()
        {
           //TODO: Criar uma viewModel para index contendo os indicadores necessários a apresentar.
            if(WebSecurity.IsAuthenticated && WebSecurity.HasUserId){
                UserProfile user = usersRepository.All.Include("entidade").Single(u => u.UserId == WebSecurity.CurrentUserId);

                Entidade entidadeAssociada = user.entidade;
                ViewBag.entidade = entidadeAssociada.nome;

                ViewBag.numIndicadores = entidadeAssociada.indicadores.Count;

                foreach(Indicador i in entidadeAssociada.indicadores){
                    ViewData["Indicador.Descricao"] = i.descricao;
                    ViewData["Indicador.Valor"] = i.calcular().valor;
                }
            }

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();

        }

        [Authorize(Roles = "AdminGroup")]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        

    }
}
