using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ISP.GestaoMatriculas.SincronizacaoParamAgent
{
    
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if(DEBUG)
            FNMPASSincronizacaoParam service = new FNMPASSincronizacaoParam();
        string[] args = new string[] { "arg1", "arg2" };
        service.StartFromDebugger(args);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
		{ 
			new FNMPASSincronizacaoParam() 
		};
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
    
}
