using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.Models;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.ViewModels;
using ISP.GestaoMatriculas.Filters;
using System.Data;
using System.Data.Entity;
using WebMatrix.WebData;
using System.Data.Entity.Infrastructure;
using ISP.GestaoMatriculas.Contracts;

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [MenuDataFilter(Order = 2)]
    public class ApolicesController : Controller
    {
        private IApoliceRepository apolicesRepository;
        private IApoliceHistoricoRepository apolicesHistoricoRepository;
        private IUserProfileRepository usersRepository;
        private IEntidadeRepository entidadesRepository;
        private ICategoriaRepository categoriasRepository;
        private IConcelhoRepository concelhosRepository;
        private IVeiculoRepository veiculosRepository;
        private IPessoaRepository pessoasRepository;

        public ApolicesController(IApoliceRepository apolicesRepository, IUserProfileRepository usersRepository, ICategoriaRepository categoriasRepository,
            IConcelhoRepository concelhosRepository, IVeiculoRepository veiculosRepository, IPessoaRepository pessoasRepository,
            IApoliceHistoricoRepository apolicesHistoricoRepository, IEntidadeRepository entidadesRepository)
        {
            this.apolicesRepository = apolicesRepository;
            this.apolicesHistoricoRepository = apolicesHistoricoRepository;
            this.usersRepository = usersRepository;
            this.categoriasRepository = categoriasRepository;
            this.concelhosRepository = concelhosRepository;
            this.veiculosRepository = veiculosRepository;
            this.pessoasRepository = pessoasRepository;
            this.entidadesRepository = entidadesRepository;
        }
        //
        // GET: /Apolices/

        public ActionResult Index(int? entidade, string apolice, string matricula, bool? avisos, bool? apagados, bool? erros)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            Entidade ent = user.entidade;
            int entId = -1;

            if (entidade != null && (int)entidade <= entidadesRepository.All.Count())
            {
                entId = (int)entidade;

                if (entidadesRepository.Find(entId).Nome == "ISP")
                {
                    entId = -1;
                }
            }


            List<Apolice> apolicesToView = null;
            IQueryable<Apolice> query = null;
            if (ent.scope == Entidade.ScopeLevel.Global)
            {
                query = apolicesRepository.All.Include("veiculo").Include("tomador").Include("concelho").Include("entidade");
                if (entId > 0)
                {
                    this.ViewBag.entidade = new SelectList(entidadesRepository.All, "entidadeId", "nome", entidadesRepository.Find(entId).Id);
                    query = query.Where(a => a.EntidadeId == entId);
                }
                else
                {
                    this.ViewBag.entidade = new SelectList(entidadesRepository.All, "entidadeId", "nome");
                }
            }
            else
            {
                //Seguradoras (Acesso Local)
                query = apolicesRepository.All.Include("veiculo").Include("tomador").Include("concelho").Include("entidade").Where(a => a.EntidadeId == ent.Id);
            }

            if (!(apolice == null || apolice == ""))
            {
                query = query.Where(a => a.NumeroApolice.Contains(apolice));
            }
            if (!(matricula == null || matricula == ""))
            {
                query = query.Where(a => a.Veiculo.numeroMatricula.Contains(matricula));
            }
            //TODO: Filtro sobre Avisos

            apolicesToView = query.ToList();

            ApoliceListViewModel viewModel = new ApoliceListViewModel();
            viewModel.apolicesEfetivas = apolicesToView;

            //viewModel.apolice = apolice;
            //viewModel.matricula = matricula;
/*            if (avisos == null)
            {
                viewModel.avisos = false;
            }
            else
            {
                viewModel.avisos = (bool)avisos;
            }
            */

            if(erros != null)
            { viewModel.erros = (bool)erros;}

            return View(viewModel);
        }

        //
        // GET: /Apolices/Details/5

        public ActionResult Details(int id)
        {
            Apolice apolice = null;
            List<ApoliceHistorico> historico = null;

            try
            {
                apolice = apolicesRepository.All.Include("tomador").Include("veiculo").Include("veiculo.categoria").Include("concelho").Single(a => a.Id == id);
                historico = apolicesHistoricoRepository.All.Include("veiculo").Where(h => ((h.EntidadeId == apolice.EntidadeId) && (h.DataInicio == apolice.DataInicio) && (h.Veiculo.numeroMatricula == apolice.Veiculo.numeroMatricula))).ToList();
            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            ApoliceDetailsViewModel viewModel = new ApoliceDetailsViewModel();
            viewModel.apolice = apolice;
            viewModel.historicoApolices = historico;

            return View(viewModel);
        }

        //
        // GET: /Apolices/Create

        public ActionResult Create()
        {
            this.ViewBag.categoriaId = new SelectList(categoriasRepository.All, "categoriaId", "nome");
            this.ViewBag.concelhoId = new SelectList(concelhosRepository.All, "concelhoId", "nomeConcelho");

            DateTime dataInicio = DateTime.Today;
            DateTime dataFim = DateTime.Today.AddMinutes(new TimeSpan(23, 59, 0).TotalMinutes);

            ApoliceCreationViewModel apoliceTemplate = new ApoliceCreationViewModel { apolice = new Apolice { DataInicio = dataInicio, DataFim = dataFim } };

            return View(apoliceTemplate);
        }

        //
        // POST: /Apolices/Create
        //[ActionLogFilter( ParameterName= "apoliceCreation" )]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApoliceCreationViewModel apoliceCreation)
        {
            //TODO: Optimizar para haver apenas a inserção de uma apólice. Tudo o resto deverá ser automático.
            if (ModelState.IsValid)
            {
                apoliceCreation.apolice.ConcelhoCirculacaoId = apoliceCreation.concelhoId;
                apoliceCreation.veiculo.categoriaId = apoliceCreation.categoriaId;

                apoliceCreation.apolice.Tomador = apoliceCreation.tomador;
                apoliceCreation.apolice.Veiculo = apoliceCreation.veiculo;

                UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
                Entidade entidadeAssociada = user.entidade;
                apoliceCreation.apolice.Entidade = entidadeAssociada;

                veiculosRepository.InsertOrUpdate(apoliceCreation.veiculo);
                pessoasRepository.InsertOrUpdate(apoliceCreation.tomador);
                apolicesRepository.InsertOrUpdate(apoliceCreation.apolice);

                apolicesRepository.Save();
                return RedirectToAction("Index");
            }

            this.ViewBag.categoriaId = new SelectList(categoriasRepository.All, "categoriaId", "nome");
            this.ViewBag.concelhoId = new SelectList(concelhosRepository.All, "concelhoId", "nomeConcelho");

            return View(apoliceCreation);
        }

        //
        // GET: /Apolices/Edit/5

        public ActionResult Edit(int id)
        {
            //Apolice apolice = this.db.Apolices.Find(id);
            Apolice apolice = null;
            List<ApoliceHistorico> historico = null;

            try
            {
                apolice = apolicesRepository.All.Include("tomador").Include("veiculo").Include("entidade").Single(a => a.Id == id);
                historico = apolicesHistoricoRepository.All.Include("veiculo").Where(h => ((h.EntidadeId == apolice.EntidadeId) && (h.DataInicio == apolice.DataInicio) && (h.Veiculo.numeroMatricula == apolice.Veiculo.numeroMatricula))).ToList();
            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            ApoliceEditViewModel apoliceView = new ApoliceEditViewModel { apolice = apolice, tomador = apolice.Tomador, veiculo = apolice.Veiculo, historicoApolices = historico};

            this.ViewBag.categoriaId = new SelectList(categoriasRepository.All, "categoriaId", "nome", apoliceView.apolice.Veiculo.categoriaId);
            this.ViewBag.concelhoId = new SelectList(concelhosRepository.All, "concelhoId", "nomeConcelho", apoliceView.apolice.ConcelhoCirculacaoId);

            return View(apoliceView);
        }

        //
        // POST: /Apolices/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApoliceCreationViewModel apoliceView)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);

            if (ModelState.IsValid)
            {
                //Adiciona Registo
                apoliceView.apolice.ConcelhoCirculacaoId = apoliceView.concelhoId;
                apoliceView.veiculo.categoriaId = apoliceView.categoriaId;

                apoliceView.apolice.Tomador = apoliceView.tomador;
                apoliceView.apolice.Veiculo = apoliceView.veiculo;

                veiculosRepository.InsertOrUpdate(apoliceView.veiculo);
                pessoasRepository.InsertOrUpdate(apoliceView.tomador);
                apolicesRepository.InsertOrUpdate(apoliceView.apolice);


                /* Esmaga Registo
                Apolice apolice = apoliceView.apolice;

                apolice.concelhoId = apoliceView.concelhoId;
                apoliceView.veiculo.categoriaId = apoliceView.categoriaId;

                apolice.seguradoId = apoliceView.segurado.pessoaId;
                apolice.veiculoId = apoliceView.veiculo.veiculoId;
                
                apolice.segurado = apoliceView.segurado;
                apolice.veiculo = apoliceView.veiculo;

                this.db.Entry<Apolice>(apolice).State = EntityState.Modified;
                this.db.Entry<Pessoa>(apolice.segurado).State = EntityState.Modified;
                this.db.Entry<Veiculo>(apolice.veiculo).State = EntityState.Modified;
                */

                apolicesRepository.Save();

                return this.RedirectToAction("Index");
            }

            this.ViewBag.categoriaId = new SelectList(categoriasRepository.All, "categoriaId", "nome", apoliceView.apolice.Veiculo.categoriaId);
            this.ViewBag.concelhoId = new SelectList(concelhosRepository.All, "concelhoId", "nomeConcelho", apoliceView.apolice.ConcelhoCirculacaoId);

            return View(apoliceView);
        }

        //
        // GET: /Apolices/Delete/5

        public ActionResult Delete(int id)
        {
            Apolice apolice = null;
            try
            {
                apolice = apolicesRepository.All.Include("tomador").Include("veiculo").Include("entidade").Single(a => a.Id == id);

            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            return this.View(apolice);

        }

        //
        // POST: /Apolices/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {

            Apolice apolice = apolicesRepository.Find(id);
            Veiculo veiculo = veiculosRepository.Find(apolice.VeiculoId);
            Pessoa tomador = pessoasRepository.Find(apolice.TomadorId);

            veiculosRepository.Delete(veiculo.veiculoId);
            pessoasRepository.Delete(tomador.pessoaId);
            apolicesRepository.Delete(apolice.Id);

            apolicesRepository.Save();

            return this.RedirectToAction("Index"); 

        }

        public ActionResult DetalhesHistorico(int id)
        {
            ApoliceHistorico apolice = null;


            //TODO: Review this: Exceptions are expensive!!!
            try
            {
                apolice = apolicesHistoricoRepository.All.Include("tomador").Include("veiculo").Include("veiculo.categoria").Include("concelho").Single(a => a.Id == id);
            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            return this.View();
        }

    }

}
