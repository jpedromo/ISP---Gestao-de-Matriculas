using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Everis.Web.Mvc;
using ISP.GestaoMatriculas.ViewModels;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Filters;
using PagedList;
using ISP.GestaoMatriculas.Utils;

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [Authorize(Order = 2)]
    public class RelatoriosController : SKController
    {
        private IApoliceRepository apolicesRepository;
        private IApoliceHistoricoRepository apolicesHistoricoRepository;
        private IEntidadeRepository entidadesRepository;
        private IValorSistemaRepository valoresSistemaRepository;

        public RelatoriosController(IApoliceRepository apolicesRepository, IApoliceHistoricoRepository apolicesHistoricoRepository, IEntidadeRepository entidadesRepository, IValorSistemaRepository valoresSistemaRepository)
        {
            this.apolicesRepository = apolicesRepository;
            this.apolicesHistoricoRepository = apolicesHistoricoRepository;
            this.entidadesRepository = entidadesRepository;
            this.valoresSistemaRepository = valoresSistemaRepository;
        }

        //[Authorize(Roles = "Admin, ISP, ISP-Leitura")]
        //public ActionResult RelatorioIncumprimento()
        //{
        //    RelatorioIncumprimentoViewModel view = new RelatorioIncumprimentoViewModel();

        //    view.dataFim = DateTime.Now;
        //    view.dataInicio = new DateTime(view.dataFim.Year - 1, view.dataFim.Month, view.dataFim.Day);
        //    view.dataInicio = view.dataInicio.Add(view.dataFim.TimeOfDay);

        //    this.ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome");

        //    this.ViewBag.operacao = new SelectList(valoresSistemaRepository.All.Where(v => v.tipologia == "OPERACAO_EVENTO").ToList(), "valorSistemaId", "descricao"  );

        //    return View(view);
        //}

        [Authorize(Roles = "Admin, ISP, ISP-Leitura")]
        public ActionResult RelatorioIncumprimento(RelatorioIncumprimentoViewModel view, string sort, string direction, int? page, string tabNr)
        {
            if (ModelState.IsValid)
            {
                if (tabNr == null || tabNr == "")
                    tabNr = "1";

                view.SetParameters(int.Parse(tabNr), page == null ? 1 : page.Value, sort, direction, Request.Params);


                if (view.dataFim != default(DateTime) && view.dataInicio != default(DateTime))
                {
                    view = getIncumprimentos(view);
                }
                else
                {
                    if (view.dataInicio == default(DateTime))
                        view.dataFim = DateTime.Now;

                    if (view.dataInicio == default(DateTime))
                        view.dataInicio = new DateTime(view.dataFim.Year - 1, view.dataFim.Month, view.dataFim.Day);

                }
            }


            if (view.dataInicio == default(DateTime))
                view.dataFim = DateTime.Now;

            if (view.dataInicio == default(DateTime))
                view.dataInicio = new DateTime(view.dataFim.Year - 1, view.dataFim.Month, view.dataFim.Day);


            if (view.entidadeId > 0)
            {
                this.ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome", view.entidadeId);
            }
            else
            {
                this.ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome");
            }

            this.ViewBag.operacao = new SelectList(valoresSistemaRepository.GetPorTipologia("OPERACAO_EVENTO"), "valorSistemaId", "descricao");


            return this.View(view);
        }

        protected RelatorioIncumprimentoViewModel getIncumprimentos(RelatorioIncumprimentoViewModel view)
        {
            IQueryable<Apolice> apolices = apolicesRepository.All.Include("entidade").Include("eventoHistorico").Include("veiculo").Include("Tomador")
                        .Where(a => a.entidadeId == view.entidadeId);

            if (view.operacao != null)
            {
                apolices = apolices.Where(a => a.eventoHistorico.codigoOperacao.valorSistemaId == view.operacao);
            }
            apolices = apolices.Where(a => a.dataReporte >= view.dataInicio && a.dataReporte <= view.dataFim);

            List<Apolice> apolicesAtivas = apolices.ToList();

            IQueryable<ApoliceHistorico> historico = apolicesHistoricoRepository.All.Include("entidade").Include("eventoHistorico").Include("veiculo").Include("Tomador")
                .Where(h => h.entidadeId == view.entidadeId);

            if (view.operacao != null)
            {
                historico = historico.Where(h => h.eventoHistorico.codigoOperacao.valorSistemaId == view.operacao);
            }

            historico = historico.Where(a => a.dataReporte >= view.dataInicio && a.dataReporte <= view.dataFim);

            List<ApoliceHistorico> apolicesHistorico = historico.ToList();

            List<RegistoIncumprimentoView> incumprimentos = new List<RegistoIncumprimentoView>();

            foreach (Apolice a in apolicesAtivas)
            {
                if (a.SLA > new TimeSpan(0))
                {
                    incumprimentos.Add(new RegistoIncumprimentoView
                    {
                        operacao = a.eventoHistorico.codigoOperacao.valor,
                        numeroApolice = a.numeroApolice,
                        seguradora = a.entidade.nome,
                        matricula = a.veiculo.numeroMatricula,
                        dataInicio = a.dataInicio,
                        dataFim = a.dataFim,
                        SLA = a.SLA,
                        tipo = RegistoIncumprimentoView.TipoApolice.Ativo,
                        registoId = a.apoliceId
                    });
                }
            }

            foreach (ApoliceHistorico h in apolicesHistorico)
            {
                if (h.SLA > new TimeSpan(0))
                {
                    incumprimentos.Add(new RegistoIncumprimentoView
                    {
                        operacao = h.eventoHistorico.codigoOperacao.valor,
                        numeroApolice = h.numeroApolice,
                        seguradora = h.entidade.nome,
                        matricula = h.veiculo.numeroMatricula,
                        dataInicio = h.dataInicio,
                        dataFim = h.dataFim,
                        SLA = h.SLA,
                        tipo = RegistoIncumprimentoView.TipoApolice.Historico,
                        registoId = h.apoliceId
                    });
                }
            }


            view.totalNumberOfRecords = incumprimentos.Count;

            if (view.PageSize == 0)
                view.PageSize = view.totalNumberOfRecords;

            switch (view.SortColumn)
            {
                case "Operacao":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        view.resultado = incumprimentos.OrderByDescending(s => s.operacao);
                    else
                        view.resultado = incumprimentos.OrderBy(s => s.operacao);
                    break;
                case "Apolice":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        view.resultado = incumprimentos.OrderByDescending(s => s.numeroApolice);
                    else
                        view.resultado = incumprimentos.OrderBy(s => s.numeroApolice);
                    break;
                case "Seguradora":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        view.resultado = incumprimentos.OrderByDescending(s => s.seguradora);
                    else
                        view.resultado = incumprimentos.OrderBy(s => s.seguradora);
                    break;
                case "Matricula":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        view.resultado = incumprimentos.OrderByDescending(s => s.matricula);
                    else
                        view.resultado = incumprimentos.OrderBy(s => s.matricula);
                    break;
                case "DataInicio":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        view.resultado = incumprimentos.OrderByDescending(s => s.dataInicio);
                    else
                        view.resultado = incumprimentos.OrderBy(s => s.dataInicio);
                    break;
                case "DataFim":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        view.resultado = incumprimentos.OrderByDescending(s => s.dataFim);
                    else
                        view.resultado = incumprimentos.OrderBy(s => s.dataFim);
                    break;
                case "SLA":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        view.resultado = incumprimentos.OrderByDescending(s => s.SLA);
                    else
                        view.resultado = incumprimentos.OrderBy(s => s.SLA);
                    break;
                default:
                    view.resultado = incumprimentos.OrderByDescending(i => i.dataInicio).ToList();
                    break;

            }

            view.resultado = view.resultado.Skip((view.CurrentPageNumber - 1) * view.PageSize).ToList();
            view.resultado = view.resultado.Take(view.PageSize).ToList();

            return view;
        }

        public ActionResult exportIncumprimentosToCsv(RelatorioIncumprimentoViewModel viewModel, string sort, string direction, string tabNr)
        {
            if (tabNr == null || tabNr == "")
                tabNr = "1";

            viewModel.SetParameters(int.Parse(tabNr), 1, sort, direction, Request.Params);

            viewModel.PageSize = 0;
            RelatorioIncumprimentoViewModel result = getIncumprimentos(viewModel);

            List<RegistoIncumprimentoCsv> listRegistos = new List<RegistoIncumprimentoCsv>();
            foreach(var item in viewModel.resultado)
            {
                listRegistos.Add(new RegistoIncumprimentoCsv(){ matricula = item.matricula,
                                                                numeroApolice = item.numeroApolice,
                                                                operacao = item.operacao,
                                                                seguradora = item.seguradora,
                                                                SLA = item.SLA,
                                                                dataInicio = item.dataInicio, 
                                                                dataFim = item.dataFim});
            }

            CsvExport<RegistoIncumprimentoCsv> csv = new CsvExport<RegistoIncumprimentoCsv>(listRegistos);

            byte[] fileBytes = csv.ExportToBytes();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "RelatorioIncumprimentos_" + DateTime.Now.ToShortDateString() + ".csv");
        }

        //[Authorize(Roles = "Admin, ISP, ISP-Leitura")]
        //public ActionResult RelatorioInatividade()
        //{
        //    RelatorioInatividadeViewModel view = new RelatorioInatividadeViewModel { dataInatividade = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day) };

        //    return View(view);
        //}

        [Authorize(Roles = "Admin, ISP, ISP-Leitura")]
        public ActionResult RelatorioInatividade(RelatorioInatividadeViewModel view, string sort, string direction, int? page, string tabNr)
        {
            if (ModelState.IsValid)
            {
                if (tabNr == null || tabNr == "")
                    tabNr = "1";

                view.SetParameters(int.Parse(tabNr), page == null ? 1 : page.Value, sort, direction, Request.Params);


                if (view.dataInatividade != default(DateTime))
                {
                    view = getInatividades(view);
                }
                else
                {
                    if (view.dataInatividade == default(DateTime))
                        view.dataInatividade = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day);
                }
            }

            if (view.dataInatividade == default(DateTime))
                view.dataInatividade = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day);

            if (view.entidadeId > 0)
            {
                this.ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome", view.entidadeId);
            }
            else
            {
                this.ViewBag.entidadeId = new SelectList(entidadesRepository.All, "entidadeId", "nome");
            }

            return this.View(view);
        }

        protected RelatorioInatividadeViewModel getInatividades(RelatorioInatividadeViewModel view)
        {
            IQueryable<Apolice> apolices = apolicesRepository.All.Include("entidade").Include("eventoHistorico").Include("veiculo").Include("Tomador")
                         .Where(a => a.dataReporte < view.dataInatividade);

            if (view.entidadeId != null)
                apolices = apolices.Where(a => a.entidadeId == view.entidadeId);

            view.totalNumberOfRecords = apolices.Count();

            if (view.PageSize == 0)
                view.PageSize = view.totalNumberOfRecords;

            IPagedList<Apolice> apolicesToView = null;
            switch (view.SortColumn)
            {
                case "Apolice":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesToView = apolices.OrderByDescending(s => s.numeroApolice).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    else
                        apolicesToView = apolices.OrderBy(s => s.numeroApolice).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    break;
                case "Seguradora":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesToView = apolices.OrderByDescending(s => s.entidade.nome).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    else
                        apolicesToView = apolices.OrderBy(s => s.entidade.nome).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    break;
                case "Matricula":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesToView = apolices.OrderByDescending(s => s.veiculo.numeroMatricula).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    else
                        apolicesToView = apolices.OrderBy(s => s.veiculo.numeroMatricula).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    break;
                case "DataInicio":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesToView = apolices.OrderByDescending(s => s.dataInicio).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    else
                        apolicesToView = apolices.OrderBy(s => s.dataInicio).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    break;
                case "DataFim":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesToView = apolices.OrderByDescending(s => s.dataFim).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    else
                        apolicesToView = apolices.OrderBy(s => s.dataFim).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    break;
                case "DataUltimoReporte":
                    if (view.SortDirection == System.ComponentModel.ListSortDirection.Descending)
                        apolicesToView = apolices.OrderByDescending(s => s.dataReporte).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    else
                        apolicesToView = apolices.OrderBy(s => s.dataReporte).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    break;
                default:
                    apolicesToView = apolices.OrderBy(s => s.dataReporte).ToPagedList(view.CurrentPageNumber, view.PageSize);
                    break;
            }
            List<RegistoInatividadeView> registosInativos = new List<RegistoInatividadeView>();

            foreach (Apolice a in apolicesToView)
            {
                RegistoInatividadeView registo = new RegistoInatividadeView
                {
                    dataInicio = a.dataInicio,
                    dataFim = a.dataFim,
                    registoId = a.apoliceId,
                    dataUltimoReporte = a.dataReporte,
                    matricula = a.veiculo.numeroMatricula,
                    numeroApolice = a.numeroApolice,
                    operacao = a.eventoHistorico.codigoOperacao.valor,
                    seguradora = a.entidade.nome
                };

                registosInativos.Add(registo);
            }

            view.resultado = registosInativos;

            return view;
        }

        public ActionResult exportInatividadesToCsv(RelatorioInatividadeViewModel viewModel, string sort, string direction, string tabNr)
        {
            if (tabNr == null || tabNr == "")
                tabNr = "1";

            viewModel.SetParameters(int.Parse(tabNr), 1, sort, direction, Request.Params);

            viewModel.PageSize = 0;
            RelatorioInatividadeViewModel result = getInatividades(viewModel);

            List<RegistoInatividadeToCsv> listRegistos = new List<RegistoInatividadeToCsv>();
            foreach(var item in viewModel.resultado)
            {
                listRegistos.Add(new RegistoInatividadeToCsv(){ matricula = item.matricula,
                                                                numeroApolice = item.numeroApolice,
                                                                operacao = item.operacao,
                                                                seguradora = item.seguradora,
                                                                dataUltimoReporte = item.dataUltimoReporte,
                                                                dataInicio = item.dataInicio, 
                                                                dataFim = item.dataFim});
            }

            CsvExport<RegistoInatividadeToCsv> csv = new CsvExport<RegistoInatividadeToCsv>(listRegistos);

            byte[] fileBytes = csv.ExportToBytes();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "RelatorioIncumprimentos_" + DateTime.Now.ToShortDateString() + ".csv");
        }
    }
}
