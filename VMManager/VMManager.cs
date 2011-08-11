using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.ServiceProcess;
using System.Text;

namespace VMManager
{
    public partial class VMManager : ServiceBase
    {
        Server server;
        VMMonitor vmMonitor;
        VMActivityQueue activityQueue;

        public VMManager()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            activityQueue = new VMActivityQueue();

            server = new Server(activityQueue);
            vmMonitor = new VMMonitor(activityQueue);
            vmMonitor.Start();
        }

        protected override void OnStop()
        {
            if (vmMonitor != null)
                vmMonitor.Stop();
        }
    }
}
