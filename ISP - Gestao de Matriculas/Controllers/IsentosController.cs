using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Everis.Web.Mvc;
using System.Data;
using System.Data.Entity;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.ViewModels;
using ISP.GestaoMatriculas.Filters;
using ISP.GestaoMatriculas.Contracts;
using WebMatrix.WebData;
using PagedList;
using ISP.GestaoMatriculas.Utils;

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [Authorize(Order = 2)]
    public class IsentosController : SKController
    {
        private IApoliceIsentoRepository apoliceIsentosRepository;
        private IUserProfileRepository usersRepository;
        public IsentosController(IApoliceIsentoRepository apoliceIsentosRepository, IUserProfileRepository usersRepository)
        {
            this.apoliceIsentosRepository = apoliceIsentosRepository;
            this.usersRepository = usersRepository;
        }
        //
        // GET: /Isentos/

        public ActionResult Index(IsentosListViewModel viewModel, string sort, string direction, int? page)
        {
            viewModel.SetParameters(1, page == null ? 1 : page.Value, sort, direction, Request.Params);

            IsentosListViewModel result = getIsentos(viewModel);
            return View(result);
        }

        public ActionResult exportIsentosToCsv(IsentosListViewModel viewModel, string sort, string direction)
        {
            viewModel.PageSize = 0;
            viewModel.SetParameters(1, 1, sort, direction, Request.Params);

            IsentosListViewModel result = getIsentos(viewModel);

            List<ApoliceIsentoToCsv> isentosCsv = new List<ApoliceIsentoToCsv>();
            foreach (ApoliceIsento apo in result.apolicesIsentos)
                isentosCsv.Add(new ApoliceIsentoToCsv
                {
                    entidade = apo.entidade.nome,
                    matricula = apo.matricula,
                    dataFim = apo.dataFim.ToString(),
                    dataInicio = apo.dataInicio.ToString(),
                    confidencial = apo.confidencial ? "Sim" : "Não",
                    dataModificacao = apo.dataModificacao.ToString()
                });

            CsvExport<ApoliceIsentoToCsv> csv = new CsvExport<ApoliceIsentoToCsv>(isentosCsv);

            byte[] fileBytes = csv.ExportToBytes();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Isentos_" + DateTime.Now.ToShortDateString() + ".csv");
        }

        protected IsentosListViewModel getIsentos(IsentosListViewModel viewModel)
        {
            IPagedList<ApoliceIsento> apolicesIsentosToView = null;
            IQueryable<ApoliceIsento> apoliceIsentos = apoliceIsentosRepository.All;

            if (!(viewModel.entidadeResponsavel == null || viewModel.entidadeResponsavel == string.Empty))
            {
                apoliceIsentos = apoliceIsentos.Where(a => a.entidadeResponsavel.Contains(viewModel.entidadeResponsavel));
            }
            if (!(viewModel.matricula == null || viewModel.matricula == string.Empty))
            {
                apoliceIsentos = apoliceIsentos.Where(a => a.matricula.Contains(viewModel.matricula) || a.matriculaCorrigida.Contains(viewModel.matricula));
            }
            if (viewModel.arquivados != null && viewModel.arquivados == true)
            {
                apoliceIsentos = apoliceIsentos.Where(a => a.arquivo == true);
            }
            else
            {
                apoliceIsentos = apoliceIsentos.Where(a => a.arquivo == false);
            }


            viewModel.totalNumberOfIsentos = apoliceIsentos.Count();

            if (viewModel.PageSize == 0)
                viewModel.PageSize = viewModel.totalNumberOfIsentos;

            switch (viewModel.SortColumn)
            {
                case "Entidade":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesIsentosToView = apoliceIsentos.OrderByDescending(s => s.entidade.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        apolicesIsentosToView = apoliceIsentos.OrderBy(s => s.entidade.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                case "Matricula":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesIsentosToView = apoliceIsentos.OrderByDescending(s => s.matricula).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        apolicesIsentosToView = apoliceIsentos.OrderBy(s => s.matricula).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                case "DataInicio":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesIsentosToView = apoliceIsentos.OrderByDescending(s => s.dataInicio).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        apolicesIsentosToView = apoliceIsentos.OrderBy(s => s.dataInicio).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                case "DataFim":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesIsentosToView = apoliceIsentos.OrderByDescending(s => s.dataFim).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        apolicesIsentosToView = apoliceIsentos.OrderBy(s => s.dataFim).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                case "Confidencial":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesIsentosToView = apoliceIsentos.OrderByDescending(s => s.confidencial).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        apolicesIsentosToView = apoliceIsentos.OrderBy(s => s.confidencial).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                case "DataModificacao":
                    if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesIsentosToView = apoliceIsentos.OrderByDescending(s => s.dataModificacao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    else
                        apolicesIsentosToView = apoliceIsentos.OrderBy(s => s.dataModificacao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
                default:
                    apolicesIsentosToView = apoliceIsentos.OrderByDescending(s => s.dataModificacao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                    break;
            }
            
            viewModel.apolicesIsentos = apolicesIsentosToView.ToList();

            return viewModel;
        }

        
        //
        // GET: /Isentos/Details/5
        [Authorize(Roles = "Admin, ISP, ISP-Leitura")]
        public ActionResult Details(int id)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            //TODO: Verificar que o utilizador tem acesso ao recurso.

            ApoliceIsento apolice = apoliceIsentosRepository.Find(id);

            if (apolice == null)
            {
                return this.HttpNotFound();
            }

            return View(apolice);
        }

        //
        // GET: /Isentos/Create
        [Authorize(Roles = "Admin, ISP")]
        public ActionResult Create()
        {
            ApoliceIsento view = new ApoliceIsento();
            view.dataInicio = DateTime.Now;

            return View(view);
        }

        //
        // POST: /Isentos/Create
        [Authorize(Roles = "Admin, ISP")]
        [HttpPost]
        public ActionResult Create(ApoliceIsento apolice)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);

            if (ModelState.IsValid)
            {
                apolice.dataReporte = DateTime.Now;
                apolice.dataCriacao = DateTime.Now;
                apolice.dataModificacao = DateTime.Now;
                apolice.entidadeId = user.entidadeId;

                if (apolice.matricula != null)
                {
                    apolice.matriculaCorrigida = apolice.matricula.Replace("-", "");
                }

                apoliceIsentosRepository.InsertOrUpdate(apolice);
                apoliceIsentosRepository.Save();

                this.Alerts.Success("Período Isento criado com sucesso.");
                return RedirectToAction("Index");
            }

            this.Alerts.Danger("Erro na criação de Período Isento.");
            return this.View(apolice);
        }

        //
        // GET: /Isentos/Edit/5
        [Authorize(Roles = "Admin, ISP")]
        public ActionResult Edit(int id)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            //TODO: Verificar acesso ao recurso.

            ApoliceIsento apolice = apoliceIsentosRepository.Find(id);

            if (apolice == null)
            {
                return this.HttpNotFound();
            }

            return View(apolice);
        }

        //
        // POST: /Isentos/Edit/5
        [Authorize(Roles = "Admin, ISP")]
        [HttpPost]
        public ActionResult Edit(ApoliceIsento editedApolice)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            //TODO: Verificar acesso ao recurso.
            ApoliceIsento apolice = apoliceIsentosRepository.Find(editedApolice.apoliceIsentoId);

            if (apolice == null)
            {
                return this.HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                editedApolice.apoliceIsentoId = default(int);
                editedApolice.entidadeId = apolice.entidadeId;
                editedApolice.dataCriacao = apolice.dataCriacao;
                editedApolice.dataReporte = apolice.dataReporte;
                editedApolice.dataModificacao = DateTime.Now;
                editedApolice.arquivo = false;

                if (editedApolice.matricula != null)
                {
                    editedApolice.matriculaCorrigida = editedApolice.matricula.Replace("-", "");
                }

                apolice.arquivo = true;

                apoliceIsentosRepository.InsertOrUpdate(editedApolice);
                apoliceIsentosRepository.InsertOrUpdate(apolice);

                apoliceIsentosRepository.Save();

                this.Alerts.Success("Período Isento editado com sucesso.");
                return RedirectToAction("Index");
            }

                this.Alerts.Danger("Erro na edição de Período Isento.");
            return this.View(editedApolice);
        }

        //
        // GET: /Isentos/Delete/5
        [Authorize(Roles = "Admin, ISP")]
        public ActionResult Arquivar(int id)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            //TODO: Verificar acesso ao recurso.

            ApoliceIsento apolice = apoliceIsentosRepository.Find(id);

            if (apolice == null)
            {
                return this.HttpNotFound();
            }

            return View(apolice);
        }

        //
        // POST: /Isentos/Delete/5
        [Authorize(Roles = "Admin, ISP")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Arquivar(int id, FormCollection collection)
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            Entidade entidadeAssociada = user.entidade;
            //TODO: Verificar que o utilizador tem acesso ao recurso.

            ApoliceIsento apoliceArquivar = null;


            apoliceArquivar = apoliceIsentosRepository.Find(id);


            if (apoliceArquivar == null)
            {
                return this.HttpNotFound();
            }

            apoliceArquivar.arquivo = true;

            apoliceIsentosRepository.InsertOrUpdate(apoliceArquivar);
            apoliceIsentosRepository.Save();

            this.Alerts.Success("Registo arquivado com sucesso.");
            return this.RedirectToAction("Index");
        }
    }
}
