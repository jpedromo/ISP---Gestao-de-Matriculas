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

namespace ISP.GestaoMatriculas.FileAgent
{
    public partial class FNMPASFileAgent : ServiceBase
    {
        private System.Timers.Timer timer;
        public FNMPASFileAgent()
        {
            InitializeComponent();
            //if (!System.Diagnostics.EventLog.SourceExists("FNMPASFileAgent"))
            //{
            //    System.Diagnostics.EventLog.CreateEventSource("FNMPASFileAgent", "FileAgentLog");
            //}
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);

            //eventLog.Source = "FNMPASFileAgent";
            //eventLog.Log = "FileAgentLog";

            //eventLog.WriteEntry("FNMPAS File Agent Constructor"); 
        }

        public void StartFromDebugger(string[] args)
        {
            //eventLog.WriteEntry("FNMPAS File Agent Starting From Debugger"); 
            timer_Tick(null, null);
        }

        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            timer.Enabled = true;
            timer.Start();

            //eventLog.WriteEntry("FNMPAS File Agent Started"); 
        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            //eventLog.WriteEntry("FNMPAS File Agent Stopped"); 
            timer.Stop();
        }

        private void timer_Tick(object source, ElapsedEventArgs e)
        {
            //eventLog.WriteEntry("FNMPAS File Agent Timer Tick"); 
            FicheiroRepository ficheiroRepository = new FicheiroRepository();
            NotificacaoRepository notificacaoRepository = new NotificacaoRepository();
            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();
            UserProfileRepository userProfileRepository = new UserProfileRepository();

            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO");
            List<ValorSistema> tiposNotificacao = valoresSistemaRepository.GetPorTipologia("TIPO_NOTIFICACAO");

            IQueryable<Ficheiro> query = null;
            query = ficheiroRepository.All.Include("entidade").Where(f => f.estado.valor == "PENDENTE").OrderBy(file => file.dataAlteracao);

            foreach (Ficheiro file in query.ToList())
            {
                file.estadoId = estadosFicheiro.Where(ef => ef.valor == "EM_PROCESSAMENTO").Single().valorSistemaId;

                //List<ErroFicheiro> errosFicheiro = new List<ErroFicheiro>();
                try
                {
                    file.validar();
                    ProcessadorFicheiros.processaFicheiro(file);
                    file.totalRegistos = file.eventosStagging.Count();
                }
                catch (ErroFicheiroException ex)
                {
                    file.errosFicheiro = ex.errosFicheiro;
                    file.estadoId = estadosFicheiro.Single(ef => ef.valor == "ERRO").valorSistemaId;

                    string mensagemNotificacao = "O 'Ficheiro Nacional de Matrículas do Parque Automóvel Seguro' com nome '" + file.nomeFicheiro + "' e data de reporte '" +
                                file.dataUpload.ToShortDateString() + "' foi processado com erro: \n";

                    foreach (ErroFicheiro erro in ex.errosFicheiro)
                    {
                        mensagemNotificacao += erro.descricao;
                    }

                    Notificacao notif = new Notificacao(){ dataCriacao = DateTime.Now,
                                                        email = true,
                                                        entidadeId = file.entidadeId,
                                                        mensagem = mensagemNotificacao,
                                                        tipologiaId = tiposNotificacao.Single(n => n.valor == "ERRO_PROCESSAMENTO_FICHEIRO").valorSistemaId};
                    
                    notificacaoRepository.InsertOrUpdate(notif);

                    UserProfile utilizadorFicheiro = userProfileRepository.All.Single(u => u.UserName == file.userName);
                    if (utilizadorFicheiro.entidadeId != file.entidadeId)
                    {
                        Notificacao notif2 = new Notificacao()
                        {
                            dataCriacao = DateTime.Now,
                            email = true,
                            entidadeId = utilizadorFicheiro.entidadeId,
                            mensagem = mensagemNotificacao,
                            tipologiaId = tiposNotificacao.Single(n => n.valor == "ERRO_PROCESSAMENTO_FICHEIRO").valorSistemaId
                        };

                        notificacaoRepository.InsertOrUpdate(notif2);
                    }


                }
                catch (Exception ex)
                {

                }

                ficheiroRepository.InsertOrUpdate(file);
                ficheiroRepository.Save();
                notificacaoRepository.Save();
            }

            
        }
        
    }
}
