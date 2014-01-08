using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ISP.GestaoMatriculas.FileAgent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if(DEBUG)
<<<<<<< HEAD
            FNMPASFileAgent service = new FNMPASFileAgent();
=======
            FNPASFileAgent service = new FNPASFileAgent();
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            string[] args = new string[] { "arg1", "arg2" };
            service.StartFromDebugger(args);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
<<<<<<< HEAD
				new FNMPASFileAgent() 
			};

=======
				new FNPASFileAgent() 
			};
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }

}
