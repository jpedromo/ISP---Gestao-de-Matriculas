using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ISP.GestaoMatriculas.AuditAgent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if(DEBUG)
            FNMPASAuditAgent service = new FNMPASAuditAgent();
            string[] args = new string[] { "arg1", "arg2" };
            service.StartFromDebugger(args);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new FNMPASAuditAgent() 
			};
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
