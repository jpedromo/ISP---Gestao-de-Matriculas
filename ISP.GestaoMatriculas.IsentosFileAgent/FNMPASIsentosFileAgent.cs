using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace ISP.GestaoMatriculas.IsentosFileAgent
{
    public partial class FNMPASIsentosFileAgent : ServiceBase
    {
        private System.Timers.Timer timer;
        public FNMPASIsentosFileAgent()
        {
            InitializeComponent();
            //if (!System.Diagnostics.EventLog.SourceExists("FNMPASIsentosFileAgent"))
            //{
            //    System.Diagnostics.EventLog.CreateEventSource("FNMPASIsentosFileAgent", "IsentosFileAgentLog");
            //}
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);

            //eventLog.Source = "FNMPASIsentosFileAgent";
            //eventLog.Log = "IsentosFileAgentLog";

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
            FicheiroIsentosRepository ficheiroIsentosRepository = new FicheiroIsentosRepository();
            ValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();

            List<ValorSistema> estadosFicheiro = valoresSistemaRepository.GetPorTipologia("ESTADO_FICHEIRO");

            IQueryable<FicheiroIsentos> query = null;
            query = ficheiroIsentosRepository.All.Where(f => f.estado.valor == "PENDENTE").OrderBy(file => file.dataAlteracao);

            foreach (FicheiroIsentos file in query.ToList())
            {
                //file.estadoId = estadosFicheiro.Where(ef => ef.valor == "EM_PROCESSAMENTO").Single().valorSistemaId;
                
                List<ErroFicheiro> errosFicheiro = new List<ErroFicheiro>();
                try
                {
                    file.validar();
                    ProcessadorFicheiros.processaFicheiroIsentos(file);
                    //file.totalRegistos = file.Count();
                }
                catch (ErroFicheiroException ex)
                {
                    //file.errosFicheiro = ex.errosFicheiro;
                    //file.estadoId = estadosFicheiro.Where(ef => ef.valor == "ERRO").Single().valorSistemaId;
                    //ficheiroRepository.Save();
                    continue;
                }
                catch (Exception ex)
                {

                }

                ficheiroIsentosRepository.InsertOrUpdate(file);
                ficheiroIsentosRepository.Save();
            }


        }

    }
}
