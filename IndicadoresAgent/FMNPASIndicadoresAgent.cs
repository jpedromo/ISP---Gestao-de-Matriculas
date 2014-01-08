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
using System.Timers;

namespace ISP.GestaoMatriculas.IndicadoresAgent
{
    public partial class FNMPASIndicadoresAgent : ServiceBase
    {
        EventoStaggingRepository eventoStaggingRepository = new EventoStaggingRepository();
        EventoHistoricoRepository eventoHistoricoRepository = new EventoHistoricoRepository();
        ErroEventoStaggingRepository erroEventoStaggingRepository = new ErroEventoStaggingRepository();
        IndicadorRepository indicadorRepository = new IndicadorRepository();
        ApoliceRepository apoliceRepository = new ApoliceRepository();
        ApoliceHistoricoRepository apoliceHistoricoRepository = new ApoliceHistoricoRepository();
        AvisoRepository avisoRepository = new AvisoRepository();

        private System.Timers.Timer timer;
        public FNMPASIndicadoresAgent()
        {
            InitializeComponent();

            timer = new System.Timers.Timer(60000);
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);

            eventLog.Source = "FNMPASIndicadoresAgent";

            eventLog.WriteEntry("FNMPAS Indicadores Agent Constructor"); 
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
            timer.Start();

            eventLog.WriteEntry("FNMPAS Indicadores Agent Started"); 

        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            eventLog.WriteEntry("FNMPAS Indicadores Agent Stopped"); 
            timer.Stop();
        }

        private void timer_Tick(object source, ElapsedEventArgs ev)
        {
            eventLog.WriteEntry("FNMPAS Indicadores Agent Timer Tick");
            indicadorRepository.processaIndicadorNrEventos();
            indicadorRepository.processaIndicadorNrEventosProcessados();
            indicadorRepository.processaIndicadorNrErros();
            indicadorRepository.processaIndicadorNrErrosTipo();
            indicadorRepository.processaIndicadorNrAvisos();
            indicadorRepository.processaIndicadorNrAvisosTipo();
            indicadorRepository.processaIndicadorNrEventosOperacao();
            indicadorRepository.processaIndicadorNrMatriculasForaSLA();
            indicadorRepository.processaIndicadorNrMatriculasDentroSLA();
            indicadorRepository.processaIndicadorMediaDesviosSLA();
            indicadorRepository.processaIndicadorNrErrosPendentes();
        }
    }
}
