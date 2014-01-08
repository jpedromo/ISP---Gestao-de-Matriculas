﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ISP.GestaoMatriculas.OperationAgent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if(DEBUG)
            FNMPASOperationAgent service = new FNMPASOperationAgent();
            string[] args = new string[] { "arg1", "arg2" };
            service.StartFromDebugger(args);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new FNMPASOperationAgent() 
			};
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}