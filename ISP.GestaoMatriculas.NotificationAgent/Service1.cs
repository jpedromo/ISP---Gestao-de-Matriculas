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


namespace ISP.GestaoMatriculas.NotificationAgent
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            FicheiroRepository r = new FicheiroRepository();
        }

        protected override void OnStop()
        {
        }
    }
}
