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
using ISP.GestaoMatriculas.Utils;
using Everis.Web.Mvc;
using PagedList;
using System.Globalization;
using ISP.GestaoMatriculas.Repositories;

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [Authorize(Order = 2)]
    //[MenuDataFilter(Order = 2)]
    public class ApolicesController : SKController
    {
        private IApoliceRepository apolicesRepository;
        private IApoliceHistoricoRepository apolicesHistoricoRepository;
        private IUserProfileRepository usersRepository;
        private IEntidadeRepository entidadesRepository;
        private ICategoriaRepository categoriasRepository;
        private IConcelhoRepository concelhosRepository;
        private IVeiculoRepository veiculosRepository;
        private IPessoaRepository pessoasRepository;
        private IAvisoRepository avisosRepository;
        private IEventoStaggingRepository eventosStaggingRepository;
        private IValorSistemaRepository valoresSistemaRepository;

        public ApolicesController(IApoliceRepository apolicesRepository, IUserProfileRepository usersRepository, ICategoriaRepository categoriasRepository,
            IConcelhoRepository concelhosRepository, IVeiculoRepository veiculosRepository, IPessoaRepository pessoasRepository,
            IApoliceHistoricoRepository apolicesHistoricoRepository, IEntidadeRepository entidadesRepository, IAvisoRepository avisosRepository,
            IEventoStaggingRepository eventosStaggingRepository, IValorSistemaRepository valoresSistemaRepository)
        {
            this.apolicesRepository = apolicesRepository;
            this.apolicesHistoricoRepository = apolicesHistoricoRepository;
            this.usersRepository = usersRepository;
            this.categoriasRepository = categoriasRepository;
            this.concelhosRepository = concelhosRepository;
            this.veiculosRepository = veiculosRepository;
            this.pessoasRepository = pessoasRepository;
            this.entidadesRepository = entidadesRepository;
            this.avisosRepository = avisosRepository;
            this.eventosStaggingRepository = eventosStaggingRepository;
            this.valoresSistemaRepository = valoresSistemaRepository;
        }
        //
        // GET: /Apolices/
        public ActionResult Index( ApoliceListViewModel viewModel, string sort, string direction, int? page, string tabNr)
        {
            if (tabNr == null || tabNr == "")
                tabNr = "1";

            viewModel.SetParameters(int.Parse(tabNr), page == null ? 1 : page.Value, sort, direction, Request.Params);

            if (tabNr != null && (viewModel.TabNumber != int.Parse(tabNr)))
            {
                viewModel.SortColumn = "";
                viewModel.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                viewModel.CurrentPageNumber = 1;
            }

            if(viewModel.dataInicio == default(DateTime))
                viewModel.dataInicio = DateTime.Now.Date.AddMonths(-1);
            if (viewModel.dataFim == default(DateTime))
                viewModel.dataFim = DateTime.Now;

            ApoliceListViewModel result = getApolices(viewModel);
            return View(result);
        }

        public ActionResult exportApolicesToCsv(ApoliceListViewModel viewModel, string sort, string direction, string tabNr)
        {
            if (tabNr == null || tabNr == "")
                tabNr = "1";

            viewModel.SetParameters(int.Parse(tabNr), 1, sort, direction, Request.Params);

            if (tabNr != null && (viewModel.TabNumber != int.Parse(tabNr)))
            {
                viewModel.SortColumn = "";
                viewModel.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                viewModel.CurrentPageNumber = 1;
            }

            viewModel.PageSize = 0;
            ApoliceListViewModel result = getApolices(viewModel);

            List<ApoliceCsvViewModel> apolicesCsv = new List<ApoliceCsvViewModel>();
            foreach(Apolice apo in result.apolicesEfetivas)
                apolicesCsv.Add(new ApoliceCsvViewModel{ entidade = apo.entidade.nome, concelho = apo.concelho.nomeConcelho, matricula = apo.veiculo.numeroMatricula,
                                                            tomador = apo.tomador.nome, dataFim = apo.dataFim.ToString(), dataInicio = apo.dataInicio.ToString()});

            CsvExport<ApoliceCsvViewModel> csv = new CsvExport<ApoliceCsvViewModel>(apolicesCsv);
                        
            byte[] fileBytes = csv.ExportToBytes();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Apolices_" + DateTime.Now.ToShortDateString() + ".csv");

        }

        public ActionResult exportErrosToCsv(ApoliceListViewModel viewModel, string sort, string direction, string tabNr)
        {
            if (tabNr == null || tabNr == "")
                tabNr = "2";

            viewModel.SetParameters(int.Parse(tabNr), 1, sort, direction, Request.Params);

            if (tabNr != null && (viewModel.TabNumber != int.Parse(tabNr)))
            {
                viewModel.SortColumn = "";
                viewModel.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                viewModel.CurrentPageNumber = 1;
            }

            viewModel.PageSize = 0;
            ApoliceListViewModel result = getApolices(viewModel);

            List<EventoCsvViewModel> eventosCsv = new List<EventoCsvViewModel>();
            foreach (EventoStagging apo in result.eventosStagging)
                eventosCsv.Add(new EventoCsvViewModel
                {
                    entidade = apo.entidade.nome,
                    operacao = apo.codigoOperacao,
                    matricula = apo.matricula,
                    tomador = apo.nomeTomadorSeguro,
                    dataFim = apo.dataFimCobertura,
                    dataInicio = apo.dataInicioCobertura
                });

            CsvExport<EventoCsvViewModel> csv = new CsvExport<EventoCsvViewModel>(eventosCsv);

            byte[] fileBytes = csv.ExportToBytes();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Erros_" + DateTime.Now.ToShortDateString() + ".csv");
        }

        public ActionResult exportProcessadosToCsv(ApoliceListViewModel viewModel, string sort, string direction, string tabNr)
        {
            if (tabNr == null || tabNr == "")
                tabNr = "3";

            viewModel.SetParameters(int.Parse(tabNr), 1, sort, direction, Request.Params);

            if (tabNr != null && (viewModel.TabNumber != int.Parse(tabNr)))
            {
                viewModel.SortColumn = "";
                viewModel.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                viewModel.CurrentPageNumber = 1;
            }

            viewModel.PageSize = 0;
            ApoliceListViewModel result = getApolices(viewModel);

            List<EventoProcessadoCsvViewModel> eventosCsv = new List<EventoProcessadoCsvViewModel>();
            foreach (EventoStagging apo in result.eventosProcessados)
                eventosCsv.Add(new EventoProcessadoCsvViewModel
                {
                    entidade = apo.entidade.nome,
                    estado = apo.estadoEvento.ToString(),
                    operacao = apo.codigoOperacao,
                    matricula = apo.matricula,
                    tomador = apo.nomeTomadorSeguro,
                    dataFim = apo.dataFimCobertura,
                    dataInicio = apo.dataInicioCobertura
                });

            CsvExport<EventoProcessadoCsvViewModel> csv = new CsvExport<EventoProcessadoCsvViewModel>(eventosCsv);

            byte[] fileBytes = csv.ExportToBytes();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Erros_" + DateTime.Now.ToShortDateString() + ".csv");
        }

        protected ApoliceListViewModel getApolices(ApoliceListViewModel viewModel)
        {            
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            Entidade ent = user.entidade;
            int entId = -1;


                if (viewModel.entidade != null && viewModel.entidade != 0)
                {
                    entId = (int)viewModel.entidade;

                    if (entidadesRepository.Find(entId).nome == "ISP")
                    {
                        entId = -1;
                    }
                }


            IPagedList<Apolice> apolicesToView = null;
            IPagedList<EventoStagging> eventosStaggingToView = null;
            IPagedList<EventoStagging> eventosStaggingProcessadosToView = null;
            IQueryable<Apolice> queryApolices = null;
            IQueryable<EventoStagging> queryEventosStagging = null;
            IQueryable<EventoStagging> queryEventosStaggingProcessados = null;

            #region Apolices
            if (viewModel.TabNumber == 1)
            {
                if (ent.scope.valor == "GLOBAL")
                {
                    queryApolices = apolicesRepository.All.Include("veiculo").Include("tomador").Include("concelho").Include("entidade");
                    queryEventosStagging = eventosStaggingRepository.All.Include("entidade");
                    if (entId > 0)
                    {
                        this.ViewBag.entidade = new SelectList(entidadesRepository.All, "entidadeId", "nome", entidadesRepository.Find(entId).entidadeId);
                        queryApolices = queryApolices.Where(a => a.entidadeId == entId);
                        queryEventosStagging = queryEventosStagging.Where(e => e.entidadeId == entId);
                    }
                    else
                    {
                        this.ViewBag.entidade = new SelectList(entidadesRepository.All, "entidadeId", "nome");
                    }
                }
                else
                {
                    //Seguradoras (Acesso Local)
                    queryApolices = apolicesRepository.All.Include("veiculo").Include("tomador").Include("concelho").Include("entidade").Where(a => a.entidadeId == ent.entidadeId);
                    queryEventosStagging = eventosStaggingRepository.All.Include("entidade").Where(e => e.entidadeId == ent.entidadeId);
                }

                if (!(viewModel.apolice == null || viewModel.apolice == ""))
                {
                    queryApolices = queryApolices.Where(a => a.numeroApolice.Contains(viewModel.apolice));
                    queryEventosStagging = queryEventosStagging.Where(e => e.nrApolice.Contains(viewModel.apolice));
                }
                if (!(viewModel.matricula == null || viewModel.matricula == ""))
                {
                    queryApolices = queryApolices.Where(a => a.veiculo.numeroMatricula.Contains(viewModel.matricula) || a.veiculo.numeroMatriculaCorrigido.Contains(viewModel.matricula));
                    queryEventosStagging = queryEventosStagging.Where(e => e.matricula.Contains(viewModel.matricula) || e.matriculaCorrigida.Contains(viewModel.matricula));
                }
                if (viewModel.avisos != null && viewModel.avisos == true)
                {
                    queryApolices = queryApolices.Where(a => a.avisos == true);
                }
                if (viewModel.arquivados != null && viewModel.arquivados == true)
                {
                    queryEventosStagging = queryEventosStagging.Where(e => e.arquivado == true);
                }
                else
                {
                    queryEventosStagging = queryEventosStagging.Where(e => e.arquivado == false);
                }
                if (viewModel.dataInicio != null && viewModel.dataFim != null && viewModel.dataInicio <= viewModel.dataFim)
                {
                    queryApolices = queryApolices.Where(a => ((viewModel.dataInicio >= a.dataInicio && viewModel.dataInicio <= a.dataFim) ||
                                                        (viewModel.dataFim >= a.dataInicio && viewModel.dataFim <= a.dataFim) ||
                                                        (viewModel.dataInicio <= a.dataInicio && viewModel.dataFim >= a.dataFim)));
                    queryEventosStagging = queryEventosStagging.Where(a => a.dataReporte >= viewModel.dataInicio && a.dataReporte <= viewModel.dataFim);
                }

                queryEventosStagging = queryEventosStagging.Where(e => e.estadoEvento.valor == "ERRO");

                viewModel.totalNumberOfApolices = queryApolices.Count();
                viewModel.totalNumberOfEventos = queryEventosStagging.Count();

                if (viewModel.PageSize == 0)
                    viewModel.PageSize = viewModel.totalNumberOfApolices;

                switch (viewModel.SortColumn)
                {
                    case "Apolices_Entidade":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            apolicesToView = queryApolices.OrderByDescending(s => s.entidade.nome).ToPagedList(viewModel.CurrentPageNumber , viewModel.PageSize);
                        else
                            apolicesToView = queryApolices.OrderBy(s => s.entidade.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;

                    case "Apolices_Apolice":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            apolicesToView = queryApolices.OrderByDescending(s => s.numeroApolice).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            apolicesToView = queryApolices.OrderBy(s => s.numeroApolice).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Apolices_DataInicio":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            apolicesToView = queryApolices.OrderByDescending(s => s.dataInicio).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            apolicesToView = queryApolices.OrderBy(s => s.dataInicio).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;

                    case "Apolices_DataFim":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            apolicesToView = queryApolices.OrderByDescending(s => s.dataFim).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            apolicesToView = queryApolices.OrderBy(s => s.dataFim).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Apolices_Matricula":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            apolicesToView = queryApolices.OrderByDescending(s => s.veiculo.numeroMatricula).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            apolicesToView = queryApolices.OrderBy(s => s.veiculo.numeroMatricula).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;

                    case "Apolices_Nome":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            apolicesToView = queryApolices.OrderByDescending(s => s.tomador.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            apolicesToView = queryApolices.OrderBy(s => s.tomador.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Apolices_Concelho":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            apolicesToView = queryApolices.OrderByDescending(s => s.concelho.nomeConcelho).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            apolicesToView = queryApolices.OrderBy(s => s.concelho.nomeConcelho).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;

                    default:
                        apolicesToView = queryApolices.OrderByDescending(a => a.dataReporte).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                }

                viewModel.apolicesEfetivas = apolicesToView.ToList();
            }
            #endregion
            
            #region Erros
            if (viewModel.TabNumber == 2)
            {
                if (ent.scope.valor == "GLOBAL")
                {
                    queryEventosStagging = eventosStaggingRepository.All.Include("entidade");
                    if (entId > 0)
                    {
                        this.ViewBag.entidade = new SelectList(entidadesRepository.All, "entidadeId", "nome", entidadesRepository.Find(entId).entidadeId);
                        queryEventosStagging = queryEventosStagging.Where(e => e.entidadeId == entId);
                    }
                    else
                    {
                        this.ViewBag.entidade = new SelectList(entidadesRepository.All, "entidadeId", "nome");
                    }
                }
                else
                {
                    //Seguradoras (Acesso Local)
                    queryEventosStagging = eventosStaggingRepository.All.Include("entidade").Where(e => e.entidadeId == ent.entidadeId);
                }

                if (!(viewModel.apolice == null || viewModel.apolice == ""))
                {
                    queryEventosStagging = queryEventosStagging.Where(e => e.nrApolice.Contains(viewModel.apolice));
                }
                if (!(viewModel.matricula == null || viewModel.matricula == ""))
                {
                    queryEventosStagging = queryEventosStagging.Where(e => e.matricula.Contains(viewModel.matricula) || e.matriculaCorrigida.Contains(viewModel.matricula));
                }
                
                if (viewModel.arquivados != null && viewModel.arquivados == true)
                {
                    queryEventosStagging = queryEventosStagging.Where(e => e.arquivado == true);
                }
                else
                {
                    queryEventosStagging = queryEventosStagging.Where(e => e.arquivado == false);
                }
                if (viewModel.dataInicio != null && viewModel.dataFim != null && viewModel.dataInicio <= viewModel.dataFim)
                {
                    queryEventosStagging = queryEventosStagging.Where(a => a.dataReporte >= viewModel.dataInicio && a.dataReporte <= viewModel.dataFim);
                }

                queryEventosStagging = queryEventosStagging.Where(e => e.estadoEvento.valor == "ERRO");

                viewModel.totalNumberOfEventos = queryEventosStagging.Count();

                switch (viewModel.SortColumn)
                {
                   
                    case "Eventos_Entidade":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingToView = queryEventosStagging.OrderByDescending(s => s.entidade.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingToView = queryEventosStagging.OrderBy(s => s.entidade.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_Estado":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingToView = queryEventosStagging.OrderByDescending(s => s.estadoEvento).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingToView = queryEventosStagging.OrderBy(s => s.estadoEvento).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_Operacao":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingToView = queryEventosStagging.OrderByDescending(s => s.codigoOperacao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingToView = queryEventosStagging.OrderBy(s => s.codigoOperacao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_Apolice":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingToView = queryEventosStagging.OrderByDescending(s => s.nrApolice).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingToView = queryEventosStagging.OrderBy(s => s.nrApolice).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_DataInicio":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingToView = queryEventosStagging.OrderByDescending(s => s.dataInicioCobertura).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingToView = queryEventosStagging.OrderBy(s => s.dataInicioCobertura).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_DataFim":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingToView = queryEventosStagging.OrderByDescending(s => s.dataFimCobertura).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingToView = queryEventosStagging.OrderBy(s => s.dataFimCobertura).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_Matricula":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingToView = queryEventosStagging.OrderByDescending(s => s.matricula).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingToView = queryEventosStagging.OrderBy(s => s.matricula).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_Tomador":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingToView = queryEventosStagging.OrderByDescending(s => s.nomeTomadorSeguro).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingToView = queryEventosStagging.OrderBy(s => s.nomeTomadorSeguro).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;


                    default:
                        eventosStaggingToView = queryEventosStagging.OrderByDescending(e => e.dataUltimaAlteracaoErro).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                }

                viewModel.eventosStagging = eventosStaggingToView.ToList();
            }

            #endregion

            #region Processados
            if (viewModel.TabNumber == 3)
            {
                if (ent.scope.valor == "GLOBAL")
                {
                    queryEventosStaggingProcessados = eventosStaggingRepository.All.Include("entidade");
                    if (entId > 0)
                    {
                        this.ViewBag.entidade = new SelectList(entidadesRepository.All, "entidadeId", "nome", entidadesRepository.Find(entId).entidadeId);
                    }
                    else
                    {
                        this.ViewBag.entidade = new SelectList(entidadesRepository.All, "entidadeId", "nome");
                    }
                }
                else
                {
                    //Seguradoras (Acesso Local)
                    queryEventosStaggingProcessados = eventosStaggingRepository.All.Include("entidade").Where(e => e.entidadeId == ent.entidadeId);
                }

                queryEventosStaggingProcessados = queryEventosStaggingProcessados.Where(e => e.estadoEvento.valor != "ERRO");

                viewModel.totalNumberOfEventos = queryEventosStaggingProcessados.Count();

                switch (viewModel.SortColumn)
                {
                    case "Eventos_Entidade":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderByDescending(s => s.entidade.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderBy(s => s.entidade.nome).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_Estado":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderByDescending(s => s.estadoEvento.descricao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderBy(s => s.estadoEvento.descricao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_Operacao":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderByDescending(s => s.codigoOperacao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderBy(s => s.codigoOperacao).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_Apolice":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderByDescending(s => s.nrApolice).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderBy(s => s.nrApolice).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_DataInicio":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderByDescending(s => s.dataInicioCobertura).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderBy(s => s.dataInicioCobertura).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_DataFim":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderByDescending(s => s.dataFimCobertura).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderBy(s => s.dataFimCobertura).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_Matricula":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderByDescending(s => s.matricula).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderBy(s => s.matricula).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                    case "Eventos_Tomador":
                        if (viewModel.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderByDescending(s => s.nomeTomadorSeguro).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        else
                            eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderBy(s => s.nomeTomadorSeguro).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;

                    default:
                        eventosStaggingProcessadosToView = queryEventosStaggingProcessados.OrderByDescending(e => e.dataUltimaAlteracaoErro).ToPagedList(viewModel.CurrentPageNumber, viewModel.PageSize);
                        break;
                }
                viewModel.eventosProcessados = eventosStaggingProcessadosToView.ToList();
            }

            #endregion

            return viewModel;
        }

        //
        // GET: /Apolices/Details/5

        public ActionResult Details(int id)
        {
            Apolice apolice = null;
            List<ApoliceHistorico> historico = null;
            
            try
            {
                apolice = apolicesRepository.All.Include("tomador").Include("veiculo").Include("veiculo.categoria").Include("concelho").
                    Include("avisosApolice").Include("eventoHistorico").Single(a => a.apoliceId == id);



                UserProfile utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);
                if (utilizador.entidade.entidadeId != apolice.entidadeId && utilizador.entidade.nome != "ISP")
                {
                    return this.HttpNotFound();
                }

                historico = apolicesHistoricoRepository.All.Include("veiculo").Where(h => ((h.entidadeId == apolice.entidadeId) && (h.dataInicio == apolice.dataInicio) && (h.veiculo.numeroMatricula == apolice.veiculo.numeroMatricula)))
                    .OrderByDescending(h => h.dataArquivo).ToList();
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
            viewModel.historicoApolices = historico.OrderByDescending(a => a.dataReporte);

            return View(viewModel);
        }

        //
        // GET: /Apolices/Create
        [Authorize(Roles = "Admin, ISP, Seguradora")]
        public ActionResult Create()
        {
            this.ViewBag.categoriaId = new SelectList(categoriasRepository.All.OrderBy(c => c.nome), "categoriaId", "nome");
            this.ViewBag.concelhoId = new SelectList(concelhosRepository.All.OrderBy(c => c.nomeConcelho), "concelhoId", "nomeConcelho");

            DateTime dataInicio = DateTime.Today;
            DateTime dataFim = DateTime.Today.AddMinutes(new TimeSpan(23, 59, 0).TotalMinutes);

            ApoliceCreationViewModel apoliceTemplate = new ApoliceCreationViewModel { apolice = new Apolice {dataInicio = dataInicio, dataFim = dataFim } };

            return View(apoliceTemplate);
        }

        //
        // POST: /Apolices/Create
        //[ActionLogFilter( ParameterName= "apoliceCreation" )]
        [Authorize(Roles = "Admin, ISP, Seguradora")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApoliceCreationViewModel apoliceCreation)
        {
            //List<ValorSistema> queryValSistema = valoresSistemaRepository.All.Where(v => v.tipologia == "OPERACAO_EVENTO" || 
            //                                    v.tipologia == "ESTADO_EVENTO_STAGGING" ||
            //                                    v.tipologia == "PARAM_HORA_LIMITE_SLA" ||
            //                                    v.tipologia == "PARAM_HORA_EXTENSAO_SLA").ToList();

            //List<ValorSistema> operacoesEvento = queryValSistema.Where(v => v.tipologia == "OPERACAO_EVENTO").ToList();
            //List<ValorSistema> estadosEvento = queryValSistema.Where(v => v.tipologia == "ESTADO_EVENTO_STAGGING").ToList();
            //int horaLimiteSLA = int.Parse(queryValSistema.Where(v => v.tipologia == "PARAM_HORA_LIMITE_SLA").Single().valor);
            //int horaExtensaoSLA = int.Parse(queryValSistema.Where(v => v.tipologia == "PARAM_HORA_EXTENSAO_SLA").Single().valor);

            List<ValorSistema> operacoesEvento = valoresSistemaRepository.GetPorTipologia("OPERACAO_EVENTO");
            List<ValorSistema> estadosEvento = valoresSistemaRepository.GetPorTipologia("ESTADO_EVENTO_STAGGING");
            int horaLimiteSLA = int.Parse(valoresSistemaRepository.GetPorTipologia("PARAM_HORA_LIMITE_SLA").Single().valor);
            int horaExtensaoSLA = int.Parse(valoresSistemaRepository.GetPorTipologia("PARAM_HORA_EXTENSAO_SLA").Single().valor);

            string errosMessage = string.Empty;
            string avisosMessage = string.Empty;

            //TODO: Optimizar para haver apenas a inserção de uma apólice. Tudo o resto deverá ser automático.
            if (ModelState.IsValid)
            {
                apoliceCreation.apolice.concelhoCirculacaoId = apoliceCreation.concelhoId;
                apoliceCreation.veiculo.categoriaId = apoliceCreation.categoriaId;
                apoliceCreation.apolice.concelhoCirculacaoId = apoliceCreation.concelhoId;
                apoliceCreation.veiculo.categoriaId = apoliceCreation.categoriaId;

                apoliceCreation.apolice.tomador = apoliceCreation.tomador;
                apoliceCreation.apolice.veiculo = apoliceCreation.veiculo;

                UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
                Entidade entidadeAssociada = user.entidade;
                apoliceCreation.apolice.entidade = entidadeAssociada;

                ValorSistema codigoOperacao = operacoesEvento.Where(o => o.valor == "C").Single();

                apoliceCreation.apolice.eventoHistorico = new EventoHistorico { entidadeId = user.entidadeId, codigoOperacao = codigoOperacao, codigoOperacaoId = codigoOperacao.valorSistemaId, idOcorrencia = string.Empty };
                apoliceCreation.apolice.dataReporte = DateTime.Now;
                EventoStagging candidatoApolice = new EventoStagging(apoliceCreation.apolice, estadosEvento.Where(e=> e.valor == "PENDENTE").Single().valorSistemaId);

                if (!ValidacaoEventos.validarEvento(candidatoApolice))
                {

                    foreach (ErroEventoStagging erro in candidatoApolice.errosEventoStagging)
                    {
                        errosMessage += erro.descricao + '\n';
                    }
                    foreach (Aviso aviso in candidatoApolice.avisosEventoStagging)
                    {
                        avisosMessage += aviso.descricao + '\n';
                    }
                    this.Alerts.Danger(errosMessage);
                    this.Alerts.Warning(avisosMessage);

                    this.ViewBag.categoriaId = new SelectList(categoriasRepository.All.OrderBy(c => c.nome), "categoriaId", "nome");
                    this.ViewBag.concelhoId = new SelectList(concelhosRepository.All.OrderBy(c =>c.nomeConcelho) , "concelhoId", "nomeConcelho");

                    return View(apoliceCreation);
                }

                foreach (Aviso aviso in candidatoApolice.avisosEventoStagging)
                {
                    avisosMessage += aviso.descricao + '\n';
                }


                Apolice apoliceValidada = new Apolice(candidatoApolice, apoliceCreation.concelhoId, apoliceCreation.categoriaId, apoliceCreation.apolice.eventoHistorico.codigoOperacaoId, horaLimiteSLA, horaExtensaoSLA);
                apoliceValidada.utilizadorReporte = WebSecurity.CurrentUserName;
/*                veiculosRepository.InsertOrUpdate(apoliceCreation.veiculo);
                pessoasRepository.InsertOrUpdate(apoliceCreation.tomador);*/

                /*List<Apolice> apoliceAnterior = apolicesRepository.All.Where(a => a.dataInicio == apoliceValidada.dataInicio &&
                                            a.entidadeId == apoliceValidada.entidadeId &&
                                            a.veiculo.numeroMatricula == apoliceValidada.veiculo.numeroMatricula).ToList();
                */

                apolicesRepository.InsertOrUpdate(apoliceValidada);

                if (avisosMessage != string.Empty)
                {
                    this.Alerts.Warning(avisosMessage);
                }
                this.Alerts.Success("Registo criado com sucesso.");

                List<EventoStagging> errosDesbloquear = eventosStaggingRepository.All.Where(e => e.entidadeId == candidatoApolice.entidadeId && e.dataInicioCobertura == candidatoApolice.dataInicioCobertura &&
                        e.horaInicioCobertura == candidatoApolice.horaInicioCobertura && e.matricula == candidatoApolice.matricula && e.estadoEvento.valor == "ERRO"
                        && e.codigoOperacao == "M" && e.arquivado == false).OrderBy(e => e.dataUltimaAlteracaoErro).ToList();

                foreach (EventoStagging evento in errosDesbloquear)
                {
                    evento.arquivado = true;
                    eventosStaggingRepository.InsertOrUpdate(evento);
                    eventosStaggingRepository.Save();
                }

                apolicesRepository.Save();
                return RedirectToAction("Index");
            }


            this.ViewBag.categoriaId = new SelectList(categoriasRepository.All.OrderBy(c => c.nome), "categoriaId", "nome");
            this.ViewBag.concelhoId = new SelectList(concelhosRepository.All.OrderBy(c=> c.nomeConcelho), "concelhoId", "nomeConcelho");

            this.Alerts.Danger("Erro na criação de registo.");
            return View(apoliceCreation);
        }

        //
        // GET: /Apolices/Edit/5
        [Authorize(Roles = "Admin, ISP, Seguradora")]
        public ActionResult Edit(int id)
        {
            //Apolice apolice = this.db.Apolices.Find(id);
            Apolice apolice = null;
            List<ApoliceHistorico> historico = null;

            try
            {
                apolice = apolicesRepository.All.Include("tomador").Include("veiculo").Include("entidade").
                    Include("avisosApolice").Include("eventoHistorico").Single(a => a.apoliceId == id);

                UserProfile utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);
                if (utilizador.entidade.entidadeId != apolice.entidadeId && utilizador.entidade.nome != "ISP")
                {
                    return this.HttpNotFound();
                }

                historico = apolicesHistoricoRepository.All.Include("veiculo").Where(h => ((h.entidadeId == apolice.entidadeId) && (h.dataInicio == apolice.dataInicio) && (h.veiculo.numeroMatricula == apolice.veiculo.numeroMatricula))).ToList();
            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            ApoliceEditViewModel apoliceView = new ApoliceEditViewModel { apolice = apolice, tomador = apolice.tomador, veiculo = apolice.veiculo, historicoApolices = historico};

            this.ViewBag.categoriaId = new SelectList(categoriasRepository.All, "categoriaId", "nome", apoliceView.apolice.veiculo.categoriaId);
            this.ViewBag.concelhoId = new SelectList(concelhosRepository.All, "concelhoId", "nomeConcelho", apoliceView.apolice.concelhoCirculacaoId);

            return View(apoliceView);
        }

        //
        // POST: /Apolices/Edit/5
        [Authorize(Roles = "Admin, ISP, Seguradora")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApoliceCreationViewModel apoliceView)
        {
            //List<ValorSistema> queryValSistema = valoresSistemaRepository.All.Where(v => v.tipologia == "OPERACAO_EVENTO" ||
            //                                    v.tipologia == "ESTADO_EVENTO_STAGGING" ||
            //                                    v.tipologia == "PARAM_HORA_LIMITE_SLA" ||
            //                                    v.tipologia == "PARAM_HORA_EXTENSAO_SLA").ToList();

            //List<ValorSistema> operacoesEvento = queryValSistema.Where(v => v.tipologia == "OPERACAO_EVENTO").ToList();
            //List<ValorSistema> estadosEvento = queryValSistema.Where(v => v.tipologia == "ESTADO_EVENTO_STAGGING").ToList();
            //int horaLimiteSLA = int.Parse(queryValSistema.Where(v => v.tipologia == "PARAM_HORA_LIMITE_SLA").Single().valor);
            //int horaExtensaoSLA = int.Parse(queryValSistema.Where(v => v.tipologia == "PARAM_HORA_EXTENSAO_SLA").Single().valor);

            
            List<ValorSistema> operacoesEvento = valoresSistemaRepository.GetPorTipologia("OPERACAO_EVENTO");
            List<ValorSistema> estadosEvento = valoresSistemaRepository.GetPorTipologia("ESTADO_EVENTO_STAGGING");
            int horaLimiteSLA = int.Parse(valoresSistemaRepository.GetPorTipologia("PARAM_HORA_LIMITE_SLA").Single().valor);
            int horaExtensaoSLA = int.Parse(valoresSistemaRepository.GetPorTipologia("PARAM_HORA_EXTENSAO_SLA").Single().valor);

            Apolice apolice = null;
            UserProfile utilizador = null;
            string errosMessage = string.Empty;
            string avisosMessage = string.Empty;

            try
            {
                apolice = apolicesRepository.All.Include("tomador").Include("veiculo").Include("entidade").
                    Include("avisosApolice").Include("eventoHistorico").Single(a => a.apoliceId == apoliceView.apolice.apoliceId);

                utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);
                if (utilizador.entidade.entidadeId != apolice.entidadeId && utilizador.entidade.nome != "ISP")
                {
                    return this.HttpNotFound();
                }
            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                //Adiciona Registo
                apoliceView.apolice.concelhoCirculacaoId = apoliceView.concelhoId;
                apoliceView.veiculo.categoriaId = apoliceView.categoriaId;
                apoliceView.apolice.concelhoCirculacaoId = apoliceView.concelhoId;
                apoliceView.veiculo.categoriaId = apoliceView.categoriaId;

                apoliceView.apolice.tomador = apoliceView.tomador;
                apoliceView.apolice.veiculo = apoliceView.veiculo;

                //Entidade entidadeAssociada = utilizador.entidade;
                apoliceView.apolice.entidade = apolice.entidade;

                apoliceView.apolice.dataFimPlaneada = apolicesRepository.Find(apoliceView.apolice.apoliceId).dataFimPlaneada;

                ValorSistema codigoOperacao = codigoOperacao = operacoesEvento.Where(o => o.valor == "M").Single();

                apoliceView.apolice.eventoHistorico = new EventoHistorico { entidadeId = utilizador.entidadeId, codigoOperacao = codigoOperacao, codigoOperacaoId = codigoOperacao.valorSistemaId, idOcorrencia = string.Empty };
                apoliceView.apolice.dataReporte = DateTime.Now;
                EventoStagging candidatoApolice = new EventoStagging(apoliceView.apolice, estadosEvento.Where(e => e.valor == "PENDENTE").Single().valorSistemaId);

                if (!ValidacaoEventos.validarEvento(candidatoApolice))
                {
                    foreach (ErroEventoStagging erro in candidatoApolice.errosEventoStagging)
                    {
                        errosMessage += erro.descricao + '\n';
                    }
                    foreach (Aviso aviso in candidatoApolice.avisosEventoStagging)
                    {
                        avisosMessage += aviso.descricao + '\n';
                    }
                    this.Alerts.Danger(errosMessage);

                    if (avisosMessage != string.Empty)
                    {
                        this.Alerts.Warning(avisosMessage);
                    }

                    List<ApoliceHistorico> historico = null;

                    try
                    {
                        apolice = apolicesRepository.Find(apoliceView.apolice.apoliceId);
                        historico = apolicesHistoricoRepository.All.Include("veiculo").Where(h => ((h.entidadeId == apolice.entidadeId) && (h.dataInicio == apolice.dataInicio) && (h.veiculo.numeroMatricula == apolice.veiculo.numeroMatricula))).ToList();
                    }
                    catch (System.ArgumentNullException)
                    {
                        return this.HttpNotFound();
                    }
                    catch (System.InvalidOperationException)
                    {
                        return this.HttpNotFound();
                    }

                    ApoliceEditViewModel apoliceEditView = new ApoliceEditViewModel { apolice = apoliceView.apolice, tomador = apoliceView.tomador, veiculo = apoliceView.veiculo, historicoApolices = historico };

                    this.ViewBag.categoriaId = new SelectList(categoriasRepository.All, "categoriaId", "nome", apoliceView.categoriaId);
                    this.ViewBag.concelhoId = new SelectList(concelhosRepository.All, "concelhoId", "nomeConcelho", apoliceView.concelhoId);

                    return View(apoliceEditView);
                }

                foreach (Aviso aviso in candidatoApolice.avisosEventoStagging)
                {
                    avisosMessage += aviso.descricao + '\n';
                }

                Apolice apoliceValidada = new Apolice(candidatoApolice, apoliceView.concelhoId, apoliceView.categoriaId, apoliceView.apolice.eventoHistorico.codigoOperacaoId, horaLimiteSLA, horaExtensaoSLA);
                apoliceValidada.utilizadorReporte = WebSecurity.CurrentUserName;

                List<Apolice> apoliceAnterior = apolicesRepository.All.Include("avisosApolice").Where(a => a.dataInicio == apoliceValidada.dataInicio &&
                                            a.entidadeId == apoliceValidada.entidadeId &&
                                            a.veiculo.numeroMatricula == apoliceValidada.veiculo.numeroMatricula).ToList();

                foreach (Apolice a in apoliceAnterior)
                {
                    int avisosNum = a.avisosApolice.Count;

                    foreach (Aviso aviso in a.avisosApolice)
                    {
                        avisosRepository.Delete(aviso.avisoId);
                    }
                    //a.avisosApolice.Clear();
                    
                    ApoliceHistorico historico = new ApoliceHistorico(a);
                    historico.utilizadorArquivo = WebSecurity.CurrentUserName;

                    apolicesHistoricoRepository.InsertOrUpdate(historico);
                                        
                    apolicesHistoricoRepository.Save();

                    int idApoliceHistorico = historico.apoliceId;
                    a.avisosApolice.ForEach(aviso => aviso.apoliceHistoricoId = idApoliceHistorico);

                    apolicesRepository.Delete(a.apoliceId);
                }

                apolicesRepository.InsertOrUpdate(apoliceValidada);

                //veiculosRepository.InsertOrUpdate(apoliceView.veiculo);
                //pessoasRepository.InsertOrUpdate(apoliceView.tomador);
                //apolicesRepository.InsertOrUpdate(apoliceView.apolice);

                apolicesRepository.Save();
                //apolicesHistoricoRepository.Save();

                if (avisosMessage != string.Empty)
                {
                    this.Alerts.Warning(avisosMessage);
                }
                this.Alerts.Success("Registo editado com sucesso.");
                return this.RedirectToAction("Index");
            }

            this.ViewBag.categoriaId = new SelectList(categoriasRepository.All, "categoriaId", "nome", apoliceView.apolice.veiculo.categoriaId);
            this.ViewBag.concelhoId = new SelectList(concelhosRepository.All, "concelhoId", "nomeConcelho", apoliceView.apolice.concelhoCirculacaoId);

            this.Alerts.Danger("Erro na criação de registo.");
            return View(apoliceView);
        }

        ////
        //// GET: /Apolices/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    Apolice apolice = null;
        //    try
        //    {
        //        apolice = apolicesRepository.All.Include("tomador").Include("veiculo").Include("entidade").Single(a => a.apoliceId == id);

        //    }
        //    catch (System.ArgumentNullException)
        //    {
        //        return this.HttpNotFound();
        //    }
        //    catch (System.InvalidOperationException)
        //    {
        //        return this.HttpNotFound();
        //    }

        //    return this.View(apolice);

        //}

        ////
        //// POST: /Apolices/Delete/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, FormCollection collection)
        //{

        //    Apolice apolice = apolicesRepository.Find(id);
        //    Veiculo veiculo = veiculosRepository.Find(apolice.veiculoId);
        //    Pessoa tomador = pessoasRepository.Find(apolice.tomadorId);

        //    veiculosRepository.Delete(veiculo.veiculoId);
        //    pessoasRepository.Delete(tomador.pessoaId);
        //    apolicesRepository.Delete(apolice.apoliceId);

        //    apolicesRepository.Save();

        //    return this.RedirectToAction("Index"); 

        //}

        public ActionResult DetalhesHistorico(int id)
        {
            ApoliceHistorico apolice = null;
            UserProfile utilizador = null;

            try
            {
                apolice = apolicesHistoricoRepository.All.Include("tomador").Include("veiculo").Include("veiculo.categoria").Include("concelho").
                    Include("eventoHistorico").Single(a => a.apoliceId == id);

                utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);
                if (utilizador.entidade.entidadeId != apolice.entidadeId && utilizador.entidade.nome != "ISP")
                {
                    return this.HttpNotFound();
                }
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

        [Authorize(Roles = "Admin, ISP, Seguradora")]
        public ActionResult Anular(int id)
        {
            Apolice apolice = null;
            UserProfile utilizador = null;
            List<ApoliceHistorico> historico = null;

            try
            {
                apolice = apolicesRepository.All.Include("tomador").Include("veiculo").Include("entidade").
                    Include("avisosApolice").Single(a => a.apoliceId == id);

                utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);
                if (utilizador.entidade.entidadeId != apolice.entidadeId && utilizador.entidade.nome != "ISP")
                {
                    return this.HttpNotFound();
                }

                historico = apolicesHistoricoRepository.All.Include("veiculo").Where(h => ((h.entidadeId == apolice.entidadeId) && (h.dataInicio == apolice.dataInicio) && (h.veiculo.numeroMatricula == apolice.veiculo.numeroMatricula))).ToList();
            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            ApoliceCancelViewModel cancelView = new ApoliceCancelViewModel { apolice = apolice, historicoApolices = historico };

            return View(cancelView);

        }


        [Authorize(Roles = "Admin, ISP, Seguradora")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Anular(ApoliceCancelViewModel cancelView)
        {
            //List<ValorSistema> queryValSistema = valoresSistemaRepository.All.Where(v => v.tipologia == "OPERACAO_EVENTO" ||
            //                                    v.tipologia == "ESTADO_EVENTO_STAGGING" ||
            //                                    v.tipologia == "PARAM_HORA_LIMITE_SLA" ||
            //                                    v.tipologia == "PARAM_HORA_EXTENSAO_SLA").ToList();

            //List<ValorSistema> operacoesEvento = queryValSistema.Where(v => v.tipologia == "OPERACAO_EVENTO").ToList();
            //List<ValorSistema> estadosEvento = queryValSistema.Where(v => v.tipologia == "ESTADO_EVENTO_STAGGING").ToList();
            //int horaLimiteSLA = int.Parse(queryValSistema.Where(v => v.tipologia == "PARAM_HORA_LIMITE_SLA").Single().valor);
            //int horaExtensaoSLA = int.Parse(queryValSistema.Where(v => v.tipologia == "PARAM_HORA_EXTENSAO_SLA").Single().valor);
            
            
            List<ValorSistema> operacoesEvento = valoresSistemaRepository.GetPorTipologia("OPERACAO_EVENTO");
            List<ValorSistema> estadosEvento = valoresSistemaRepository.GetPorTipologia("ESTADO_EVENTO_STAGGING");
            int horaLimiteSLA = int.Parse(valoresSistemaRepository.GetPorTipologia("PARAM_HORA_LIMITE_SLA").Single().valor);
            int horaExtensaoSLA = int.Parse(valoresSistemaRepository.GetPorTipologia("PARAM_HORA_EXTENSAO_SLA").Single().valor);

            UserProfile utilizador = null;
            Apolice apolice = null;
            //TODO: Verificar que o utilizador tem acesso ao recurso.

            try
            {
                apolice = apolicesRepository.All.Single(a => a.apoliceId == cancelView.apolice.apoliceId);

                utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);
                if (utilizador.entidade.entidadeId != apolice.entidadeId && utilizador.entidade.nome != "ISP")
                {
                    return this.HttpNotFound();
                }
            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }



            string errosString = string.Empty;

            if (ModelState.IsValid)
            {
                //Adiciona Registo
                Apolice apoliceProducao = apolicesRepository.Find(cancelView.apolice.apoliceId);

                EventoHistorico eventoProducao = apoliceProducao.eventoHistorico;

                ValorSistema codigoOperacao = operacoesEvento.Where(o => o.valor == "A").Single();

                apoliceProducao.eventoHistorico = new EventoHistorico { entidadeId = apoliceProducao.entidadeId, codigoOperacao = codigoOperacao, codigoOperacaoId = codigoOperacao.valorSistemaId, idOcorrencia = string.Empty };
                EventoStagging candidatoApolice = new EventoStagging(apoliceProducao, estadosEvento.Where(e => e.valor == "PENDENTE").Single().valorSistemaId);
                candidatoApolice.dataReporte = DateTime.Now;
                apoliceProducao.eventoHistorico = eventoProducao;

                candidatoApolice.dataFimCobertura = cancelView.apolice.dataFim.Date.ToString();
                candidatoApolice.horaFimCobertura = cancelView.apolice.dataFim.TimeOfDay.ToString();

                if (!ValidacaoEventos.validarEvento(candidatoApolice))
                {
                    //TODO: Tratamento de erros (Visualização em página)
                    foreach (ErroEventoStagging erro in candidatoApolice.errosEventoStagging)
                    {
                        errosString += erro.descricao + '\n';
                    }

                    this.Alerts.Danger(errosString);
                    return View(cancelView);
                }

                Apolice apoliceValidada = new Apolice(candidatoApolice, apoliceProducao.concelhoCirculacaoId, apoliceProducao.veiculo.categoriaId, codigoOperacao.valorSistemaId, horaLimiteSLA, horaExtensaoSLA);
                apoliceValidada.utilizadorReporte = WebSecurity.CurrentUserName;

                List<Apolice> apoliceAnterior = apolicesRepository.All.Include("avisosApolice").Include("eventoHistorico").Include("veiculo").Where(a => a.dataInicio == apoliceValidada.dataInicio &&
                                            a.entidadeId == apoliceValidada.entidadeId &&
                                            a.veiculo.numeroMatricula == apoliceValidada.veiculo.numeroMatricula).ToList();

                foreach (Apolice a in apoliceAnterior)
                {
                    int avisosNum = a.avisosApolice.Count;

                    foreach (Aviso aviso in a.avisosApolice)
                    {
                        avisosRepository.Delete(aviso.avisoId);
                    }
                    //a.avisosApolice.Clear();
                    
                    ApoliceHistorico historico = new ApoliceHistorico(a);
                    historico.utilizadorArquivo = WebSecurity.CurrentUserName;
                    historico.dataArquivo = DateTime.Now;

                    apolicesHistoricoRepository.InsertOrUpdate(historico);
                    apolicesHistoricoRepository.Save();

                    int idApoliceHistorico = historico.apoliceId;
                    a.avisosApolice.ForEach(aviso => aviso.apoliceHistoricoId = idApoliceHistorico);

                    apolicesRepository.Delete(a.apoliceId);
                }

                apolicesRepository.InsertOrUpdate(apoliceValidada);

                apolicesRepository.Save();

                this.Alerts.Success("Registo anulado com sucesso.");

                return this.RedirectToAction("Index");
            }

            return View(cancelView);
        }


        public ActionResult DetailsStagging(int id)
        {

            EventoStagging eventoStagging = null;
            UserProfile utilizador = null;
            List<EventoStagging> historicoErro = null;

            try
            {
                eventoStagging = eventosStaggingRepository.All.Include("avisosEventoStagging").Include("errosEventoStagging").Single(e => e.eventoStaggingId == id);

                utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);
                if (utilizador.entidade.entidadeId != eventoStagging.entidadeId && utilizador.entidade.nome != "ISP")
                {
                    return this.HttpNotFound();
                }

                historicoErro = eventosStaggingRepository.All.Where(h => ((h.entidadeId == eventoStagging.entidadeId) && (h.dataInicioCobertura == eventoStagging.dataInicioCobertura) &&
                    (h.horaInicioCobertura == eventoStagging.horaInicioCobertura) && (h.matricula == eventoStagging.matricula) && h.eventoStaggingId != id)
                    && h.estadoEvento.valor == "ERRO").OrderByDescending(h => h.dataUltimaAlteracaoErro).ToList();

            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            StaggingDetailsViewModel viewModel = new StaggingDetailsViewModel();
            viewModel.eventoStagging = eventoStagging;
            viewModel.historicoStagging = historicoErro;

            return View(viewModel);
        }


        [Authorize(Roles = "Admin, ISP, Seguradora")]
        public ActionResult ArquivarStagging(int id)
        {
            EventoStagging eventoStagging = null;
            UserProfile utilizador = null;
            List<EventoStagging> historicoErro = null;



            try
            {
                eventoStagging = eventosStaggingRepository.All.Include("avisosEventoStagging").Include("errosEventoStagging").Single(e => e.eventoStaggingId == id);

                utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);
                if (utilizador.entidade.entidadeId != eventoStagging.entidadeId && utilizador.entidade.nome != "ISP")
                {
                    return this.HttpNotFound();
                }

                historicoErro = eventosStaggingRepository.All.Where(h => ((h.entidadeId == eventoStagging.entidadeId) && (h.dataInicioCobertura == eventoStagging.dataInicioCobertura) &&
                    (h.horaInicioCobertura == eventoStagging.horaInicioCobertura) && (h.matricula == eventoStagging.matricula) && h.eventoStaggingId != id)
                    && h.estadoEvento.valor == "ERRO").OrderByDescending(h => h.dataUltimaAlteracaoErro).ToList();

            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            StaggingDetailsViewModel viewModel = new StaggingDetailsViewModel();
            viewModel.eventoStagging = eventoStagging;
            viewModel.historicoStagging = historicoErro;

            return View(viewModel);

        }

        //
        // POST:
        [Authorize(Roles = "Admin, ISP, Seguradora")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArquivarStagging(int id, FormCollection collection)
        {
            UserProfile utilizador = null;
            //TODO: Verificar que o utilizador tem acesso ao recurso.

            EventoStagging eventoArquivar = null;

            try{
                eventoArquivar = eventosStaggingRepository.Find(id);

                    utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);
                    if (utilizador.entidade.entidadeId != eventoArquivar.entidadeId && utilizador.entidade.nome != "ISP")
                    {
                        return this.HttpNotFound();
                    }

            }
                catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException){
                return this.HttpNotFound();
            }
            if (eventoArquivar == null)
            {
                return this.HttpNotFound();
            }

            eventoArquivar.dataArquivo = DateTime.Now;
            eventoArquivar.utilizadorArquivo = WebSecurity.CurrentUserName;
            eventoArquivar.arquivado = true;

            eventosStaggingRepository.InsertOrUpdate(eventoArquivar);
            eventosStaggingRepository.Save();

            this.Alerts.Success("Registo arquivado com sucesso.");
            return this.RedirectToAction("Index", new { erros = true });
       }


        [Authorize(Roles = "Admin, ISP, Seguradora")]
        public ActionResult EditStagging(int id)
        {
            EventoStagging eventoStagging = null;
            UserProfile utilizador = null;
            List<EventoStagging> historicoErro = null;

            try
            {
                eventoStagging = eventosStaggingRepository.All.Include("avisosEventoStagging").Include("errosEventoStagging").Single(e => e.eventoStaggingId == id);

                utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);
                if (utilizador.entidade.entidadeId != eventoStagging.entidadeId && utilizador.entidade.nome != "ISP")
                {
                    return this.HttpNotFound();
                }

                historicoErro = eventosStaggingRepository.All.Where(h => ((h.entidadeId == eventoStagging.entidadeId) && (h.dataInicioCobertura == eventoStagging.dataInicioCobertura) &&
                    (h.horaInicioCobertura == eventoStagging.horaInicioCobertura) && (h.matricula == eventoStagging.matricula) && h.eventoStaggingId != id)
                    && h.estadoEvento.valor == "ERRO").OrderByDescending(h => h.dataUltimaAlteracaoErro).ToList();

            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            StaggingEditViewModel viewModel = new StaggingEditViewModel();
            viewModel.eventoStagging = eventoStagging;
            viewModel.historicoStagging = historicoErro;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, ISP, Seguradora")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStagging(StaggingEditViewModel staggingView)
        {            
            string errosMessage = string.Empty;
            string avisosMessage = string.Empty;

            UserProfile utilizador = null;
            //TODO: Verificar que o utilizador tem acesso ao recurso.

            EventoStagging eventoAnterior = eventosStaggingRepository.Find(staggingView.eventoStagging.eventoStaggingId);
            if (eventoAnterior == null)
            {
                return this.HttpNotFound();
            }

            try {

                utilizador = usersRepository.GetUserByUsernameIncludeEntidade(WebSecurity.CurrentUserName);

                if (utilizador.entidade.entidadeId != eventoAnterior.entidadeId && utilizador.entidade.nome != "ISP")
                {
                    return this.HttpNotFound();
                }
            }
            catch (System.ArgumentNullException)
            {
                return this.HttpNotFound();
            }
            catch (System.InvalidOperationException)
            {
                return this.HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                EventoStagging eventoEditado = staggingView.eventoStagging;

                eventoEditado.codigoOperacao = eventoAnterior.codigoOperacao;
                eventoEditado.entidadeId = eventoAnterior.entidadeId;

                eventoEditado.dataUltimaAlteracaoErro = DateTime.Now;
                EventoStagging eventoProcessar = EventoStaggingFactory.duplicarEventoProducao(eventoEditado.matricula, (int)eventoAnterior.entidadeId, eventoEditado.dataInicioCobertura, eventoEditado.horaInicioCobertura);
                eventoProcessar.esmagaDados(eventoEditado);

                if (!ValidacaoEventos.validarEvento(eventoProcessar))
                {
                    foreach (ErroEventoStagging erro in eventoProcessar.errosEventoStagging)
                    {
                        errosMessage += erro.descricao + '\n';
                    }
                    foreach (Aviso aviso in eventoProcessar.avisosEventoStagging)
                    {
                        avisosMessage += aviso.descricao + '\n';
                    }
                    this.Alerts.Danger(errosMessage);

                    if (avisosMessage != string.Empty)
                    {
                        this.Alerts.Warning(avisosMessage);
                    }

                    return View(staggingView);
                }

                foreach (Aviso aviso in eventoProcessar.avisosEventoStagging)
                {
                    avisosMessage += aviso.descricao + '\n';
                }

                int? concelhoId = null;
                int? categoriaId = null;
              
                if (eventoProcessar.codigoConcelhoCirculacao != null)
                {
                    Concelho con = concelhosRepository.All.Where(c => c.codigoConcelho == eventoProcessar.codigoConcelhoCirculacao).FirstOrDefault();
                    concelhoId = con == null ? (int?)null : con.concelhoId;
                }

                if (eventoProcessar.codigoCategoriaVeiculo != null)
                {
                    Categoria cat = categoriasRepository.All.Where(c => c.codigoCategoriaVeiculo == eventoProcessar.codigoCategoriaVeiculo).FirstOrDefault();
                    categoriaId = cat == null? (int?)null : cat.categoriaId;
                }
                
                //List<ValorSistema> queryValSistema = valoresSistemaRepository.All.Where(v => v.tipologia == "OPERACAO_EVENTO" ||
                //                                v.tipologia == "PARAM_HORA_LIMITE_SLA" ||
                //                                v.tipologia == "PARAM_HORA_EXTENSAO_SLA").ToList();

                //int operacaoId = queryValSistema.Where(v => v.tipologia == "OPERACAO_EVENTO" && v.valor == eventoProcessar.codigoOperacao).Single().valorSistemaId;
                //int horaLimiteSLA = int.Parse(queryValSistema.Where(v => v.tipologia == "PARAM_HORA_LIMITE_SLA").Single().valor);
                //int horaExtensaoSLA = int.Parse(queryValSistema.Where(v => v.tipologia == "PARAM_HORA_EXTENSAO_SLA").Single().valor);
                                
                int operacaoId = valoresSistemaRepository.GetPorTipologia("OPERACAO_EVENTO").Where(v => v.valor == eventoProcessar.codigoOperacao).Single().valorSistemaId;
                int horaLimiteSLA = int.Parse(valoresSistemaRepository.GetPorTipologia("PARAM_HORA_LIMITE_SLA").Single().valor);
                int horaExtensaoSLA = int.Parse(valoresSistemaRepository.GetPorTipologia("PARAM_HORA_EXTENSAO_SLA").Single().valor);

                eventoAnterior.dataCorrecaoErro = DateTime.Now;
                eventoAnterior.utilizadorCorrecao = WebSecurity.CurrentUserName;
                eventoAnterior.utilizadorArquivo = eventoAnterior.utilizadorCorrecao;
                eventoAnterior.dataArquivo = eventoAnterior.dataCorrecaoErro;
                eventoAnterior.arquivado = true;

                eventosStaggingRepository.InsertOrUpdate(eventoAnterior);
                eventosStaggingRepository.Save();
                
                Apolice apoliceValidada = new Apolice(eventoProcessar, concelhoId, categoriaId, operacaoId, horaLimiteSLA, horaExtensaoSLA);
                apoliceValidada.utilizadorReporte = WebSecurity.CurrentUserName;

                List<Apolice> apoliceAnterior = apolicesRepository.All.Include("avisosApolice").Where(a => a.dataInicio == apoliceValidada.dataInicio &&
                                            a.entidadeId == apoliceValidada.entidadeId &&
                                            a.veiculo.numeroMatricula == apoliceValidada.veiculo.numeroMatricula).ToList();

                foreach (Apolice a in apoliceAnterior)
                {
                    int avisosNum = a.avisosApolice.Count;

                    foreach (Aviso aviso in a.avisosApolice)
                    {
                        avisosRepository.Delete(aviso.avisoId);
                    }
                    //a.avisosApolice.Clear();

                    ApoliceHistorico historico = new ApoliceHistorico(a);
                    historico.dataArquivo = DateTime.Now;
                    historico.utilizadorArquivo = WebSecurity.CurrentUserName;

                    apolicesHistoricoRepository.InsertOrUpdate(historico);
                    apolicesHistoricoRepository.Save();

                    int idApoliceHistorico = historico.apoliceId;
                    a.avisosApolice.ForEach(aviso => aviso.apoliceHistoricoId = idApoliceHistorico);

                    apolicesRepository.Delete(a.apoliceId);
                }

                apolicesRepository.InsertOrUpdate(apoliceValidada);
                apolicesRepository.Save();

                if (avisosMessage != string.Empty)
                {
                    this.Alerts.Warning(avisosMessage);
                }
                this.Alerts.Success("Registo editado com sucesso.");

                return this.RedirectToAction("Index", new { erros = true });
            }

            this.Alerts.Danger("Erro na edição do registo.");
            return View(staggingView);
        }

        

    }

}
