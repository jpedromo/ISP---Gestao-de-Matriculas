using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Filters;
using WebMatrix.WebData;
using ISP.GestaoMatriculas.Models;
using ISP.GestaoMatriculas.Model;
<<<<<<< HEAD
using ISP.GestaoMatriculas.Contracts;
using System.Data.Entity;
using ISP.GestaoMatriculas.ViewModels;
=======
using ISP.GestaoMatriculas.Model.Indicadores;
using ISP.GestaoMatriculas.Contracts;
using System.Data.Entity;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
<<<<<<< HEAD
    //[MenuDataFilter(Order = 2)]
    public class HomeController : Controller
    {
        private IUserProfileRepository usersRepository;
        private IIndicadorRepository indicadoresRepository;
        private IApoliceRepository apolicesRepository;


        public HomeController(IUserProfileRepository usersRepository, IIndicadorRepository indicadoresRepository, IApoliceRepository apolicesRepository)
        {
            this.usersRepository = usersRepository;
            this.indicadoresRepository = indicadoresRepository;
            this.apolicesRepository = apolicesRepository;
=======
    [MenuDataFilter(Order = 2)]
    public class HomeController : Controller
    {
        private IUserProfileRepository usersRepository;

        public HomeController(IUserProfileRepository usersRepository)
        {
            this.usersRepository = usersRepository;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }


        public ActionResult Index()
        {
<<<<<<< HEAD
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            if (user == null)
                return Redirect("~/SitePublico/Index");     

            Entidade ent = user.entidade;
            int? entidadeId = null;
            DateTime data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime primeiraData;
            if(DateTime.Now.Month >= 6)
                primeiraData = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 5, DateTime.Now.Day);
            else
                primeiraData = new DateTime(DateTime.Now.Year-1, 12 + (DateTime.Now.Month - 5), DateTime.Now.Day);


            double nrApolices;
            List<Indicador> indicadores;
            if (ent.nome != "ISP")
            {
                entidadeId = user.entidadeId;
                indicadores = indicadoresRepository.All.Where(i => i.entidadeId == entidadeId && i.dataIndicador >= primeiraData).ToList();
                nrApolices = apolicesRepository.All.Where(i => i.entidadeId == entidadeId).Count();
            }
            else
            {
                indicadores = indicadoresRepository.All.Where(i => i.dataIndicador >= primeiraData).ToList();
                nrApolices = apolicesRepository.All.Count();
            }

            string codCriacao = "C";
            double nrCriacoes = 0;
            List<Indicador> indicadoresCriacoes = indicadores.Where(i => i.tipologia.valor == "NR_EVENTOS_OPERACAO" && i.subTipo == codCriacao && i.dataIndicador.Month == data.Month).ToList();
            if(indicadoresCriacoes != null && indicadoresCriacoes.Count() > 0)
                nrCriacoes = indicadoresCriacoes.Sum(ind => ind.valor);

            double nrErros = 0;
            List<Indicador> indicadoresErros = indicadores.Where(i => i.tipologia.valor == "NR_ERROS_PENDENTES" && i.dataIndicador == data).ToList();
            if (indicadoresErros != null && indicadoresErros.Count() > 0)
                nrErros = indicadoresErros.Sum(ind => ind.valor);
            

            List<double> listaErros = new List<double> { 0,0,0,0,0,0 };
            var erros = indicadores.Where(i => i.tipologia.valor == "NR_ERROS_EVENTOS").GroupBy(gr => gr.dataIndicador.Month).Select(g => new { Mes = g.Key, Sum = g.Sum(x => x.valor) });
            foreach (var dt in erros)
            {
                int index = (data.Month < dt.Mes) ? dt.Mes - primeiraData.Month : 5 - (data.Month - dt.Mes);

                listaErros[index] = dt.Sum;
            }
                       
            List<double> listaAvisos = new List<double> { 0,0,0,0,0,0};
            var avisos = indicadores.Where(i => i.tipologia.valor == "NR_AVISOS_PERIODO_SEGURO").GroupBy(gr => gr.dataIndicador.Month).Select(g => new { Mes = g.Key, Sum = g.Sum(x => x.valor) });
            foreach (var dt in avisos)
            {
                int index = (data.Month < dt.Mes) ? dt.Mes - primeiraData.Month : 5 - (data.Month - dt.Mes);
                
                listaAvisos[index] = dt.Sum;
            }

            List<double> listaEventos = new List<double> { 0, 0, 0, 0, 0, 0 };
            var eventos = indicadores.Where(i => i.tipologia.valor == "NR_EVENTOS").GroupBy(gr => gr.dataIndicador.Month).Select(g => new { Mes = g.Key, Sum = g.Sum(x => x.valor) });
            foreach (var dt in eventos)
            {
                int index = (data.Month < dt.Mes) ? dt.Mes - primeiraData.Month : 5 - (data.Month - dt.Mes);

                listaEventos[index] = dt.Sum;
            }

            List<double> listaForaSLA = new List<double> { 0, 0, 0, 0, 0, 0 };
            var foraSLA = indicadores.Where(i => i.tipologia.valor == "NR_OPERACOES_FORA_SLA").GroupBy(gr => gr.dataIndicador.Month).Select(g => new { Mes = g.Key, Sum = g.Sum(x => x.valor) });
            foreach (var dt in foraSLA)
            {
                int index = (data.Month < dt.Mes) ? dt.Mes - primeiraData.Month : 5 - (data.Month - dt.Mes);

                listaForaSLA[index] = dt.Sum;
            }

            List<string> listaDatas = new List<string>();
            string ano, mes;
            for (int i = 0; i < 6; i++)
            {
                ano = primeiraData.AddMonths(i).Year.ToString();
                mes = primeiraData.AddMonths(i).ToString("MMM");
                listaDatas.Add(mes + " de " + ano);
            }

            HomeViewModel homeVM = new HomeViewModel
            {
                nrApolices = nrApolices,
                nrErros = nrErros,
                nrNovasApolices = nrCriacoes,
                listaAvisos = listaAvisos,
                listaErros = listaErros,
                listaEventos = listaEventos,
                listaForaSLA = listaForaSLA,
                listaDatas = listaDatas
            };

            return View(homeVM);

=======
           //TODO: Criar uma viewModel para index contendo os indicadores necessários a apresentar.
            if(WebSecurity.IsAuthenticated && WebSecurity.HasUserId){
                UserProfile user = usersRepository.All.Include("entidade").Single(u => u.UserId == WebSecurity.CurrentUserId);

                Entidade entidadeAssociada = user.entidade;
                ViewBag.entidade = entidadeAssociada.Nome;

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
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }

        public ActionResult Error()
        {
            return View();
        }

        

    }
}
