using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP.GestaoMatriculas.ViewModels;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Filters;
using ISP.GestaoMatriculas.Model;
using WebMatrix.WebData;

namespace ISP.GestaoMatriculas.Controllers
{
    [InitializeSimpleMembership(Order = 1)]
    [Authorize(Order = 2)]
    public class IndicadoresController : Controller
    {

        private IIndicadorRepository indicadoresRepository;
        private IUserProfileRepository usersRepository;
        private IValorSistemaRepository valoresSistemaRepository;

        public IndicadoresController(IIndicadorRepository indicadoresRepository, IRoleRepository rolesRepository, IUserProfileRepository usersRepository, IValorSistemaRepository valoresSistemaRepository)
        {
            this.indicadoresRepository = indicadoresRepository;
            this.usersRepository = usersRepository;
            this.valoresSistemaRepository = valoresSistemaRepository;
        }

        //
        // GET: /Indicadores/
        public ActionResult Index()
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            Entidade ent = user.entidade;
            DateTime data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int? entidadeId = null;

            if (ent.nome != "ISP")
            {
                entidadeId = user.entidadeId;
            }


            List<TipoIndicadoresViewModel> listaTiposIndicadores = new List<TipoIndicadoresViewModel>();

            List<ValorSistema> queryTipos = valoresSistemaRepository.GetPorTipologia("INDICADOR");
            
            foreach (var tipo in queryTipos)
            {
                if (data != null)
                {
                    listaTiposIndicadores.Add(new TipoIndicadoresViewModel { idTipo = tipo.valorSistemaId, tipo = tipo.valor, nome = tipo.descricaoLonga, data = data });
                }
            }
            return View(listaTiposIndicadores);
        }


        public ActionResult Details(int tipoIndicadorId, string tipoIndicador, DateTime? dataInicio, DateTime? dataFim)
        {
            IndicadorListViewModel indicadorListViewModel = new IndicadorListViewModel();
            indicadorListViewModel.indicadores = new List<IndicadorViewModel>();

            DateTime dataInicioConsulta;
            DateTime dataFimConsulta;
            if (dataInicio != null)
                dataInicioConsulta = dataInicio.Value.Date;
            else
                dataInicioConsulta = DateTime.Now.Date;

            if (dataFim != null)
                dataFimConsulta = dataFim.Value.Date;
            else
                dataFimConsulta = DateTime.Now.Date;

            indicadorListViewModel.dataInicio = dataInicioConsulta;
            indicadorListViewModel.dataFim = dataFimConsulta;
            indicadorListViewModel.tipoIndicador = tipoIndicador;
            indicadorListViewModel.tipoIndicadorId = tipoIndicadorId;

            return Details(indicadorListViewModel);
        }

        //
        // GET: /Indicadores/
        [HttpPost]
        public ActionResult Details(IndicadorListViewModel model )
        {
            UserProfile user = usersRepository.Find(WebSecurity.CurrentUserId);
            Entidade ent = user.entidade;
            int? entidadeId = null;

            if (ent.nome != "ISP")
            {
                entidadeId = user.entidadeId;
            }

            model.indicadores = new List<IndicadorViewModel>();

            
            DateTime dataInicioConsulta;
            DateTime dataFimConsulta;
            
            if (model.dataInicio != null)
                dataInicioConsulta = model.dataInicio.Value.Date;
            else
                dataInicioConsulta = DateTime.Now.Date;

            DateTime dataFimConsultaModel;
            if (model.dataFim != null)
                dataFimConsultaModel = model.dataFim.Value.Date;
            else
                dataFimConsultaModel = DateTime.Now.Date;

            dataFimConsulta = dataFimConsultaModel.AddDays(1);

            IQueryable<Indicador> queryIndicador1 = null;
            IQueryable<Indicador> queryIndicador2 = null;
            IQueryable<Indicador> queryIndicador3 = null;
            IQueryable<Indicador> queryIndicador4 = null;


            switch (model.tipoIndicador)
            {
                case ("TOTAIS_EVENTOS"):

                    if (entidadeId != null)
                    {
                        queryIndicador1 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS");
                        queryIndicador2 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_ERROS_EVENTOS");
                        queryIndicador3 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_PROCESSADOS");
                    }
                    else
                    {
                        queryIndicador1 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS");
                        queryIndicador2 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_ERROS_EVENTOS");
                        queryIndicador3 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_PROCESSADOS");
                    }

                    var queryIndicadorNrEventos = queryIndicador1.GroupBy(g => new { g.entidade.nome, g.entidadeId }).Select(x => new  {
                                                                                                                                    entidadeNome = x.Key.nome,
                                                                                                                                    entidadeId = x.Key.entidadeId,
                                                                                                                                    nrEventos = x.Sum(p => p.valor)
                                                                                                                                });

                    foreach (var indicadorEventos in queryIndicadorNrEventos)
                    {
                        double sucessos = 0;
                        double erros = 0;
                        double percentagemSucessos = 0;
                        double percentagemErros = 0;
                        
                        var indicadorErro = queryIndicador2.Where(i => i.entidadeId == indicadorEventos.entidadeId);
                        var indicadorSucesso = queryIndicador3.Where(i => i.entidadeId == indicadorEventos.entidadeId);
                        if (indicadorErro != null && indicadorErro.Count() > 0)
                        {
                            erros = indicadorErro.Sum(i => i.valor);
                            percentagemErros = erros == 0 ? 0 : erros * 100 / indicadorEventos.nrEventos;
                        }

                        if (indicadorSucesso != null && indicadorSucesso.Count() > 0)
                        {
                            sucessos = indicadorSucesso.First().valor;
                            percentagemSucessos = sucessos == 0 ? 0 : sucessos * 100 / indicadorEventos.nrEventos;
                        }

                        model.indicadores.Add(new IndicadorViewModel
                        {
                            Entidade = indicadorEventos.entidadeNome,
                            Valor1 = indicadorEventos.nrEventos.ToString(),
                            Valor2 = erros.ToString(),
                            Valor3 = percentagemErros.ToString(),
                            Valor4 = sucessos.ToString(),
                            Valor5 = percentagemSucessos.ToString(),
                            Header1 = "Total Eventos",
                            Header2 = "Total Erros",
                            Header3 = "% Erros",
                            Header4 = "Total Sucessos",
                            Header5 = "% Sucessos"

                        });
                    }

                    model.DescricaoTipoIndicador = "Indicador de eventos reportados.";
                    model.dataInicio = dataInicioConsulta;
                    model.dataFim = dataFimConsultaModel;

                    return View("DetailsTotais", model);
                    break;

                case ("ERROS_TIPOLOGIA"):

                    if (entidadeId != null)
                    {
                        queryIndicador1 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS");
                        queryIndicador2 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_ERROS_EVENTOS");
                        queryIndicador3 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_ERROS_EVENTOS_TIPO");
                    }
                    else
                    {
                        queryIndicador1 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS");
                        queryIndicador2 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_ERROS_EVENTOS");
                        queryIndicador3 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_ERROS_EVENTOS_TIPO");
                    }

                    var queryIndicadorNrErrosTipo = queryIndicador3.GroupBy(g => new { g.entidade.nome, g.entidadeId, g.tipologiaId, g.subTipo  }).Select(x => new
                                                                                                                                                                {
                                                                                                                                                                    entidadeNome = x.Key.nome,
                                                                                                                                                                    entidadeId = x.Key.entidadeId,
                                                                                                                                                                    tipologiaid = x.Key.tipologiaId,
                                                                                                                                                                    subtipo = x.Key.subTipo,
                                                                                                                                                                    nrErros = x.Sum(p => p.valor)
                                                                                                                                                                });

                    foreach (var indicadorErrosTipologia in queryIndicadorNrErrosTipo.ToList())
                    {
                        double errosTipo = 0;
                        double erros = 0;
                        double eventos = 0;
                        double percentagemErrosTipologia = 0;
                        double percentagemErrosTipologia2 = 0;

                        errosTipo = indicadorErrosTipologia.nrErros;

                        var indicadorErros = queryIndicador2.Where(i => i.entidadeId == indicadorErrosTipologia.entidadeId).ToList();
                        var indicadorEventos = queryIndicador1.Where(i => i.entidadeId == indicadorErrosTipologia.entidadeId).ToList();
                        if (indicadorErros != null && indicadorErros.Sum(g => g.valor) != 0)
                        {
                            erros = indicadorErros.Sum(g => g.valor);
                            percentagemErrosTipologia = errosTipo == 0 ? 0 : errosTipo * 100 / erros;
                        }

                        if (indicadorEventos != null && indicadorEventos.Sum(g => g.valor) != 0)
                        {
                            eventos = indicadorEventos.Sum(g => g.valor);
                            percentagemErrosTipologia2 = errosTipo == 0 ? 0 : errosTipo * 100 / eventos;
                        }


                        model.indicadores.Add(new IndicadorViewModel
                        {
                            Entidade = indicadorErrosTipologia.entidadeNome,
                            Valor1 = eventos.ToString(),
                            Valor2 = erros.ToString(),
                            Valor3 = indicadorErrosTipologia.subtipo,
                            Valor4 = errosTipo.ToString(),
                            Valor5 = percentagemErrosTipologia2.ToString(),
                            Valor6 = percentagemErrosTipologia.ToString(),
                            Header1 = "Total Eventos",
                            Header2 = "Total Eventos com Erros",
                            Header3 = "Tipologia",
                            Header4 = "Número de Erros",
                            Header5 = "% Erros (Total Eventos)",
                            Header6 = "% Erros (Total Erros)"

                        });
                    }

                    model.DescricaoTipoIndicador = "Indicador de erros reportados, por tipologia.";
                    model.dataInicio = dataInicioConsulta;
                    model.dataFim = dataFimConsultaModel;

                    return View("DetailsErrosTipologia", model);
                    break;

                case ("AVISOS_TIPOLOGIA"):

                    if (entidadeId != null)
                    {
                        queryIndicador1 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS");
                        queryIndicador2 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_AVISOS_PERIODO_SEGURO");
                        queryIndicador3 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_AVISOS_PERIODO_SEGURO_TIPO");
                    }
                    else
                    {
                        queryIndicador1 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS");
                        queryIndicador2 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_AVISOS_PERIODO_SEGURO");
                        queryIndicador3 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_AVISOS_PERIODO_SEGURO_TIPO");
                    }

                    var queryIndicadorNrAvisosTipo = queryIndicador3.GroupBy(g => new { g.entidade.nome, g.entidadeId, g.tipologiaId, g.subTipo }).Select(x => new
                                                                                                                                                                {
                                                                                                                                                                    entidadeNome = x.Key.nome,
                                                                                                                                                                    entidadeId = x.Key.entidadeId,
                                                                                                                                                                    tipologiaid = x.Key.tipologiaId,
                                                                                                                                                                    subtipo = x.Key.subTipo,
                                                                                                                                                                    nrAvisos = x.Sum(p => p.valor)
                                                                                                                                                                });

                    foreach (var indicadorAvisosTipologia in queryIndicadorNrAvisosTipo.ToList())
                    {
                        double avisosTipo = 0;
                        double avisos = 0;
                        double eventos = 0;
                        double percentagemAvisosTipologia = 0;
                        double percentagemAvisosTipologia2 = 0;

                        avisosTipo = indicadorAvisosTipologia.nrAvisos;

                        var indicadorAvisos = queryIndicador2.Where(i => i.entidadeId == indicadorAvisosTipologia.entidadeId).ToList();
                        var indicadorEventos = queryIndicador1.Where(i => i.entidadeId == indicadorAvisosTipologia.entidadeId).ToList();
                        if (indicadorAvisos != null && indicadorAvisos.Sum(g => g.valor) != 0)
                        {
                            avisos = indicadorAvisos.Sum(g => g.valor);
                            percentagemAvisosTipologia = avisosTipo == 0 ? 0 : avisosTipo * 100 / avisos;
                        }

                        if (indicadorEventos != null && indicadorEventos.Sum(g => g.valor) != 0)
                        {
                            eventos = indicadorEventos.Sum(g => g.valor);
                            percentagemAvisosTipologia2 = avisosTipo == 0 ? 0 : avisosTipo * 100 / eventos;
                        }


                        model.indicadores.Add(new IndicadorViewModel
                        {
                            Entidade = indicadorAvisosTipologia.entidadeNome,
                            Valor1 = eventos.ToString(),
                            Valor2 = avisos.ToString(),
                            Valor3 = indicadorAvisosTipologia.subtipo,
                            Valor4 = avisosTipo.ToString(),
                            Valor5 = percentagemAvisosTipologia2.ToString(),
                            Valor6 = percentagemAvisosTipologia.ToString(),
                            Header1 = "Total Eventos",
                            Header2 = "Total Eventos com Avisos",
                            Header3 = "Tipologia",
                            Header4 = "Número de Avisos",
                            Header5 = "% Avisos (Total Eventos)",
                            Header6 = "% Avisos (Total Avisos)"

                        });
                    }

                    model.DescricaoTipoIndicador = "Indicador de Avisos reportados, por tipologia.";
                    model.dataInicio = dataInicioConsulta;
                    model.dataFim = dataFimConsultaModel;

                    return View("DetailsAvisosTipologia", model);
                    break;

                case ("OPERACOES_TIPOLOGIA"):

                    if (entidadeId != null)
                    {
                        queryIndicador1 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_PROCESSADOS");
                        queryIndicador2 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_OPERACAO" && i.subTipo == "C");
                        queryIndicador3 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_OPERACAO" && i.subTipo == "M");
                        queryIndicador4 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_OPERACAO" && i.subTipo == "A");
                    }
                    else
                    {
                        queryIndicador1 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_PROCESSADOS");
                        queryIndicador2 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_OPERACAO" && i.subTipo == "C");
                        queryIndicador3 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_OPERACAO" && i.subTipo == "M");
                        queryIndicador4 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_OPERACAO" && i.subTipo == "A");
                    }


                    var queryIndicadorNrEventosProcessados = queryIndicador1.GroupBy(g => new { g.entidade.nome, g.entidadeId}).Select(x => new
                                                                                                                                                {
                                                                                                                                                    entidadeNome = x.Key.nome,
                                                                                                                                                    entidadeId = x.Key.entidadeId,
                                                                                                                                                    nrEventos = x.Sum(p => p.valor)
                                                                                                                                                });


                    foreach (var indicadorEventos in queryIndicadorNrEventosProcessados.ToList())
                    {
                        double eventos = 0;
                        double criacoes = 0;
                        double modificacoes = 0;
                        double anulacoes = 0;
                        double percentagemCriacoes= 0;
                        double percentagemModificacoes = 0;
                        double percentagemAnulacoes = 0;

                        eventos = indicadorEventos.nrEventos;

                        var queryCriacoes = queryIndicador2.Where(i => i.entidadeId == indicadorEventos.entidadeId);
                        var queryModificacoes = queryIndicador3.Where(i => i.entidadeId == indicadorEventos.entidadeId);
                        var queryAnulacoes = queryIndicador4.Where(i => i.entidadeId == indicadorEventos.entidadeId);

                        if (!(queryCriacoes == null || queryCriacoes.Count() == 0))
                            criacoes = queryCriacoes.Sum(c => c.valor);

                        if (!(queryModificacoes == null || queryModificacoes.Count() == 0))
                            modificacoes = queryModificacoes.Sum(c => c.valor);

                        if (!(queryAnulacoes == null || queryAnulacoes.Count() == 0))
                            anulacoes = queryAnulacoes.Sum(c => c.valor);
                        
                        percentagemCriacoes = eventos == 0 ? 0 : criacoes * 100 / eventos;
                        percentagemModificacoes = eventos == 0 ? 0 : modificacoes * 100 / eventos;
                        percentagemAnulacoes = eventos == 0 ? 0 : anulacoes * 100 / eventos;

                        model.indicadores.Add(new IndicadorViewModel
                        {
                            Entidade = indicadorEventos.entidadeNome,
                            Valor1 = eventos.ToString(),
                            Valor2 = criacoes.ToString(),
                            Valor3 = percentagemCriacoes.ToString(),
                            Valor4 = modificacoes.ToString(),
                            Valor5 = percentagemModificacoes.ToString(),
                            Valor6 = anulacoes.ToString(),
                            Valor7 = percentagemAnulacoes.ToString(),
                            Header1 = "Total Eventos",
                            Header2 = "Eventos de Criação",
                            Header3 = "% Criações",
                            Header4 = "Eventos de Modificação",
                            Header5 = "% Modificações",
                            Header6 = "Eventos de Anulação",
                            Header7 = "% Anulações"
                            
                        });
                    }

                    model.DescricaoTipoIndicador = "Indicador de operações processadas, por tipologia de operação.";
                    model.dataInicio = dataInicioConsulta;
                    model.dataFim = dataFimConsultaModel;

                    return View("DetailsOperacoesTipologia", model);
                    break;

                case ("SLA"):
                    if (entidadeId != null)
                    {
                        queryIndicador1 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_PROCESSADOS");
                        queryIndicador2 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_OPERACOES_DENTRO_SLA");
                        queryIndicador3 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_OPERACOES_FORA_SLA");
                        queryIndicador4 = indicadoresRepository.All.Where(i => i.publico == true && i.entidadeId == entidadeId && i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "TEMPO_MEDIO_DESVIOS_SLA");
                    }
                    else
                    {
                        queryIndicador1 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_EVENTOS_PROCESSADOS");
                        queryIndicador2 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_OPERACOES_DENTRO_SLA");
                        queryIndicador3 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "NR_OPERACOES_FORA_SLA");
                        queryIndicador4 = indicadoresRepository.All.Where(i => i.dataIndicador >= dataInicioConsulta && i.dataIndicador <= dataFimConsulta && i.tipologia.valor == "TEMPO_MEDIO_DESVIOS_SLA");
                    }


                    var queryIndicadorEventosProcessados = queryIndicador1.GroupBy(g => new { g.entidade.nome, g.entidadeId }).Select(x => new
                                                                                                                                        {
                                                                                                                                            entidadeNome = x.Key.nome,
                                                                                                                                            entidadeId = x.Key.entidadeId,
                                                                                                                                             nrEventos = x.Sum(p => p.valor)
                                                                                                                                        });


                    foreach (var indicadorEventos in queryIndicadorEventosProcessados.ToList())
                    {
                        double eventos = 0;
                        double eventosDentro = 0;
                        double eventosFora = 0;
                        double mediaDesvio = 0;
                        double percentagemDentro= 0;
                        double percentagemFora = 0;

                        eventos = indicadorEventos.nrEventos;

                        var queryDentro = queryIndicador2.Where(i => i.entidadeId == indicadorEventos.entidadeId);
                        var queryFora = queryIndicador3.Where(i => i.entidadeId == indicadorEventos.entidadeId);
                        var queryMediaDesvio = queryIndicador4.Where(i => i.entidadeId == indicadorEventos.entidadeId);

                        if (!(queryDentro == null || queryDentro.Count() == 0))
                            eventosDentro = queryDentro.Sum(e => e.valor);

                        if (!(queryFora == null || queryFora.Count() == 0))
                            eventosFora = queryFora.Sum(e => e.valor);

                        if (!(queryMediaDesvio == null || queryMediaDesvio.Count() == 0))
                            mediaDesvio = queryMediaDesvio.Sum(e => e.valor) / (dataFimConsulta.Subtract(dataInicioConsulta).Days);

                        percentagemDentro = eventos == 0 ? 0 : eventosDentro * 100 / eventos;
                        percentagemFora = eventos == 0 ? 0 : eventosFora * 100 / eventos;
                                                               
                        int dias = 0;
                        int horas = 0;
                        int minutos = 0;
                        int resto = 0;
                        if(mediaDesvio > 0)
                        {
                            dias = (int) mediaDesvio / (24*60);
                            resto = (int) mediaDesvio % (24*60);
                            horas = resto / 60;
                            minutos = resto % 60; 
                        }



                        model.indicadores.Add(new IndicadorViewModel
                        {
                            Entidade = indicadorEventos.entidadeNome,
                            Valor1 = eventos.ToString(),
                            Valor2 = eventosDentro.ToString(),
                            Valor3 = percentagemDentro.ToString(),
                            Valor4 = eventosFora.ToString(),
                            Valor5 = percentagemFora.ToString(),
                            Valor6 = String.Format("{0}d {1}h {2}m", dias, horas, minutos ),
                            Header1 = "Total Eventos",
                            Header2 = "Eventos Dentro SLA",
                            Header3 = "% Dentro SLA",
                            Header4 = "Eventos Fora SLA",
                            Header5 = "% Fora SLA",
                            Header6 = "Media Desvio",                           
                        });
                    }

                    model.DescricaoTipoIndicador = "Indicador decumprimento dos SLA's.";
                    model.dataInicio = dataInicioConsulta;
                    model.dataFim = dataFimConsultaModel;

                    return View("DetailsSLA", model);
                    break;
                
                
                default:
                    return View(model);
                        break;
            }

        }
    }
}
