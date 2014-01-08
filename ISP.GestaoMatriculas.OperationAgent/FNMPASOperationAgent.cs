using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Repositories;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Model.Exceptions;
using ISP.GestaoMatriculas.Utils;
using System.Timers;
using System.Threading;

namespace ISP.GestaoMatriculas.OperationAgent
{
    public partial class FNMPASOperationAgent : ServiceBase
    {
        private System.Timers.Timer timer;

        protected bool isAgentRunning = false;
        protected bool isAgentStopping = false;

        public FNMPASOperationAgent()
        {
            InitializeComponent();

            timer = new System.Timers.Timer(60000);
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);

            eventLog.Source = "FNMPASOperationAgent";
            eventLog.WriteEntry("FNMPAS Operation Agent Constructor"); 

        }

        public void StartFromDebugger(string[] args)
        {
            timer_Tick(null, null);
        }

        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            timer.Enabled = true;
            isAgentStopping = false;
            isAgentRunning = false;
            timer.Start();

            eventLog.WriteEntry("FNMPAS Operation Agent Started"); 
        }


        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            eventLog.WriteEntry("FNMPAS Operation Agent Stopping"); 
            timer.Stop();
            isAgentStopping = true;
            isAgentRunning = false;
        }

        private void timer_Tick(object source, ElapsedEventArgs ev)
        {
            eventLog.WriteEntry("FNMPAS Operation Agent Timer Tick Start. Is AgentRunning: " + isAgentRunning + ". Is AgentStopping : "+ isAgentStopping );
            if (isAgentRunning || isAgentStopping)
            {
                return;
            }

            this.isAgentRunning = true;
            eventLog.WriteEntry("FNMPAS Operation Agent Timer Tick");

            //... thread content
            OperationWorkers workers = new OperationWorkers(15);

            workers.start();

            workers.join();

            this.isAgentRunning = false;
        }   
    }


    public class OperationWorkers
    {
        bool isAgentStopping = false;
        //bool isAgentRunning = false;

        List<Thread> threadPool;
        Dictionary<int, string> threadKeyLocks;
        Mutex aquisitionMutex;
        Mutex cacheMutex;
        Mutex fileMutex;

        public OperationWorkers(int numWorkers)
        {
            threadPool = new List<Thread>();
            threadKeyLocks = new Dictionary<int, string>();
            aquisitionMutex = new Mutex(true, "aquisitionMutex");
            cacheMutex = new Mutex(true, "cacheMutex");
            fileMutex = new Mutex(true, "fileMutex");

            for (int i = 0; i < numWorkers; i++)
            {
                threadPool.Add(new Thread(new ThreadStart(this.Worker)));
                threadKeyLocks.Add(i, null);
            }
        }

        public void start(){
            foreach (Thread t in threadPool)
            {
                t.Start();
            }
            this.isAgentStopping = false;
            aquisitionMutex.ReleaseMutex();
            cacheMutex.ReleaseMutex();
            fileMutex.ReleaseMutex();
        }

        public void stop()
        {
            this.isAgentStopping = true;
        }

        public void join()
        {
            foreach (Thread t in threadPool)
            {
                t.Join();
            }
        }

        private int adquireEventoPendente(int estadoPendente, int estadoEmProcessamento)
        {
            EventoStaggingRepository eventosStaggingRepository = new EventoStaggingRepository();
            //Adicionar Mutex + fila de threads para gestao de concorrência
            EventoStagging eventoPendente = null;
            aquisitionMutex.WaitOne();
            try
            {
                eventoPendente = eventosStaggingRepository.All.FirstOrDefault<EventoStagging>(e => e.estadoEventoId == estadoPendente && !threadKeyLocks.Values.Contains(e.matricula));

                if (eventoPendente != null) {
                        threadKeyLocks[threadPool.IndexOf(Thread.CurrentThread)] = eventoPendente.matricula;
                    }     
            }
            catch (Exception e)
            {

            }
            finally
            {
                aquisitionMutex.ReleaseMutex();
            }

            if (eventoPendente != null)
            {
                return eventoPendente.eventoStaggingId;
            }

            return default(int);
        }

        private void releaseEventoPendente()
        {
            threadKeyLocks[threadPool.IndexOf(Thread.CurrentThread)] = null;
        }

        public void Worker()
        {
            EventoStaggingRepository eventosStaggingRepository = new EventoStaggingRepository();
            ConcelhoRepository concelhosRepository = new ConcelhoRepository();
            CategoriaRepository categoriasRepository = new CategoriaRepository();
            ApoliceRepository apolicesRepository = new ApoliceRepository();
            AvisoRepository avisosRepository = new AvisoRepository();
            ApoliceHistoricoRepository apolicesHistoricoRepository = new ApoliceHistoricoRepository();
            EntidadeRepository entidadesRepository = new EntidadeRepository();
            NotificacaoRepository notificacoesRepository = new NotificacaoRepository();
            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();
            UserProfileRepository userProfileRepository = new UserProfileRepository();

            List<ValorSistema> estadosEvento = valoresSistemaRepository.GetPorTipologia("ESTADO_EVENTO_STAGGING", cacheMutex);
            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO", cacheMutex);
            List<ValorSistema> tipoNotificacao = valoresSistemaRepository.GetPorTipologia("TIPO_NOTIFICACAO", cacheMutex);
            List<ValorSistema> operacoesEvento = valoresSistemaRepository.GetPorTipologia("OPERACAO_EVENTO", cacheMutex);
            List<Concelho> listaConcelhos = concelhosRepository.All.ToList();
            List<Categoria> listaCategorias = categoriasRepository.All.ToList();


            int horaLimiteSLA = int.Parse(valoresSistemaRepository.GetPorTipologia("PARAM_HORA_LIMITE_SLA", cacheMutex).Single().valor);
            int horaExtensaoSLA = int.Parse(valoresSistemaRepository.GetPorTipologia("PARAM_HORA_EXTENSAO_SLA", cacheMutex).Single().valor);

            int estadoEmProcessamento = estadosEvento.Where(e => e.valor == "EM_PROCESSAMENTO").Single().valorSistemaId;
            int estadoPendente = estadosEvento.Where(e => e.valor == "PENDENTE").Single().valorSistemaId;
            int estadoErro = estadosEvento.Where(e => e.valor == "ERRO").Single().valorSistemaId;
            int estadoProcessado = estadosEvento.Where(e => e.valor == "PROCESSADO").Single().valorSistemaId;
            int estadoFicheiroProcessado = estadosFicheiro.Where(e => e.valor == "PROCESSADO").Single().valorSistemaId;

            EventoStagging eventoPendente = null;
            int eventoPendenteId = default(int);

            while ((eventoPendenteId = adquireEventoPendente(estadoPendente, estadoEmProcessamento)) != default(int))
            {
                eventoPendente = eventosStaggingRepository.Find(eventoPendenteId);

                if (isAgentStopping)
                {
                    releaseEventoPendente();
                    return;
                }

                eventoPendente.estadoEventoId = estadoEmProcessamento;    
                eventosStaggingRepository.InsertOrUpdate(eventoPendente);
                

                EventoStagging evento = EventoStaggingFactory.criarEventoStagging(eventoPendente, cacheMutex);
                EventoStagging eventoProcessar = null;

                if (evento.errosEventoStagging.Count == 0)
                {
                    try
                    {
                        eventoProcessar = EventoStaggingFactory.duplicarEventoProducao(evento.matricula, (int)evento.entidadeId, evento.dataInicioCobertura, evento.horaInicioCobertura, cacheMutex);
                        eventoProcessar.esmagaDados(evento);
                    }
                    catch (ErroEventoStaggingException e)
                    {

                        foreach (ErroEventoStagging erro in e.errosEventoStagging)
                        {
                            evento.errosEventoStagging.Add(new ErroEventoStagging { campo = erro.campo, descricao = erro.descricao, tipologiaId = erro.tipologiaId, eventoStaggingId = erro.eventoStaggingId });
                        }
                    }
                }

                if (eventoProcessar == null || !ValidacaoEventos.validarEvento(eventoProcessar, cacheMutex))
                {
                    //if (evento.CodigoOperacao == EventoStagging.operacaoEventoStagging.A)
                    //{
                    //    //anulação foi testada em producao e falhou.
                    //    //esmaga evento em stagging e adiciona-se como erro.
                    //    evento = EventoStaggingFactory.duplicarEventoStagging(eventoPendente.matricula, (int)eventoPendente.entidadeId, eventoPendente.dataInicioCobertura, eventoPendente.horaInicioCobertura);
                    //    if (evento == null)
                    //    {
                    //        evento = new EventoStagging();
                    //        evento.CodigoOperacao = EventoStagging.operacaoEventoStagging.A;
                    //    }
                    //    evento.esmagaDados(eventoPendente);
                    //    ValidacaoEventos.validarEvento(evento); //reconstruir avisos e erros. Sabemos que vai falhar.
                    //}
                    // validação falhou definitivamente

                    evento.estadoEventoId = estadoErro;

                    if (eventoProcessar != null)
                    {
                        foreach (ErroEventoStagging e in eventoProcessar.errosEventoStagging)
                        {
                            evento.errosEventoStagging.Add(new ErroEventoStagging { campo = e.campo, descricao = e.descricao, tipologiaId = e.tipologiaId, eventoStaggingId = e.eventoStaggingId });
                        }
                        eventoProcessar.errosEventoStagging.Clear();

                        if (eventoProcessar.codigoOperacao != "A")
                        {
                            foreach (Aviso a in eventoProcessar.avisosEventoStagging)
                            {
                                evento.avisosEventoStagging.Add(new Aviso { campo = a.campo, descricao = a.descricao, tipologiaId = a.tipologiaId, eventoStaggingId = a.eventoStaggingId });
                            }
                        }
                        eventoProcessar.avisosEventoStagging.Clear();
                    }

                    evento.totalAvisosCumulativo += evento.totalAvisosCumulativo + evento.avisosEventoStagging.Count;
                    evento.totalErrosCumulativo += evento.totalErrosCumulativo + evento.errosEventoStagging.Count;

                    List<EventoStagging> errosStagging = eventosStaggingRepository.All.Include("estadoEvento").Where(e => e.entidadeId == evento.entidadeId && e.dataInicioCobertura == evento.dataInicioCobertura &&
                    e.horaInicioCobertura == evento.horaInicioCobertura && e.matricula == evento.matricula && e.estadoEvento.valor == "ERRO"
                    && e.codigoOperacao == evento.codigoOperacao && e.arquivado == false).ToList();

                    foreach (EventoStagging erro in errosStagging)
                    {
                        erro.arquivado = true;
                        eventosStaggingRepository.InsertOrUpdate(erro);
                    }

                    eventosStaggingRepository.InsertOrUpdate(evento);                      
                }
                else
                {
                    Concelho concelho = listaConcelhos.Where(c => c.codigoConcelho == eventoProcessar.codigoConcelhoCirculacao).FirstOrDefault();
                    Categoria categoria = listaCategorias.Where(c => c.codigoCategoriaVeiculo == eventoProcessar.codigoCategoriaVeiculo).FirstOrDefault();
                    int? concelhoId = concelho == null ? (int?)null : concelho.concelhoId;
                    int? categoriaId = categoria == null ? (int?)null : categoria.categoriaId;

                    ValorSistema operacao = operacoesEvento.Single(c => c.valor == eventoProcessar.codigoOperacao);

                    Apolice apoliceValidada = new Apolice(eventoProcessar, concelhoId, categoriaId, operacao.valorSistemaId, horaLimiteSLA, horaExtensaoSLA);

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
                        historico.utilizadorArquivo = apoliceValidada.utilizadorReporte;

                        apolicesHistoricoRepository.InsertOrUpdate(historico);
                        apolicesHistoricoRepository.Save();

                        int idApoliceHistorico = historico.apoliceId;
                        a.avisosApolice.ForEach(aviso => aviso.apoliceHistoricoId = idApoliceHistorico);

                        apolicesRepository.Delete(a.apoliceId);
                    }

                    apolicesRepository.InsertOrUpdate(apoliceValidada);
                    apolicesRepository.Save();
             
                    List<EventoStagging> errosStagging = eventosStaggingRepository.All.Include("estadoEvento").Where(e => e.entidadeId == evento.entidadeId && e.dataInicioCobertura == evento.dataInicioCobertura &&
                    e.horaInicioCobertura == evento.horaInicioCobertura && e.matricula == evento.matricula && e.estadoEvento.valor == "ERRO"
                    && e.codigoOperacao == eventoProcessar.codigoOperacao && e.arquivado == false).ToList();

                    foreach (EventoStagging erro in errosStagging)
                    {
                        erro.dataCorrecaoErro = DateTime.Now;
                        erro.arquivado = true;
                        eventosStaggingRepository.InsertOrUpdate(erro);
                    }   
                    
                    verificaOutrasOperacoes(eventoProcessar);

                }


                eventoPendente.estadoEventoId = estadoProcessado;
                eventosStaggingRepository.InsertOrUpdate(eventoPendente);
                eventosStaggingRepository.Save();

                FicheiroRepository ficheirosRepository = null;
                Ficheiro ficheiro = null;
                if (eventoPendente.ficheiroID != null)
                {
                    fileMutex.WaitOne();
                    try
                    {
                        ficheirosRepository = new FicheiroRepository();
                        ficheiro = ficheirosRepository.Find((int)eventoPendente.ficheiroID);

                        if (evento.errosEventoStagging.Count > 0)
                        {
                            ficheiro.numEventosErro++;
                        }
                        if (eventoPendente == null || eventoPendente.avisosEventoStagging.Count > 0 || evento.avisosEventoStagging.Count > 0)
                        {
                            ficheiro.numEventosAviso++;
                        }
                        ficheiro.totalRegistosProcessados++;
                        //int theadNumber = threadPool.IndexOf(Thread.CurrentThread);
                        if (ficheiro.totalRegistos == ficheiro.totalRegistosProcessados)
                        {
                            ficheiro.estadoId = estadoFicheiroProcessado;

                            int tipoNotificacaoId;
                            string mensagemNotificacao;

                            if (ficheiro.numEventosErro > 0)
                            {
                                tipoNotificacaoId = tipoNotificacao.Where(t => t.valor == "ERRO_PROCESSAMENTO_FICHEIRO").Single().valorSistemaId;
                                mensagemNotificacao = "O 'Ficheiro Nacional de Matrículas do Parque Automóvel Seguro' com nome '" + ficheiro.nomeFicheiro + "' e data de reporte '" +
                                    ficheiro.dataUpload.ToShortDateString() + "' foi processado com  " +
                                    ficheiro.numEventosErro + " ocorrências de erro e " + ficheiro.numEventosAviso + " de aviso " +
                                    "num total de " + ficheiro.totalRegistos + " registos.";
                            }
                            else
                            {
                                if (ficheiro.numEventosAviso > 0)
                                {
                                    tipoNotificacaoId = tipoNotificacao.Where(t => t.valor == "AVISO_PROCESSAMENTO_FICHEIRO").Single().valorSistemaId;

                                    mensagemNotificacao = "O 'Ficheiro Nacional de Matrículas do Parque Automóvel Seguro' com nome '" + ficheiro.nomeFicheiro + "' e data de reporte '" +
                                    ficheiro.dataUpload.ToShortDateString() + "' foi processado com  " +
                                    ficheiro.numEventosAviso + " ocorrências de aviso " +
                                    "num total de " + ficheiro.totalRegistos + " registos.";
                                }
                                else
                                {
                                    tipoNotificacaoId = tipoNotificacao.Where(t => t.valor == "SUCESSO_PROCESSAMENTO_FICHEIRO").Single().valorSistemaId;

                                    mensagemNotificacao = "O 'Ficheiro Nacional de Matrículas do Parque Automóvel Seguro' com nome '" + ficheiro.nomeFicheiro + "' e data de reporte '" +
                                    ficheiro.dataUpload.ToShortDateString() + "' foi processado com sucesso em " +
                                    ficheiro.totalRegistos + " ocorrências.";
                                }
                            }


                            Notificacao notificacao = new Notificacao
                            {
                                dataCriacao = DateTime.Now,
                                email = true,
                                entidadeId = ficheiro.entidadeId,
                                tipologiaId = tipoNotificacaoId,
                                mensagem = mensagemNotificacao,
                            };

                            Entidade entidade = entidadesRepository.Find((int)notificacao.entidadeId);
                            entidade.notificacoes.Add(notificacao);
                            entidadesRepository.InsertOrUpdate(entidade);



                            UserProfile utilizadorFicheiro = userProfileRepository.All.Single(u => u.UserName == ficheiro.userName);
                            if (utilizadorFicheiro.entidadeId != ficheiro.entidadeId)
                            {
                                Notificacao notificacao2 = new Notificacao()
                                {
                                    dataCriacao = DateTime.Now,
                                    email = true,
                                    entidadeId = utilizadorFicheiro.entidadeId,
                                    tipologiaId = tipoNotificacaoId,
                                    mensagem = mensagemNotificacao,
                                };

                                notificacoesRepository.InsertOrUpdate(notificacao2);
                            }


                            entidadesRepository.Save();
                            notificacoesRepository.Save();
                                                       
                        }

                        ficheirosRepository.InsertOrUpdate(ficheiro);
                        ficheirosRepository.Save();
                    }
                    finally
                    {
                        fileMutex.ReleaseMutex();
                    }
                }

                releaseEventoPendente();
            }
        }

        private void verificaOutrasOperacoes(EventoStagging eventoValidado)
        {
            EventoStaggingRepository eventosStaggingRepository = new EventoStaggingRepository();

            List<string> operacoesDebloquear = new List<string>();

            switch (eventoValidado.codigoOperacao)
            {
                case "C":
                    {
                        operacoesDebloquear.Add("M");
                        break;
                    }
                default: { return; }
            }

            List<EventoStagging> errosDesbloquear = eventosStaggingRepository.All.Where(e => e.entidadeId == eventoValidado.entidadeId && e.dataInicioCobertura == eventoValidado.dataInicioCobertura &&
                        e.horaInicioCobertura == eventoValidado.horaInicioCobertura && e.matricula == eventoValidado.matricula && e.estadoEvento.valor == "ERRO"
                        && operacoesDebloquear.Contains(e.codigoOperacao) && e.arquivado == false).OrderBy(e => e.dataUltimaAlteracaoErro).ToList();

            foreach (EventoStagging evento in errosDesbloquear)
            {
                evento.arquivado = true;
                evento.dataArquivo = DateTime.Now;
                evento.utilizadorArquivo = eventoValidado.utilizadorReporte;
                eventosStaggingRepository.InsertOrUpdate(evento);
                eventosStaggingRepository.Save();
            }
        }
    }

}
