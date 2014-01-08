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

namespace ISP.GestaoMatriculas.FileAgent
{
    public partial class FNPASFileAgent : ServiceBase
    {
        public FNPASFileAgent()
        {
            InitializeComponent();
        }

        public void StartFromDebugger(string[] args)
        {
            OnStart(args);
        }

        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            FicheiroRepository ficheiroRepository = new FicheiroRepository();
            DomainModels context = new DomainModels();
            var x = context.Ficheiros;
            IQueryable<Ficheiro> query = null;

            query = ficheiroRepository.All;
            query = ficheiroRepository.All.Where(f => f.estado == Ficheiro.EstadoFicheiro.pendente);
            int d = 3;
            foreach (Ficheiro file in query.ToList())
            {
                file.estado = Ficheiro.EstadoFicheiro.emProcessamento;

                List<ErroFicheiro> errosFicheiro = new List<ErroFicheiro>();
                try
                {
                    file.validar();
                }
                catch (ErroFicheiroException ex)
                {
                    file.errosFicheiro = ex.errosFicheiro;
                    file.estado = Ficheiro.EstadoFicheiro.erro;
                    continue;
                }

                file.carregaEventos();
            }

            //if (!System.IO.Directory.Exists(folderPath))
            //    System.IO.Directory.CreateDirectory(folderPath);

            //FileStream fs = new FileStream(folderPath + "\\WindowsService.txt",
            //                    FileMode.OpenOrCreate, FileAccess.Write);
            //StreamWriter m_streamWriter = new StreamWriter(fs);
            //m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            //m_streamWriter.WriteLine(" WindowsService: Service Started at " +
            //   DateTime.Now.ToShortDateString() + " " +
            //   DateTime.Now.ToShortTimeString() + "\n");
            //m_streamWriter.Flush();
            //m_streamWriter.Close();
        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            //FileStream fs = new FileStream(folderPath +
            //  "\\WindowsService.txt",
            //  FileMode.OpenOrCreate, FileAccess.Write);
            //StreamWriter m_streamWriter = new StreamWriter(fs);
            //m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            //m_streamWriter.WriteLine(" WindowsService: Service Stopped at " +
            //  DateTime.Now.ToShortDateString() + " " +
            //  DateTime.Now.ToShortTimeString() + "\n");
            //m_streamWriter.Flush();
            //m_streamWriter.Close();
        }
        
    }
}
