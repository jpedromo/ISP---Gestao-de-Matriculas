using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ISP.GestaoMatriculas.NotificationAgent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
<<<<<<< HEAD
#if(DEBUG)
            FNMPASNotificationAgent service = new FNMPASNotificationAgent();
            string[] args = new string[] { "arg1", "arg2" };
            service.StartFromDebugger(args);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new FNMPASNotificationAgent() 
			};
            ServiceBase.Run(ServicesToRun);
#endif
=======
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new Service1() 
			};
            ServiceBase.Run(ServicesToRun);
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }
    }
}
