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

namespace ISP.GestaoMatriculas.IsentosOperationAgent
{
    public partial class FNMPASIsentosOperationAgent : ServiceBase
    {
        private System.Timers.Timer timer;

        public FNMPASIsentosOperationAgent()
        {
            InitializeComponent();

            timer = new System.Timers.Timer(60000);
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);

            //eventLog.Source = "FNMPASOperationAgent";

            //eventLog.WriteEntry("FNMPAS Operation Agent Constructor"); 

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

            //eventLog.WriteEntry("FNMPAS Operation Agent Started"); 
        }


        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            //eventLog.WriteEntry("FNMPAS Operation Agent Stopped"); 
            timer.Stop();
        }

        private void timer_Tick(object source, ElapsedEventArgs ev)
        {

        }


        protected void verificaOutrasOperacoes(EventoStagging eventoValidado)
        {


        }

    }
}
