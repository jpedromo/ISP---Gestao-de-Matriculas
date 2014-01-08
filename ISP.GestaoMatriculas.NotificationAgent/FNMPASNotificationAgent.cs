using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Repositories;
using ISP.GestaoMatriculas.Model;
using System.Timers;
using ISP.GestaoMatriculas.Model.Exceptions;
using System.Data.Entity;
using System.Net.Mail;
using System.IO;


namespace ISP.GestaoMatriculas.NotificationAgent
{
    public partial class FNMPASNotificationAgent : ServiceBase
    {
        NotificacaoRepository notificacaoRepository = new NotificacaoRepository();
        ValorSistemaRepository valorSistemaRepository = new ValorSistemaRepository();

        private System.Timers.Timer timer;
        public FNMPASNotificationAgent()
        {
            InitializeComponent();

            timer = new System.Timers.Timer(60000);
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);

            //eventLog.Source = "FNMPASNotificationAgent";

            //eventLog.WriteEntry("FNMPAS Notification Agent Constructor"); 
        }

        public void StartFromDebugger(string[] args)
        {
            //eventLog.WriteEntry("FNMPAS Notification Agent Starting From Debugger"); 
            timer_Tick(null, null);
        }

        protected override void OnStart(string[] args)
        {
            timer.Enabled = true;
            timer.Start();

            //eventLog.WriteEntry("FNMPAS Notification Agent Started"); 
        }

        protected override void OnStop()
        {
            //eventLog.WriteEntry("FNMPAS Notification Agent Stopped"); 
            timer.Stop();
        }

        private void timer_Tick(object source, ElapsedEventArgs e)
        {
            //eventLog.WriteEntry("FNMPAS Notification Agent Timer Tick"); 
            INotificacaoRepository notificacaoRepository = new NotificacaoRepository();
            IValorSistemaRepository valoresSistemaRepository = new ValorSistemaRepository();

            var smtpValues = valorSistemaRepository.All.Where(v => v.tipologia == "SMTP_SERVER" || v.tipologia == "SMTP_PORT" || v.tipologia == "SMTP_USER" || v.tipologia == "SMTP_PASSWORD").ToList();
            string smtpServer = smtpValues.Where(s => s.tipologia == "SMTP_SERVER").Single().valor;
            int smtpPort = int.Parse(smtpValues.Where(s => s.tipologia == "SMTP_PORT").Single().valor);
            string smtpUser = smtpValues.Where(s => s.tipologia == "SMTP_USER").Single().valor;
            string smtpPassword = smtpValues.Where(s => s.tipologia == "SMTP_PASSWORD").Single().valor;

            IQueryable<Notificacao> query = null;
            query = notificacaoRepository.All.Include("entidade").Where(n => n.email == true && n.enviadaEmail == false && n.dataEnvioEmail == null).OrderBy(notif => notif.dataCriacao);

            foreach (Notificacao notif in query.ToList())
            {
                
                try
                {
                    bool result = enviaNotificacao(notif, smtpServer, smtpPort, smtpUser, smtpPassword);

                    if (result)
                    {
                        notif.enviadaEmail = true;
                        notif.dataEnvioEmail = DateTime.Now;
                    }

                    notificacaoRepository.Save();
                }
                catch (ErroNotificacaoException ex)
                {
                    notif.enviadaEmail = true;
                    notificacaoRepository.Save();
                    continue;
                }
                catch (Exception ex)
                {
                    notificacaoRepository.Save();
                    continue;
                }
            }
        }

        private bool enviaNotificacao(Notificacao notif, string smtpServer, int smtpPort, string smtpUser, string smtpPassword)
        {
            bool result = false;


            string destinatario = notif.entidade.emailResponsavel;
            string mensagem = notif.mensagem;
            string nomeEntidade = notif.entidade.nome;
            string codigoEntidade = notif.entidade.codigoEntidade;
            string nomeResponsavel = notif.entidade.nomeResponsavel;
            string smtpUserPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(smtpPassword));

            if (destinatario == null || destinatario.Trim() == "")
                throw new ErroNotificacaoException();

            SmtpClient client = new SmtpClient();
            client.Port = smtpPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = smtpServer;
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpUserPassword);

            using (StreamReader reader = File.OpenText("templateNotificacao.htm")) // Path to your 
            {
                MailMessage mail = new MailMessage("donotreply@isp.pt", destinatario);
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Subject = "Notificação ISP (Instituto de Seguros de Portugal)";
                string bodyText = reader.ReadToEnd(); 
                bodyText = bodyText.Replace("[COD_ENTIDADE]",codigoEntidade);
                bodyText = bodyText.Replace("[NOME_ENTIDADE]", nomeEntidade);
                bodyText = bodyText.Replace("[DATA]", DateTime.Now.ToString("D"));
                bodyText = bodyText.Replace("[BODY_EMAIL]", mensagem);
                client.Send(mail);
            }

            return result;
        }


       
    }
}
