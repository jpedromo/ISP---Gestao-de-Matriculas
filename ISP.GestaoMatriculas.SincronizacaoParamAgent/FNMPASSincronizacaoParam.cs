using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Data.SqlClient;
using System.Configuration;

namespace ISP.GestaoMatriculas.SincronizacaoParamAgent
{
    public partial class FNMPASSincronizacaoParam : ServiceBase
    {
        private System.Timers.Timer timer;

        public FNMPASSincronizacaoParam()
        {
            InitializeComponent();

            //correr diariamente, intervalo de um dia
            timer = new System.Timers.Timer(86400000);
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);

            eventLog.Source = "FNMPASSincronizacaoParamAgent";
            eventLog.WriteEntry("FNMPAS Sincronização Agent Constructor"); 
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

            eventLog.WriteEntry("FNMPAS Sincronização Parametros Agent Started"); 
        }


        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            eventLog.WriteEntry("FNMPAS Sincronização Parametros Agent Stopping"); 
            timer.Stop();
        }

        private void timer_Tick(object source, ElapsedEventArgs ev)
        {
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var conn = new SqlConnection(connect))
            using (var command = new SqlCommand("SP_MAT_UPDATE_PARAMETROS_SISTEMA", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                SqlParameter parm1 = new SqlParameter("@CodRetorno", SqlDbType.Int);
                parm1.Value = 0;
                parm1.Direction = ParameterDirection.Output;

                SqlParameter parm2 = new SqlParameter("@MensagemErro", SqlDbType.VarChar);
                //parm2.Size = 500; 
                parm2.Value = "";
                parm2.Direction = ParameterDirection.Output;
                command.Parameters.Add(parm1);
                command.Parameters.Add(parm2);
               
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                int codRetorno = (int) command.Parameters["@CodRetorno"].Value;
                if (codRetorno == 0)
                    eventLog.WriteEntry("FNMPAS Sincronização Parametros Agent executado com sucesso");
                if (codRetorno == -1)
                    eventLog.WriteEntry("FNMPAS Sincronização Parametros Agent executado com erro"); 
            }
        }
    }
}
